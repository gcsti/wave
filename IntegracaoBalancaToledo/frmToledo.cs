using AxMSWinsockLib;
using IntegracaoBalancaToledo.Properties;
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

namespace IntegracaoBalancaToledo
{
	public class frmToledo : Form
	{
		private bool fecharTudo;

		private IContainer components;

		private Button btnDisconnect;

		private AxWinsock SocketScanner;

		private Label label2;

		private Button btnConnect;

		private TextBox txtIpScanner;

		private TextBox txtPortaScanner;

		private AxWinsock SocketPortal;

		private Label label1;

		private TextBox txtIpPortal;

		private TextBox txtPortaPortal;

		private AxWinsock SocketBalanca;

		private Label lblTitulo;

		private TextBox txtIpBalanca;

		private TextBox txtPortaBalanca;

		private RichTextBox rchLog;

		private Label label3;

		private TextBox txtDestinoArquivos;

		private CheckBox chkLogLeituras;

		private Button btnFechar;

		private StatusStrip statusStrip1;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private AxWinsock SocketPLC;

		private Label label4;

		private TextBox txtIPplc;

		private TextBox txtPortaPLC;

		private System.Windows.Forms.Timer timerStatusCnn;

		private Label label5;

		private TextBox txtMaskArquivoLeitura;

		private CheckBox chkExibirErroCnn;

		private PictureBox pictureBox1;

		private CheckBox chkLogLeiturasConsolidada;

		private TabControl tabControl1;

		private TabPage tabPageDisp;

		private TabPage tabPageLog;

		private Button btnRearmarPLC;

		private Button btnRearmarScanner;

		private Button btnRearmarPortal;

		private Button btnRearmarBalanca;

		private Button btnLimpar;

		private Button btnSalvarLog;

		private SaveFileDialog saveFileDialog1;

		private CheckBox chkLogLeiturasConsolidadaResumida;

		private Button btnReamarLeituras;

		private CheckBox chkExibirDestinoPLC4;

		private CheckBox chkExibirErroDestCx;

		private PictureBox pictureBox3;

		private Label lblFilaScanner;

		private Label lblFilaDim;

		private Label lblFilaBalanca;

		private Label label6;

		private System.Windows.Forms.Timer timerFilaLeituras;

		private TabPage tabPageSorter;

		private Panel pnlSorter;

		private Button btnPcaDelS5;

		private Button btnPcaAddS5;

		private Label label11;

		private ListBox lstPcaSaida5;

		private TextBox txtPcaS5;

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

		private ListBox lstPcaSaida1;

		private TextBox txtPcaS1;

		private ComboBox cboSaidaAnvisa;

		private Label label13;

		private ComboBox cboSaidaRejeito;

		private Label label12;

		private CheckBox chkDesconectarSeFalhaNaFila;

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
				this.cboSaidaRejeito.Text = value;
			}
		}

		public string ArquivoConfiguracoes
		{
			get
			{
				return Application.ExecutablePath.Replace(".EXE", "") + ".Config.xml";
			}
		}

		public string ArquivoErros
		{
			get
			{
				return Application.ExecutablePath.Replace(".EXE", "") + ".Erros.txt";
			}
		}

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

		public frmToledo()
		{
			this.InitializeComponent();
			this.CarregarArquivoConfiguracoes();
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			ToolStripStatusLabel expr_29 = this.toolStripStatusLabel1;
			expr_29.Text = expr_29.Text + " Ver.: " + string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
		}

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
			this.pnlSorter.Enabled = false;
			this.timerStatusCnn.Enabled = true;
			this.timerStatusCnn.Start();
		}

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
		}

		private void btnFechar_Click(object sender, EventArgs e)
		{
			this.btnDisconnect_Click(null, null);
			Application.DoEvents();
			GC.Collect();
			Application.Exit();
		}

		private void timerStatusCnn_Tick(object sender, EventArgs e)
		{
			this.statusConexaoDispositivos();
			if (this.btnDisconnect.Enabled)
			{
				if (this.SocketBalanca.CtlState != 7)
				{
					this.btnRearmarBalanca_Click(null, null);
				}
				if (this.SocketPortal.CtlState != 7)
				{
					this.btnRearmarPortal_Click(null, null);
				}
				if (this.SocketScanner.CtlState != 7)
				{
					this.btnRearmarScanner_Click(null, null);
				}
				if (this.SocketPLC.CtlState != 7)
				{
					this.btnRearmarPLC_Click(null, null);
				}
			}
		}

		private void timerFilaLeituras_Tick(object sender, EventArgs e)
		{
			this.exibirStatusFila();
		}

		private void frmToledo_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.fecharTudo)
			{
				return;
			}
			e.Cancel = true;
			try
			{
				if (Interaction.InputBox("Para fechar esta aplicação, forneça a senha de administrador?", "Segurança", "****", -1, -1) == "solidez2017")
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

		private void btnRearmarBalanca_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketBalanca.Close();
				this.SocketBalanca.Protocol = (0);
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

		private void btnRearmarPortal_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketPortal.Close();
				this.SocketPortal.Protocol = (0);
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

		private void btnRearmarScanner_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketScanner.Close();
				this.SocketScanner.Protocol = (0);
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

		private void btnRearmarPLC_Click(object sender, EventArgs e)
		{
			try
			{
				this.SocketPLC.Close();
				this.SocketPLC.Protocol = (0);
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

		private void btnReamarLeituras_Click(object sender, EventArgs e)
		{
			Leitura.Limpar();
			this.rchLog.AppendText("Recursos de memória de leitura redefinidos.\r");
		}

		private void btnLimpar_Click(object sender, EventArgs e)
		{
			this.rchLog.Clear();
		}

		private void btnSalvarLog_Click(object sender, EventArgs e)
		{
			this.saveFileDialog1.Title = "Salvar arquivo de log";
			this.saveFileDialog1.FileName = "Solidez_IntegraToledo_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".log";
			this.saveFileDialog1.Filter = "Arquivo de Log (.log)|*.log|Arquivo de Texto (.txt)|*.txt";
			this.saveFileDialog1.ShowDialog();
			if (!string.IsNullOrEmpty(this.saveFileDialog1.FileName.Trim()))
			{
				this.rchLog.AppendLog("Arquivo de log salvo em " + this.saveFileDialog1.FileName, Color.DarkGreen, this.rchLog.Font);
				this.rchLog.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
			}
		}

		private void chkLogLeituras_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as CheckBox).Name == "chkLogLeiturasConsolidada")
			{
				this.chkLogLeiturasConsolidadaResumida.Checked = !this.chkLogLeiturasConsolidada.Checked;
				return;
			}
			this.chkLogLeiturasConsolidada.Checked = !this.chkLogLeiturasConsolidadaResumida.Checked;
		}

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

		private void Socket_DataArrival(object sender, DMSWinsockControlEvents_DataArrivalEvent e)
		{
			AxWinsock socket = sender as AxWinsock;
			int num = 20170901;
			if (Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) > num)
			{
				Thread.Sleep(1000);
			}
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

		private void informarDestinoCaixaParaPLC(string destino)
		{

           // System.IO.File.WriteAllText(@"C:\Logstore\AVISAPLCINICIO.txt", DateTime.Now.ToString());


            if (!string.IsNullOrEmpty(destino))
			{
				try
				{
					this.SocketPLC.SendData(destino);

                  //  System.IO.File.WriteAllText(@"C:\Logstore\AVISAPLCFIM.txt", DateTime.Now.ToString());

                    try
                    {
                        //System.IO.File.WriteAllText(@"C:\Logstore\DestinoPLC.txt", destino);
                        
                    }
                    catch
                    {

                    }


                    if (this.chkExibirDestinoPLC4.Checked)
					{
						this.rchLog.AppendSendMessage("Saída do Sorter: " + destino + "\r", "");
					}
				}
				catch (Exception ex)
				{
					this.GravarArquivoErro(ex.ToString());
				}
			}
		}

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

		private void GravarArquivoConfig()
		{
			if (this.cboSaidaAnvisa.Text == this.cboSaidaRejeito.Text)
			{
				//MessageBox.Show("Saídas Anvisa e Rejeito não podem ser iguais.", "Atenção");
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

		private void statusConexaoDispositivos()
		{
			if (this.SocketBalanca.CtlState == 7)
			{
				this.setBgColor(this.txtIpBalanca, Color.Green);
			}
			else
			{
				this.setBgColor(this.txtIpBalanca, Color.Red);
			}
			if (this.SocketPortal.CtlState == 7)
			{
				this.setBgColor(this.txtIpPortal, Color.Green);
			}
			else
			{
				this.setBgColor(this.txtIpPortal, Color.Red);
			}
			if (this.SocketScanner.CtlState == 7)
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmToledo));
			this.btnDisconnect = new Button();
			this.chkExibirErroCnn = new CheckBox();
			this.label5 = new Label();
			this.txtMaskArquivoLeitura = new TextBox();
			this.label4 = new Label();
			this.txtIPplc = new TextBox();
			this.txtPortaPLC = new TextBox();
			this.btnFechar = new Button();
			this.chkLogLeituras = new CheckBox();
			this.label3 = new Label();
			this.txtDestinoArquivos = new TextBox();
			this.label2 = new Label();
			this.btnConnect = new Button();
			this.txtIpScanner = new TextBox();
			this.txtPortaScanner = new TextBox();
			this.label1 = new Label();
			this.txtIpPortal = new TextBox();
			this.txtPortaPortal = new TextBox();
			this.lblTitulo = new Label();
			this.txtIpBalanca = new TextBox();
			this.txtPortaBalanca = new TextBox();
			this.rchLog = new RichTextBox();
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.timerStatusCnn = new System.Windows.Forms.Timer(this.components);
			this.chkLogLeiturasConsolidada = new CheckBox();
			this.tabControl1 = new TabControl();
			this.tabPageDisp = new TabPage();
			this.lblFilaScanner = new Label();
			this.lblFilaDim = new Label();
			this.lblFilaBalanca = new Label();
			this.label6 = new Label();
			this.btnReamarLeituras = new Button();
			this.btnRearmarPLC = new Button();
			this.btnRearmarScanner = new Button();
			this.btnRearmarPortal = new Button();
			this.btnRearmarBalanca = new Button();
			this.SocketScanner = new AxWinsock();
			this.SocketBalanca = new AxWinsock();
			this.SocketPortal = new AxWinsock();
			this.SocketPLC = new AxWinsock();
			this.tabPageSorter = new TabPage();
			this.pnlSorter = new Panel();
			this.cboSaidaAnvisa = new ComboBox();
			this.label13 = new Label();
			this.cboSaidaRejeito = new ComboBox();
			this.label12 = new Label();
			this.btnPcaDelS5 = new Button();
			this.btnPcaAddS5 = new Button();
			this.label11 = new Label();
			this.lstPcaSaida5 = new ListBox();
			this.txtPcaS5 = new TextBox();
			this.lstPcaSaida1 = new ListBox();
			this.btnPcaDelS4 = new Button();
			this.btnPcaAddS4 = new Button();
			this.label10 = new Label();
			this.lstPcaSaida4 = new ListBox();
			this.txtPcaS4 = new TextBox();
			this.btnPcaDelS3 = new Button();
			this.btnPcaAddS3 = new Button();
			this.label9 = new Label();
			this.lstPcaSaida3 = new ListBox();
			this.txtPcaS3 = new TextBox();
			this.btnPcaDelS2 = new Button();
			this.btnPcaAddS2 = new Button();
			this.label8 = new Label();
			this.lstPcaSaida2 = new ListBox();
			this.txtPcaS2 = new TextBox();
			this.btnPcaDelS1 = new Button();
			this.btnPcaAddS1 = new Button();
			this.label7 = new Label();
			this.txtPcaS1 = new TextBox();
			this.tabPageLog = new TabPage();
			this.chkExibirErroDestCx = new CheckBox();
			this.chkExibirDestinoPLC4 = new CheckBox();
			this.chkLogLeiturasConsolidadaResumida = new CheckBox();
			this.btnLimpar = new Button();
			this.btnSalvarLog = new Button();
			this.saveFileDialog1 = new SaveFileDialog();
			this.timerFilaLeituras = new System.Windows.Forms.Timer(this.components);
			this.pictureBox3 = new PictureBox();
			this.pictureBox1 = new PictureBox();
			this.chkDesconectarSeFalhaNaFila = new CheckBox();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPageDisp.SuspendLayout();
			this.SocketScanner.BeginInit();
			this.SocketBalanca.BeginInit();
			this.SocketPortal.BeginInit();
			this.SocketPLC.BeginInit();
			this.tabPageSorter.SuspendLayout();
			this.pnlSorter.SuspendLayout();
			this.tabPageLog.SuspendLayout();
			((ISupportInitialize)this.pictureBox3).BeginInit();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.btnDisconnect.Enabled = false;
			this.btnDisconnect.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnDisconnect.Location = new Point(775, 57);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new Size(144, 35);
			this.btnDisconnect.TabIndex = 75;
			this.btnDisconnect.Text = "Desconectar";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new EventHandler(this.btnDisconnect_Click);
			this.chkExibirErroCnn.AutoSize = true;
			this.chkExibirErroCnn.Checked = true;
			this.chkExibirErroCnn.CheckState = CheckState.Checked;
			this.chkExibirErroCnn.Location = new Point(503, 115);
			this.chkExibirErroCnn.Name = "chkExibirErroCnn";
			this.chkExibirErroCnn.Size = new Size(131, 17);
			this.chkExibirErroCnn.TabIndex = 90;
			this.chkExibirErroCnn.Text = "Exibir erro de conexão";
			this.chkExibirErroCnn.UseVisualStyleBackColor = true;
			this.label5.AutoSize = true;
			this.label5.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label5.Location = new Point(376, 25);
			this.label5.Name = "label5";
			this.label5.Size = new Size(253, 17);
			this.label5.TabIndex = 88;
			this.label5.Text = "Máscara nome dos  arquivos de leitura";
			this.txtMaskArquivoLeitura.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtMaskArquivoLeitura.Location = new Point(379, 45);
			this.txtMaskArquivoLeitura.Name = "txtMaskArquivoLeitura";
			this.txtMaskArquivoLeitura.Size = new Size(332, 23);
			this.txtMaskArquivoLeitura.TabIndex = 89;
			this.txtMaskArquivoLeitura.Text = "SOLIDEZ_{data}_{hora}{data}{hora}{minuto}.txt";
			this.label4.AutoSize = true;
			this.label4.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label4.Location = new Point(577, 21);
			this.label4.Name = "label4";
			this.label4.Size = new Size(96, 17);
			this.label4.TabIndex = 84;
			this.label4.Text = "PLC IP/Porta: ";
			this.txtIPplc.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtIPplc.Location = new Point(580, 41);
			this.txtIPplc.Name = "txtIPplc";
			this.txtIPplc.Size = new Size(98, 23);
			this.txtIPplc.TabIndex = 85;
			this.txtIPplc.Text = "192.168.1.54";
			this.txtPortaPLC.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPortaPLC.Location = new Point(684, 41);
			this.txtPortaPLC.Name = "txtPortaPLC";
			this.txtPortaPLC.Size = new Size(47, 23);
			this.txtPortaPLC.TabIndex = 86;
			this.txtPortaPLC.Text = "2001";
			this.btnFechar.BackColor = SystemColors.ButtonFace;
			this.btnFechar.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnFechar.ForeColor = SystemColors.ActiveCaptionText;
			this.btnFechar.Location = new Point(775, 102);
			this.btnFechar.Name = "btnFechar";
			this.btnFechar.Size = new Size(144, 35);
			this.btnFechar.TabIndex = 83;
			this.btnFechar.Text = "Fechar";
			this.btnFechar.UseVisualStyleBackColor = false;
			this.btnFechar.Click += new EventHandler(this.btnFechar_Click);
			this.chkLogLeituras.AutoSize = true;
			this.chkLogLeituras.Location = new Point(19, 82);
			this.chkLogLeituras.Name = "chkLogLeituras";
			this.chkLogLeituras.Size = new Size(157, 17);
			this.chkLogLeituras.TabIndex = 82;
			this.chkLogLeituras.Text = "Exibir leituras por dispositivo";
			this.chkLogLeituras.UseVisualStyleBackColor = true;
			this.label3.AutoSize = true;
			this.label3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label3.Location = new Point(16, 25);
			this.label3.Name = "label3";
			this.label3.Size = new Size(211, 17);
			this.label3.TabIndex = 80;
			this.label3.Text = "Caminho dos arquivos de leitura";
			this.txtDestinoArquivos.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtDestinoArquivos.Location = new Point(19, 45);
			this.txtDestinoArquivos.Name = "txtDestinoArquivos";
			this.txtDestinoArquivos.Size = new Size(332, 23);
			this.txtDestinoArquivos.TabIndex = 81;
			this.txtDestinoArquivos.Text = "C:\\comum\\ftp\\txt_copia ";
			this.label2.AutoSize = true;
			this.label2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label2.Location = new Point(393, 21);
			this.label2.Name = "label2";
			this.label2.Size = new Size(123, 17);
			this.label2.TabIndex = 76;
			this.label2.Text = "Scanner IP/Porta: ";
			this.btnConnect.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnConnect.Location = new Point(775, 18);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new Size(144, 35);
			this.btnConnect.TabIndex = 74;
			this.btnConnect.Text = "Conectar";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new EventHandler(this.btnConnect_Click);
			this.txtIpScanner.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtIpScanner.Location = new Point(396, 41);
			this.txtIpScanner.Name = "txtIpScanner";
			this.txtIpScanner.Size = new Size(98, 23);
			this.txtIpScanner.TabIndex = 77;
			this.txtIpScanner.Text = "192.168.1.51";
			this.txtPortaScanner.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPortaScanner.Location = new Point(500, 41);
			this.txtPortaScanner.Name = "txtPortaScanner";
			this.txtPortaScanner.Size = new Size(47, 23);
			this.txtPortaScanner.TabIndex = 78;
			this.txtPortaScanner.Text = "51236";
			this.label1.AutoSize = true;
			this.label1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label1.Location = new Point(207, 21);
			this.label1.Name = "label1";
			this.label1.Size = new Size(165, 17);
			this.label1.TabIndex = 72;
			this.label1.Text = "Dimensionador IP/Porta: ";
			this.txtIpPortal.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtIpPortal.Location = new Point(210, 41);
			this.txtIpPortal.Name = "txtIpPortal";
			this.txtIpPortal.Size = new Size(98, 23);
			this.txtIpPortal.TabIndex = 73;
			this.txtIpPortal.Text = "192.168.1.52";
			this.txtPortaPortal.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPortaPortal.Location = new Point(314, 41);
			this.txtPortaPortal.Name = "txtPortaPortal";
			this.txtPortaPortal.Size = new Size(47, 23);
			this.txtPortaPortal.TabIndex = 74;
			this.txtPortaPortal.Text = "5210";
			this.lblTitulo.AutoSize = true;
			this.lblTitulo.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lblTitulo.Location = new Point(22, 21);
			this.lblTitulo.Name = "lblTitulo";
			this.lblTitulo.Size = new Size(121, 17);
			this.lblTitulo.TabIndex = 65;
			this.lblTitulo.Text = "Balança IP/Porta: ";
			this.txtIpBalanca.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtIpBalanca.Location = new Point(25, 41);
			this.txtIpBalanca.Name = "txtIpBalanca";
			this.txtIpBalanca.Size = new Size(98, 23);
			this.txtIpBalanca.TabIndex = 66;
			this.txtIpBalanca.Text = "192.168.1.53";
			this.txtPortaBalanca.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPortaBalanca.Location = new Point(129, 41);
			this.txtPortaBalanca.Name = "txtPortaBalanca";
			this.txtPortaBalanca.Size = new Size(47, 23);
			this.txtPortaBalanca.TabIndex = 67;
			this.txtPortaBalanca.Text = "1702";
			this.rchLog.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.rchLog.BackColor = Color.WhiteSmoke;
			this.rchLog.BorderStyle = BorderStyle.None;
			this.rchLog.Font = new Font("Courier New", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.rchLog.Location = new Point(11, 254);
			this.rchLog.Name = "rchLog";
			this.rchLog.Size = new Size(950, 381);
			this.rchLog.TabIndex = 74;
			this.rchLog.Text = "";
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel1
			});
			this.statusStrip1.Location = new Point(0, 640);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(973, 22);
			this.statusStrip1.TabIndex = 75;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel1.BackColor = SystemColors.Control;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(233, 17);
			this.toolStripStatusLabel1.Text = "Studio Log - Controle de fluxo de materiais";
			this.timerStatusCnn.Interval = 2000;
			this.timerStatusCnn.Tick += new EventHandler(this.timerStatusCnn_Tick);
			this.chkLogLeiturasConsolidada.AutoSize = true;
			this.chkLogLeiturasConsolidada.Location = new Point(19, 115);
			this.chkLogLeiturasConsolidada.Name = "chkLogLeiturasConsolidada";
			this.chkLogLeiturasConsolidada.Size = new Size(202, 17);
			this.chkLogLeiturasConsolidada.TabIndex = 91;
			this.chkLogLeiturasConsolidada.Text = "Exibir leituras consolidadas detalhada";
			this.chkLogLeiturasConsolidada.UseVisualStyleBackColor = true;
			this.chkLogLeiturasConsolidada.CheckedChanged += new EventHandler(this.chkLogLeituras_CheckedChanged);
			this.tabControl1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.tabControl1.Controls.Add(this.tabPageDisp);
			this.tabControl1.Controls.Add(this.tabPageSorter);
			this.tabControl1.Controls.Add(this.tabPageLog);
			this.tabControl1.Location = new Point(12, 67);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(948, 181);
			this.tabControl1.TabIndex = 92;
			this.tabPageDisp.Controls.Add(this.lblFilaScanner);
			this.tabPageDisp.Controls.Add(this.lblFilaDim);
			this.tabPageDisp.Controls.Add(this.lblFilaBalanca);
			this.tabPageDisp.Controls.Add(this.label6);
			this.tabPageDisp.Controls.Add(this.btnReamarLeituras);
			this.tabPageDisp.Controls.Add(this.btnRearmarPLC);
			this.tabPageDisp.Controls.Add(this.btnRearmarScanner);
			this.tabPageDisp.Controls.Add(this.btnRearmarPortal);
			this.tabPageDisp.Controls.Add(this.btnRearmarBalanca);
			this.tabPageDisp.Controls.Add(this.btnFechar);
			this.tabPageDisp.Controls.Add(this.txtIpBalanca);
			this.tabPageDisp.Controls.Add(this.txtPortaBalanca);
			this.tabPageDisp.Controls.Add(this.label2);
			this.tabPageDisp.Controls.Add(this.SocketScanner);
			this.tabPageDisp.Controls.Add(this.lblTitulo);
			this.tabPageDisp.Controls.Add(this.txtIpScanner);
			this.tabPageDisp.Controls.Add(this.txtPortaPLC);
			this.tabPageDisp.Controls.Add(this.SocketBalanca);
			this.tabPageDisp.Controls.Add(this.btnDisconnect);
			this.tabPageDisp.Controls.Add(this.txtPortaScanner);
			this.tabPageDisp.Controls.Add(this.btnConnect);
			this.tabPageDisp.Controls.Add(this.txtIPplc);
			this.tabPageDisp.Controls.Add(this.txtPortaPortal);
			this.tabPageDisp.Controls.Add(this.SocketPortal);
			this.tabPageDisp.Controls.Add(this.label4);
			this.tabPageDisp.Controls.Add(this.txtIpPortal);
			this.tabPageDisp.Controls.Add(this.label1);
			this.tabPageDisp.Controls.Add(this.SocketPLC);
			this.tabPageDisp.Location = new Point(4, 22);
			this.tabPageDisp.Name = "tabPageDisp";
			this.tabPageDisp.Padding = new Padding(3);
			this.tabPageDisp.Size = new Size(940, 155);
			this.tabPageDisp.TabIndex = 0;
			this.tabPageDisp.Text = "Dispositivos";
			this.tabPageDisp.UseVisualStyleBackColor = true;
			this.lblFilaScanner.AutoSize = true;
			this.lblFilaScanner.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lblFilaScanner.Location = new Point(518, 115);
			this.lblFilaScanner.Name = "lblFilaScanner";
			this.lblFilaScanner.Size = new Size(33, 17);
			this.lblFilaScanner.TabIndex = 96;
			this.lblFilaScanner.Text = "S[0]";
			this.lblFilaDim.AutoSize = true;
			this.lblFilaDim.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lblFilaDim.Location = new Point(473, 115);
			this.lblFilaDim.Name = "lblFilaDim";
			this.lblFilaDim.Size = new Size(34, 17);
			this.lblFilaDim.TabIndex = 95;
			this.lblFilaDim.Text = "D[0]";
			this.lblFilaBalanca.AutoSize = true;
			this.lblFilaBalanca.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lblFilaBalanca.Location = new Point(429, 115);
			this.lblFilaBalanca.Name = "lblFilaBalanca";
			this.lblFilaBalanca.Size = new Size(33, 17);
			this.lblFilaBalanca.TabIndex = 94;
			this.lblFilaBalanca.Text = "B[0]";
			this.label6.AutoSize = true;
			this.label6.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label6.Location = new Point(393, 115);
			this.label6.Name = "label6";
			this.label6.Size = new Size(39, 17);
			this.label6.TabIndex = 93;
			this.label6.Text = "Fila:";
			this.btnReamarLeituras.BackColor = Color.FromArgb(255, 255, 192);
			this.btnReamarLeituras.Enabled = false;
			this.btnReamarLeituras.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnReamarLeituras.Location = new Point(25, 109);
			this.btnReamarLeituras.Name = "btnReamarLeituras";
			this.btnReamarLeituras.Size = new Size(336, 28);
			this.btnReamarLeituras.TabIndex = 92;
			this.btnReamarLeituras.Text = "Limpar Falha de Leituras";
			this.btnReamarLeituras.UseVisualStyleBackColor = false;
			this.btnReamarLeituras.Click += new EventHandler(this.btnReamarLeituras_Click);
			this.btnRearmarPLC.BackColor = Color.FromArgb(255, 255, 192);
			this.btnRearmarPLC.Enabled = false;
			this.btnRearmarPLC.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnRearmarPLC.Location = new Point(580, 70);
			this.btnRearmarPLC.Name = "btnRearmarPLC";
			this.btnRearmarPLC.Size = new Size(151, 28);
			this.btnRearmarPLC.TabIndex = 91;
			this.btnRearmarPLC.Text = "Rearmar";
			this.btnRearmarPLC.UseVisualStyleBackColor = false;
			this.btnRearmarPLC.Click += new EventHandler(this.btnRearmarPLC_Click);
			this.btnRearmarScanner.BackColor = Color.FromArgb(255, 255, 192);
			this.btnRearmarScanner.Enabled = false;
			this.btnRearmarScanner.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnRearmarScanner.Location = new Point(396, 70);
			this.btnRearmarScanner.Name = "btnRearmarScanner";
			this.btnRearmarScanner.Size = new Size(151, 28);
			this.btnRearmarScanner.TabIndex = 90;
			this.btnRearmarScanner.Text = "Rearmar";
			this.btnRearmarScanner.UseVisualStyleBackColor = false;
			this.btnRearmarScanner.Click += new EventHandler(this.btnRearmarScanner_Click);
			this.btnRearmarPortal.BackColor = Color.FromArgb(255, 255, 192);
			this.btnRearmarPortal.Enabled = false;
			this.btnRearmarPortal.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnRearmarPortal.Location = new Point(210, 70);
			this.btnRearmarPortal.Name = "btnRearmarPortal";
			this.btnRearmarPortal.Size = new Size(151, 28);
			this.btnRearmarPortal.TabIndex = 89;
			this.btnRearmarPortal.Text = "Rearmar";
			this.btnRearmarPortal.UseVisualStyleBackColor = false;
			this.btnRearmarPortal.Click += new EventHandler(this.btnRearmarPortal_Click);
			this.btnRearmarBalanca.BackColor = Color.FromArgb(255, 255, 192);
			this.btnRearmarBalanca.Enabled = false;
			this.btnRearmarBalanca.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnRearmarBalanca.Location = new Point(25, 70);
			this.btnRearmarBalanca.Name = "btnRearmarBalanca";
			this.btnRearmarBalanca.Size = new Size(151, 28);
			this.btnRearmarBalanca.TabIndex = 88;
			this.btnRearmarBalanca.Text = "Rearmar";
			this.btnRearmarBalanca.UseVisualStyleBackColor = false;
			this.btnRearmarBalanca.Click += new EventHandler(this.btnRearmarBalanca_Click);
			this.SocketScanner.Enabled = true;
			this.SocketScanner.Location = new Point(508, -4);
			this.SocketScanner.Name = "SocketScanner";
			this.SocketScanner.OcxState = (AxHost.State)componentResourceManager.GetObject("SocketScanner.OcxState");
			this.SocketScanner.Size = new Size(28, 28);
			this.SocketScanner.TabIndex = 79;
			this.SocketScanner.Tag = "Scanner";
			this.SocketScanner.Error += (new DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error));
			this.SocketScanner.DataArrival += (new DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival));
			this.SocketScanner.ConnectEvent += (new EventHandler(this.Socket_ConnectEvent));
			this.SocketScanner.CloseEvent += (new EventHandler(this.Socket_CloseEvent));
			this.SocketBalanca.Enabled = true;
			this.SocketBalanca.Location = new Point(145, -4);
			this.SocketBalanca.Name = "SocketBalanca";
			this.SocketBalanca.OcxState = (AxHost.State)componentResourceManager.GetObject("SocketBalanca.OcxState");
			this.SocketBalanca.Size = new Size(28, 28);
			this.SocketBalanca.TabIndex = 71;
			this.SocketBalanca.Tag = "Balança";
			this.SocketBalanca.Error += (new DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error));
			this.SocketBalanca.DataArrival += (new DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival));
			this.SocketBalanca.ConnectEvent += (new EventHandler(this.Socket_ConnectEvent));
			this.SocketBalanca.CloseEvent += (new EventHandler(this.Socket_CloseEvent));
			this.SocketPortal.Enabled = true;
			this.SocketPortal.Location = new Point(326, -4);
			this.SocketPortal.Name = "SocketPortal";
			this.SocketPortal.OcxState = (AxHost.State)componentResourceManager.GetObject("SocketPortal.OcxState");
			this.SocketPortal.Size = new Size(28, 28);
			this.SocketPortal.TabIndex = 75;
			this.SocketPortal.Tag = "Portal";
			this.SocketPortal.Error += (new DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error));
			this.SocketPortal.DataArrival += (new DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival));
			this.SocketPortal.ConnectEvent += (new EventHandler(this.Socket_ConnectEvent));
			this.SocketPortal.CloseEvent += (new EventHandler(this.Socket_CloseEvent));
			this.SocketPLC.Enabled = true;
			this.SocketPLC.Location = new Point(688, -4);
			this.SocketPLC.Name = "SocketPLC";
			this.SocketPLC.OcxState = (AxHost.State)componentResourceManager.GetObject("SocketPLC.OcxState");
			this.SocketPLC.Size = new Size(28, 28);
			this.SocketPLC.TabIndex = 87;
			this.SocketPLC.Tag = "PLC";
			this.SocketPLC.Error += (new DMSWinsockControlEvents_ErrorEventHandler(this.Socket_Error));
			this.SocketPLC.DataArrival += (new DMSWinsockControlEvents_DataArrivalEventHandler(this.Socket_DataArrival));
			this.SocketPLC.ConnectEvent += (new EventHandler(this.Socket_ConnectEvent));
			this.SocketPLC.CloseEvent += (new EventHandler(this.Socket_CloseEvent));
			this.tabPageSorter.Controls.Add(this.pnlSorter);
			this.tabPageSorter.Location = new Point(4, 22);
			this.tabPageSorter.Name = "tabPageSorter";
			this.tabPageSorter.Padding = new Padding(3);
			this.tabPageSorter.Size = new Size(940, 155);
			this.tabPageSorter.TabIndex = 2;
			this.tabPageSorter.Text = "Configuração do Sorter";
			this.tabPageSorter.UseVisualStyleBackColor = true;
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
			this.pnlSorter.Location = new Point(6, 6);
			this.pnlSorter.Name = "pnlSorter";
			this.pnlSorter.Size = new Size(928, 143);
			this.pnlSorter.TabIndex = 0;
			this.cboSaidaAnvisa.FormattingEnabled = true;
			this.cboSaidaAnvisa.Items.AddRange(new object[]
			{
				"01",
				"02",
				"03",
				"04",
				"05"
			});
			this.cboSaidaAnvisa.Location = new Point(697, 77);
			this.cboSaidaAnvisa.Name = "cboSaidaAnvisa";
			this.cboSaidaAnvisa.Size = new Size(93, 21);
			this.cboSaidaAnvisa.TabIndex = 121;
			this.cboSaidaAnvisa.Text = "03";
			this.label13.AutoSize = true;
			this.label13.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label13.Location = new Point(694, 56);
			this.label13.Name = "label13";
			this.label13.Size = new Size(96, 17);
			this.label13.TabIndex = 120;
			this.label13.Text = "Anvisa saída?";
			this.cboSaidaRejeito.FormattingEnabled = true;
			this.cboSaidaRejeito.Items.AddRange(new object[]
			{
				"01",
				"02",
				"03",
				"04",
				"05"
			});
			this.cboSaidaRejeito.Location = new Point(697, 28);
			this.cboSaidaRejeito.Name = "cboSaidaRejeito";
			this.cboSaidaRejeito.Size = new Size(93, 21);
			this.cboSaidaRejeito.TabIndex = 119;
			this.cboSaidaRejeito.Text = "01";
			this.label12.AutoSize = true;
			this.label12.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label12.Location = new Point(694, 7);
			this.label12.Name = "label12";
			this.label12.Size = new Size(98, 17);
			this.label12.TabIndex = 118;
			this.label12.Text = "Rejeito saída?";
			this.btnPcaDelS5.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaDelS5.Location = new Point(637, 27);
			this.btnPcaDelS5.Name = "btnPcaDelS5";
			this.btnPcaDelS5.Size = new Size(31, 23);
			this.btnPcaDelS5.TabIndex = 116;
			this.btnPcaDelS5.Text = "-";
			this.btnPcaDelS5.UseVisualStyleBackColor = true;
			this.btnPcaDelS5.Click += new EventHandler(this.btnPcaDel_Click);
			this.btnPcaAddS5.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaAddS5.Location = new Point(605, 27);
			this.btnPcaAddS5.Name = "btnPcaAddS5";
			this.btnPcaAddS5.Size = new Size(31, 23);
			this.btnPcaAddS5.TabIndex = 115;
			this.btnPcaAddS5.Text = "+";
			this.btnPcaAddS5.UseVisualStyleBackColor = true;
			this.btnPcaAddS5.Click += new EventHandler(this.btnPcaAdd_Click);
			this.label11.AutoSize = true;
			this.label11.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label11.Location = new Point(566, 7);
			this.label11.Name = "label11";
			this.label11.Size = new Size(108, 17);
			this.label11.TabIndex = 114;
			this.label11.Text = "Praças Saída 5:";
			this.lstPcaSaida5.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lstPcaSaida5.FormattingEnabled = true;
			this.lstPcaSaida5.ItemHeight = 18;
			this.lstPcaSaida5.Location = new Point(569, 56);
			this.lstPcaSaida5.Name = "lstPcaSaida5";
			this.lstPcaSaida5.Size = new Size(99, 76);
			this.lstPcaSaida5.TabIndex = 113;
			this.txtPcaS5.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPcaS5.Location = new Point(569, 27);
			this.txtPcaS5.Name = "txtPcaS5";
			this.txtPcaS5.Size = new Size(30, 23);
			this.txtPcaS5.TabIndex = 112;
			this.lstPcaSaida1.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lstPcaSaida1.FormattingEnabled = true;
			this.lstPcaSaida1.ItemHeight = 18;
			this.lstPcaSaida1.Location = new Point(24, 56);
			this.lstPcaSaida1.Name = "lstPcaSaida1";
			this.lstPcaSaida1.Size = new Size(99, 76);
			this.lstPcaSaida1.Sorted = true;
			this.lstPcaSaida1.TabIndex = 93;
			this.btnPcaDelS4.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaDelS4.Location = new Point(505, 27);
			this.btnPcaDelS4.Name = "btnPcaDelS4";
			this.btnPcaDelS4.Size = new Size(31, 23);
			this.btnPcaDelS4.TabIndex = 111;
			this.btnPcaDelS4.Text = "-";
			this.btnPcaDelS4.UseVisualStyleBackColor = true;
			this.btnPcaDelS4.Click += new EventHandler(this.btnPcaDel_Click);
			this.btnPcaAddS4.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaAddS4.Location = new Point(473, 27);
			this.btnPcaAddS4.Name = "btnPcaAddS4";
			this.btnPcaAddS4.Size = new Size(31, 23);
			this.btnPcaAddS4.TabIndex = 110;
			this.btnPcaAddS4.Text = "+";
			this.btnPcaAddS4.UseVisualStyleBackColor = true;
			this.btnPcaAddS4.Click += new EventHandler(this.btnPcaAdd_Click);
			this.label10.AutoSize = true;
			this.label10.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label10.Location = new Point(434, 7);
			this.label10.Name = "label10";
			this.label10.Size = new Size(108, 17);
			this.label10.TabIndex = 109;
			this.label10.Text = "Praças Saída 4:";
			this.lstPcaSaida4.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lstPcaSaida4.FormattingEnabled = true;
			this.lstPcaSaida4.ItemHeight = 18;
			this.lstPcaSaida4.Location = new Point(437, 56);
			this.lstPcaSaida4.Name = "lstPcaSaida4";
			this.lstPcaSaida4.Size = new Size(99, 76);
			this.lstPcaSaida4.TabIndex = 108;
			this.txtPcaS4.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPcaS4.Location = new Point(437, 27);
			this.txtPcaS4.Name = "txtPcaS4";
			this.txtPcaS4.Size = new Size(30, 23);
			this.txtPcaS4.TabIndex = 107;
			this.btnPcaDelS3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaDelS3.Location = new Point(369, 27);
			this.btnPcaDelS3.Name = "btnPcaDelS3";
			this.btnPcaDelS3.Size = new Size(31, 23);
			this.btnPcaDelS3.TabIndex = 106;
			this.btnPcaDelS3.Text = "-";
			this.btnPcaDelS3.UseVisualStyleBackColor = true;
			this.btnPcaDelS3.Click += new EventHandler(this.btnPcaDel_Click);
			this.btnPcaAddS3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaAddS3.Location = new Point(337, 27);
			this.btnPcaAddS3.Name = "btnPcaAddS3";
			this.btnPcaAddS3.Size = new Size(31, 23);
			this.btnPcaAddS3.TabIndex = 105;
			this.btnPcaAddS3.Text = "+";
			this.btnPcaAddS3.UseVisualStyleBackColor = true;
			this.btnPcaAddS3.Click += new EventHandler(this.btnPcaAdd_Click);
			this.label9.AutoSize = true;
			this.label9.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label9.Location = new Point(298, 7);
			this.label9.Name = "label9";
			this.label9.Size = new Size(108, 17);
			this.label9.TabIndex = 104;
			this.label9.Text = "Praças Saída 3:";
			this.lstPcaSaida3.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lstPcaSaida3.FormattingEnabled = true;
			this.lstPcaSaida3.ItemHeight = 18;
			this.lstPcaSaida3.Location = new Point(301, 56);
			this.lstPcaSaida3.Name = "lstPcaSaida3";
			this.lstPcaSaida3.Size = new Size(99, 76);
			this.lstPcaSaida3.TabIndex = 103;
			this.txtPcaS3.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPcaS3.Location = new Point(301, 27);
			this.txtPcaS3.Name = "txtPcaS3";
			this.txtPcaS3.Size = new Size(30, 23);
			this.txtPcaS3.TabIndex = 102;
			this.btnPcaDelS2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaDelS2.Location = new Point(230, 27);
			this.btnPcaDelS2.Name = "btnPcaDelS2";
			this.btnPcaDelS2.Size = new Size(31, 23);
			this.btnPcaDelS2.TabIndex = 101;
			this.btnPcaDelS2.Text = "-";
			this.btnPcaDelS2.UseVisualStyleBackColor = true;
			this.btnPcaDelS2.Click += new EventHandler(this.btnPcaDel_Click);
			this.btnPcaAddS2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaAddS2.Location = new Point(198, 27);
			this.btnPcaAddS2.Name = "btnPcaAddS2";
			this.btnPcaAddS2.Size = new Size(31, 23);
			this.btnPcaAddS2.TabIndex = 100;
			this.btnPcaAddS2.Text = "+";
			this.btnPcaAddS2.UseVisualStyleBackColor = true;
			this.btnPcaAddS2.Click += new EventHandler(this.btnPcaAdd_Click);
			this.label8.AutoSize = true;
			this.label8.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label8.Location = new Point(159, 7);
			this.label8.Name = "label8";
			this.label8.Size = new Size(108, 17);
			this.label8.TabIndex = 99;
			this.label8.Text = "Praças Saída 2:";
			this.lstPcaSaida2.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lstPcaSaida2.FormattingEnabled = true;
			this.lstPcaSaida2.ItemHeight = 18;
			this.lstPcaSaida2.Location = new Point(162, 56);
			this.lstPcaSaida2.Name = "lstPcaSaida2";
			this.lstPcaSaida2.Size = new Size(99, 76);
			this.lstPcaSaida2.TabIndex = 98;
			this.txtPcaS2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPcaS2.Location = new Point(162, 27);
			this.txtPcaS2.Name = "txtPcaS2";
			this.txtPcaS2.Size = new Size(30, 23);
			this.txtPcaS2.TabIndex = 97;
			this.btnPcaDelS1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaDelS1.Location = new Point(92, 27);
			this.btnPcaDelS1.Name = "btnPcaDelS1";
			this.btnPcaDelS1.Size = new Size(31, 23);
			this.btnPcaDelS1.TabIndex = 96;
			this.btnPcaDelS1.Text = "-";
			this.btnPcaDelS1.UseVisualStyleBackColor = true;
			this.btnPcaDelS1.Click += new EventHandler(this.btnPcaDel_Click);
			this.btnPcaAddS1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnPcaAddS1.Location = new Point(60, 27);
			this.btnPcaAddS1.Name = "btnPcaAddS1";
			this.btnPcaAddS1.Size = new Size(31, 23);
			this.btnPcaAddS1.TabIndex = 95;
			this.btnPcaAddS1.Text = "+";
			this.btnPcaAddS1.UseVisualStyleBackColor = true;
			this.btnPcaAddS1.Click += new EventHandler(this.btnPcaAdd_Click);
			this.label7.AutoSize = true;
			this.label7.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label7.Location = new Point(21, 7);
			this.label7.Name = "label7";
			this.label7.Size = new Size(108, 17);
			this.label7.TabIndex = 94;
			this.label7.Text = "Praças Saída 1:";
			this.txtPcaS1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.txtPcaS1.Location = new Point(24, 27);
			this.txtPcaS1.Name = "txtPcaS1";
			this.txtPcaS1.Size = new Size(30, 23);
			this.txtPcaS1.TabIndex = 92;
			this.tabPageLog.Controls.Add(this.chkExibirErroDestCx);
			this.tabPageLog.Controls.Add(this.chkExibirDestinoPLC4);
			this.tabPageLog.Controls.Add(this.chkLogLeiturasConsolidadaResumida);
			this.tabPageLog.Controls.Add(this.btnLimpar);
			this.tabPageLog.Controls.Add(this.btnSalvarLog);
			this.tabPageLog.Controls.Add(this.txtDestinoArquivos);
			this.tabPageLog.Controls.Add(this.chkExibirErroCnn);
			this.tabPageLog.Controls.Add(this.chkLogLeiturasConsolidada);
			this.tabPageLog.Controls.Add(this.label3);
			this.tabPageLog.Controls.Add(this.txtMaskArquivoLeitura);
			this.tabPageLog.Controls.Add(this.chkLogLeituras);
			this.tabPageLog.Controls.Add(this.label5);
			this.tabPageLog.Location = new Point(4, 22);
			this.tabPageLog.Name = "tabPageLog";
			this.tabPageLog.Padding = new Padding(3);
			this.tabPageLog.Size = new Size(940, 155);
			this.tabPageLog.TabIndex = 1;
			this.tabPageLog.Text = "Log de Eventos";
			this.tabPageLog.UseVisualStyleBackColor = true;
			this.chkExibirErroDestCx.AutoSize = true;
			this.chkExibirErroDestCx.Checked = true;
			this.chkExibirErroDestCx.CheckState = CheckState.Checked;
			this.chkExibirErroDestCx.Location = new Point(503, 82);
			this.chkExibirErroDestCx.Name = "chkExibirErroDestCx";
			this.chkExibirErroDestCx.Size = new Size(167, 17);
			this.chkExibirErroDestCx.TabIndex = 96;
			this.chkExibirErroDestCx.Text = "Exibir erro de destino de caixa";
			this.chkExibirErroDestCx.UseVisualStyleBackColor = true;
			this.chkExibirDestinoPLC4.AutoSize = true;
			this.chkExibirDestinoPLC4.Checked = true;
			this.chkExibirDestinoPLC4.CheckState = CheckState.Checked;
			this.chkExibirDestinoPLC4.Location = new Point(258, 115);
			this.chkExibirDestinoPLC4.Name = "chkExibirDestinoPLC4";
			this.chkExibirDestinoPLC4.Size = new Size(139, 17);
			this.chkExibirDestinoPLC4.TabIndex = 95;
			this.chkExibirDestinoPLC4.Text = "Exibir destino caixa PLC";
			this.chkExibirDestinoPLC4.UseVisualStyleBackColor = true;
			this.chkLogLeiturasConsolidadaResumida.AutoSize = true;
			this.chkLogLeiturasConsolidadaResumida.Checked = true;
			this.chkLogLeiturasConsolidadaResumida.CheckState = CheckState.Checked;
			this.chkLogLeiturasConsolidadaResumida.Location = new Point(258, 82);
			this.chkLogLeiturasConsolidadaResumida.Name = "chkLogLeiturasConsolidadaResumida";
			this.chkLogLeiturasConsolidadaResumida.Size = new Size(197, 17);
			this.chkLogLeiturasConsolidadaResumida.TabIndex = 94;
			this.chkLogLeiturasConsolidadaResumida.Text = "Exibir leituras consolidadas resumida";
			this.chkLogLeiturasConsolidadaResumida.UseVisualStyleBackColor = true;
			this.chkLogLeiturasConsolidadaResumida.CheckedChanged += new EventHandler(this.chkLogLeituras_CheckedChanged);
			this.btnLimpar.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnLimpar.Location = new Point(770, 86);
			this.btnLimpar.Name = "btnLimpar";
			this.btnLimpar.Size = new Size(144, 35);
			this.btnLimpar.TabIndex = 93;
			this.btnLimpar.Text = "Limpar Log";
			this.btnLimpar.UseVisualStyleBackColor = true;
			this.btnLimpar.Click += new EventHandler(this.btnLimpar_Click);
			this.btnSalvarLog.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.btnSalvarLog.Location = new Point(770, 39);
			this.btnSalvarLog.Name = "btnSalvarLog";
			this.btnSalvarLog.Size = new Size(144, 35);
			this.btnSalvarLog.TabIndex = 92;
			this.btnSalvarLog.Text = "Salvar";
			this.btnSalvarLog.UseVisualStyleBackColor = true;
			this.btnSalvarLog.Click += new EventHandler(this.btnSalvarLog_Click);
			this.timerFilaLeituras.Enabled = true;
			this.timerFilaLeituras.Interval = 80;
			this.timerFilaLeituras.Tick += new EventHandler(this.timerFilaLeituras_Tick);
			this.pictureBox3.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.pictureBox3.Image = Resources.topToledo;
			this.pictureBox3.Location = new Point(737, -1);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new Size(245, 60);
			this.pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox3.TabIndex = 95;
			this.pictureBox3.TabStop = false;
			this.pictureBox1.Dock = DockStyle.Top;
			this.pictureBox1.Image = Resources.topTitle;
			this.pictureBox1.Location = new Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(973, 60);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 91;
			this.pictureBox1.TabStop = false;
			this.chkDesconectarSeFalhaNaFila.AutoSize = true;
			this.chkDesconectarSeFalhaNaFila.Checked = true;
			this.chkDesconectarSeFalhaNaFila.CheckState = CheckState.Checked;
			this.chkDesconectarSeFalhaNaFila.Location = new Point(697, 111);
			this.chkDesconectarSeFalhaNaFila.Name = "chkDesconectarSeFalhaNaFila";
			this.chkDesconectarSeFalhaNaFila.Size = new Size(158, 17);
			this.chkDesconectarSeFalhaNaFila.TabIndex = 122;
			this.chkDesconectarSeFalhaNaFila.Text = "Desconectar se falha na fila";
			this.chkDesconectarSeFalhaNaFila.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.ControlDark;
			base.ClientSize = new Size(973, 662);
			base.Controls.Add(this.pictureBox3);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.rchLog);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "frmToledo";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Studio Log :: Sorter Manager";
			base.FormClosing += new FormClosingEventHandler(this.frmToledo_FormClosing);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPageDisp.ResumeLayout(false);
			this.tabPageDisp.PerformLayout();
			this.SocketScanner.EndInit();
			this.SocketBalanca.EndInit();
			this.SocketPortal.EndInit();
			this.SocketPLC.EndInit();
			this.tabPageSorter.ResumeLayout(false);
			this.pnlSorter.ResumeLayout(false);
			this.pnlSorter.PerformLayout();
			this.tabPageLog.ResumeLayout(false);
			this.tabPageLog.PerformLayout();
			((ISupportInitialize)this.pictureBox3).EndInit();
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
