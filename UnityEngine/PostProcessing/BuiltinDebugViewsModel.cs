using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000269 RID: 617
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x000535D5 File Offset: 0x000519D5
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x000535DD File Offset: 0x000519DD
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x000535E6 File Offset: 0x000519E6
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00053626 File Offset: 0x00051A26
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00053633 File Offset: 0x00051A33
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x04000F0C RID: 3852
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x0200026A RID: 618
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000170 RID: 368
			// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00053644 File Offset: 0x00051A44
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x04000F0D RID: 3853
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x0200026B RID: 619
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000171 RID: 369
			// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00053668 File Offset: 0x00051A68
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x04000F0E RID: 3854
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x04000F0F RID: 3855
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x04000F10 RID: 3856
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x04000F11 RID: 3857
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x04000F12 RID: 3858
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x04000F13 RID: 3859
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x0200026C RID: 620
		public enum Mode
		{
			// Token: 0x04000F15 RID: 3861
			None,
			// Token: 0x04000F16 RID: 3862
			Depth,
			// Token: 0x04000F17 RID: 3863
			Normals,
			// Token: 0x04000F18 RID: 3864
			MotionVectors,
			// Token: 0x04000F19 RID: 3865
			AmbientOcclusion,
			// Token: 0x04000F1A RID: 3866
			EyeAdaptation,
			// Token: 0x04000F1B RID: 3867
			FocusPlane,
			// Token: 0x04000F1C RID: 3868
			PreGradingLog,
			// Token: 0x04000F1D RID: 3869
			LogLut,
			// Token: 0x04000F1E RID: 3870
			UserLut
		}

		// Token: 0x0200026D RID: 621
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000172 RID: 370
			// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000536C4 File Offset: 0x00051AC4
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000F1F RID: 3871
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x04000F20 RID: 3872
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x04000F21 RID: 3873
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
