using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000280 RID: 640
	[Serializable]
	public class EyeAdaptationModel : PostProcessingModel
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00053D81 File Offset: 0x00052181
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x00053D89 File Offset: 0x00052189
		public EyeAdaptationModel.Settings settings
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

		// Token: 0x06000C86 RID: 3206 RVA: 0x00053D92 File Offset: 0x00052192
		public override void Reset()
		{
			this.m_Settings = EyeAdaptationModel.Settings.defaultSettings;
		}

		// Token: 0x04000F67 RID: 3943
		[SerializeField]
		private EyeAdaptationModel.Settings m_Settings = EyeAdaptationModel.Settings.defaultSettings;

		// Token: 0x02000281 RID: 641
		public enum EyeAdaptationType
		{
			// Token: 0x04000F69 RID: 3945
			Progressive,
			// Token: 0x04000F6A RID: 3946
			Fixed
		}

		// Token: 0x02000282 RID: 642
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00053DA0 File Offset: 0x000521A0
			public static EyeAdaptationModel.Settings defaultSettings
			{
				get
				{
					return new EyeAdaptationModel.Settings
					{
						lowPercent = 45f,
						highPercent = 95f,
						minLuminance = -5f,
						maxLuminance = 1f,
						keyValue = 0.25f,
						dynamicKeyValue = true,
						adaptationType = EyeAdaptationModel.EyeAdaptationType.Progressive,
						speedUp = 2f,
						speedDown = 1f,
						logMin = -8,
						logMax = 4
					};
				}
			}

			// Token: 0x04000F6B RID: 3947
			[Range(1f, 99f)]
			[Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float lowPercent;

			// Token: 0x04000F6C RID: 3948
			[Range(1f, 99f)]
			[Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float highPercent;

			// Token: 0x04000F6D RID: 3949
			[Tooltip("Minimum average luminance to consider for auto exposure (in EV).")]
			public float minLuminance;

			// Token: 0x04000F6E RID: 3950
			[Tooltip("Maximum average luminance to consider for auto exposure (in EV).")]
			public float maxLuminance;

			// Token: 0x04000F6F RID: 3951
			[Min(0f)]
			[Tooltip("Exposure bias. Use this to control the global exposure of the scene.")]
			public float keyValue;

			// Token: 0x04000F70 RID: 3952
			[Tooltip("Set this to true to let Unity handle the key value automatically based on average luminance.")]
			public bool dynamicKeyValue;

			// Token: 0x04000F71 RID: 3953
			[Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
			public EyeAdaptationModel.EyeAdaptationType adaptationType;

			// Token: 0x04000F72 RID: 3954
			[Min(0f)]
			[Tooltip("Adaptation speed from a dark to a light environment.")]
			public float speedUp;

			// Token: 0x04000F73 RID: 3955
			[Min(0f)]
			[Tooltip("Adaptation speed from a light to a dark environment.")]
			public float speedDown;

			// Token: 0x04000F74 RID: 3956
			[Range(-16f, -1f)]
			[Tooltip("Lower bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMin;

			// Token: 0x04000F75 RID: 3957
			[Range(1f, 16f)]
			[Tooltip("Upper bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMax;
		}
	}
}
