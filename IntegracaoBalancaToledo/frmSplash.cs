using IntegracaoBalancaToledo.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace IntegracaoBalancaToledo
{
	public class frmSplash : Form
	{
		private IContainer components;

		private Label label1;

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
			Thread.Sleep(2000);
			base.Close();
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
			this.label1 = new Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.Transparent;
			this.label1.Font = new Font("Arial", 13f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label1.ForeColor = Color.White;
			this.label1.ImageAlign = ContentAlignment.MiddleRight;
			this.label1.Location = new Point(229, 201);
			this.label1.Name = "label1";
			this.label1.Size = new Size(187, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Integração Automação";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackgroundImage = Resources.splash;
			base.ClientSize = new Size(645, 365);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "frmSplash";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "frmSplash";
			base.Activated += new EventHandler(this.frmSplash_Activated);
			base.Load += new EventHandler(this.frmSplash_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
