using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000230 RID: 560
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x0004D9B6 File Offset: 0x0004BDB6
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04000DE9 RID: 3561
		public readonly string method;
	}
}
