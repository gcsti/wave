using AxMSWinsockLib;
using Wave.Properties;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Wave
{
	public class frmWaveAutomatic : Form
	{
		private bool fecharTudo;

		private IContainer components;

		private RichTextBox rchLog;

		private Label label3;

		private TextBox txtDestinoArquivos;

		private CheckBox chkLogLeituras;

		private StatusStrip statusStrip1;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private System.Windows.Forms.Timer timerStatusCnn;

		private Label label5;

		private TextBox txtMaskArquivoLeitura;

		private CheckBox chkExibirErroCnn;

		private CheckBox chkLogLeiturasConsolidada;

		private TabControl tabControl1;

		private TabPage tabPageLog;

		private Button btnLimpar;

		private Button btnSalvarLog;

		private SaveFileDialog saveFileDialog1;

		private CheckBox chkLogLeiturasConsolidadaResumida;

		private Button btnReamarLeituras;

		private CheckBox chkExibirDestinoPLC4;

		private CheckBox chkExibirErroDestCx;

		private Label lblFilaScanner;

		private Label lblFilaDim;

		private Label lblFilaBalanca;

		private System.Windows.Forms.Timer timerFilaLeituras;

		private TabPage tabPageSorter;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;

        private int Saida01;
        private int Saida02;
        private int Saida03;
        private int Saida04;
        private int Saida05;
        private int Saida99;
        private Panel pnlSorter;
        private CheckBox chkDesconectarSeFalhaNaFila;
        private ComboBox cboSaidaAnvisa;
        private Label label13;
        private ComboBox cboSaidaRejeito;
        private Label label12;
        private Button btnPcaDelS5;
        private Button btnPcaAddS5;
        private Label label11;
        private ListBox lstPcaSaida5;
        private TextBox txtPcaS5;
        private ListBox lstPcaSaida1;
        private Button btnPcaDelS4;
        private Button btnPcaAddS4;
        private Label label10;
        private ListBox lstPcaSaida4;
        private TextBox txtPcaS4;
        private Button btnPcaDelS3;
        private Button btnPcaAddS3;
        private Label label9;
        private ListBox lstPcaSaida3;
        private TextBox txtPcaS3;
        private Button btnPcaDelS2;
        private Button btnPcaAddS2;
        private Label label8;
        private ListBox lstPcaSaida2;
        private TextBox txtPcaS2;
        private Button btnPcaDelS1;
        private Button btnPcaAddS1;
        private Label label7;
        private TextBox txtPcaS1;
        private Button btnRearmarPLC;
        private Button btnRearmarScanner;
        private Button btnRearmarPortal;
        private Button btnRearmarBalanca;
        private TextBox txtIpBalanca;
        private TextBox txtPortaBalanca;
        private Label label2;
        private AxWinsock SocketScanner;
        private Label lblTitulo;
        private TextBox txtIpScanner;
        private TextBox txtPortaPLC;
        private AxWinsock SocketBalanca;
        private Button btnDisconnect;
        private TextBox txtPortaScanner;
        private Button btnConnect;
        private TextBox txtIPplc;
        private TextBox txtPortaPortal;
        private AxWinsock SocketPortal;
        private Label label4;
        private TextBox txtIpPortal;
        private Label label1;
        private AxWinsock SocketPLC;
        private int SaidaTotal;

        /// <summary>
        /// 
        /// </summary>
		public string DESTINO_REJEITO
		{
			get
			{
				if (string.IsNullOrEmpty(this.cboSaidaRejeito.Text))
				{
					this.cboSaidaRejeito.Text = "01";
				}
				return this.cboSaidaRejeito.Text;
			}
			set
			{
				this.cboSaidaRejeito.Text = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string DESTINO_ANVISA
		{
			get
			{
				if (string.IsNullOrEmpty(this.cboSaidaAnvisa.Text))
				{
					this.cboSaidaAnvisa.Text = "03";
				}
				return this.cboSaidaAnvisa.Text;
			}
			set
			{
				this.cboSaidaAnvisa.Text = value;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string ArquivoConfiguracoes
		{
			get
			{
				return Application.ExecutablePath.Replace(".EXE", "") + ".Config.xml";
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string ArquivoErros
		{
			get
			{
				return Application.ExecutablePath.Replace(".EXE", "") + ".Erros.txt";
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public string ArquivoLeituras
		{
			get
			{
				string text = this.txtMaskArquivoLeitura.Text;
				text = text.Replace("{data}", DateTime.Now.ToString("ddMMyyyy"));
				text = text.Replace("{hora}", DateTime.Now.ToString("HH"));
				text = text.Replace("{minuto}", DateTime.Now.ToString("mm"));
				text = text.Replace("{segundo}", DateTime.Now.ToString("ss"));
				return (this.txtDestinoArquivos.Text + "\\" + text).Replace("\\", "\\");
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public frmWaveAutomatic()
		{
			this.InitializeComponent();
			this.CarregarArquivoConfiguracoes();
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			ToolStripStatusLabel expr_29 = this.toolStripStatusLabel1;
			expr_29.Text = expr_29.Text + " Ver.: " + string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnConnect_Click(object sender, EventArgs e)
		{
			this.GravarArquivoConfig();
			this.rchLog.AppendText("Arquivo de configuração atualizado em " + RichTextBoxExtension.GetDateTime() + ".\r");
			if (!Directory.Exists(this.txtDestinoArquivos.Text))
			{
				Directory.CreateDirectory(this.txtDestinoArquivos.Text);
				this.rchLog.AppendText("Pasta de destino das leituras atualizada em " + RichTextBoxExtension.GetDateTime() + ".\r");
			}
			this.rchLog.AppendText(RichTextBoxExtension.GetDateTime() + " --> Iniciando conexão com dispositivos...\r");
			Leitura.Limpar();
			this.btnRearmarBalanca_Click(null, null);
			this.btnRearmarPortal_Click(null, null);
			this.btnRearmarScanner_Click(null, null);
			this.btnRearmarPLC_Click(null, null);
			this.timerStatusCnn_Tick(null, null);
			this.txtIpBalanca.ReadOnly = true;
			this.txtPortaBalanca.ReadOnly = true;
			this.txtPortaPortal.ReadOnly = true;
			this.txtIpPortal.ReadOnly = true;
			this.txtIpScanner.ReadOnly = true;
			this.txtPortaScanner.ReadOnly = true;
			this.txtIPplc.ReadOnly = true;
			this.txtPortaPLC.ReadOnly = true;
			this.txtDestinoArquivos.ReadOnly = true;
			this.txtMaskArquivoLeitura.ReadOnly = true;
			this.btnConnect.Enabled = false;
			this.btnDisconnect.Enabled = true;
			this.btnRearmarBalanca.Enabled = true;
			this.btnRearmarPortal.Enabled = true;
			this.btnRearmarScanner.Enabled = true;
			this.btnRearmarPLC.Enabled = true;
			this.btnReamarLeituras.Enabled = true;
			//this.pnlSorter.Enabled = false;
			this.timerStatusCnn.Enabled = true;
			this.timerStatusCnn.Start();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			this.timerStatusCnn.Stop();
			this.timerStatusCnn.Enabled = false;
			Leitura.Limpar();
			this.rchLog.AppendText("Buffer de leituras excluído.\r");
			this.SocketBalanca.Close();
			this.SocketPortal.Close();
			this.SocketScanner.Close();
			this.SocketPLC.Close();
			this.rchLog.AppendText(RichTextBoxExtension.GetDateTime() + " --> Todos os dispositivos desconectados.\r");
			this.txtIpBalanca.ReadOnly = false;
			this.txtPortaBalanca.ReadOnly = false;
			this.txtPortaPortal.ReadOnly = false;
			this.txtIpPortal.ReadOnly = false;
			this.txtIpScanner.ReadOnly = false;
			this.txtPortaScanner.ReadOnly = false;
			this.txtIPplc.ReadOnly = false;
			this.txtPortaPLC.ReadOnly = false;
			this.txtDestinoArquivos.ReadOnly = false;
			this.txtMaskArquivoLeitura.ReadOnly = false;
			this.btnConnect.Enabled = true;
			this.btnDisconnect.Enabled = false;
			this.btnRearmarBalanca.Enabled = false;
			this.btnRearmarPortal.Enabled = false;
			this.btnRearmarScanner.Enabled = false;
			this.btnRearmarPLC.Enabled = false;
			this.btnReamarLeituras.Enabled = false;
			this.pnlSorter.Enabled = true;
			this.setBgColor(this.txtIpBalanca, Color.White);
			this.setBgColor(this.txtIpPortal, Color.White);
			this.setBgColor(this.txtIpScanner, Color.White);
			this.setBgColor(this.txtIPplc, Color.White);
			GC.Collect();
			this.rchLog.AppendText(RichTextBoxExtension.GetDateTime() + " --> Todos os recursos liberados.\r");
			Application.DoEvents();
            Saida01 = 0;
            Saida02 = 0;
            Saida03 = 0;
            Saida04 = 0;
            Saida05 = 0;
            Saida99 = 0;
            SaidaTotal = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnFechar_Click(object sender, EventArgs e)
		{
			this.btnDisconnect_Click(null, null);
			Application.DoEvents();
			GC.Collect();
			Application.Exit();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void timerStatusCnn_Tick(object sender, EventArgs e)
		{
			this.statusConexaoDispositivos();
			if (this.btnDisconnect.Enabled)
			{
				if (this.SocketBalanca.CtlState != SocketConsts.sckConnected)
				{
					this.btnRearmarBalanca_Click(null, null);
				}
				if (this.SocketPortal.CtlState != SocketConsts.sckConnected)
				{
					this.btnRearmarPortal_Click(null, null);
				}
				if (this.SocketScanner.CtlState != SocketConsts.sckConnected)
				{
					this.btnRearmarScanner_Click(null, null);
				}
				if (this.SocketPLC.CtlState != SocketConsts.sckConnected)
				{
					this.btnRearmarPLC_Click(null, null);
				}
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void timerFilaLeituras_Tick(object sender, EventArgs e)
		{
			this.exibirStatusFila();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void frmToledo_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.fecharTudo)
			{
				return;
			}
			e.Cancel = true;
			try
			{
				if (Interaction.InputBox("Digite a senha de administrador para fechar:", "Segurança", "****", -1, -1) == "SOLIDEZ")
				{
					this.fecharTudo = true;
					base.Close();
					Application.DoEvents();
					GC.Collect();
					Application.Exit();
				}
			}
			catch (Exception ex)
			{
				this.rchLog.AppendError(ex.ToString(), "");
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnRearmarBalanca_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketBalanca.Close();
				this.SocketBalanca.Protocol = SocketConsts.sckClosed;
                if (this.pingDispositivo(this.txtIpBalanca.Text))
				{
					this.SocketBalanca.RemoteHost = (this.txtIpBalanca.Text);
					this.SocketBalanca.RemotePort= (this.txtPortaBalanca.Text.ToInt32());
					this.SocketBalanca.Connect(this.txtIpBalanca.Text, this.txtPortaBalanca.Text.ToInt32());
				}
				this.SocketBalanca.MakeDirty();
			}
			catch (Exception ex)
			{
				this.GravarArquivoErro(ex.ToString());
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnRearmarPortal_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketPortal.Close();
				this.SocketPortal.Protocol = SocketConsts.sckClosed;
                if (this.pingDispositivo(this.txtIpPortal.Text))
				{
					this.SocketPortal.RemoteHost = (this.txtIpPortal.Text);
					this.SocketPortal.RemotePort = (this.txtPortaPortal.Text.ToInt32());
					this.SocketPortal.Connect(this.txtIpPortal.Text, this.txtPortaPortal.Text.ToInt32());
				}
				this.SocketPortal.MakeDirty();
			}
			catch (Exception ex)
			{
				this.GravarArquivoErro(ex.ToString());
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnRearmarScanner_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketScanner.Close();
				this.SocketScanner.Protocol = SocketConsts.sckClosed;
				if (this.pingDispositivo(this.txtIpScanner.Text))
				{
					this.SocketScanner.RemoteHost = (this.txtIpScanner.Text);
					this.SocketScanner.RemotePort = (this.txtPortaScanner.Text.ToInt32());
					this.SocketScanner.Connect(this.txtIpScanner.Text, this.txtPortaScanner.Text.ToInt32());
				}
				this.SocketScanner.MakeDirty();
			}
			catch (Exception ex)
			{
				this.GravarArquivoErro(ex.ToString());
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnRearmarPLC_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketPLC.Close();
				this.SocketPLC.Protocol = SocketConsts.sckClosed;
				if (this.pingDispositivo(this.txtIPplc.Text))
				{
					this.SocketPLC.RemoteHost = (this.txtIPplc.Text);
					this.SocketPLC.RemotePort = (this.txtPortaPLC.Text.ToInt32());
					this.SocketPLC.Connect(this.txtIPplc.Text, this.txtPortaPLC.Text.ToInt32());
				}
				this.SocketPLC.MakeDirty();
			}
			catch (Exception ex)
			{
				this.GravarArquivoErro(ex.ToString());
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnReamarLeituras_Click(object sender, EventArgs e)
		{
			Leitura.Limpar();
            this.exibirStatusFila();
            this.rchLog.AppendText("Recursos de memória de leitura redefinidos.\r");
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnLimpar_Click(object sender, EventArgs e)
		{
			this.rchLog.Clear();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnSalvarLog_Click(object sender, EventArgs e)
		{
			this.saveFileDialog1.Title = "Salvar arquivo de log";
			this.saveFileDialog1.FileName = "Solidez_Wave_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".log";
			this.saveFileDialog1.Filter = "Arquivo de Log (.log)|*.log|Arquivo de Texto (.txt)|*.txt";
			this.saveFileDialog1.ShowDialog();
			if (!string.IsNullOrEmpty(this.saveFileDialog1.FileName.Trim()))
			{
				this.rchLog.AppendLog("Arquivo de log salvo em " + this.saveFileDialog1.FileName, Color.DarkGreen, this.rchLog.Font);
				this.rchLog.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void chkLogLeituras_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as CheckBox).Name == "chkLogLeiturasConsolidada")
			{
				this.chkLogLeiturasConsolidadaResumida.Checked = !this.chkLogLeiturasConsolidada.Checked;
				return;
			}
			this.chkLogLeiturasConsolidada.Checked = !this.chkLogLeiturasConsolidadaResumida.Checked;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Socket_ConnectEvent(object sender, EventArgs e)
		{
			if (!Directory.Exists(this.txtDestinoArquivos.Text))
			{
				Directory.CreateDirectory(this.txtDestinoArquivos.Text);
			}
			AxWinsock axWinsock = sender as AxWinsock;
			this.rchLog.AppendText(string.Concat(new string[]
			{
				"\tConectado em '",
				axWinsock.Tag.ToString(),
				"' ",
				axWinsock.RemoteHostIP,
				":",
				axWinsock.RemotePort.ToString(),
				"\r"
			}), Color.Gray);
			this.statusConexaoDispositivos();
			if (this.SocketBalanca.CtlState == 7 & this.SocketPortal.CtlState == 7 & this.SocketScanner.CtlState == 7 & this.SocketPLC.CtlState == 7)
			{
				this.rchLog.AppendText("Dispositivos conectados com sucesso!\r");
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Socket_CloseEvent(object sender, EventArgs e)
		{
			AxWinsock axWinsock = sender as AxWinsock;
			this.rchLog.AppendText(string.Concat(new string[]
			{
				"Conexão encerrada de '",
				axWinsock.Tag.ToString(),
				"' ",
				axWinsock.RemoteHostIP,
				":",
				axWinsock.RemotePort.ToString(),
				"\r"
			}), Color.Gray);
			this.statusConexaoDispositivos();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Socket_DataArrival(object sender, DMSWinsockControlEvents_DataArrivalEvent e)
		{
			AxWinsock socket = sender as AxWinsock;
		//	int num = 20170901;
		//	if (Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) > num)
		//	{
		//		Thread.Sleep(1000);
		//	}
			ReceivingEventArgs receivingEventArgs = this.obterTelegramaSocket(socket);
			string dispositivo = receivingEventArgs.Dispositivo;
			if (!(dispositivo == "Balança"))
			{
				if (!(dispositivo == "Portal"))
				{
					if (dispositivo == "Scanner")
					{
						Leitura.Scanner.Add(receivingEventArgs);
						if (this.chkLogLeituras.Checked)
						{
							this.rchLog.AppendLog("\rSCANNER " + receivingEventArgs.ToString() + "\r", Color.Green, this.rchLog.Font);
						}
						if (receivingEventArgs.FalhaDeLeitura)
						{
							this.rchLog.AppendLog("\r" + RichTextBoxExtension.GetDateTime() + " -- SCANNER INFORMOU UM 'NOREAD'.\r", Color.Red, Color.Silver, this.rchLog.Font);
						}
					}
				}
				else
				{
					Leitura.Portal.Add(receivingEventArgs);
					if (this.chkLogLeituras.Checked)
					{
						this.rchLog.AppendLog("\rDIMENSIONADOR: " + receivingEventArgs.ToString() + "\r", Color.Blue, this.rchLog.Font);
					}
					if (receivingEventArgs.FalhaDeLeitura)
					{
						this.rchLog.AppendLog(string.Concat(new string[]
						{
							"\r",
							RichTextBoxExtension.GetDateTime(),
							" -- DIMENSIONADOR INFORMOU UM ERRO (",
							receivingEventArgs.DataString,
							").\r"
						}), Color.Red, Color.Silver, this.rchLog.Font);
					}
				}
			}
			else
			{
				Leitura.Balanca.Add(receivingEventArgs);
				if (this.chkLogLeituras.Checked)
				{
					this.rchLog.AppendLog("\rBALANÇA: " + receivingEventArgs.ToString() + "\r", Color.DarkOrange, this.rchLog.Font);
				}
				if (receivingEventArgs.FalhaDeLeitura)
				{
					this.rchLog.AppendLog(string.Concat(new string[]
					{
						"\r",
						RichTextBoxExtension.GetDateTime(),
						" -- ERRO NA BALANÇA (",
						receivingEventArgs.DataString,
						").\r"
					}), Color.Red, Color.Silver, this.rchLog.Font);
				}
			}
			this.exibirStatusFila();
			string destino = "";
			if (receivingEventArgs.FalhaDeLeitura)
			{
				destino = this.DESTINO_REJEITO;
			}
			string text = Leitura.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				receivingEventArgs.Dispositivo = "Scanner\\Balança\\Portal";
				receivingEventArgs.DataString = text;
				if (this.chkLogLeiturasConsolidada.Checked)
				{
					this.rchLog.AppendReceiveMessage("\r" + receivingEventArgs.ToString() + "\r", "Volume aferido:");
				}
				if (this.chkLogLeiturasConsolidadaResumida.Checked)
				{
					this.rchLog.AppendReceiveMessage(receivingEventArgs.DataString.ToString() + "\r", "Volume aferido:");
				}
				if (!Leitura.HouveFalhaNaLeituraConsolidada(text))
				{
					try
					{
						string text2 = text.Split(new char[]
						{
							';'
						})[0].ToLower();
						if (text2.Length >= 26)
						{
							int num2 = text2.Substring(text2.Length - 2, 2).ToInt32();
							string text3 = text2.Substring(14, 2);
							if (this.chkExibirDestinoPLC4.Checked)
							{
								this.rchLog.AppendSendMessage("Praça de Destino: " + text3 + "\r", "");
							}
							if (num2 != 0)
							{
								if (this.chkExibirDestinoPLC4.Checked)
								{
									this.rchLog.AppendSendMessage("Anvisa: " + num2.ToString() + "\r", "");
								}
								destino = this.DESTINO_ANVISA;
							}
							else
							{
								destino = Sorter.ObterDestinoCaixa(text3);
							}
						}
						else if (this.chkExibirErroDestCx.Checked)
						{
							this.rchLog.AppendLog("\r" + RichTextBoxExtension.GetDateTime() + " ** O código de barras está fora do padrão. Esperado código de barras com 26 dígitos.\r", Color.Red, Color.Silver, this.rchLog.Font);
						}
						goto IL_41D;
					}
					catch
					{
						destino = this.DESTINO_REJEITO;
						goto IL_41D;
					}
				}
				destino = this.DESTINO_REJEITO;
				IL_41D:
				if (Leitura.MultiplasLeiturasScanner)
				{
					destino = this.DESTINO_REJEITO;
				}
				this.informarDestinoCaixaParaPLC(destino);
				if (text.Split(new char[]
				{
					';'
				}).Length > 4)
				{
					this.GravarArquivoLeitura(text);
				}
				if (Leitura.HouveFalhaNaFila())
				{
					this.rchLog.AppendError("\r**************************\r  ATENÇÃO: \r  OCORREU UMA FALHA NA FILA DE LEITURAS E PARA PREVINIR PERDA DE DADOS O TRANSPORTADOR FOI DESATIVADO. \r  FAVOR, RECONECTAR TODOS OS DISPOSITIVOS NO SOFTWARE DE AUTOMAÇÃO!. \r***************************", "");
					Leitura.Limpar();
					this.informarDestinoCaixaParaPLC(this.DESTINO_REJEITO);
					this.informarDestinoCaixaParaPLC("99");
					this.btnReamarLeituras_Click(null, null);
				}
			}
			this.GravarArquivoBackupLeitura();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Socket_Error(object sender, DMSWinsockControlEvents_ErrorEvent e)
		{
			AxWinsock axWinsock = sender as AxWinsock;
			this.GravarArquivoErro("Erro no socket '" + axWinsock.Tag.ToString() + "': " + e.description, false);
			if (this.chkExibirErroCnn.Checked)
			{
				this.rchLog.AppendError(e.description, axWinsock.Tag.ToString());
			}
			this.statusConexaoDispositivos();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
		private ReceivingEventArgs obterTelegramaSocket(AxWinsock socket)
		{
			DateTime now = DateTime.Now;
			object obj = new object();
			string text = "";
			string text2 = "";
			obj = text;
			socket.GetData(ref obj);
			if (obj != null)
			{
				text = obj.ToString();
				text2 = text.Trim().ToLower();
			}
			byte[] bytes = Encoding.Default.GetBytes(text);
			bool falhaDeLeitura = false;
			if (socket.Tag.ToString() == "Balança")
			{
				if (string.IsNullOrEmpty(text2))
				{
					falhaDeLeitura = true;
				}
				if (Convert.ToDouble(TelegramaBase.RemoverCaracteresEspeciais(text2).Replace(".", ",")) == 0.0)
				{
					falhaDeLeitura = true;
				}
			}
			if (socket.Tag.ToString() == "Scanner" && text2.Contains("noread"))
			{
				falhaDeLeitura = true;
			}
			if (socket.Tag.ToString() == "Portal" && !text2.Contains("odm"))
			{
				falhaDeLeitura = true;
			}
			return new ReceivingEventArgs
			{
				DateTimeReceived = now,
				DataString = text,
				DataStringNoSpecialChars = TelegramaBase.RemoverCaracteresEspeciais(text),
				Dispositivo = socket.Tag.ToString(),
				DataBytes = bytes,
				HostName = socket.RemoteHost,
				IpAddress = socket.RemoteHostIP,
				Port = socket.RemotePort,
				FalhaDeLeitura = falhaDeLeitura
			};
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destino"></param>
		private void informarDestinoCaixaParaPLC(string destino)
		{

           // System.IO.File.WriteAllText(@"C:\Logstore\AVISAPLCINICIO.txt", DateTime.Now.ToString());


            if (!string.IsNullOrEmpty(destino))
			{
				try
				{
					this.SocketPLC.SendData(destino);
                    this.exibirStatusFila();
                    //  System.IO.File.WriteAllText(@"C:\Logstore\AVISAPLCFIM.txt", DateTime.Now.ToString());




                    if (this.chkExibirDestinoPLC4.Checked)
					{
						this.rchLog.AppendSendMessage("Saída do Sorter: " + destino + "\r", "");

                        try
                        {

                            switch (destino)
                            {
                                case "01":
                                    Saida01++;
                                    break;

                                case "02":
                                    Saida02++;
                                    break;

                                case "03":
                                    Saida03++;
                                    break;

                                case "04":
                                    Saida04++;
                                    break;

                                case "05":
                                    Saida05++;
                                    break;


                                default:
                                    Saida99++;
                                    break;
                            }

                            SaidaTotal++;

                            System.IO.File.WriteAllText(@"C:\comum\ftp\Saida.txt", string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",Saida01,Saida02,Saida03,Saida04,Saida05,Saida99,SaidaTotal));

                        }
                        catch
                        {

                        }

                    }
				}
				catch (Exception ex)
				{
					this.GravarArquivoErro(ex.ToString());
				}
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
		private void GravarArquivoLeitura(string log)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(this.ArquivoLeituras, true))
				{
					streamWriter.WriteLine(log);
				}
			}
			catch (Exception ex)
			{
				this.GravarArquivoErro(ex.ToString());
			}
		}
        /// <summary>
        /// 
        /// </summary>
		private void GravarArquivoBackupLeitura()
		{
			try
			{
				this.rchLog.SaveFile(Application.ExecutablePath + "." + DateTime.Now.ToString("yyyyMMdd") + ".log", RichTextBoxStreamType.PlainText);
				this.rchLog.SaveFile(Application.ExecutablePath + "." + DateTime.Now.ToString("yyyyMMdd") + ".rtf", RichTextBoxStreamType.RichText);
			}
			catch (Exception ex)
			{
				this.GravarArquivoErro(ex.ToString());
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
		private void GravarArquivoErro(string log)
		{
			this.GravarArquivoErro(log, true);
		}

		private void GravarArquivoErro(string log, bool exibirLog)
		{
			try
			{
				if (exibirLog)
				{
					this.rchLog.AppendError(log, "");
				}
				using (StreamWriter streamWriter = new StreamWriter(this.ArquivoErros, true))
				{
					streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " --> " + log);
				}
			}
			catch (Exception arg_52_0)
			{
				MessageBox.Show(arg_52_0.ToString(), "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public bool CarregarArquivoConfiguracoes()
		{
			bool result;
			try
			{
				using (XmlReader xmlReader = XmlReader.Create(this.ArquivoConfiguracoes))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "IpBalanca")
						{
							this.txtIpBalanca.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "PortaBalanca")
						{
							this.txtPortaBalanca.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "IpPortal")
						{
							this.txtIpPortal.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "PortaPortal")
						{
							this.txtPortaPortal.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "IpScanner")
						{
							this.txtIpScanner.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "PortaScanner")
						{
							this.txtPortaScanner.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "IpPLC")
						{
							this.txtIPplc.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "PortaPLC")
						{
							this.txtPortaPLC.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "CaminhoArquivoLeituras")
						{
							this.txtDestinoArquivos.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "MascaraArquivoLeituras")
						{
							this.txtMaskArquivoLeitura.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "RegistrarLogLeituras")
						{
							this.chkLogLeituras.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "RegistrarLogLeituraConsolidada")
						{
							this.chkLogLeiturasConsolidada.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "RegistrarLogLeituraConsolidadaResumida")
						{
							this.chkLogLeiturasConsolidadaResumida.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "ExibirErroConexao")
						{
							this.chkExibirErroCnn.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "ExibirLogDestinoCaixaPLC")
						{
							this.chkExibirDestinoPLC4.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "ExibirErroDestinoCaixaPLC")
						{
							this.chkExibirErroDestCx.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "PracasPorSaida")
						{
							Sorter.ComporRegras(xmlReader.ReadElementContentAsString().ToLower());
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "DesconectarSeFalhaNaFila")
						{
							this.chkDesconectarSeFalhaNaFila.Checked = xmlReader.ReadElementContentAsString().ToLower().Contains("true");
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "RejeitoSaida")
						{
							this.cboSaidaRejeito.Text = xmlReader.ReadElementContentAsString();
						}
						if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "AnvisaSaida")
						{
							this.cboSaidaAnvisa.Text = xmlReader.ReadElementContentAsString();
						}
					}
				}
				if (Sorter.PracasPorSaida.Count == 0)
				{
					Sorter.Default();
				}
				foreach (SorterSaida current in Sorter.PracasPorSaida)
				{
					string key = "lstPcaSaida" + current.Saida.ToInt32().ToString();
					(base.Controls.Find(key, true).FirstOrDefault<Control>() as ListBox).Items.Add(current.Praca);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
        /// <summary>
        /// 
        /// </summary>
		private void GravarArquivoConfig()
		{
			if (this.cboSaidaAnvisa.Text == this.cboSaidaRejeito.Text)
			{
				MessageBox.Show("Saídas Anvisa e Rejeito não podem ser iguais.", "Atenção");
				return;
			}
			this.DESTINO_ANVISA = this.cboSaidaAnvisa.Text;
			this.DESTINO_REJEITO = this.cboSaidaRejeito.Text;
			if (File.Exists(this.ArquivoConfiguracoes))
			{
				File.Delete(this.ArquivoConfiguracoes);
			}
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(this.ArquivoConfiguracoes, Encoding.UTF8))
			{
				xmlTextWriter.WriteStartDocument(true);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.Indentation = 2;
				xmlTextWriter.WriteStartElement("Configuracoes");
				xmlTextWriter.WriteStartElement("IpBalanca");
				xmlTextWriter.WriteString(this.txtIpBalanca.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PortaBalanca");
				xmlTextWriter.WriteString(this.txtPortaBalanca.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("IpPortal");
				xmlTextWriter.WriteString(this.txtIpPortal.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PortaPortal");
				xmlTextWriter.WriteString(this.txtPortaPortal.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("IpScanner");
				xmlTextWriter.WriteString(this.txtIpScanner.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PortaScanner");
				xmlTextWriter.WriteString(this.txtPortaScanner.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("IpPLC");
				xmlTextWriter.WriteString(this.txtIPplc.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PortaPLC");
				xmlTextWriter.WriteString(this.txtPortaPLC.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("CaminhoArquivoLeituras");
				xmlTextWriter.WriteString(this.txtDestinoArquivos.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("MascaraArquivoLeituras");
				xmlTextWriter.WriteString(this.txtMaskArquivoLeitura.Text);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("RegistrarLogLeituras");
				xmlTextWriter.WriteString(this.chkLogLeituras.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("RegistrarLogLeituraConsolidada");
				xmlTextWriter.WriteString(this.chkLogLeiturasConsolidada.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("RegistrarLogLeituraConsolidadaResumida");
				xmlTextWriter.WriteString(this.chkLogLeiturasConsolidadaResumida.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("ExibirLogDestinoCaixaPLC");
				xmlTextWriter.WriteString(this.chkExibirDestinoPLC4.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("ExibirErroDestinoCaixaPLC");
				xmlTextWriter.WriteString(this.chkExibirErroDestCx.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("ExibirErroConexao");
				xmlTextWriter.WriteString(this.chkExibirErroCnn.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("PracasPorSaida");
				xmlTextWriter.WriteString(Sorter.ObterRegras());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("RejeitoSaida");
				xmlTextWriter.WriteString(this.cboSaidaRejeito.Text.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("AnvisaSaida");
				xmlTextWriter.WriteString(this.cboSaidaAnvisa.Text.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("DesconectarSeFalhaNaFila");
				xmlTextWriter.WriteString(this.chkDesconectarSeFalhaNaFila.Checked.ToString());
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.Close();
			}
		}
        /// <summary>
        /// 
        /// </summary>
		private void statusConexaoDispositivos()
		{
			if (this.SocketBalanca.CtlState == SocketConsts.sckConnected)
			{
				this.setBgColor(this.txtIpBalanca, Color.Green);
			}
			else
			{
				this.setBgColor(this.txtIpBalanca, Color.Red);
			}
			if (this.SocketPortal.CtlState == SocketConsts.sckConnected)
			{
				this.setBgColor(this.txtIpPortal, Color.Green);
			}
			else
			{
				this.setBgColor(this.txtIpPortal, Color.Red);
			}
			if (this.SocketScanner.CtlState == SocketConsts.sckConnected)
			{
				this.setBgColor(this.txtIpScanner, Color.Green);
			}
			else
			{
				this.setBgColor(this.txtIpScanner, Color.Red);
			}
			if (this.SocketPLC.CtlState == 7)
			{
				this.setBgColor(this.txtIPplc, Color.Green);
				return;
			}
			this.setBgColor(this.txtIPplc, Color.Red);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
		public bool pingDispositivo(string ip)
		{
			bool arg_2F_0 = new Ping().Send(ip, 10).Status.ToString().Equals("Success");
			Application.DoEvents();
			return arg_2F_0;
		}

		private void setBgColor(TextBox txt, Color bgColor)
		{
			txt.BackColor = bgColor;
			if (bgColor != Color.White)
			{
				txt.ForeColor = Color.White;
				return;
			}
			txt.ForeColor = Color.Black;
		}

		private void exibirStatusFila()
		{
			if (this.btnDisconnect.Enabled)
			{
				this.lblFilaBalanca.Text = string.Format("B[{0}]", Leitura.Balanca.Count.ToString());
				this.lblFilaDim.Text = string.Format("D[{0}]", Leitura.Portal.Count.ToString());
				this.lblFilaScanner.Text = string.Format("S[{0}]", Leitura.Scanner.Count.ToString());
			}
		}

		private void btnPcaAdd_Click(object sender, EventArgs e)
		{
			string key = (sender as Control).Name.Replace("btnPcaAddS", "txtPcaS");
			TextBox textBox = base.Controls.Find(key, true).FirstOrDefault<Control>() as TextBox;
			if (!string.IsNullOrEmpty(textBox.Text))
			{
				string saida = (sender as Control).Name.Replace("btnPcaAddS", "").ToInt32().ToString("00");
				string key2 = (sender as Control).Name.Replace("btnPcaAddS", "lstPcaSaida");
				ListBox listBox = base.Controls.Find(key2, true).FirstOrDefault<Control>() as ListBox;
				string text = textBox.Text.ToInt32().ToString("00");
				if (Sorter.AddRegra(text, saida))
				{
					listBox.Items.Add(text);
					textBox.Text = "";
					return;
				}
				MessageBox.Show("Esta praça já foi associada a uma saída.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				textBox.SelectionStart = 0;
				textBox.SelectionLength = textBox.Text.Length;
				textBox.Focus();
			}
		}

		private void btnPcaDel_Click(object sender, EventArgs e)
		{
			string key = (sender as Control).Name.Replace("btnPcaDelS", "lstPcaSaida");
			ListBox listBox = base.Controls.Find(key, true).FirstOrDefault<Control>() as ListBox;
			if (listBox.Items.Count > 0 && listBox.SelectedIndex >= 0)
			{
				Sorter.DelRegra(listBox.SelectedItem.ToString());
				listBox.Items.Remove(listBox.SelectedItem);
			}
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaveAutomatic));
            this.chkExibirErroCnn = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaskArquivoLeitura = new System.Windows.Forms.TextBox();
            this.chkLogLeituras = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDestinoArquivos = new System.Windows.Forms.TextBox();
            this.rchLog = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerStatusCnn = new System.Windows.Forms.Timer(this.components);
            this.chkLogLeiturasConsolidada = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSorter = new System.Windows.Forms.TabPage();
            this.pnlSorter = new System.Windows.Forms.Panel();
            this.btnRearmarPLC = new System.Windows.Forms.Button();
            this.btnRearmarScanner = new System.Windows.Forms.Button();
            this.btnRearmarPortal = new System.Windows.Forms.Button();
            this.btnRearmarBalanca = new System.Windows.Forms.Button();
            this.txtIpBalanca = new System.Windows.Forms.TextBox();
            this.txtPortaBalanca = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SocketScanner = new AxMSWinsockLib.AxWinsock();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtIpScanner = new System.Windows.Forms.TextBox();
            this.txtPortaPLC = new System.Windows.Forms.TextBox();
            this.SocketBalanca = new AxMSWinsockLib.AxWinsock();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.txtPortaScanner = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIPplc = new System.Windows.Forms.TextBox();
            this.txtPortaPortal = new System.Windows.Forms.TextBox();
            this.SocketPortal = new AxMSWinsockLib.AxWinsock();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIpPortal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SocketPLC = new AxMSWinsockLib.AxWinsock();
            this.chkDesconectarSeFalhaNaFila = new System.Windows.Forms.CheckBox();
            this.cboSaidaAnvisa = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cboSaidaRejeito = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnPcaDelS5 = new System.Windows.Forms.Button();
            this.btnPcaAddS5 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.lstPcaSaida5 = new System.Windows.Forms.ListBox();
            this.txtPcaS5 = new System.Windows.Forms.TextBox();
            this.lstPcaSaida1 = new System.Windows.Forms.ListBox();
            this.btnPcaDelS4 = new System.Windows.Forms.Button();
            this.btnPcaAddS4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.lstPcaSaida4 = new System.Windows.Forms.ListBox();
            this.txtPcaS4 = new System.Windows.Forms.TextBox();
            this.btnPcaDelS3 = new System.Windows.Forms.Button();
            this.btnPcaAddS3 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lstPcaSaida3 = new System.Windows.Forms.ListBox();
            this.txtPcaS3 = new System.Windows.Forms.TextBox();
            this.btnPcaDelS2 = new System.Windows.Forms.Button();
            this.btnPcaAddS2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lstPcaSaida2 = new System.Windows.Forms.ListBox();
            this.txtPcaS2 = new System.Windows.Forms.TextBox();
            this.btnPcaDelS1 = new System.Windows.Forms.Button();
            this.btnPcaAddS1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPcaS1 = new System.Windows.Forms.TextBox();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.chkExibirErroDestCx = new System.Windows.Forms.CheckBox();
            this.chkExibirDestinoPLC4 = new System.Windows.Forms.CheckBox();
            this.chkLogLeiturasConsolidadaResumida = new System.Windows.Forms.CheckBox();
            this.btnSalvarLog = new System.Windows.Forms.Button();
            this.lblFilaScanner = new System.Windows.Forms.Label();
            this.lblFilaDim = new System.Windows.Forms.Label();
            this.lblFilaBalanca = new System.Windows.Forms.Label();
            this.btnReamarLeituras = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timerFilaLeituras = new System.Windows.Forms.Timer(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSorter.SuspendLayout();
            this.pnlSorter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SocketScanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SocketBalanca)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SocketPortal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SocketPLC)).BeginInit();
            this.tabPageLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // chkExibirErroCnn
            // 
            this.chkExibirErroCnn.AutoSize = true;
            this.chkExibirErroCnn.Checked = true;
            this.chkExibirErroCnn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExibirErroCnn.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExibirErroCnn.Location = new System.Drawing.Point(21, 302);
            this.chkExibirErroCnn.Name = "chkExibirErroCnn";
            this.chkExibirErroCnn.Size = new System.Drawing.Size(175, 23);
            this.chkExibirErroCnn.TabIndex = 90;
            this.chkExibirErroCnn.Text = "Exibir erro de conexão";
            this.chkExibirErroCnn.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(283, 19);
            this.label5.TabIndex = 88;
            this.label5.Text = "Máscara do nome dos  arquivos de leitura";
            // 
            // txtMaskArquivoLeitura
            // 
            this.txtMaskArquivoLeitura.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaskArquivoLeitura.Location = new System.Drawing.Point(21, 101);
            this.txtMaskArquivoLeitura.Name = "txtMaskArquivoLeitura";
            this.txtMaskArquivoLeitura.Size = new System.Drawing.Size(332, 26);
            this.txtMaskArquivoLeitura.TabIndex = 89;
            this.txtMaskArquivoLeitura.Text = "SOLIDEZ_{data}_{data}{hora}{minuto}{segundo}.txt";
            // 
            // chkLogLeituras
            // 
            this.chkLogLeituras.AutoSize = true;
            this.chkLogLeituras.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLogLeituras.Location = new System.Drawing.Point(21, 151);
            this.chkLogLeituras.Name = "chkLogLeituras";
            this.chkLogLeituras.Size = new System.Drawing.Size(216, 23);
            this.chkLogLeituras.TabIndex = 82;
            this.chkLogLeituras.Text = "Exibir leituras por dispositivo";
            this.chkLogLeituras.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(288, 19);
            this.label3.TabIndex = 80;
            this.label3.Text = "Caminho para salvar os arquivos de leitura";
            // 
            // txtDestinoArquivos
            // 
            this.txtDestinoArquivos.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestinoArquivos.Location = new System.Drawing.Point(19, 45);
            this.txtDestinoArquivos.Name = "txtDestinoArquivos";
            this.txtDestinoArquivos.Size = new System.Drawing.Size(332, 26);
            this.txtDestinoArquivos.TabIndex = 81;
            this.txtDestinoArquivos.Text = "C:\\comum\\ftp\\txt_copia ";
            // 
            // rchLog
            // 
            this.rchLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rchLog.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rchLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rchLog.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchLog.Location = new System.Drawing.Point(395, 107);
            this.rchLog.Name = "rchLog";
            this.rchLog.Size = new System.Drawing.Size(551, 526);
            this.rchLog.TabIndex = 74;
            this.rchLog.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 640);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(963, 22);
            this.statusStrip1.TabIndex = 75;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel1.Text = "Logstore ";
            // 
            // timerStatusCnn
            // 
            this.timerStatusCnn.Interval = 5000;
            this.timerStatusCnn.Tick += new System.EventHandler(this.timerStatusCnn_Tick);
            // 
            // chkLogLeiturasConsolidada
            // 
            this.chkLogLeiturasConsolidada.AutoSize = true;
            this.chkLogLeiturasConsolidada.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLogLeiturasConsolidada.Location = new System.Drawing.Point(21, 184);
            this.chkLogLeiturasConsolidada.Name = "chkLogLeiturasConsolidada";
            this.chkLogLeiturasConsolidada.Size = new System.Drawing.Size(274, 23);
            this.chkLogLeiturasConsolidada.TabIndex = 91;
            this.chkLogLeiturasConsolidada.Text = "Exibir leituras consolidadas detalhada";
            this.chkLogLeiturasConsolidada.UseVisualStyleBackColor = true;
            this.chkLogLeiturasConsolidada.CheckedChanged += new System.EventHandler(this.chkLogLeituras_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPageSorter);
            this.tabControl1.Controls.Add(this.tabPageLog);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 79);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(377, 554);
            this.tabControl1.TabIndex = 92;
            // 
            // tabPageSorter
            // 
            this.tabPageSorter.Controls.Add(this.pnlSorter);
            this.tabPageSorter.Location = new System.Drawing.Point(4, 28);
            this.tabPageSorter.Name = "tabPageSorter";
            this.tabPageSorter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSorter.Size = new System.Drawing.Size(369, 522);
            this.tabPageSorter.TabIndex = 2;
            this.tabPageSorter.Text = "Configuração do Sorter";
            this.tabPageSorter.UseVisualStyleBackColor = true;
            // 
            // pnlSorter
            // 
            this.pnlSorter.Controls.Add(this.btnRearmarPLC);
            this.pnlSorter.Controls.Add(this.btnRearmarScanner);
            this.pnlSorter.Controls.Add(this.btnRearmarPortal);
            this.pnlSorter.Controls.Add(this.btnRearmarBalanca);
            this.pnlSorter.Controls.Add(this.txtIpBalanca);
            this.pnlSorter.Controls.Add(this.txtPortaBalanca);
            this.pnlSorter.Controls.Add(this.label2);
            this.pnlSorter.Controls.Add(this.SocketScanner);
            this.pnlSorter.Controls.Add(this.lblTitulo);
            this.pnlSorter.Controls.Add(this.txtIpScanner);
            this.pnlSorter.Controls.Add(this.txtPortaPLC);
            this.pnlSorter.Controls.Add(this.SocketBalanca);
            this.pnlSorter.Controls.Add(this.btnDisconnect);
            this.pnlSorter.Controls.Add(this.txtPortaScanner);
            this.pnlSorter.Controls.Add(this.btnConnect);
            this.pnlSorter.Controls.Add(this.txtIPplc);
            this.pnlSorter.Controls.Add(this.txtPortaPortal);
            this.pnlSorter.Controls.Add(this.SocketPortal);
            this.pnlSorter.Controls.Add(this.label4);
            this.pnlSorter.Controls.Add(this.txtIpPortal);
            this.pnlSorter.Controls.Add(this.label1);
            this.pnlSorter.Controls.Add(this.SocketPLC);
            this.pnlSorter.Controls.Add(this.chkDesconectarSeFalhaNaFila);
            this.pnlSorter.Controls.Add(this.cboSaidaAnvisa);
            this.pnlSorter.Controls.Add(this.label13);
            this.pnlSorter.Controls.Add(this.cboSaidaRejeito);
            this.pnlSorter.Controls.Add(this.label12);
            this.pnlSorter.Controls.Add(this.btnPcaDelS5);
            this.pnlSorter.Controls.Add(this.btnPcaAddS5);
            this.pnlSorter.Controls.Add(this.label11);
            this.pnlSorter.Controls.Add(this.lstPcaSaida5);
            this.pnlSorter.Controls.Add(this.txtPcaS5);
            this.pnlSorter.Controls.Add(this.lstPcaSaida1);
            this.pnlSorter.Controls.Add(this.btnPcaDelS4);
            this.pnlSorter.Controls.Add(this.btnPcaAddS4);
            this.pnlSorter.Controls.Add(this.label10);
            this.pnlSorter.Controls.Add(this.lstPcaSaida4);
            this.pnlSorter.Controls.Add(this.txtPcaS4);
            this.pnlSorter.Controls.Add(this.btnPcaDelS3);
            this.pnlSorter.Controls.Add(this.btnPcaAddS3);
            this.pnlSorter.Controls.Add(this.label9);
            this.pnlSorter.Controls.Add(this.lstPcaSaida3);
            this.pnlSorter.Controls.Add(this.txtPcaS3);
            this.pnlSorter.Controls.Add(this.btnPcaDelS2);
            this.pnlSorter.Controls.Add(this.btnPcaAddS2);
            this.pnlSorter.Controls.Add(this.label8);
            this.pnlSorter.Controls.Add(this.lstPcaSaida2);
            this.pnlSorter.Controls.Add(this.txtPcaS2);
            this.pnlSorter.Controls.Add(this.btnPcaDelS1);
            this.pnlSorter.Controls.Add(this.btnPcaAddS1);
            this.pnlSorter.Controls.Add(this.label7);
            this.pnlSorter.Controls.Add(this.txtPcaS1);
            this.pnlSorter.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSorter.Location = new System.Drawing.Point(6, 10);
            this.pnlSorter.Name = "pnlSorter";
            this.pnlSorter.Size = new System.Drawing.Size(363, 520);
            this.pnlSorter.TabIndex = 0;
            // 
            // btnRearmarPLC
            // 
            this.btnRearmarPLC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRearmarPLC.Enabled = false;
            this.btnRearmarPLC.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRearmarPLC.Location = new System.Drawing.Point(199, 414);
            this.btnRearmarPLC.Name = "btnRearmarPLC";
            this.btnRearmarPLC.Size = new System.Drawing.Size(151, 28);
            this.btnRearmarPLC.TabIndex = 144;
            this.btnRearmarPLC.Text = "Conectar P";
            this.btnRearmarPLC.UseVisualStyleBackColor = false;
            this.btnRearmarPLC.Click += new System.EventHandler(this.btnRearmarPLC_Click);
            // 
            // btnRearmarScanner
            // 
            this.btnRearmarScanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRearmarScanner.Enabled = false;
            this.btnRearmarScanner.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRearmarScanner.Location = new System.Drawing.Point(10, 414);
            this.btnRearmarScanner.Name = "btnRearmarScanner";
            this.btnRearmarScanner.Size = new System.Drawing.Size(151, 28);
            this.btnRearmarScanner.TabIndex = 143;
            this.btnRearmarScanner.Text = "Conectar S";
            this.btnRearmarScanner.UseVisualStyleBackColor = false;
            this.btnRearmarScanner.Click += new System.EventHandler(this.btnRearmarScanner_Click);
            // 
            // btnRearmarPortal
            // 
            this.btnRearmarPortal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRearmarPortal.Enabled = false;
            this.btnRearmarPortal.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRearmarPortal.Location = new System.Drawing.Point(199, 333);
            this.btnRearmarPortal.Name = "btnRearmarPortal";
            this.btnRearmarPortal.Size = new System.Drawing.Size(151, 28);
            this.btnRearmarPortal.TabIndex = 142;
            this.btnRearmarPortal.Text = "Conectar D";
            this.btnRearmarPortal.UseVisualStyleBackColor = false;
            this.btnRearmarPortal.Click += new System.EventHandler(this.btnRearmarPortal_Click);
            // 
            // btnRearmarBalanca
            // 
            this.btnRearmarBalanca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRearmarBalanca.Enabled = false;
            this.btnRearmarBalanca.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRearmarBalanca.Location = new System.Drawing.Point(10, 333);
            this.btnRearmarBalanca.Name = "btnRearmarBalanca";
            this.btnRearmarBalanca.Size = new System.Drawing.Size(151, 28);
            this.btnRearmarBalanca.TabIndex = 141;
            this.btnRearmarBalanca.Text = "Conectar B";
            this.btnRearmarBalanca.UseVisualStyleBackColor = false;
            this.btnRearmarBalanca.Click += new System.EventHandler(this.btnRearmarBalanca_Click);
            // 
            // txtIpBalanca
            // 
            this.txtIpBalanca.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIpBalanca.Location = new System.Drawing.Point(10, 304);
            this.txtIpBalanca.Name = "txtIpBalanca";
            this.txtIpBalanca.Size = new System.Drawing.Size(98, 23);
            this.txtIpBalanca.TabIndex = 124;
            this.txtIpBalanca.Text = "192.168.1.53";
            // 
            // txtPortaBalanca
            // 
            this.txtPortaBalanca.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortaBalanca.Location = new System.Drawing.Point(114, 304);
            this.txtPortaBalanca.Name = "txtPortaBalanca";
            this.txtPortaBalanca.Size = new System.Drawing.Size(47, 23);
            this.txtPortaBalanca.TabIndex = 125;
            this.txtPortaBalanca.Text = "1702";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 365);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 17);
            this.label2.TabIndex = 133;
            this.label2.Text = "Scanner IP/Porta: ";
            // 
            // SocketScanner
            // 
            this.SocketScanner.Enabled = true;
            this.SocketScanner.Location = new System.Drawing.Point(39, 245);
            this.SocketScanner.Name = "SocketScanner";
            this.SocketScanner.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SocketScanner.OcxState")));
            this.SocketScanner.Size = new System.Drawing.Size(28, 28);
            this.SocketScanner.TabIndex = 136;
            this.SocketScanner.Tag = "Scanner";
            this.SocketScanner.Error += new AxMSWinsockLib.DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error);
            this.SocketScanner.DataArrival += new AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival);
            this.SocketScanner.ConnectEvent += new System.EventHandler(this.Socket_ConnectEvent);
            this.SocketScanner.CloseEvent += new System.EventHandler(this.Socket_CloseEvent);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(7, 284);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(121, 17);
            this.lblTitulo.TabIndex = 123;
            this.lblTitulo.Text = "Balança IP/Porta: ";
            // 
            // txtIpScanner
            // 
            this.txtIpScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIpScanner.Location = new System.Drawing.Point(10, 385);
            this.txtIpScanner.Name = "txtIpScanner";
            this.txtIpScanner.Size = new System.Drawing.Size(98, 23);
            this.txtIpScanner.TabIndex = 134;
            this.txtIpScanner.Text = "192.168.1.51";
            // 
            // txtPortaPLC
            // 
            this.txtPortaPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortaPLC.Location = new System.Drawing.Point(303, 385);
            this.txtPortaPLC.Name = "txtPortaPLC";
            this.txtPortaPLC.Size = new System.Drawing.Size(47, 23);
            this.txtPortaPLC.TabIndex = 139;
            this.txtPortaPLC.Text = "2001";
            // 
            // SocketBalanca
            // 
            this.SocketBalanca.Enabled = true;
            this.SocketBalanca.Location = new System.Drawing.Point(8, 245);
            this.SocketBalanca.Name = "SocketBalanca";
            this.SocketBalanca.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SocketBalanca.OcxState")));
            this.SocketBalanca.Size = new System.Drawing.Size(28, 28);
            this.SocketBalanca.TabIndex = 126;
            this.SocketBalanca.Tag = "Balança";
            this.SocketBalanca.Error += new AxMSWinsockLib.DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error);
            this.SocketBalanca.DataArrival += new AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival);
            this.SocketBalanca.ConnectEvent += new System.EventHandler(this.Socket_ConnectEvent);
            this.SocketBalanca.CloseEvent += new System.EventHandler(this.Socket_CloseEvent);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.Location = new System.Drawing.Point(10, 471);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(151, 35);
            this.btnDisconnect.TabIndex = 132;
            this.btnDisconnect.Text = "Desconectar";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // txtPortaScanner
            // 
            this.txtPortaScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortaScanner.Location = new System.Drawing.Point(114, 385);
            this.txtPortaScanner.Name = "txtPortaScanner";
            this.txtPortaScanner.Size = new System.Drawing.Size(47, 23);
            this.txtPortaScanner.TabIndex = 135;
            this.txtPortaScanner.Text = "51236";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(199, 471);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(151, 35);
            this.btnConnect.TabIndex = 129;
            this.btnConnect.Text = "Conectar Todos";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtIPplc
            // 
            this.txtIPplc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIPplc.Location = new System.Drawing.Point(199, 385);
            this.txtIPplc.Name = "txtIPplc";
            this.txtIPplc.Size = new System.Drawing.Size(98, 23);
            this.txtIPplc.TabIndex = 138;
            this.txtIPplc.Text = "192.168.1.54";
            // 
            // txtPortaPortal
            // 
            this.txtPortaPortal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortaPortal.Location = new System.Drawing.Point(303, 304);
            this.txtPortaPortal.Name = "txtPortaPortal";
            this.txtPortaPortal.Size = new System.Drawing.Size(47, 23);
            this.txtPortaPortal.TabIndex = 130;
            this.txtPortaPortal.Text = "5210";
            // 
            // SocketPortal
            // 
            this.SocketPortal.Enabled = true;
            this.SocketPortal.Location = new System.Drawing.Point(323, 249);
            this.SocketPortal.Name = "SocketPortal";
            this.SocketPortal.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SocketPortal.OcxState")));
            this.SocketPortal.Size = new System.Drawing.Size(28, 28);
            this.SocketPortal.TabIndex = 131;
            this.SocketPortal.Tag = "Portal";
            this.SocketPortal.Error += new AxMSWinsockLib.DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error);
            this.SocketPortal.DataArrival += new AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival);
            this.SocketPortal.ConnectEvent += new System.EventHandler(this.Socket_ConnectEvent);
            this.SocketPortal.CloseEvent += new System.EventHandler(this.Socket_CloseEvent);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(196, 365);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 17);
            this.label4.TabIndex = 137;
            this.label4.Text = "PLC IP/Porta: ";
            // 
            // txtIpPortal
            // 
            this.txtIpPortal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIpPortal.Location = new System.Drawing.Point(199, 304);
            this.txtIpPortal.Name = "txtIpPortal";
            this.txtIpPortal.Size = new System.Drawing.Size(98, 23);
            this.txtIpPortal.TabIndex = 128;
            this.txtIpPortal.Text = "192.168.1.52";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(196, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 17);
            this.label1.TabIndex = 127;
            this.label1.Text = "Dimensionador IP/Porta: ";
            // 
            // SocketPLC
            // 
            this.SocketPLC.Enabled = true;
            this.SocketPLC.Location = new System.Drawing.Point(294, 249);
            this.SocketPLC.Name = "SocketPLC";
            this.SocketPLC.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SocketPLC.OcxState")));
            this.SocketPLC.Size = new System.Drawing.Size(28, 28);
            this.SocketPLC.TabIndex = 140;
            this.SocketPLC.Tag = "PLC";
            this.SocketPLC.Error += new AxMSWinsockLib.DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error);
            this.SocketPLC.DataArrival += new AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival);
            this.SocketPLC.ConnectEvent += new System.EventHandler(this.Socket_ConnectEvent);
            this.SocketPLC.CloseEvent += new System.EventHandler(this.Socket_CloseEvent);
            // 
            // chkDesconectarSeFalhaNaFila
            // 
            this.chkDesconectarSeFalhaNaFila.AutoSize = true;
            this.chkDesconectarSeFalhaNaFila.Checked = true;
            this.chkDesconectarSeFalhaNaFila.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDesconectarSeFalhaNaFila.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDesconectarSeFalhaNaFila.Location = new System.Drawing.Point(80, 254);
            this.chkDesconectarSeFalhaNaFila.Name = "chkDesconectarSeFalhaNaFila";
            this.chkDesconectarSeFalhaNaFila.Size = new System.Drawing.Size(208, 23);
            this.chkDesconectarSeFalhaNaFila.TabIndex = 122;
            this.chkDesconectarSeFalhaNaFila.Text = "Desconectar se falha na fila";
            this.chkDesconectarSeFalhaNaFila.UseVisualStyleBackColor = true;
            // 
            // cboSaidaAnvisa
            // 
            this.cboSaidaAnvisa.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSaidaAnvisa.FormattingEnabled = true;
            this.cboSaidaAnvisa.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05"});
            this.cboSaidaAnvisa.Location = new System.Drawing.Point(259, 212);
            this.cboSaidaAnvisa.Name = "cboSaidaAnvisa";
            this.cboSaidaAnvisa.Size = new System.Drawing.Size(93, 27);
            this.cboSaidaAnvisa.TabIndex = 121;
            this.cboSaidaAnvisa.Text = "03";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(255, 190);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 19);
            this.label13.TabIndex = 120;
            this.label13.Text = "Anvisa saída?";
            // 
            // cboSaidaRejeito
            // 
            this.cboSaidaRejeito.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSaidaRejeito.FormattingEnabled = true;
            this.cboSaidaRejeito.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05"});
            this.cboSaidaRejeito.Location = new System.Drawing.Point(259, 150);
            this.cboSaidaRejeito.Name = "cboSaidaRejeito";
            this.cboSaidaRejeito.Size = new System.Drawing.Size(93, 27);
            this.cboSaidaRejeito.TabIndex = 119;
            this.cboSaidaRejeito.Text = "01";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(256, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 19);
            this.label12.TabIndex = 118;
            this.label12.Text = "Rejeito saída?";
            // 
            // btnPcaDelS5
            // 
            this.btnPcaDelS5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaDelS5.Location = new System.Drawing.Point(199, 149);
            this.btnPcaDelS5.Name = "btnPcaDelS5";
            this.btnPcaDelS5.Size = new System.Drawing.Size(31, 23);
            this.btnPcaDelS5.TabIndex = 116;
            this.btnPcaDelS5.Text = "-";
            this.btnPcaDelS5.UseVisualStyleBackColor = true;
            this.btnPcaDelS5.Click += new System.EventHandler(this.btnPcaDel_Click);
            // 
            // btnPcaAddS5
            // 
            this.btnPcaAddS5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaAddS5.Location = new System.Drawing.Point(167, 149);
            this.btnPcaAddS5.Name = "btnPcaAddS5";
            this.btnPcaAddS5.Size = new System.Drawing.Size(31, 23);
            this.btnPcaAddS5.TabIndex = 115;
            this.btnPcaAddS5.Text = "+";
            this.btnPcaAddS5.UseVisualStyleBackColor = true;
            this.btnPcaAddS5.Click += new System.EventHandler(this.btnPcaAdd_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(128, 129);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 19);
            this.label11.TabIndex = 114;
            this.label11.Text = "Praças Saída 5:";
            // 
            // lstPcaSaida5
            // 
            this.lstPcaSaida5.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPcaSaida5.FormattingEnabled = true;
            this.lstPcaSaida5.ItemHeight = 19;
            this.lstPcaSaida5.Location = new System.Drawing.Point(131, 178);
            this.lstPcaSaida5.Name = "lstPcaSaida5";
            this.lstPcaSaida5.Size = new System.Drawing.Size(99, 61);
            this.lstPcaSaida5.TabIndex = 113;
            // 
            // txtPcaS5
            // 
            this.txtPcaS5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcaS5.Location = new System.Drawing.Point(131, 149);
            this.txtPcaS5.Name = "txtPcaS5";
            this.txtPcaS5.Size = new System.Drawing.Size(30, 23);
            this.txtPcaS5.TabIndex = 112;
            // 
            // lstPcaSaida1
            // 
            this.lstPcaSaida1.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPcaSaida1.FormattingEnabled = true;
            this.lstPcaSaida1.ItemHeight = 19;
            this.lstPcaSaida1.Location = new System.Drawing.Point(3, 56);
            this.lstPcaSaida1.Name = "lstPcaSaida1";
            this.lstPcaSaida1.Size = new System.Drawing.Size(99, 61);
            this.lstPcaSaida1.Sorted = true;
            this.lstPcaSaida1.TabIndex = 93;
            // 
            // btnPcaDelS4
            // 
            this.btnPcaDelS4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaDelS4.Location = new System.Drawing.Point(76, 149);
            this.btnPcaDelS4.Name = "btnPcaDelS4";
            this.btnPcaDelS4.Size = new System.Drawing.Size(31, 23);
            this.btnPcaDelS4.TabIndex = 111;
            this.btnPcaDelS4.Text = "-";
            this.btnPcaDelS4.UseVisualStyleBackColor = true;
            this.btnPcaDelS4.Click += new System.EventHandler(this.btnPcaDel_Click);
            // 
            // btnPcaAddS4
            // 
            this.btnPcaAddS4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaAddS4.Location = new System.Drawing.Point(44, 149);
            this.btnPcaAddS4.Name = "btnPcaAddS4";
            this.btnPcaAddS4.Size = new System.Drawing.Size(31, 23);
            this.btnPcaAddS4.TabIndex = 110;
            this.btnPcaAddS4.Text = "+";
            this.btnPcaAddS4.UseVisualStyleBackColor = true;
            this.btnPcaAddS4.Click += new System.EventHandler(this.btnPcaAdd_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 19);
            this.label10.TabIndex = 109;
            this.label10.Text = "Praças Saída 4:";
            // 
            // lstPcaSaida4
            // 
            this.lstPcaSaida4.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPcaSaida4.FormattingEnabled = true;
            this.lstPcaSaida4.ItemHeight = 19;
            this.lstPcaSaida4.Location = new System.Drawing.Point(8, 178);
            this.lstPcaSaida4.Name = "lstPcaSaida4";
            this.lstPcaSaida4.Size = new System.Drawing.Size(99, 61);
            this.lstPcaSaida4.TabIndex = 108;
            // 
            // txtPcaS4
            // 
            this.txtPcaS4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcaS4.Location = new System.Drawing.Point(8, 149);
            this.txtPcaS4.Name = "txtPcaS4";
            this.txtPcaS4.Size = new System.Drawing.Size(30, 23);
            this.txtPcaS4.TabIndex = 107;
            // 
            // btnPcaDelS3
            // 
            this.btnPcaDelS3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaDelS3.Location = new System.Drawing.Point(326, 27);
            this.btnPcaDelS3.Name = "btnPcaDelS3";
            this.btnPcaDelS3.Size = new System.Drawing.Size(31, 23);
            this.btnPcaDelS3.TabIndex = 106;
            this.btnPcaDelS3.Text = "-";
            this.btnPcaDelS3.UseVisualStyleBackColor = true;
            this.btnPcaDelS3.Click += new System.EventHandler(this.btnPcaDel_Click);
            // 
            // btnPcaAddS3
            // 
            this.btnPcaAddS3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaAddS3.Location = new System.Drawing.Point(294, 27);
            this.btnPcaAddS3.Name = "btnPcaAddS3";
            this.btnPcaAddS3.Size = new System.Drawing.Size(31, 23);
            this.btnPcaAddS3.TabIndex = 105;
            this.btnPcaAddS3.Text = "+";
            this.btnPcaAddS3.UseVisualStyleBackColor = true;
            this.btnPcaAddS3.Click += new System.EventHandler(this.btnPcaAdd_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(255, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 19);
            this.label9.TabIndex = 104;
            this.label9.Text = "Praças Saída 3:";
            // 
            // lstPcaSaida3
            // 
            this.lstPcaSaida3.Font = new System.Drawing.Font("Calibri Light", 12F);
            this.lstPcaSaida3.FormattingEnabled = true;
            this.lstPcaSaida3.ItemHeight = 19;
            this.lstPcaSaida3.Location = new System.Drawing.Point(258, 56);
            this.lstPcaSaida3.Name = "lstPcaSaida3";
            this.lstPcaSaida3.Size = new System.Drawing.Size(99, 61);
            this.lstPcaSaida3.TabIndex = 103;
            // 
            // txtPcaS3
            // 
            this.txtPcaS3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcaS3.Location = new System.Drawing.Point(258, 27);
            this.txtPcaS3.Name = "txtPcaS3";
            this.txtPcaS3.Size = new System.Drawing.Size(30, 23);
            this.txtPcaS3.TabIndex = 102;
            // 
            // btnPcaDelS2
            // 
            this.btnPcaDelS2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaDelS2.Location = new System.Drawing.Point(199, 27);
            this.btnPcaDelS2.Name = "btnPcaDelS2";
            this.btnPcaDelS2.Size = new System.Drawing.Size(31, 23);
            this.btnPcaDelS2.TabIndex = 101;
            this.btnPcaDelS2.Text = "-";
            this.btnPcaDelS2.UseVisualStyleBackColor = true;
            this.btnPcaDelS2.Click += new System.EventHandler(this.btnPcaDel_Click);
            // 
            // btnPcaAddS2
            // 
            this.btnPcaAddS2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaAddS2.Location = new System.Drawing.Point(167, 27);
            this.btnPcaAddS2.Name = "btnPcaAddS2";
            this.btnPcaAddS2.Size = new System.Drawing.Size(31, 23);
            this.btnPcaAddS2.TabIndex = 100;
            this.btnPcaAddS2.Text = "+";
            this.btnPcaAddS2.UseVisualStyleBackColor = true;
            this.btnPcaAddS2.Click += new System.EventHandler(this.btnPcaAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(128, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 19);
            this.label8.TabIndex = 99;
            this.label8.Text = "Praças Saída 2:";
            // 
            // lstPcaSaida2
            // 
            this.lstPcaSaida2.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPcaSaida2.FormattingEnabled = true;
            this.lstPcaSaida2.ItemHeight = 19;
            this.lstPcaSaida2.Location = new System.Drawing.Point(131, 56);
            this.lstPcaSaida2.Name = "lstPcaSaida2";
            this.lstPcaSaida2.Size = new System.Drawing.Size(99, 61);
            this.lstPcaSaida2.TabIndex = 98;
            // 
            // txtPcaS2
            // 
            this.txtPcaS2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcaS2.Location = new System.Drawing.Point(131, 27);
            this.txtPcaS2.Name = "txtPcaS2";
            this.txtPcaS2.Size = new System.Drawing.Size(30, 23);
            this.txtPcaS2.TabIndex = 97;
            // 
            // btnPcaDelS1
            // 
            this.btnPcaDelS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaDelS1.Location = new System.Drawing.Point(71, 27);
            this.btnPcaDelS1.Name = "btnPcaDelS1";
            this.btnPcaDelS1.Size = new System.Drawing.Size(31, 23);
            this.btnPcaDelS1.TabIndex = 96;
            this.btnPcaDelS1.Text = "-";
            this.btnPcaDelS1.UseVisualStyleBackColor = true;
            this.btnPcaDelS1.Click += new System.EventHandler(this.btnPcaDel_Click);
            // 
            // btnPcaAddS1
            // 
            this.btnPcaAddS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPcaAddS1.Location = new System.Drawing.Point(39, 27);
            this.btnPcaAddS1.Name = "btnPcaAddS1";
            this.btnPcaAddS1.Size = new System.Drawing.Size(31, 23);
            this.btnPcaAddS1.TabIndex = 95;
            this.btnPcaAddS1.Text = "+";
            this.btnPcaAddS1.UseVisualStyleBackColor = true;
            this.btnPcaAddS1.Click += new System.EventHandler(this.btnPcaAdd_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 19);
            this.label7.TabIndex = 94;
            this.label7.Text = "Praças Saída 1:";
            // 
            // txtPcaS1
            // 
            this.txtPcaS1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcaS1.Location = new System.Drawing.Point(3, 27);
            this.txtPcaS1.Name = "txtPcaS1";
            this.txtPcaS1.Size = new System.Drawing.Size(30, 23);
            this.txtPcaS1.TabIndex = 92;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.chkExibirErroDestCx);
            this.tabPageLog.Controls.Add(this.chkExibirDestinoPLC4);
            this.tabPageLog.Controls.Add(this.chkLogLeiturasConsolidadaResumida);
            this.tabPageLog.Controls.Add(this.btnSalvarLog);
            this.tabPageLog.Controls.Add(this.txtDestinoArquivos);
            this.tabPageLog.Controls.Add(this.chkExibirErroCnn);
            this.tabPageLog.Controls.Add(this.chkLogLeiturasConsolidada);
            this.tabPageLog.Controls.Add(this.label3);
            this.tabPageLog.Controls.Add(this.txtMaskArquivoLeitura);
            this.tabPageLog.Controls.Add(this.chkLogLeituras);
            this.tabPageLog.Controls.Add(this.label5);
            this.tabPageLog.Location = new System.Drawing.Point(4, 28);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(369, 522);
            this.tabPageLog.TabIndex = 1;
            this.tabPageLog.Text = "Configuração do Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // chkExibirErroDestCx
            // 
            this.chkExibirErroDestCx.AutoSize = true;
            this.chkExibirErroDestCx.Checked = true;
            this.chkExibirErroDestCx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExibirErroDestCx.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExibirErroDestCx.Location = new System.Drawing.Point(21, 273);
            this.chkExibirErroDestCx.Name = "chkExibirErroDestCx";
            this.chkExibirErroDestCx.Size = new System.Drawing.Size(226, 23);
            this.chkExibirErroDestCx.TabIndex = 96;
            this.chkExibirErroDestCx.Text = "Exibir erro de destino de caixa";
            this.chkExibirErroDestCx.UseVisualStyleBackColor = true;
            // 
            // chkExibirDestinoPLC4
            // 
            this.chkExibirDestinoPLC4.AutoSize = true;
            this.chkExibirDestinoPLC4.Checked = true;
            this.chkExibirDestinoPLC4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExibirDestinoPLC4.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExibirDestinoPLC4.Location = new System.Drawing.Point(21, 244);
            this.chkExibirDestinoPLC4.Name = "chkExibirDestinoPLC4";
            this.chkExibirDestinoPLC4.Size = new System.Drawing.Size(182, 23);
            this.chkExibirDestinoPLC4.TabIndex = 95;
            this.chkExibirDestinoPLC4.Text = "Exibir destino caixa PLC";
            this.chkExibirDestinoPLC4.UseVisualStyleBackColor = true;
            // 
            // chkLogLeiturasConsolidadaResumida
            // 
            this.chkLogLeiturasConsolidadaResumida.AutoSize = true;
            this.chkLogLeiturasConsolidadaResumida.Checked = true;
            this.chkLogLeiturasConsolidadaResumida.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogLeiturasConsolidadaResumida.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLogLeiturasConsolidadaResumida.Location = new System.Drawing.Point(21, 215);
            this.chkLogLeiturasConsolidadaResumida.Name = "chkLogLeiturasConsolidadaResumida";
            this.chkLogLeiturasConsolidadaResumida.Size = new System.Drawing.Size(270, 23);
            this.chkLogLeiturasConsolidadaResumida.TabIndex = 94;
            this.chkLogLeiturasConsolidadaResumida.Text = "Exibir leituras consolidadas resumida";
            this.chkLogLeiturasConsolidadaResumida.UseVisualStyleBackColor = true;
            this.chkLogLeiturasConsolidadaResumida.CheckedChanged += new System.EventHandler(this.chkLogLeituras_CheckedChanged);
            // 
            // btnSalvarLog
            // 
            this.btnSalvarLog.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvarLog.Location = new System.Drawing.Point(20, 367);
            this.btnSalvarLog.Name = "btnSalvarLog";
            this.btnSalvarLog.Size = new System.Drawing.Size(144, 35);
            this.btnSalvarLog.TabIndex = 92;
            this.btnSalvarLog.Text = "Salvar";
            this.btnSalvarLog.UseVisualStyleBackColor = true;
            this.btnSalvarLog.Click += new System.EventHandler(this.btnSalvarLog_Click);
            // 
            // lblFilaScanner
            // 
            this.lblFilaScanner.AutoSize = true;
            this.lblFilaScanner.Font = new System.Drawing.Font("Calibri Light", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilaScanner.Location = new System.Drawing.Point(635, 62);
            this.lblFilaScanner.Name = "lblFilaScanner";
            this.lblFilaScanner.Size = new System.Drawing.Size(72, 42);
            this.lblFilaScanner.TabIndex = 96;
            this.lblFilaScanner.Text = "S[0]";
            // 
            // lblFilaDim
            // 
            this.lblFilaDim.AutoSize = true;
            this.lblFilaDim.Font = new System.Drawing.Font("Calibri Light", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilaDim.Location = new System.Drawing.Point(544, 63);
            this.lblFilaDim.Name = "lblFilaDim";
            this.lblFilaDim.Size = new System.Drawing.Size(77, 42);
            this.lblFilaDim.TabIndex = 95;
            this.lblFilaDim.Text = "D[0]";
            // 
            // lblFilaBalanca
            // 
            this.lblFilaBalanca.AutoSize = true;
            this.lblFilaBalanca.Font = new System.Drawing.Font("Calibri Light", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilaBalanca.Location = new System.Drawing.Point(463, 62);
            this.lblFilaBalanca.Name = "lblFilaBalanca";
            this.lblFilaBalanca.Size = new System.Drawing.Size(75, 42);
            this.lblFilaBalanca.TabIndex = 94;
            this.lblFilaBalanca.Text = "B[0]";
            // 
            // btnReamarLeituras
            // 
            this.btnReamarLeituras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnReamarLeituras.Enabled = false;
            this.btnReamarLeituras.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnReamarLeituras.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReamarLeituras.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReamarLeituras.ForeColor = System.Drawing.Color.White;
            this.btnReamarLeituras.Location = new System.Drawing.Point(713, 66);
            this.btnReamarLeituras.Name = "btnReamarLeituras";
            this.btnReamarLeituras.Size = new System.Drawing.Size(116, 35);
            this.btnReamarLeituras.TabIndex = 92;
            this.btnReamarLeituras.Text = "Limpar Falhas";
            this.btnReamarLeituras.UseVisualStyleBackColor = false;
            this.btnReamarLeituras.Click += new System.EventHandler(this.btnReamarLeituras_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLimpar.Font = new System.Drawing.Font("Calibri Light", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpar.ForeColor = System.Drawing.Color.White;
            this.btnLimpar.Location = new System.Drawing.Point(835, 66);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(111, 35);
            this.btnLimpar.TabIndex = 93;
            this.btnLimpar.Text = "Limpar Log";
            this.btnLimpar.UseVisualStyleBackColor = false;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // timerFilaLeituras
            // 
            this.timerFilaLeituras.Interval = 80;
            this.timerFilaLeituras.Tick += new System.EventHandler(this.timerFilaLeituras_Tick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::Properties.Resources.BARRA_LOGO;
            this.pictureBox2.Location = new System.Drawing.Point(813, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(150, 60);
            this.pictureBox2.TabIndex = 94;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Properties.Resources.BARRA_LARANJA;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1400, 60);
            this.pictureBox1.TabIndex = 93;
            this.pictureBox1.TabStop = false;
            // 
            // frmWaveAutomatic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(963, 662);
            this.Controls.Add(this.lblFilaScanner);
            this.Controls.Add(this.lblFilaDim);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.lblFilaBalanca);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnReamarLeituras);
            this.Controls.Add(this.rchLog);
            this.Name = "frmWaveAutomatic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logstore Wave";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmToledo_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSorter.ResumeLayout(false);
            this.pnlSorter.ResumeLayout(false);
            this.pnlSorter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SocketScanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SocketBalanca)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SocketPortal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SocketPLC)).EndInit();
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        
    }
}
