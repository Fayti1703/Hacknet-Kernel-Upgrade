using Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KernelUpgradeMod {
	public class KernelUpgradeMod : Pathfinder.ModManager.IMod {

		static public Dictionary<string, string> aliasedCommands = new Dictionary<string, string>();

		public string Identifier {
			get {
				return "Kernel Upgrade";
			}
		}

		public void Load () {

		}

		public void LoadContent () {
			Pathfinder.Command.Handler.RegisterCommand(
				"netmap",
				(Pathfinder.Command.Handler.CommandFunc) Commands.netMapCommand,
				"Utilities for the netmap",
				true);
			Pathfinder.Command.Handler.RegisterCommand(
				"cp",
				(Pathfinder.Command.Handler.CommandFunc) Commands.cpCommand,
				"Copy files",
				true);
			Pathfinder.Command.Handler.RegisterCommand(
				"kill",
				(Pathfinder.Command.Handler.CommandFunc)  Commands.killCommand,
				"Kill command upgraded",
				true);
			Pathfinder.Command.Handler.RegisterCommand(
				"mkdir",
				(Pathfinder.Command.Handler.CommandFunc)  Commands.mkdirCommand,
				"Creates a new directory",
				true);
			Pathfinder.Command.Handler.RegisterCommand(
				"mkfile",
				(Pathfinder.Command.Handler.CommandFunc)  Commands.mkfileCommand,
				"Creates a new file",
				true);
			Pathfinder.Command.Handler.RegisterCommand(
				"rmdir",
				(Pathfinder.Command.Handler.CommandFunc)  Commands.rmdirCommand,
				"Removes a directory",
				true);
			Pathfinder.Command.Handler.RegisterCommand(
				"~",
				(Pathfinder.Command.Handler.CommandFunc)  Commands.rootShortcutCommand,
				"Root shortcut",
				true);

			Pathfinder.Command.Handler.RegisterCommand(
				"views",
				(Pathfinder.Command.Handler.CommandFunc)  Views.Views.Command,
				"Views application",
				true);
		}

		public void Unload () {

		}
	}
}
