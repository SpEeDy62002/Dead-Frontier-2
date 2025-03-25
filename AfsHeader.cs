using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class AfsHeader : PropertyAttribute
{
	// Token: 0x06000136 RID: 310 RVA: 0x00009EAC File Offset: 0x000082AC
	public AfsHeader(string labeltext)
	{
		this.labeltext = labeltext;
	}

	// Token: 0x040000BB RID: 187
	public string labeltext;
}
