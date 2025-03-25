using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Token: 0x02000003 RID: 3
public class BinarySerialization
{
	// Token: 0x06000005 RID: 5 RVA: 0x0000217C File Offset: 0x0000057C
	public static string SerializeObject(object o)
	{
		if (!o.GetType().IsSerializable)
		{
			return null;
		}
		string text;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			new BinaryFormatter().Serialize(memoryStream, o);
			text = Convert.ToBase64String(memoryStream.ToArray());
		}
		return text;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000021DC File Offset: 0x000005DC
	public static object DeserializeObject(string str)
	{
		if (str == "{}")
		{
			return new Dictionary<string, object>();
		}
		byte[] array = Convert.FromBase64String(str);
		object obj;
		using (MemoryStream memoryStream = new MemoryStream(array))
		{
			obj = new BinaryFormatter().Deserialize(memoryStream);
		}
		return obj;
	}
}
