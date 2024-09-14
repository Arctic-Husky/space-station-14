using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;
using Robust.Shared.Utility;

namespace Content.Server.Administration.Commands;

[UsedImplicitly]
[AdminCommand(AdminFlags.Stealth)]
public sealed class StealthminCommand : LocalizedCommands
{
    public override string Command => "stealthmin";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine(Loc.GetString("cmd-stealthmin-no-console"));
                return;
            }

            var mgr = IoCManager.Resolve<IAdminManager>();
<<<<<<< HEAD

=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
            var adminData = mgr.GetAdminData(player);

            DebugTools.AssertNotNull(adminData);

            if (!adminData!.Stealth)
<<<<<<< HEAD
            {
                mgr.Stealth(player);
            }
            else
            {
                mgr.UnStealth(player);
            }
=======
                mgr.Stealth(player);
            else
                mgr.UnStealth(player);
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    }
}
