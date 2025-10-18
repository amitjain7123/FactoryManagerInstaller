using System;
using System.Drawing;
using System.Windows.Forms;

namespace FactoryManagerInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Button ExitButton
        {
            get
            {
                return btnExit;
            }
        }

        public Button NextButton
        {
            get
            {
                return btnNext;
            }
        }
        public string rtbText
        {
            set
            {
                this.txtBox.Text = value;
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            btnExit.Visible = false;
            btnNext.Visible = false;
            InstallApplications installApplications = new InstallApplications();
            installApplications.StartInstallations(this);

        }
        public void UpdateUI(int sequence)
        {
            progressBar.Value = sequence + 1;
            switch (sequence)
            {
                case 0:
                    {
                        rtbText = "Installation progress started. System will be restarted";
                        // updateProgresslabel(lblSqlInstaller);
                        updateProgresslabel(lblDrvUpdate);
                        break;
                    }
                case 1:
                    {
                        rtbText = "Installation progress in continuation. Please wait for setup to be completed";
                        //updateProgresslabel(lblAddDatabase);
                        updateProgresslabel(lblFMInstall);
                        break;
                    }
                case 2:               
                    {
                        rtbText = "Installation progress in continuation. System will be restarted";
                        updateProgresslabel(lblRemoveEntries);
                        break;
                    }               
            }
            this.Update();
        }

        private void updateProgresslabel(Label label)
        {
            float fontSize = 8.25F;
            lblDrvUpdate.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            lblRemoveEntries.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            lblFMInstall.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);

            label.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnExit.Text == "Exit" && MessageBox.Show("Do you really wish to exit Installation", "Exit Installation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Startup.RemoveInstallerFromStartup(Common.appName);
                SequenceChecker.RemoveSequence();
            }
            if (btnExit.Text == "Finish")
            {
                InstallApplications installApplications = new InstallApplications();
                installApplications.RestartSystem();
            }
            Application.Exit();
        }
    }
}
