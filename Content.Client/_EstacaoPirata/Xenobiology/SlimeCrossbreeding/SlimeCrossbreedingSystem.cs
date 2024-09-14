using Content.Shared._EstacaoPirata.Xenobiology.SlimeCrossbreeding;
using Robust.Client.GameObjects;

namespace Content.Client._EstacaoPirata.Xenobiology.SlimeCrossbreeding;

/// <summary>
/// This handles...
/// </summary>
public sealed class SlimeCrossbreedingSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeNetworkEvent<ExtractColorChangeEvent>(OnColorChange);
    }

    private void OnColorChange(ExtractColorChangeEvent ev)
    {
        var entity = GetEntity(ev.Extract);

        if (!TryComp(entity, out SpriteComponent? spriteComponent))
            return;

        spriteComponent.LayerSetColor(0, ev.Color);
    }
}
