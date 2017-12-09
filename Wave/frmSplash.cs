using Wave.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Wave
{
    public class frmSplash : Form
    {
        private IContainer components;
        private PictureBox pictureBox1;

        public frmSplash()
        {

            this.InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {

        }

        private void frmSplash_Activated(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (System.IO.File.Exists(@"c:\wave\licence.txt"))
            {
                string licence = System.IO.File.ReadAllText(@"c:\wave\licence.txt");
                
                int a = int.Parse(licence.Substring(6, 6));
                int b = int.Parse(DateTime.Now.ToString("yyMMdd"));
                int c = int.Parse(licence.Substring(22, 6));
                if (!(a < b && b < c))
                {
                    MessageBox.Show("Licença Expirada");
                    Application.Exit();
                }
            }
            else
            {


                MessageBox.Show("Licença Expirada");
                Application.Exit();

            }
            Thread.Sleep(2000);
            new frmWaveAutomatic().Show();

            this.Hide();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Properties.Resources.TELA_LOADING_WAVE;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(647, 366);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(645, 365);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSplash";
            this.Activated += new System.EventHandler(this.frmSplash_Activated);
            this.Load += new System.EventHandler(this.frmSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

    }

}
