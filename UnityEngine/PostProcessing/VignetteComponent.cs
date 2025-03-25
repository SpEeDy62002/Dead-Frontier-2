using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000258 RID: 600
	public sealed class VignetteComponent : PostProcessingComponentRenderTexture<VignetteModel>
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00052F04 File Offset: 0x00051304
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00052F28 File Offset: 0x00051328
		public override void Prepare(Material uberMaterial)
		{
			VignetteModel.Settings settings = base.model.settings;
			uberMaterial.SetColor(VignetteComponent.Uniforms._Vignette_Color, settings.color);
			if (settings.mode == VignetteModel.Mode.Classic)
			{
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center);
				uberMaterial.EnableKeyword("VIGNETTE_CLASSIC");
				float num = (1f - settings.roundness) * 6f + settings.roundness;
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, new Vector4(settings.intensity * 3f, settings.smoothness * 5f, num, (!settings.rounded) ? 0f : 1f));
			}
			else if (settings.mode == VignetteModel.Mode.Masked && settings.mask != null && settings.opacity > 0f)
			{
				uberMaterial.EnableKeyword("VIGNETTE_MASKED");
				uberMaterial.SetTexture(VignetteComponent.Uniforms._Vignette_Mask, settings.mask);
				uberMaterial.SetFloat(VignetteComponent.Uniforms._Vignette_Opacity, settings.opacity);
			}
		}

		// Token: 0x02000259 RID: 601
		private static class Uniforms
		{
			// Token: 0x04000ED5 RID: 3797
			internal static readonly int _Vignette_Color = Shader.PropertyToID("_Vignette_Color");

			// Token: 0x04000ED6 RID: 3798
			internal static readonly int _Vignette_Center = Shader.PropertyToID("_Vignette_Center");

			// Token: 0x04000ED7 RID: 3799
			internal static readonly int _Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");

			// Token: 0x04000ED8 RID: 3800
			internal static readonly int _Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");

			// Token: 0x04000ED9 RID: 3801
			internal static readonly int _Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");
		}
	}
}
