using System;
using System.Text;

namespace Wave
{
	public class ReceivingEventArgs : EventArgs
	{
		public string DataString
		{
			get;
			set;
		}

		public string DataStringNoSpecialChars
		{
			get;
			set;
		}

		public byte[] DataBytes
		{
			get;
			set;
		}

		public DateTime DateTimeReceived
		{
			get;
			set;
		}

		public string IpAddress
		{
			get;
			set;
		}

		public string HostName
		{
			get;
			set;
		}

		public int Port
		{
			get;
			set;
		}

		public string Dispositivo
		{
			get;
			set;
		}

		public bool FalhaDeLeitura
		{
			get;
			set;
		}

		public new string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("    Dados da Mensagem:");
			stringBuilder.AppendLine("    -------------------");
			stringBuilder.AppendLine("    Mensagem...: " + this.DataString.TrimEnd(new char[]
			{
				'\r',
				'\n'
			}));
			stringBuilder.AppendLine("    Data/Hora..: " + this.DateTimeReceived.ToString("dd/MM/yyyy HH:mm:ss.fff"));
			stringBuilder.AppendLine("    Dispositivo: " + this.Dispositivo.ToString());
			stringBuilder.AppendLine("    HostName...: " + this.HostName);
			stringBuilder.AppendLine("    IpAddress..: " + this.IpAddress);
			stringBuilder.AppendLine("    Port.......: " + this.Port.ToString());
			return stringBuilder.ToString();
		}
	}
}
