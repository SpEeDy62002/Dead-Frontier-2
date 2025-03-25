using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200026E RID: 622
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x0005370D File Offset: 0x00051B0D
		// (set) Token: 0x06000C65 RID: 3173 RVA: 0x00053715 File Offset: 0x00051B15
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x06000C66 RID: 3174 RVA: 0x0005371E File Offset: 0x00051B1E
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x04000F22 RID: 3874
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x0200026F RID: 623
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000174 RID: 372
			// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0005372C File Offset: 0x00051B2C
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x04000F23 RID: 3875
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x04000F24 RID: 3876
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
