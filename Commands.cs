﻿using Hacknet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KernelUpgradeMod {
	static class Commands {
		static public bool netMapCommand (OS os, List<string> args) {
			if(args.Count < 2) {
				os.write("Usage : netmap [add/remove/set/get]");
				return false;
			}
			switch(args[1]) {
				case "set":
					if(args.Count != 6) {
						os.write("Usage : netmap set pos [IP] [X] [Y]");
						return false;
					}
					for(int i = 0; i < os.netMap.nodes.Count; i++) {
						if(os.netMap.nodes[i].ip.Equals(args[3])) {
							float x, y;
							try {
								x = float.Parse(
									args[4],
									System.Globalization.CultureInfo.InvariantCulture);
								y = float.Parse(
									args[5],
									System.Globalization.CultureInfo.InvariantCulture);
								os.netMap.nodes[i].location.X = x;
								os.netMap.nodes[i].location.Y = y;
								os.write(args[3] + " was moved successfully.");
								return false;
							} catch(Exception ex) {
								os.write("Position arguments not valid. Write them like '0.55'");
								return false;
							}
						}
					}
					os.write("Could not move " + args[3] + " : IP invalid.");
					return false;
				case "get":
					if(args.Count < 4) {
						os.write("Usage : netmap get pos [IP]");
						return false;
					}
					for(int i = 0; i < os.netMap.nodes.Count; i++) {
						if(os.netMap.nodes[i].ip == args[3]) {
							os.write(args[3] + " is at : x = " + os.netMap.nodes[i].location.X + "; y = " + os.netMap.nodes[i].location.Y + ";");
							return false;
						}
					}
					os.write("IP " + args[3] + " invalid.");
					return false;
				case "remove":
					if(args.Count == 2) {
						if(os.connectedIP == os.thisComputer.ip) {
							os.write("Cannot remove your own node from the netmap.");
							return false;
						}
						for(int i = 0; i < os.netMap.nodes.Count; i++) {
							if(os.netMap.nodes[i].ip.Equals(os.connectedIP)) {
								os.netMap.visibleNodes.Remove(i);
								os.write("IP " + os.connectedIP + " successfully removed.");
								return false;
							}
						}
					}
					for(int i = 0; i < os.netMap.nodes.Count; i++) {
						if(os.netMap.nodes[i].ip.Equals(args[2])) {
							os.netMap.visibleNodes.Remove(i);
							os.write("IP " + args[2] + " successfully removed.");
							return false;

						}
					}
					os.write("Unable to forget node : IP not discovered");
					return false;
				case "add":
					if(args.Count < 3) {
						os.write("Usage : netmap add [IP]");
						return false;
					}
					for(int i = 0; i < os.netMap.nodes.Count; i++) {
						if(os.netMap.nodes[i].ip.Equals(args[2])) {
							if(!os.netMap.visibleNodes.Contains(os.netMap.nodes.IndexOf(os.netMap.nodes[i]))) {
								os.netMap.visibleNodes.Add(os.netMap.nodes.IndexOf(os.netMap.nodes[i]));
								os.netMap.lastAddedNode = os.netMap.nodes[i];
								os.write("IP " + args[2] + " successfully added.");
								return false;
							}
						}

					}
					os.write("Unable to add node : Could not resolve IP");
					return false;

			}
			return false;
		}

		static public bool rootShortcutCommand (OS os, List<string> args) {
			os.execute("cd /");
			return false;
		}

		static public bool cpCommand (OS os, List<string> args) {
			if(os.connectedIP != os.thisComputer.ip) {
				os.write("Cannot copy files on remote host.");
				return false;
			}
			if(args.Count < 2) {
				os.write("Usage : cp [FileName] (destination)");
				return false;
			}
			FileEntry sourceFile = Programs.getCurrentFolder(os).searchForFile(args[1]);
			Folder sourceFolder = Programs.getCurrentFolder(os);
			if(sourceFile == null) { //Find Source File
				Folder rootFolder = os.thisComputer.files.root;
				int length = args[1].LastIndexOf('/');
				if(length <= 0) {
					os.write("File " + args[1] + " not found.");
					return false;
				}
				string path = args[1].Substring(0, length);
				sourceFolder = Programs.getFolderAtPath(
					path,
					os,
					rootFolder,
					false);
				if(sourceFolder == null) {
					os.write("Local Folder " + path + " not found.");
					return false;
				} else {
					string fileName = args[1].Substring(length + 1);
					sourceFile = sourceFolder.searchForFile(fileName);
					if(sourceFile == null) {
						os.write("File " + fileName + " not found at specified filepath.");
						return false;
					}
				}
			}
			if(args.Count == 2) {
				bool isCorrect = false;
				bool hasBeenChanged = false;
				int passes = 0;
				string newFileName = args[1];
				while(isCorrect == false) {
					hasBeenChanged = false;
					for(int i = 0; i < sourceFolder.files.Count; i++)
						if(newFileName == sourceFolder.files[i].name) {
							newFileName += "-c";
							hasBeenChanged = true;
						}
					if(hasBeenChanged == false)
						isCorrect = true;
					else if(passes > 5)
							break;
						else
							passes++;
				}
				if(isCorrect == false) {
					os.write("Could not copy file " + args[1] + " : Failed to fix name conflict.");
					return false;
				}
				sourceFolder.files.Add(new FileEntry(
						sourceFile.data,
						newFileName));
				os.write("File successfully copied.");
				return false;
			} else {
				int length = args[2].LastIndexOf('/');
				if(length <= 0) {
					bool isCorrect = false;
					bool hasBeenChanged = false;
					int passes = 0;
					string newFileName = args[2];
					while(isCorrect == false) {
						hasBeenChanged = false;
						for(int i = 0; i < sourceFolder.files.Count; i++)
							if(newFileName == sourceFolder.files[i].name) {
								newFileName += "-c";
								hasBeenChanged = true;
							}
						if(hasBeenChanged == false)
							isCorrect = true;
						else if(passes > 5)
								break;
							else
								passes++;
					}
					if(isCorrect == false) {
						os.write("Could not copy file " + args[1] + " : Failed to fix name conflict.");
						return false;
					}
					sourceFolder.files.Add(new FileEntry(
							sourceFile.data,
							newFileName));
					os.write("File successfully copied.");
					return false;
				} else {
					string path = args[2].Substring(0, length);
					Folder directionFolder = Programs.getFolderAtPath(path, os, os.thisComputer.files.root, false);

					bool isCorrect = false;
					bool hasBeenChanged = false;
					int passes = 0;
					string newFileName = args[2].Substring(length + 1);
					while(isCorrect == false) {
						hasBeenChanged = false;
						for(int i = 0; i < directionFolder.files.Count; i++)
							if(newFileName == directionFolder.files[i].name) {
								newFileName += "-c";
								hasBeenChanged = true;
							}
						if(hasBeenChanged == false)
							isCorrect = true;
						else if(passes > 5)
								break;
							else
								passes++;
					}
					if(isCorrect == false) {
						os.write("Could not copy file " + args[1] + " : Failed to fix name conflict.");
						return false;
					}
					directionFolder.files.Add(new FileEntry(
							sourceFile.data,
							newFileName));
					os.write("File successfully copied.");
					return false;
				}
			}
		}

		static public bool killCommand (OS os, List<string> args) {
			if(args.Count == 2) {
				if(args[0] == "kill" && args[1] == "all") {
					if(os.exes.Count == 0) {
						os.write("No Processes to kill.");
						return false;
					}
					while(os.exes.Count != 0)
						for(int i = 0; i < os.exes.Count; i++) {
							os.exes[i].Killed();
							os.exes.RemoveAt(i);
						}
					os.write("Killed all running processes.");
					return false;
				} else {
					Programs.kill(args.ToArray(), os);
					return false;
				}
			} else {
				Programs.kill(args.ToArray(), os);
				return false;
			}

		}

		static public bool mkdirCommand (OS os, List<string> args) {
			if(args.Count <= 1) {
				os.write("Usage : mkdir [FOLDER NAME]");
				return false;
			}
			Folder folder = Programs.getCurrentFolder(os);
			folder.folders.Add(new Folder(args[1]));
			os.write("Folder " + args[1] + " created.");
			return false;
		}

		static public bool rmdirCommand (OS os, List<string> args) {
			if(args.Count < 2) {
				os.write("Usage : rmdir [FOLDER NAME]");
				return false;
			}
			if(args[1] == "sys" || args[1] == "bin" || args[1] == "home" || args[1] == "log") {
				os.write("Could not remove folder " + args[1] + " : Permission denied.");
				return false;
			}

			Folder currentFolder = Programs.getCurrentFolder(os);
			foreach(Folder folder in currentFolder.folders) {
				if(folder.getName() == args[1]) {
					currentFolder.folders.Remove(folder);
					os.write("Folder " + args[1] + " successfully removed.");
					return false;
				}
			}
			os.write("Folder " + args[1] + " not found.");
			return false;
		}

		static public bool mkfileCommand (OS os, List<string> args) {
			if(args.Count <= 1) {
				os.write("Usage : mkfile [FILE NAME]");
				return false;
			}
			Folder folder = Programs.getCurrentFolder(os);
			if(args.Count == 2) {
				folder.files.Add(new FileEntry("", args[1]));
				os.write("File " + args[1] + " created.");
				return false;
			} else {
				string FileData = args[2];
				for(int i = 1; i < args.Count - 2; i++) {
					FileData = FileData + " " + args[i + 2];
				}
				folder.files.Add(new FileEntry(FileData, args[1]));
				os.write("File " + args[1] + " created.");
				return false;
			}
		}
	}
}
