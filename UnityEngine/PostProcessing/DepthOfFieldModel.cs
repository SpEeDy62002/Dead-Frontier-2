using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200027B RID: 635
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00053CBD File Offset: 0x000520BD
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x00053CC5 File Offset: 0x000520C5
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x06000C7C RID: 3196 RVA: 0x00053CCE File Offset: 0x000520CE
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x04000F5B RID: 3931
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x0200027C RID: 636
		public enum KernelSize
		{
			// Token: 0x04000F5D RID: 3933
			Small,
			// Token: 0x04000F5E RID: 3934
			Medium,
			// Token: 0x04000F5F RID: 3935
			Large,
			// Token: 0x04000F60 RID: 3936
			VeryLarge
		}

		// Token: 0x0200027D RID: 637
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000181 RID: 385
			// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00053CDC File Offset: 0x000520DC
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x04000F61 RID: 3937
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04000F62 RID: 3938
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04000F63 RID: 3939
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04000F64 RID: 3940
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera.")]
			public bool useCameraFov;

			// Token: 0x04000F65 RID: 3941
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
