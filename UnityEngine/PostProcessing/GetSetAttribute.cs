using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200022E RID: 558
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x0004D998 File Offset: 0x0004BD98
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04000DE6 RID: 3558
		public readonly string name;

		// Token: 0x04000DE7 RID: 3559
		public bool dirty;
	}
}
