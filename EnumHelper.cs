using System;

public static class EnumHelper
{
	public static T GetEnumValue<T>(string str) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new Exception("T must be an Enumeration type.");
		}
		T result;
		if (!Enum.TryParse<T>(str, true, out result))
		{
			return default(T);
		}
		return result;
	}

	public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
	{
		Type expr_0A = typeof(T);
		if (!expr_0A.IsEnum)
		{
			throw new Exception("T must be an Enumeration type. ");
		}
		return (T)((object)Enum.ToObject(expr_0A, intValue));
	}
}
