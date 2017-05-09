using Hacknet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KernelUpgradeMod.Views
{
    static class Views
    {
        public static bool Command(Hacknet.OS os, List<string> args)
        {
            if(args.Count < 2)
            {
                os.write("Usage : views [install/load/config/save]");
                return false;
            }

            if(args.Count > 1)
            {
                if(args[1] == "load")
                {
                    if(args.Count != 3)
                    {
                        os.write("Usage : views load [viewname]");
                        return false;
                    }
                    Folder viewsFolder = os.thisComputer.files.root.searchForFolder("home").searchForFolder("Views");
                    if (viewsFolder == null)
                    {
                        os.write("An internal error occured.");
                        return false;
                    }
                    FileEntry viewFile = viewsFolder.searchForFile(args[2]+".view");
                    if (viewFile == null)
                    {
                        os.write("That view configuration doesn't exist.");
                        return false;
                    }
                    os.netMap.visibleNodes.Clear();


                    os.netMap.discoverNode(os.thisComputer);

                    List<string> lines = viewFile.data.Split('\n').ToList<string>();
                    foreach(string line in lines)
                    {
                        List<string> words = line.Split(' ').ToList<string>();
                        if(words.Count >= 1)
                        {
                            foreach (Computer comp in os.netMap.nodes)
                            {
                                if (comp.ip == words[0])
                                {
                                    os.netMap.discoverNode(comp);
                                    if(words.Count == 3)
                                    {
                                        float tLoc = 0;
                                        if (float.TryParse(words[1], out tLoc) == true)
                                            comp.location.X = tLoc;
                                        if (float.TryParse(words[2], out tLoc) == true)
                                            comp.location.Y = tLoc;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    os.write("View loaded successfully.");
                    return false;
                }

                if(args[1] == "save")
                {
                    if (args.Count == 3)
                    {
                        string viewname = args[2] + ".view";
                        Folder viewsFolder = os.thisComputer.files.root.searchForFolder("home").searchForFolder("Views");
                        if (viewsFolder == null)
                        {
                            os.write("An internal error occured.");
                            return false;
                        }
                        FileEntry viewFile = viewsFolder.searchForFile(viewname);
                        if (viewFile == null)
                        {
                            viewFile = new FileEntry("", viewname);
                            viewsFolder.files.Add(viewFile);
                        }

                        viewFile.data = "";

                        foreach(int index in os.netMap.visibleNodes)
                        {
                            Computer comp = os.netMap.nodes[index];
                            string text = comp.ip + " " + comp.location.X + " " + comp.location.Y + "\n";
                            viewFile.data += text;
                        }
                    }
                    else
                    {
                        os.write("Usage : views save [viewname]");
                        return false;
                    }
                }
                if(args[1] == "install")
                {
                    if(args.Count > 2)
                    {
                        if(args[2] == "clean")
                        {
                            uninstall(os);
                        }
                        if(checkInstalled(os))
                        {
                            os.write("Views is already installed. Maybe you want a clean install : views install clean");
                            return false;
                        }
                        install(os);
                    }
                }
                if(args[1] == "config")
                {
                    if(!checkInstalled(os))
                    {
                        os.write("Views is not installed.");
                        return false;
                    }
                    if(args.Count == 2)
                    {
                        os.write("Usage : views config [viewname] [add/remove/delete] [ip] (xpos) (ypos)");
                        return false;
                    }
                    if(args.Count == 4)
                    {
                        if (args[3] == "delete")
                        {
                            string viewname = args[2] + ".view";
                            Folder viewsFolder = os.thisComputer.files.root.searchForFolder("home").searchForFolder("Views");
                            if (viewsFolder == null)
                            {
                                os.write("An internal error occured.");
                                return false;
                            }

                            foreach (FileEntry file in viewsFolder.files)
                            {
                                if (file.getName() == viewname)
                                {
                                    viewsFolder.files.Remove(file);
                                    os.write("Views configuration removed.");
                                    return false;
                                }
                            }
                        }
                        os.write("Usage : views config [viewname] [add/remove/delete] [ip] (xpos) (ypos)");
                        return false;
                    }
                    if(args.Count > 4)
                    {
                        if(args[3] != "add" && args[3] != "remove" && args[3] != "delete")
                        {
                            os.write("Wrong argument, usage : views config [viewname] [add/remove/delete] [ip] (xpos) (ypos)");
                            return false;
                        }
                        string viewname = args[2]+".view";
                        Folder viewsFolder = os.thisComputer.files.root.searchForFolder("home").searchForFolder("Views");
                        if(viewsFolder == null)
                        {
                            os.write("An internal error occured.");
                            return false;
                        }
                        else
                        {
                            FileEntry viewFile = viewsFolder.searchForFile(viewname);
                            if (viewFile == null)
                            {
                                viewFile = new FileEntry("", viewname);
                                viewsFolder.files.Add(viewFile);
                            }
                            if(args[3] == "add")
                            {
                                string ip = "";
                                foreach(Computer comp in os.netMap.nodes)
                                {
                                    if(comp.ip == args[4])
                                    {
                                        ip = args[4];
                                        break;
                                    }
                                }
                                if(ip == "")
                                {
                                    os.write("Invalid IP.");
                                    return false;
                                }
                                string newText = ip;
                                if (args.Count == 7)
                                    newText += " " + args[5] + " " + args[6];
                                newText += "\n";

                                viewFile.data += newText;

                                os.write("Added the IP to the views config file.");
                            }
                            if(args[3] == "remove")
                            {
                                string text = viewFile.data;
                                List<string> lines = text.Split('\n').ToList<string>();

                                foreach(string line in lines)
                                {
                                    if(line.StartsWith(args[4]))
                                    {
                                        lines.Remove(line);
                                        text = string.Join("\n", lines);
                                        viewFile.data = text;
                                        os.write("Removed the IP from the views config file.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        os.write("Usage : views config [viewname] [add/remove/delete] [ip] (xpos) (ypos)");
                        return false;
                    }
                }
            }

            return false;
        }

        public static bool checkInstalled(Hacknet.OS os)
        {
            Folder homeFolder = os.thisComputer.files.root.searchForFolder("home");
            if(homeFolder == null)
            {
                os.write("Your OS is corrupted. Please reinstall Hacknet OS or download the latest updates.");
                return false;
            }
            Folder viewsFolder = homeFolder.searchForFolder("Views");
            if (viewsFolder == null)
                return false;
            return true;
        }

        public static void uninstall(Hacknet.OS os)
        {
            Folder homeFolder = os.thisComputer.files.root.searchForFolder("home");
            if (homeFolder == null)
            {
                os.write("Your OS is corrupted. Please reinstall Hacknet OS or download the latest updates.");
                return;
            }
            foreach (Folder folder in homeFolder.folders)
            {
                if (folder.getName() == "Views")
                {
                    homeFolder.folders.Remove(folder);
                    return;
                }
            }
        }

        public static void install(Hacknet.OS os)
        {
            Folder homeFolder = os.thisComputer.files.root.searchForFolder("home");

            Folder viewsFolder = new Folder("Views");

            viewsFolder.files.Add(new FileEntry("1111110000010010110001011101101111100101011101001101011111010101010000100101000100100101100001", "views.bin"));
            homeFolder.folders.Add(viewsFolder);
        }
    }
}
