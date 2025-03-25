using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000297 RID: 663
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0004D9E4 File Offset: 0x0004BDE4
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x0004D9EC File Offset: 0x0004BDEC
		public T model { get; internal set; }

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0004D9F5 File Offset: 0x0004BDF5
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0004DA05 File Offset: 0x0004BE05
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
