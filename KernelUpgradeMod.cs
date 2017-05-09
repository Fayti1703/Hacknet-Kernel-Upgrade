using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KernelUpgradeMod
{
    public class KernelUpgradeMod : Pathfinder.PathfinderMod
    {

        static public Dictionary<string, string> aliasedCommands = new Dictionary<string, string>();


        public override string GetIdentifier()
        {
            return "Kernel Upgrade v1";
        }

        public override void Load()
        {

        }

        public override void LoadContent()
        {
            Pathfinder.Command.Handler.AddCommand("netmap", Commands.netMapCommand, "Utilities for the netmap", true);
            Pathfinder.Command.Handler.AddCommand("cp", Commands.cpCommand, "Copy files", true);
            Pathfinder.Command.Handler.AddCommand("kill", Commands.killCommand, "Kill command upgraded", true);
            Pathfinder.Command.Handler.AddCommand("mkdir", Commands.mkdirCommand, "Creates a new directory", true);
            Pathfinder.Command.Handler.AddCommand("mkfile", Commands.mkfileCommand, "Creates a new file", true);
            Pathfinder.Command.Handler.AddCommand("rmdir", Commands.rmdirCommand, "Removes a directory", true);
            Pathfinder.Command.Handler.AddCommand("~", Commands.rootShortcutCommand, "Root shortcut", true);
        }

        public override void Unload()
        {

        }
    }
}
