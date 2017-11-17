using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public static class RichTextBoxExtension
{
	private const int lineOffSet = 4;

	private const int fontSize = 16;

	public const string DateTimeFormat = "MM/dd/yyyy hh:mm:ss.fff tt";

	public static void AppendText(this RichTextBox control, string text, Color forecolor)
	{
		control.AppendText(text, forecolor, Color.White);
	}

	public static void AppendText(this RichTextBox control, string text, Color forecolor, Color backcolor)
	{
		control.SelectionStart = control.TextLength;
		control.SelectionLength = 0;
		control.SelectionCharOffset = 4;
		control.SelectionColor = forecolor;
		control.SelectionBackColor = backcolor;
		char[] array = text.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			control.AppendText(c.ToString());
		}
		control.SelectionColor = control.ForeColor;
	}

	public static void AppendSelection(this RichTextBox control, string text)
	{
		int textLength = control.TextLength;
		control.AppendText(text);
		control.SelectionStart = textLength;
		control.SelectionLength = text.Length;
		control.SelectionCharOffset = 4;
		control.ScrollToCaret();
	}

	public static string AppendSelection(this RichTextBox control, string text, Color colour, Font font)
	{
		control.AppendSelection(text);
		control.SelectionColor = colour;
		control.SelectionBackColor = Color.White;
		control.SelectionFont = font;
		control.SelectionCharOffset = 4;
		RichTextBoxExtension.replaceAscii(control);
		return text;
	}

	public static string AppendSelection(this RichTextBox control, string text, Color colour, Color backColour, Font font)
	{
		control.AppendSelection(text);
		control.SelectionColor = colour;
		control.SelectionBackColor = backColour;
		control.SelectionFont = font;
		control.SelectionCharOffset = 4;
		RichTextBoxExtension.replaceAscii(control);
		return text;
	}

	private static void replaceAscii(RichTextBox control)
	{
	}

	public static void ReplaceAll_ASCII(RichTextBox myRtb, string word, string replacement)
	{
		int num = 0;
		int num2 = 0;
		int num3 = replacement.Length - word.Length;
		foreach (Match match in Regex.Matches(myRtb.Text, word))
		{
			myRtb.Select(match.Index + num, word.Length);
			num += num3;
			myRtb.SelectedText = replacement + " ";
			myRtb.Select(match.Index, replacement.Length);
			myRtb.SelectionColor = Color.White;
			myRtb.SelectionBackColor = Color.Gray;
			myRtb.SelectionFont = new Font(new FontFamily("Arial Rounded MT Bold"), myRtb.Font.Size, FontStyle.Bold);
			num2++;
		}
	}

	public static void AppendLog(this RichTextBox control, string text, Color colour, Font font)
	{
		Action action = delegate
		{
			control.AppendSelection(text, colour, font);
		};
		if (control.InvokeRequired)
		{
			control.Invoke(action);
			return;
		}
		action();
	}

	public static void AppendLog(this RichTextBox control, string text, Color colour, Color bgColor, Font font)
	{
		Action action = delegate
		{
			control.AppendSelection(text, colour, bgColor, font);
		};
		if (control.InvokeRequired)
		{
			control.Invoke(action);
			return;
		}
		action();
	}

	public static string GetDateTime()
	{
		return RichTextBoxExtension.GetDateTime(DateTime.Now);
	}

	public static string GetDateTime(DateTime now)
	{
		return now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
	}

	public static void AppendError(this RichTextBox control, string text, string source = "")
	{
		Action action = delegate
		{
			control.AppendSelection(string.Concat(new string[]
			{
				DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"),
				" | ",
				source,
				" <<- ",
				text,
				"\n"
			}), Color.Red, Color.Yellow, new Font("Courrier New", 16f, FontStyle.Italic));
		};
		if (control.InvokeRequired)
		{
			control.Invoke(action);
			return;
		}
		action();
	}

	public static void AppendSendMessage(this RichTextBox control, string text, string source = "")
	{
		control.AppendSendMessage(text, Color.Orange, source);
	}

	public static void AppendSendMessage(this RichTextBox control, string text, Color foreColor, string source = "")
	{
		Action action = delegate
		{
			control.AppendSelection(string.Concat(new string[]
			{
				DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"),
				" | ",
				source,
				" ->> ",
				text,
				"\n"
			}), foreColor, new Font("Courrier New", 16f));
		};
		if (control.InvokeRequired)
		{
			control.Invoke(action);
			return;
		}
		action();
	}

	public static void AppendReceiveMessage(this RichTextBox control, string text, string source = "")
	{
		Action action = delegate
		{
			control.AppendSelection(string.Concat(new string[]
			{
				DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"),
				" | ",
				source,
				" <<- ",
				text,
				"\n"
			}), Color.DarkBlue, new Font("Courrier New", 16f));
		};
		if (control.InvokeRequired)
		{
			control.Invoke(action);
			return;
		}
		action();
	}
}
