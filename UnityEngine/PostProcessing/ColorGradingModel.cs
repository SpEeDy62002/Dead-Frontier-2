using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000270 RID: 624
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00053769 File Offset: 0x00051B69
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x00053771 File Offset: 0x00051B71
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00053780 File Offset: 0x00051B80
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00053788 File Offset: 0x00051B88
		public bool isDirty { get; internal set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00053791 File Offset: 0x00051B91
		// (set) Token: 0x06000C6E RID: 3182 RVA: 0x00053799 File Offset: 0x00051B99
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x06000C6F RID: 3183 RVA: 0x000537A2 File Offset: 0x00051BA2
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x000537B5 File Offset: 0x00051BB5
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x04000F25 RID: 3877
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x02000271 RID: 625
		public enum Tonemapper
		{
			// Token: 0x04000F29 RID: 3881
			None,
			// Token: 0x04000F2A RID: 3882
			ACES,
			// Token: 0x04000F2B RID: 3883
			Neutral
		}

		// Token: 0x02000272 RID: 626
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000C71 RID: 3185 RVA: 0x000537C0 File Offset: 0x00051BC0
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x04000F2C RID: 3884
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x04000F2D RID: 3885
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x04000F2E RID: 3886
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x04000F2F RID: 3887
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x04000F30 RID: 3888
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x04000F31 RID: 3889
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x04000F32 RID: 3890
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02000273 RID: 627
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17000179 RID: 377
			// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00053828 File Offset: 0x00051C28
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x04000F33 RID: 3891
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04000F34 RID: 3892
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x04000F35 RID: 3893
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x04000F36 RID: 3894
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x04000F37 RID: 3895
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x04000F38 RID: 3896
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x02000274 RID: 628
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00053888 File Offset: 0x00051C88
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x04000F39 RID: 3897
			public Vector3 red;

			// Token: 0x04000F3A RID: 3898
			public Vector3 green;

			// Token: 0x04000F3B RID: 3899
			public Vector3 blue;

			// Token: 0x04000F3C RID: 3900
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x02000275 RID: 629
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x1700017B RID: 379
			// (get) Token: 0x06000C74 RID: 3188 RVA: 0x000538F8 File Offset: 0x00051CF8
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x04000F3D RID: 3901
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x04000F3E RID: 3902
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x04000F3F RID: 3903
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x02000276 RID: 630
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x1700017C RID: 380
			// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00053934 File Offset: 0x00051D34
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x04000F40 RID: 3904
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x04000F41 RID: 3905
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x04000F42 RID: 3906
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x02000277 RID: 631
		public enum ColorWheelMode
		{
			// Token: 0x04000F44 RID: 3908
			Linear,
			// Token: 0x04000F45 RID: 3909
			Log
		}

		// Token: 0x02000278 RID: 632
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x1700017D RID: 381
			// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00053970 File Offset: 0x00051D70
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000F46 RID: 3910
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x04000F47 RID: 3911
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x04000F48 RID: 3912
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x02000279 RID: 633
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x1700017E RID: 382
			// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000539A8 File Offset: 0x00051DA8
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x04000F49 RID: 3913
			public ColorGradingCurve master;

			// Token: 0x04000F4A RID: 3914
			public ColorGradingCurve red;

			// Token: 0x04000F4B RID: 3915
			public ColorGradingCurve green;

			// Token: 0x04000F4C RID: 3916
			public ColorGradingCurve blue;

			// Token: 0x04000F4D RID: 3917
			public ColorGradingCurve hueVShue;

			// Token: 0x04000F4E RID: 3918
			public ColorGradingCurve hueVSsat;

			// Token: 0x04000F4F RID: 3919
			public ColorGradingCurve satVSsat;

			// Token: 0x04000F50 RID: 3920
			public ColorGradingCurve lumVSsat;

			// Token: 0x04000F51 RID: 3921
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04000F52 RID: 3922
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04000F53 RID: 3923
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04000F54 RID: 3924
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04000F55 RID: 3925
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x0200027A RID: 634
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700017F RID: 383
			// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00053C58 File Offset: 0x00052058
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000F56 RID: 3926
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x04000F57 RID: 3927
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x04000F58 RID: 3928
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x04000F59 RID: 3929
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x04000F5A RID: 3930
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
