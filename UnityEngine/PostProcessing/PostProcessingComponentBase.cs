using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000296 RID: 662
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06000CB8 RID: 3256 RVA: 0x0004D9D5 File Offset: 0x0004BDD5
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000CB9 RID: 3257
		public abstract bool active { get; }

		// Token: 0x06000CBA RID: 3258 RVA: 0x0004D9D8 File Offset: 0x0004BDD8
		public virtual void OnEnable()
		{
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0004D9DA File Offset: 0x0004BDDA
		public virtual void OnDisable()
		{
		}

		// Token: 0x06000CBC RID: 3260
		public abstract PostProcessingModel GetModel();

		// Token: 0x04000FC5 RID: 4037
		public PostProcessingContext context;
	}
}
