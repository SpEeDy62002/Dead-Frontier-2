using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200027E RID: 638
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00053D39 File Offset: 0x00052139
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x00053D41 File Offset: 0x00052141
		public DitheringModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x00053D4A File Offset: 0x0005214A
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x04000F66 RID: 3942
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x0200027F RID: 639
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000183 RID: 387
			// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00053D58 File Offset: 0x00052158
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
