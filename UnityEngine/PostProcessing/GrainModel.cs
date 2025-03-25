using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000285 RID: 645
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00053E8D File Offset: 0x0005228D
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x00053E95 File Offset: 0x00052295
		public GrainModel.Settings settings
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

		// Token: 0x06000C90 RID: 3216 RVA: 0x00053E9E File Offset: 0x0005229E
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x04000F78 RID: 3960
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x02000286 RID: 646
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000189 RID: 393
			// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00053EAC File Offset: 0x000522AC
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04000F79 RID: 3961
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04000F7A RID: 3962
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x04000F7B RID: 3963
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size in \"Filmic\" mode.")]
			public float size;

			// Token: 0x04000F7C RID: 3964
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
