using System.Linq;
using System.Numerics;
using Content.Server.Power.Components;
using Content.Shared._EstacaoPirata.Xenobiology.SlimeGrinder;
using Content.Shared.Administration.Logs;
using Content.Shared.Audio;
using Content.Shared.Body.Components;
using Content.Shared.Climbing.Events;
using Content.Shared.Construction.Components;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Jittering;
using Content.Shared.Mobs.Systems;
using Content.Shared.Throwing;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Physics.Components;
using Robust.Shared.Random;

namespace Content.Server._EstacaoPirata.Xenobiology.SlimeGrinder;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeGrinderSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly ThrowingSystem _throwing = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _ambientSoundSystem = default!;
    [Dependency] private readonly SharedJitteringSystem _jitteringSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<SlimeGrinderComponent, ComponentInit>(OnInit);

        SubscribeLocalEvent<SlimeGrinderComponent, AfterInteractUsingEvent>(OnAfterInteractUsing);
        SubscribeLocalEvent<SlimeGrinderComponent, ClimbedOnEvent>(OnClimbedOn);
        SubscribeLocalEvent<SlimeGrinderComponent, SlimeGrinderDoAfterEvent>(OnDoAfter);

        SubscribeLocalEvent<SlimeGrinderComponent, UnanchorAttemptEvent>(OnUnanchorAttempt);
        SubscribeLocalEvent<SlimeGrinderComponent, PowerChangedEvent>(OnPowerChanged);

        SubscribeLocalEvent<ActiveSlimeGrinderComponent, ComponentInit>(OnActiveInit);
        SubscribeLocalEvent<ActiveSlimeGrinderComponent, ComponentShutdown>(OnActiveShutdown);

        SubscribeLocalEvent<SlimeGrinderComponent, SlimeGrinderStartGrindingMessage>((u,c,m) => StartGrinder(u,c));
        SubscribeLocalEvent<SlimeGrinderComponent, SlimeGrinderStopGrindingMessage>((u,c,m) => StopGrinder(u,c));

        SubscribeLocalEvent<SlimeGrinderComponent, SlimeGrinderEjectSolidIndexedMessage>(OnEjectIndex);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<ActiveSlimeGrinderComponent, SlimeGrinderComponent>();
        while (query.MoveNext(out var uid, out var _, out var grinder))
        {
            if (!HasContents(grinder))
                continue;

            grinder.ProcessingTimer -= frameTime;

            if (grinder.ProcessingTimer > 0)
            {
                continue;
            }

            // Spawnar o extract do slime do topo da lista, resetar o processing timer e deletar o slime

            var coordinates = _transform.GetMapCoordinates(uid);
            if(!TryComp<SlimeCoreComponent>(grinder.Storage.ContainedEntities.First(), out var slimeGrindableComponent))
                continue;

            var yieldRounded = Math.Round(slimeGrindableComponent.Yield);
            // TODO: randomizar um pouco a posicao do spawn de cada extract
            for (int i = 0; i < yieldRounded; i++)
            {
                Spawn(slimeGrindableComponent.Core, coordinates);
            }

            // TODO: mudar isso do timer pra ele com a massa do bicho, biomass reclaimer pra ter uma referencia
            grinder.ProcessingTimer = grinder.ProcessingTimerTotal;
            QueueDel(grinder.Storage.ContainedEntities.First());
            UpdateUserInterfaceState(uid, grinder);
        }
    }

    private void OnActiveInit(Entity<ActiveSlimeGrinderComponent> ent, ref ComponentInit args)
    {
        SetAppearance(ent.Owner, SlimeGrinderVisualState.Grinding);
        _jitteringSystem.AddJitter(ent.Owner, -80, 80); // usar valores de componente
        var sound = _audioSystem.PlayPvs(ent.Comp.StartingSound, ent.Owner);
        _audioSystem.SetVolume(sound.Value.Entity, sound.Value.Component.Volume - 15, sound.Value.Component); // usar valores de componente
        _ambientSoundSystem.SetAmbience(ent.Owner, true);
    }

    private void OnActiveShutdown(Entity<ActiveSlimeGrinderComponent> ent, ref ComponentShutdown args)
    {
        RemComp<JitteringComponent>(ent.Owner);
        SetAppearance(ent.Owner, SlimeGrinderVisualState.Idle);
        _ambientSoundSystem.SetAmbience(ent.Owner, false);
    }
    private void OnInit(Entity<SlimeGrinderComponent> ent, ref ComponentInit args)
    {
        ent.Comp.Storage = _container.EnsureContainer<Container>(ent, "slime_grinder_entity_container");
    }

    private void OnPowerChanged(Entity<SlimeGrinderComponent> ent, ref PowerChangedEvent args)
    {
        if (!args.Powered)
        {
            RemComp<ActiveSlimeGrinderComponent>(ent.Owner);
            SetAppearance(ent.Owner, SlimeGrinderVisualState.Idle);
        }

    }

    private void OnUnanchorAttempt(Entity<SlimeGrinderComponent> ent, ref UnanchorAttemptEvent args)
    {
        // TODO: fazer cuspir os slimes
        args.Cancel();
    }

    // Inserir slime da mao, mas nao da pra fazer isso agora
    private void OnAfterInteractUsing(Entity<SlimeGrinderComponent> ent, ref AfterInteractUsingEvent args)
    {
        if (!args.CanReach || args.Target == null)
            return;

        if (!CanInsert(args.Used, ent.Comp))
            return;

        var delay = ent.Comp.BaseInsertionDelay;
        _doAfterSystem.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, delay, new SlimeGrinderDoAfterEvent(), ent, target: args.Target, used: args.Used)
        {
            NeedHand = true,
            BreakOnMove = true
        });
    }

    private void OnClimbedOn(Entity<SlimeGrinderComponent> ent, ref ClimbedOnEvent args)
    {
        if (!CanInsert(args.Climber, ent.Comp))
        {
            var direction = new Vector2(_robustRandom.Next(-2, 2), _robustRandom.Next(-2, 2));
            _throwing.TryThrow(args.Climber, direction, 0.5f);
            return;
        }

        _adminLogger.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(args.Instigator):player} used a slime grinder to gib {ToPrettyString(args.Climber):target} in {ToPrettyString(ent):ent}");

        // Inserir corpo do slime no container aqui
        InsertSlimeIntoContainer(user:args.Instigator, slime:args.Climber, grinder:ent.Owner, grinderComponent:ent.Comp);
    }

    private void OnEjectIndex(Entity<SlimeGrinderComponent> ent, ref SlimeGrinderEjectSolidIndexedMessage args)
    {
        if (!HasContents(ent.Comp))
            return;

        _container.Remove(EntityManager.GetEntity(args.EntityID), ent.Comp.Storage);
        UpdateUserInterfaceState(ent, ent.Comp);
    }

    private void OnDoAfter(Entity<SlimeGrinderComponent> ent, ref SlimeGrinderDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (args.Args.Used == null || args.Args.Target == null || !HasComp<SlimeGrinderComponent>(args.Args.Target.Value))
            return;

        //_adminLogger.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(args.Args.User):player} used a slime grinder to gib {ToPrettyString(args.Args.Target.Value):target} in {ToPrettyString(ent):ent}");

        // Inserir corpo do slime no container aqui

        InsertSlimeIntoContainer(user:args.Args.User, slime:args.Args.Target.Value, grinder:ent.Owner, grinderComponent:ent.Comp);

        args.Handled = true;
    }

    public bool InsertSlimeIntoContainer(EntityUid user, EntityUid slime, EntityUid grinder, SlimeGrinderComponent grinderComponent)
    {
        // Checks ja foram feitos pra chegar aqui
        // if (!CanInsert(slime, grinderComponent))
        // {
        //     return false;
        // }

        if (!_containerSystem.Insert(slime, grinderComponent.Storage))
            return false;

        _adminLogger.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(user):player} inserted {ToPrettyString(slime)} into {ToPrettyString(grinder)}");

        UpdateUserInterfaceState(grinder, grinderComponent);
        return true;
    }

    public bool CanInsert(EntityUid entity, SlimeGrinderComponent component)
    {
        // if (!Transform(uid).Anchored)
        //     return false;

        if (!HasComp<SlimeCoreComponent>(entity))
            return false;

        // Talvez mudar depois pra poder aceitar slime vivo, com algum upgrade de maquina sei la
        if (!_mobState.IsDead(entity))
            return false;

        if (!HasComp<BodyComponent>(entity))
            return false;

        if (!TryComp<PhysicsComponent>(entity, out var physics))
            return false;

        // if (!physics.CanCollide)
        //     return false;

        return _containerSystem.CanInsert(entity, component.Storage);
    }

    public void UpdateUserInterfaceState(EntityUid uid, SlimeGrinderComponent component)
    {
        _userInterface.SetUiState(uid, SlimeGrinderUiKey.Key, new SlimeGrinderUpdateUserInterfaceState(
            GetNetEntityArray(component.Storage.ContainedEntities.ToArray()),
            HasComp<ActiveSlimeGrinderComponent>(uid)));
    }

    public static bool HasContents(SlimeGrinderComponent component)
    {
        return component.Storage.ContainedEntities.Any();
    }

    public void StartGrinder(EntityUid grinder, SlimeGrinderComponent component) // , EntityUid beingGrinded
    {
        if (HasComp<ActiveSlimeGrinderComponent>(grinder) || !(TryComp<ApcPowerReceiverComponent>(grinder, out var apc) && apc.Powered))
            return;

        AddComp<ActiveSlimeGrinderComponent>(grinder);

        // var solidsDict = new Dictionary<string, int>();
        //
        // foreach (var item in component.Storage.ContainedEntities.ToArray())
        // {
        //     // para algum possivel comportamento especial ao grindar
        //     var ev = new BeingSlimeGrindedEvent(grinder, beingGrinded);
        //     RaiseLocalEvent(item, ev);
        //
        //     if (ev.Handled)
        //     {
        //         UpdateUserInterfaceState(grinder, component);
        //         return;
        //     }
        // }
    }

    private void StopGrinder(EntityUid grinder, SlimeGrinderComponent component)
    {
        if (!HasComp<ActiveSlimeGrinderComponent>(grinder))
            return;

        RemComp<ActiveSlimeGrinderComponent>(grinder);
    }

    public void SetAppearance(EntityUid uid, SlimeGrinderVisualState state, SlimeGrinderComponent? component = null, AppearanceComponent? appearanceComponent = null)
    {
        if (!Resolve(uid, ref component, ref appearanceComponent, false))
            return;

        _appearance.SetData(uid, SlimeGrinderVisuals.VisualState, state, appearanceComponent);
    }
}
