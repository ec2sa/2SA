using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.ServiceProcess;
using Scans2SAServiceManager.Properties;
using System.Drawing.Imaging;

namespace Scans2SAServiceManager
{
    public partial class MainForm : Form
    {
        private string serviceName = "RemoteScans";
        private string serviceExecutableName = "RemoteScansService.exe";

        public MainForm()
        {
            InitializeComponent();
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //this.ShowInTaskbar = false;
         //   notifyIcon1.Visible = true;
         //   this.Hide();
            SynchronizeWithServiceState();
          
        }

        private void SynchronizeWithServiceState()
        {
            if (ServicesManager.IsServiceInstalled(serviceName))
            {
                zainstalujToolStripMenuItem.Enabled = false;
                odinstalujToolStripMenuItem.Enabled = true;

                if (ServicesManager.GetServiceState(serviceName) != ServicesManager.ServiceState.Stopped)
                {
                    startToolStripMenuItem.Enabled = false;
                    stopToolStripMenuItem1.Enabled = true;
                    notifyIcon1.Icon = MakeIcon(Properties.Resources.serviceOK, 32, true);
                }
                else
                {
                    startToolStripMenuItem.Enabled = true;
                    stopToolStripMenuItem1.Enabled = false;
                    notifyIcon1.Icon = MakeIcon(Properties.Resources.serviceWarn, 32, true);
                }

            }
            else
            {
                zainstalujToolStripMenuItem.Enabled = true;
                odinstalujToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem1.Enabled = false;
                notifyIcon1.Icon = MakeIcon(Properties.Resources.serviceError, 32, true);
            }  
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        private void koniecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Zakończyć działanie aplikacji?","Pytanie",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            this.ShowInTaskbar = false;
            Properties.Settings.Default.Save();
        }

        private void konfiguracjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Show();
            notifyIcon1.Visible = false;
        }

        private void zainstalujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string serviceExecutablePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),serviceExecutableName);

            ServicesManager.ReturnValue rv= ServicesManager.InstallService(serviceName, "Usługa zdalnych skanów aplikacji 2SA", serviceExecutablePath, ServicesManager.ServiceType.OwnProcess, ServicesManager.OnError.UserIsNotified, ServicesManager.StartMode.Automatic, false, "LocalSystem", null, null, null, null);

            if (rv != ServicesManager.ReturnValue.Success)
            {
                MessageBox.Show("Nie udało się zainstalować usługi (błąd: "+rv.ToString()+")", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Usługa została zainstalowana poprawnie", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SynchronizeWithServiceState();  
         
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] startParams = new string[5];
            startParams[0] = Settings.Default.WatchDirectory;
            startParams[1] = Settings.Default.BackupDirectory;
            startParams[2] = Settings.Default.ServiceUrl;
            startParams[3] = Settings.Default.Username;
            startParams[4] = Settings.Default.Password;
            ServicesManager.ReturnValue rv = ServicesManager.StartService(serviceName, startParams);

            if(rv==ServicesManager.ReturnValue.Success)
                {
          //      MessageBox.Show("Usługa została uruchomiona poprawnie", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nie udało się uruchomić usługi (błąd: "+rv.ToString()+")", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SynchronizeWithServiceState();
        }

        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ServicesManager.ReturnValue rv = ServicesManager.StopService(serviceName);
            if (rv==ServicesManager.ReturnValue.Success)
            {
           //     MessageBox.Show("Usługa została zatrzymana poprawnie", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nie udało się zatrzymać usługi (błąd: " + rv.ToString() + ")", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SynchronizeWithServiceState();
        }

        private void konToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            SynchronizeWithServiceState();
        }

        private void odinstalujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServicesManager.ReturnValue rv = ServicesManager.UninstallService(serviceName);

            if (rv==ServicesManager.ReturnValue.Success)
            {
         //       MessageBox.Show("Usługa została odinstalowana poprawnie", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nie udało się odinstalować usługi", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SynchronizeWithServiceState();
        }

        private void btnScansDirectory_Click(object sender, EventArgs e)
        {
            GetDirectory(tbScansDirectory);
        }

        private void GetDirectory(TextBox tb)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb.Text = fbd.SelectedPath;
            }
        }

        private void btnBackupDirectory_Click(object sender, EventArgs e)
        {
            GetDirectory(tbBackupDirectory);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            konfiguracjaToolStripMenuItem_Click(this, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            this.ShowInTaskbar = false;
            Properties.Settings.Default.Reload();
        }


        /// <summary>
        /// Converts an image into an icon.
        /// </summary>
        /// <param name="img">The image that shall become an icon</param>
        /// <param name="size">The width and height of the icon. Standard
        /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
        /// <param name="keepAspectRatio">Whether the image should be squashed into a
        /// square or whether whitespace should be put around it.</param>
        /// <returns>An icon!!</returns>
        private Icon MakeIcon(Image img, int size, bool keepAspectRatio)
        {
            Bitmap square = new Bitmap(size, size); // create new bitmap
            Graphics g = Graphics.FromImage(square); // allow drawing to it

            int x, y, w, h; // dimensions for new image

            if (!keepAspectRatio || img.Height == img.Width)
            {
                // just fill the square
                x = y = 0; // set x and y to 0
                w = h = size; // set width and height to size
            }
            else
            {
                // work out the aspect ratio
                float r = (float)img.Width / (float)img.Height;

                // set dimensions accordingly to fit inside size^2 square
                if (r > 1)
                { // w is bigger, so divide h by r
                    w = size;
                    h = (int)((float)size / r);
                    x = 0;
                    y = (size - h) / 2; // center the image
                }
                else
                { // h is bigger, so multiply w by r
                    w = (int)((float)size * r);
                    h = size;
                    y = 0;
                    x = (size - w) / 2; // center the image
                }
            }

            // make the image shrink nicely by using HighQualityBicubic mode
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
            g.Flush(); // make sure all drawing operations complete before we get the icon

            // following line would work directly on any image, but then
            // it wouldn't look as nice.
            return Icon.FromHandle(square.GetHicon());
        }

    }
}
