using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000299 RID: 665
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000CC7 RID: 3271 RVA: 0x0004DF3C File Offset: 0x0004C33C
		public virtual void Prepare(Material material)
		{
		}
	}
}
