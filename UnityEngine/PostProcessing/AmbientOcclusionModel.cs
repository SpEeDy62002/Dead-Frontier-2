using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200025A RID: 602
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x000530DA File Offset: 0x000514DA
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x000530E2 File Offset: 0x000514E2
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x06000C46 RID: 3142 RVA: 0x000530EB File Offset: 0x000514EB
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x0200025B RID: 603
		public enum SampleCount
		{
			// Token: 0x04000EDC RID: 3804
			Lowest = 3,
			// Token: 0x04000EDD RID: 3805
			Low = 6,
			// Token: 0x04000EDE RID: 3806
			Medium = 10,
			// Token: 0x04000EDF RID: 3807
			High = 16
		}

		// Token: 0x0200025C RID: 604
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000164 RID: 356
			// (get) Token: 0x06000C47 RID: 3143 RVA: 0x000530F8 File Offset: 0x000514F8
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x04000EE0 RID: 3808
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x04000EE1 RID: 3809
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04000EE2 RID: 3810
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04000EE3 RID: 3811
			[Tooltip("Halves the resolution of the effect to increase performance.")]
			public bool downsampling;

			// Token: 0x04000EE4 RID: 3812
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x04000EE5 RID: 3813
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x04000EE6 RID: 3814
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
