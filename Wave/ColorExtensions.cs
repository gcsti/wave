using System;
using System.Drawing;

public static class ColorExtensions
{
	public static Color FromHex(this Color color, string hexValue)
	{
		return ColorTranslator.FromHtml(hexValue);
	}

	public static Color GetContrastingColor(this Color value)
	{
		int num;
		if (1.0 - (0.299 * (double)value.R + 0.587 * (double)value.G + 0.114 * (double)value.B) / 255.0 < 0.5)
		{
			num = 0;
		}
		else
		{
			num = 255;
		}
		return Color.FromArgb(num, num, num);
	}
}
