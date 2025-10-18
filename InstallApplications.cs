using System;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Windows.Forms;

namespace FactoryManagerInstaller
{
    class InstallApplications
    {

        Form1 mainUI;
        public void StartInstallations(Form1 form)
        {
            int sequence = SequenceChecker.GetSequence();
            mainUI = form;
            InstallAsPerSequence(sequence);

        }

        private void InstallAsPerSequence(int sequence)
        {
            mainUI.UpdateUI(sequence);
            try
            {
                switch (sequence)
                {
                    case 0: //Add this Installer to startup and then Start SQL Installer
                        {
                            Startup.AddInstallerToStartup(Common.appName, Common.currentFolder);
                            //MessageBox.Show("Add this Installer to startup and then Start SQL Installer");
                            
                            ExecuteAsAdmin(Common.currentFolder + @"Dependency\mysql-installer-community-5.7.16.0.msi");
                            break;
                        }
                    case 1: //Start Adding Database
                        {
                            //MessageBox.Show("Start Adding Database");
                            ExecuteAsAdmin(Common.currentFolder + @"Dependency\CRRuntime_64bit_13_0_32.msi");

                            RunScripts.AddInitialData(Common.driver, Common.server);
                            break;
                        }
                    case 2: 
                        {

                            ExecuteAsAdmin(Common.currentFolder + @"FactoryManager\setup.exe");
                            break;
                        }
                    default:
                        {
                            Startup.RemoveInstallerFromStartup(Common.appName);
                            SequenceChecker.RemoveSequence();
                            mainUI.ExitButton.Text = "Finish";
                            mainUI.ExitButton.Visible = true;
                            mainUI.rtbText = "Click 'Finish' button to complete setup and restart system.";
                            return;
                        }
                }
            }
            catch (Exception ex)
            {

            }
            SequenceChecker.SetSequence(sequence + 1);
            if (sequence == 0)
            {
                mainUI.rtbText = "Please wait while database is being installed. Click Next once it is completed.";
                System.Threading.Thread.Sleep(10000);
                mainUI.NextButton.Visible = true;
                return;
            }
            InstallAsPerSequence(sequence + 1);
        }

        //private void CopyDirectory(string sourcedirectory, string target)
        //{


        //    foreach (string dirPath in Directory.GetDirectories(sourcedirectory, "*", SearchOption.AllDirectories))
        //    {
        //        Directory.CreateDirectory(dirPath.Replace(sourcedirectory, target));
        //    }
        //    foreach (string newPath in Directory.GetFiles(sourcedirectory, "*.*", SearchOption.AllDirectories))
        //    {
        //        File.Copy(newPath, newPath.Replace(sourcedirectory, target), true);
        //    }
        //}

        public void RestartSystem()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection moc = mos.Get();
            foreach (ManagementObject mo in moc)
            {
                try
                {
                    mo.Scope.Options.EnablePrivileges = true;
                    mo.InvokeMethod("Reboot", new string[] { "" });
                }
                catch
                {

                }
            }
        }

        //private void ExecuteAsAdminNoWait(string fileName)
        //{
        //    try
        //    {
        //        Process proc = new Process();
        //        //MessageBox.Show("Installing " + fileName);
        //        if (fileName.EndsWith("msi"))
        //        {
        //            proc.StartInfo.FileName = "cmd.exe";
        //            proc.StartInfo.Arguments = "/c start /wait msiexec.exe /i " + "\"" + fileName + "\"" + " /qf ";
        //        }
        //        else
        //        {
        //            proc.StartInfo.FileName = fileName;
        //        }
        //        proc.StartInfo.UseShellExecute = true;
        //        proc.StartInfo.Verb = "runas";
        //        proc.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Unable to install " + ex.Message);
        //    }
        //}
        private void ExecuteAsAdmin(string fileName)
        {
            try
            {
                Process proc = new Process();
                //MessageBox.Show("Installing " + fileName);
                if (fileName.EndsWith("msi"))
                {
                    proc.StartInfo.FileName = "cmd.exe";
                    proc.StartInfo.Arguments = "/c start /wait msiexec.exe /i " + "\"" + fileName + "\"" + " /qf ";
                }
                else
                {
                    proc.StartInfo.FileName = fileName;
                }
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to install " + ex.Message);
            }
        }
    }
}
