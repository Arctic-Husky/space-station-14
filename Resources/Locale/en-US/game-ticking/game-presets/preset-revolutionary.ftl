## Rev Head

roles-antag-rev-head-name = Head Revolutionary
roles-antag-rev-head-objective = Your objective is to take over the station by converting people to your cause and killing all Command staff on station.

head-rev-role-greeting =
    You are a Head Revolutionary.
<<<<<<< HEAD
    You are tasked with removing all of Command from station via death, exilement or imprisonment.
=======
    You are tasked with removing all of Command from station via death or exilement.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
    The Syndicate has sponsored you with a flash that converts the crew to your side.
    Beware, this won't work on Security, Command, or those wearing sunglasses.
    Viva la revolución!

head-rev-briefing =
    Use flashes to convert people to your cause.
<<<<<<< HEAD
    Get rid of all heads to take over the station.
=======
    Kill all heads to take over the station.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

head-rev-break-mindshield = The Mindshield was destroyed!

## Rev

roles-antag-rev-name = Revolutionary
<<<<<<< HEAD
roles-antag-rev-objective = Your objective is to ensure the safety and follow the orders of the Head Revolutionaries as well as getting rid of all Command staff on station.
=======
roles-antag-rev-objective = Your objective is to ensure the safety and follow the orders of the Head Revolutionaries as well as killing all Command staff on station.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

rev-break-control = {$name} has remembered their true allegiance!

rev-role-greeting =
    You are a Revolutionary.
    You are tasked with taking over the station and protecting the Head Revolutionaries.
<<<<<<< HEAD
    Get rid of all of the Command staff.
    Viva la revolución!

rev-briefing = Help your head revolutionaries get rid of every head to take over the station.
=======
    Eliminate all of the Command staff.
    Viva la revolución!

rev-briefing = Help your head revolutionaries kill every head to take over the station.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

## General

rev-title = Revolutionaries
rev-description = Revolutionaries are among us.

rev-not-enough-ready-players = Not enough players readied up for the game. There were {$readyPlayersCount} players readied up out of {$minimumPlayers} needed. Can't start a Revolution.
rev-no-one-ready = No players readied up! Can't start a Revolution.
rev-no-heads = There were no Head Revolutionaries to be selected. Can't start a Revolution.

<<<<<<< HEAD
rev-won = The Head Revs survived and successfully seized control of the station.
=======
rev-all-heads-dead = All the heads are dead, now finish up the rest of the crew!

rev-won = The Head Revs survived and killed all of Command.
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f

rev-lost = Command survived and killed all of the Head Revs.

rev-stalemate = All of the Head Revs and Command died. It's a draw.

rev-reverse-stalemate = Both Command and Head Revs survived.

rev-headrev-count = {$initialCount ->
    [one] There was one Head Revolutionary:
    *[other] There were {$initialCount} Head Revolutionaries:
}

rev-headrev-name-user = [color=#5e9cff]{$name}[/color] ([color=gray]{$username}[/color]) converted {$count} {$count ->
    [one] person
    *[other] people
}

rev-headrev-name = [color=#5e9cff]{$name}[/color] converted {$count} {$count ->
    [one] person
    *[other] people
}

## Deconverted window

rev-deconverted-title = Deconverted!
rev-deconverted-text =
    As the last headrev has died, the revolution is over.

    You are no longer a revolutionary, so be nice.
rev-deconverted-confirm = Confirm
