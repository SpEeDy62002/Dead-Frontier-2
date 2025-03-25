using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200029B RID: 667
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x000530A8 File Offset: 0x000514A8
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x000530B0 File Offset: 0x000514B0
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x06000CD5 RID: 3285
		public abstract void Reset();

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000530C5 File Offset: 0x000514C5
		public virtual void OnValidate()
		{
		}

		// Token: 0x04000FCC RID: 4044
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
