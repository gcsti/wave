using System;

namespace Wave
{
	public class SorterSaida
	{
		public string Praca
		{
			get;
			set;
		}

		public string Saida
		{
			get;
			set;
		}

		public new string ToString()
		{
			return this.Praca + "|" + this.Saida;
		}
	}
}
