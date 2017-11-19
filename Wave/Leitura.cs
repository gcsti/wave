using System;
using System.Collections.Generic;

namespace Wave
{
	public class Leitura
	{
		public static List<ReceivingEventArgs> Balanca
		{
			get;
			set;
		}

		public static List<ReceivingEventArgs> Portal
		{
			get;
			set;
		}

		public static List<ReceivingEventArgs> Scanner
		{
			get;
			set;
		}

		public static string UltimaLeituraConsolidada
		{
			get;
			set;
		}

		public static bool MultiplasLeiturasScanner
		{
			get;
			set;
		}

		public static bool HouveFalhaNaLeitura()
		{
			return Leitura.HouveFalhaNaLeitura(0);
		}

		public static bool HouveFalhaNaLeitura(int index)
		{
			bool result = false;
			if (Leitura.Balanca.Count > 0 && Leitura.Balanca[index].FalhaDeLeitura)
			{
				result = true;
			}
			if (Leitura.Portal.Count > 0 && Leitura.Portal[index].FalhaDeLeitura)
			{
				result = true;
			}
			if (Leitura.Scanner.Count > 0 && Leitura.Scanner[index].FalhaDeLeitura)
			{
				result = true;
			}
			return result;
		}

		public static bool HouveFalhaNaLeituraConsolidada()
		{
			return Leitura.HouveFalhaNaLeituraConsolidada(Leitura.UltimaLeituraConsolidada);
		}

		public static bool HouveFalhaNaLeituraConsolidada(string leitura)
		{
			bool result = Leitura.MultiplasLeiturasScanner;
			if (leitura.Contains("noread"))
			{
				result = true;
			}
			if (leitura.Contains("A"))
			{
				result = true;
			}
			if (leitura.Trim().EndsWith(";"))
			{
				result = true;
			}
			return result;
		}

		public static bool HouveFalhaNaFila()
		{
			return Leitura.Scanner.Count > 0 | Leitura.Portal.Count > 0 | Leitura.Balanca.Count > 0;
		}

		public new static string ToString()
		{

           // System.IO.File.WriteAllText(@"C:\Logstore\LEITURAINICIO.txt", DateTime.Now.ToString());

            string text = "{0};{1};{2};{3};{4};{5}";
			if (Leitura.Scanner.Count > 0 & Leitura.Portal.Count > 0 & Leitura.Balanca.Count > 0)
			{

                try
                {
                    //string log = string.Format("{0}|||{1}|||{2}", Leitura.Scanner[0].DataString, Leitura.Balanca[0].DataString, Leitura.Portal[0].DataString);

                    //string log1 = string.Format("{0}|||{1}|||{2}", Leitura.Scanner[0].DataStringNoSpecialChars, Leitura.Balanca[0].DataStringNoSpecialChars, Leitura.Portal[0].DataStringNoSpecialChars);

                    //string log2 = string.Format("{0}|||{1}|||{2}", Leitura.Scanner[0].DataBytes, Leitura.Balanca[0].DataBytes, Leitura.Portal[0].DataBytes);
                    //System.IO.File.WriteAllText(@"C:\Logstore\Log.txt", log +  "|||" + DateTime.Now.ToString());
                    //System.IO.File.WriteAllText(@"C:\Logstore\Log1.txt", log1 + "|||" + DateTime.Now.ToString());
                    //System.IO.File.WriteAllText(@"C:\Logstore\Log2.txt", log2 + "|||" + DateTime.Now.ToString());

                }
                catch
                {

                }


                string text2 = TelegramaBase.RemoverCaracteresEspeciais(Leitura.Scanner[0].DataStringNoSpecialChars);
				string text3 = TelegramaBase.RemoverCaracteresEspeciais(Leitura.Balanca[0].DataStringNoSpecialChars);
				if (!Leitura.Portal[0].FalhaDeLeitura)
				{
					string expr_85 = Leitura.Portal[0].DataStringNoSpecialChars;
					string text4 = ((double)expr_85.Substring(3, 4).ToInt32() / 10.0).ToString().Replace(",", ".");
					string text5 = ((double)expr_85.Substring(7, 4).ToInt32() / 10.0).ToString().Replace(",", ".");
					string text6 = ((double)expr_85.Substring(11, 4).ToInt32() / 10.0).ToString().Replace(",", ".");
					string text7 = ((double)expr_85.Substring(15, 8).ToInt32() / 10.0).ToString().Replace(",", ".");
					text = string.Format(text, new object[]
					{
						text2,
						text4,
						text5,
						text6,
						text7,
						text3
					});
				}
				else
				{
					text = string.Format("{0};{1};{2}", text2, Leitura.Portal[0].DataStringNoSpecialChars, text3);
				}
				Leitura.Scanner.Remove(Leitura.Scanner[0]);
				Leitura.Portal.Remove(Leitura.Portal[0]);
				Leitura.Balanca.Remove(Leitura.Balanca[0]);
				Leitura.MultiplasLeiturasScanner = (Leitura.Scanner.Count > 0);
				Leitura.UltimaLeituraConsolidada = text;

               // System.IO.File.WriteAllText(@"C:\Logstore\LEITURAFIM.txt", DateTime.Now.ToString());
                return text;
			}
			return string.Empty;
		}

		public static void Limpar()
		{
			Leitura.Scanner = new List<ReceivingEventArgs>();
			Leitura.Portal = new List<ReceivingEventArgs>();
			Leitura.Balanca = new List<ReceivingEventArgs>();
			Leitura.MultiplasLeiturasScanner = false;
			GC.Collect();
		}
	}
}
