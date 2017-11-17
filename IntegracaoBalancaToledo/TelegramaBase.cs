using System;
using System.Text.RegularExpressions;

namespace IntegracaoBalancaToledo
{
	public class TelegramaBase
	{
		public const char STX = '\u0002';

		public const char ETX = '\u0003';

		public const char EOT = '\u0004';

		public const char ENQ = '\u0005';

		public const char ACK = '\u0006';

		public const char NAK = '\u0016';

		public static string RemoverCaracteresEspeciais(string mensagem)
		{
			return Regex.Replace(mensagem, "[\\u0000-\\u001F]", string.Empty).Trim();
		}

		public static string IncluirCaracteresInicioFimDeTexto(string mensagem)
		{
			return "\u0002" + mensagem + "\u0003";
		}
	}
}
