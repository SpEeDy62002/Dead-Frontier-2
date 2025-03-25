using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000265 RID: 613
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x000534DD File Offset: 0x000518DD
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x000534E5 File Offset: 0x000518E5
		public BloomModel.Settings settings
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

		// Token: 0x06000C54 RID: 3156 RVA: 0x000534EE File Offset: 0x000518EE
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04000F02 RID: 3842
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x02000266 RID: 614
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x1700016A RID: 362
			// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00053509 File Offset: 0x00051909
			// (set) Token: 0x06000C55 RID: 3157 RVA: 0x000534FB File Offset: 0x000518FB
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00053518 File Offset: 0x00051918
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04000F03 RID: 3843
			[Min(0f)]
			[Tooltip("Blend factor of the result image.")]
			public float intensity;

			// Token: 0x04000F04 RID: 3844
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x04000F05 RID: 3845
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x04000F06 RID: 3846
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04000F07 RID: 3847
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x02000267 RID: 615
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00053568 File Offset: 0x00051968
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04000F08 RID: 3848
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04000F09 RID: 3849
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x02000268 RID: 616
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00053594 File Offset: 0x00051994
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000F0A RID: 3850
			public BloomModel.BloomSettings bloom;

			// Token: 0x04000F0B RID: 3851
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
