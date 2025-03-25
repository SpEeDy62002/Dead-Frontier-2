using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000283 RID: 643
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00053E3E File Offset: 0x0005223E
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x00053E46 File Offset: 0x00052246
		public FogModel.Settings settings
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

		// Token: 0x06000C8B RID: 3211 RVA: 0x00053E4F File Offset: 0x0005224F
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04000F76 RID: 3958
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x02000284 RID: 644
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000187 RID: 391
			// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00053E5C File Offset: 0x0005225C
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04000F77 RID: 3959
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
