using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000247 RID: 583
	public sealed class FxaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00050B58 File Offset: 0x0004EF58
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Fxaa && !this.context.interrupted;
			}
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00050BA0 File Offset: 0x0004EFA0
		public void Render(RenderTexture source, RenderTexture destination)
		{
			AntialiasingModel.FxaaSettings fxaaSettings = base.model.settings.fxaaSettings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/FXAA");
			AntialiasingModel.FxaaQualitySettings fxaaQualitySettings = AntialiasingModel.FxaaQualitySettings.presets[(int)fxaaSettings.preset];
			AntialiasingModel.FxaaConsoleSettings fxaaConsoleSettings = AntialiasingModel.FxaaConsoleSettings.presets[(int)fxaaSettings.preset];
			material.SetVector(FxaaComponent.Uniforms._QualitySettings, new Vector3(fxaaQualitySettings.subpixelAliasingRemovalAmount, fxaaQualitySettings.edgeDetectionThreshold, fxaaQualitySettings.minimumRequiredLuminance));
			material.SetVector(FxaaComponent.Uniforms._ConsoleSettings, new Vector4(fxaaConsoleSettings.subpixelSpreadAmount, fxaaConsoleSettings.edgeSharpnessAmount, fxaaConsoleSettings.edgeDetectionThreshold, fxaaConsoleSettings.minimumRequiredLuminance));
			Graphics.Blit(source, destination, material, 0);
		}

		// Token: 0x02000248 RID: 584
		private static class Uniforms
		{
			// Token: 0x04000E5E RID: 3678
			internal static readonly int _QualitySettings = Shader.PropertyToID("_QualitySettings");

			// Token: 0x04000E5F RID: 3679
			internal static readonly int _ConsoleSettings = Shader.PropertyToID("_ConsoleSettings");
		}
	}
}
