using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Wave
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			string processName = Process.GetCurrentProcess().ProcessName;
			if (Process.GetProcessesByName(processName).Length > 1)
			{
				MessageBox.Show("Não é possível abrir duas instâncias deste programa. (" + processName + ")", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Application.Exit();
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmSplash());
		}
	}
}
