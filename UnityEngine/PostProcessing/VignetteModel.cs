using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000292 RID: 658
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x000540C9 File Offset: 0x000524C9
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x000540D1 File Offset: 0x000524D1
		public VignetteModel.Settings settings
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

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000540DA File Offset: 0x000524DA
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x04000F9B RID: 3995
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x02000293 RID: 659
		public enum Mode
		{
			// Token: 0x04000F9D RID: 3997
			Classic,
			// Token: 0x04000F9E RID: 3998
			Masked
		}

		// Token: 0x02000294 RID: 660
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000191 RID: 401
			// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x000540E8 File Offset: 0x000524E8
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x04000F9F RID: 3999
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use \"Round\" to get a perfectly round vignette no matter what the aspect ratio is. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x04000FA0 RID: 4000
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x04000FA1 RID: 4001
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x04000FA2 RID: 4002
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x04000FA3 RID: 4003
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x04000FA4 RID: 4004
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04000FA5 RID: 4005
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04000FA6 RID: 4006
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04000FA7 RID: 4007
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
