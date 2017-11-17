using System;
using System.Drawing;

public static class StringExtensions
{
	public static string Space(this string str, int count)
	{
		return str.PadLeft(count);
	}

	public static string Space(int count)
	{
		return "".PadLeft(count);
	}

	public static byte[] GetBytes(this string str)
	{
		byte[] array = new byte[str.Length * 2];
		Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
		return array;
	}

	public static string GetString(byte[] bytes)
	{
		char[] array = new char[bytes.Length / 2];
		Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
		return new string(array);
	}

	public static int ToInt32(this string str)
	{
		int result;
		try
		{
			result = Convert.ToInt32(str);
		}
		catch
		{
			result = 0;
		}
		return result;
	}

	public static long ToInt64(this string str)
	{
		return Convert.ToInt64(str);
	}

	public static Color FromHexToColor(this string hexValue)
	{
		return ColorTranslator.FromHtml(hexValue);
	}
}
