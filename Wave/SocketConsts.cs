using System;

namespace AxMSWinsockLib
{
	public static class SocketConsts
	{
		public const int sckClosed = 0;

		public const int sckOpen = 1;

		public const int sckListening = 2;

		public const int sckConnectionPending = 3;

		public const int sckResolvingHost = 4;

		public const int sckHostResolved = 5;

		public const int sckConnecting = 6;

		public const int sckConnected = 7;

		public const int sckClosing = 8;

		public const int sckError = 9;
	}
}
