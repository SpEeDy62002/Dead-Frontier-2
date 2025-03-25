using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200029C RID: 668
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x04000FCD RID: 4045
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x04000FCE RID: 4046
		public FogModel fog = new FogModel();

		// Token: 0x04000FCF RID: 4047
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x04000FD0 RID: 4048
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x04000FD1 RID: 4049
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04000FD2 RID: 4050
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04000FD3 RID: 4051
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04000FD4 RID: 4052
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04000FD5 RID: 4053
		public BloomModel bloom = new BloomModel();

		// Token: 0x04000FD6 RID: 4054
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04000FD7 RID: 4055
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04000FD8 RID: 4056
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04000FD9 RID: 4057
		public GrainModel grain = new GrainModel();

		// Token: 0x04000FDA RID: 4058
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x04000FDB RID: 4059
		public DitheringModel dithering = new DitheringModel();
	}
}
