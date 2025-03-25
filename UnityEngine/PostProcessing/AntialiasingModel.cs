using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200025D RID: 605
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00053162 File Offset: 0x00051562
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x0005316A File Offset: 0x0005156A
		public AntialiasingModel.Settings settings
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

		// Token: 0x06000C4B RID: 3147 RVA: 0x00053173 File Offset: 0x00051573
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x04000EE7 RID: 3815
		[SerializeField]
		private AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x0200025E RID: 606
		public enum Method
		{
			// Token: 0x04000EE9 RID: 3817
			Fxaa,
			// Token: 0x04000EEA RID: 3818
			Taa
		}

		// Token: 0x0200025F RID: 607
		public enum FxaaPreset
		{
			// Token: 0x04000EEC RID: 3820
			ExtremePerformance,
			// Token: 0x04000EED RID: 3821
			Performance,
			// Token: 0x04000EEE RID: 3822
			Default,
			// Token: 0x04000EEF RID: 3823
			Quality,
			// Token: 0x04000EF0 RID: 3824
			ExtremeQuality
		}

		// Token: 0x02000260 RID: 608
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x04000EF1 RID: 3825
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x04000EF2 RID: 3826
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x04000EF3 RID: 3827
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x04000EF4 RID: 3828
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x02000261 RID: 609
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x04000EF5 RID: 3829
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x04000EF6 RID: 3830
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x04000EF7 RID: 3831
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x04000EF8 RID: 3832
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x04000EF9 RID: 3833
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x02000262 RID: 610
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x17000166 RID: 358
			// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0005342C File Offset: 0x0005182C
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x04000EFA RID: 3834
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x02000263 RID: 611
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x17000167 RID: 359
			// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0005344C File Offset: 0x0005184C
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x04000EFB RID: 3835
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x04000EFC RID: 3836
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x04000EFD RID: 3837
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x04000EFE RID: 3838
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x02000264 RID: 612
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000168 RID: 360
			// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00053494 File Offset: 0x00051894
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000EFF RID: 3839
			public AntialiasingModel.Method method;

			// Token: 0x04000F00 RID: 3840
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x04000F01 RID: 3841
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}
