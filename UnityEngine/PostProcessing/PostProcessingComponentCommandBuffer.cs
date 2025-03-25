using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000298 RID: 664
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000CC3 RID: 3267
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x06000CC4 RID: 3268
		public abstract string GetName();

		// Token: 0x06000CC5 RID: 3269
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
