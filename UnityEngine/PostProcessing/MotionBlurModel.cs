using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000287 RID: 647
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00053F01 File Offset: 0x00052301
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x00053F09 File Offset: 0x00052309
		public MotionBlurModel.Settings settings
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

		// Token: 0x06000C95 RID: 3221 RVA: 0x00053F12 File Offset: 0x00052312
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x04000F7D RID: 3965
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x02000288 RID: 648
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700018B RID: 395
			// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00053F20 File Offset: 0x00052320
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x04000F7E RID: 3966
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x04000F7F RID: 3967
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x04000F80 RID: 3968
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
