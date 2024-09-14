using Content.Server.Chat.Systems;
using Content.Shared.Administration;
<<<<<<< HEAD
=======
using Content.Shared.Chat;
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    sealed class DSay : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _e = default!;

        public string Command => "dsay";

        public string Description => Loc.GetString("dsay-command-description");

        public string Help => Loc.GetString("dsay-command-help-text", ("command", Command));

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine("shell-only-players-can-run-this-command");
                return;
            }

            if (player.AttachedEntity is not { Valid: true } entity)
                return;

            if (args.Length < 1)
                return;

            var message = string.Join(" ", args).Trim();
            if (string.IsNullOrEmpty(message))
                return;

            var chat = _e.System<ChatSystem>();
            chat.TrySendInGameOOCMessage(entity, message, InGameOOCChatType.Dead, false, shell, player);
        }
    }
}
