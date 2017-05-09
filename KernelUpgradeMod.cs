using Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KernelUpgradeMod
{
    public class KernelUpgradeMod : IPathfinderMod
    {

        static public Dictionary<string, string> aliasedCommands = new Dictionary<string, string>();

        public string Identifier
        {
            get
            {
                return "Kernel Upgrade";
            }
        }

        public void Load()
        {

        }

        public void LoadContent()
        {
            Pathfinder.Command.Handler.RegisterCommand("netmap", Commands.netMapCommand, "Utilities for the netmap", true);
            Pathfinder.Command.Handler.RegisterCommand("cp", Commands.cpCommand, "Copy files", true);
            Pathfinder.Command.Handler.RegisterCommand("kill", Commands.killCommand, "Kill command upgraded", true);
            Pathfinder.Command.Handler.RegisterCommand("mkdir", Commands.mkdirCommand, "Creates a new directory", true);
            Pathfinder.Command.Handler.RegisterCommand("mkfile", Commands.mkfileCommand, "Creates a new file", true);
            Pathfinder.Command.Handler.RegisterCommand("rmdir", Commands.rmdirCommand, "Removes a directory", true);
            Pathfinder.Command.Handler.RegisterCommand("~", Commands.rootShortcutCommand, "Root shortcut", true);

            Pathfinder.Command.Handler.RegisterCommand("views", Views.Views.Command, "Views application", true);
        }

        public void Unload()
        {

        }
    }
}
