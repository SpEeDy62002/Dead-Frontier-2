using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000290 RID: 656
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0005406D File Offset: 0x0005246D
		// (set) Token: 0x06000C9E RID: 3230 RVA: 0x00054075 File Offset: 0x00052475
		public UserLutModel.Settings settings
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

		// Token: 0x06000C9F RID: 3231 RVA: 0x0005407E File Offset: 0x0005247E
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x04000F98 RID: 3992
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x02000291 RID: 657
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700018F RID: 399
			// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0005408C File Offset: 0x0005248C
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04000F99 RID: 3993
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04000F9A RID: 3994
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
