using IWshRuntimeLibrary;
using System;
using System.IO;

namespace FactoryManagerInstaller
{
    class Startup
    {
        public static void AddInstallerToStartup(string appName, string directory)
        {
            WshShell wshShell = new WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut;
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Create the shortcut
            shortcut =
              (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                startUpFolderPath + "\\" +
                appName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0].ToString() + ".lnk");

            shortcut.TargetPath = directory + "\\" + appName;
            shortcut.WorkingDirectory = directory;
            shortcut.Description = "Launch Factory Manager Installer";
            // shortcut.IconLocation = Application.StartupPath + @"\App.ico";
            shortcut.Save();
        }

        public static void RemoveInstallerFromStartup(string appName)
        {
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
            FileInfo[] files = di.GetFiles("*.lnk");

            foreach (FileInfo fi in files)
            {
                //string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);

                if (fi.FullName.EndsWith(appName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0].ToString() + ".lnk",
                      StringComparison.InvariantCultureIgnoreCase))
                {
                    System.IO.File.Delete(fi.FullName);
                }
            }
        }
    }
}
