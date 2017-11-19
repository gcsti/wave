using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wave
{
	public class Sorter
	{
		private static List<SorterSaida> pracasPorSaida;

		public static List<SorterSaida> PracasPorSaida
		{
			get
			{
				if (Sorter.pracasPorSaida == null)
				{
					Sorter.pracasPorSaida = new List<SorterSaida>();
				}
				return Sorter.pracasPorSaida;
			}
		}

		public static void Limpar()
		{
			Sorter.PracasPorSaida.Clear();
		}

		public static void Default()
		{
			Sorter.PracasPorSaida.Clear();
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "00",
				Saida = "01"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "02",
				Saida = "01"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "03",
				Saida = "02"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "04",
				Saida = "02"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "07",
				Saida = "02"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "45",
				Saida = "03"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "05",
				Saida = "04"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "01",
				Saida = "05"
			});
			Sorter.PracasPorSaida.Add(new SorterSaida
			{
				Praca = "06",
				Saida = "05"
			});
		}

		public static void ComporRegras(string regra)
		{
			Sorter.Limpar();
			string[] array = regra.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'|'
				});
				Sorter.PracasPorSaida.Add(new SorterSaida
				{
					Praca = array2[0],
					Saida = array2[1]
				});
			}
		}

		public static string ObterRegras()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (SorterSaida current in Sorter.PracasPorSaida)
			{
				if (flag)
				{
					stringBuilder.Append(current.ToString());
					flag = !flag;
				}
				else
				{
					stringBuilder.Append(";" + current.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		public static string ObterDestinoCaixa(string praca)
		{
			SorterSaida sorterSaida = Sorter.PesquisarSaida(praca);
			if (sorterSaida != null)
			{
				return sorterSaida.Saida;
			}
			return "01";
		}

		public static bool AddRegra(string praca, string saida)
		{
			if (Sorter.PracasPorSaida.Exists((SorterSaida p) => p.Praca.Equals(praca)))
			{
				return false;
			}
			SorterSaida item = new SorterSaida
			{
				Praca = praca,
				Saida = saida
			};
			Sorter.PracasPorSaida.Add(item);
			return true;
		}

		public static SorterSaida PesquisarSaida(string praca)
		{
			return (from p in Sorter.PracasPorSaida
			where p.Praca == praca
			select p).FirstOrDefault<SorterSaida>();
		}

		public static bool DelRegra(string praca)
		{
			SorterSaida sorterSaida = Sorter.PesquisarSaida(praca);
			if (sorterSaida != null)
			{
				Sorter.PracasPorSaida.Remove(sorterSaida);
				return true;
			}
			return false;
		}
	}
}
