using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200022F RID: 559
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x06000BA4 RID: 2980 RVA: 0x0004D9A7 File Offset: 0x0004BDA7
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000DE8 RID: 3560
		public readonly float min;
	}
}
