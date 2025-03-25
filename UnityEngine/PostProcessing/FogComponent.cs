using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000245 RID: 581
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0005095B File Offset: 0x0004ED5B
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00050998 File Offset: 0x0004ED98
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0005099F File Offset: 0x0004ED9F
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000509A2 File Offset: 0x0004EDA2
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.BeforeImageEffectsOpaque;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000509A8 File Offset: 0x0004EDA8
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			material.SetColor(FogComponent.Uniforms._FogColor, RenderSettings.fogColor);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			FogMode fogMode = RenderSettings.fogMode;
			if (fogMode != FogMode.Linear)
			{
				if (fogMode != FogMode.Exponential)
				{
					if (fogMode == FogMode.ExponentialSquared)
					{
						material.EnableKeyword("FOG_EXP2");
					}
				}
				else
				{
					material.EnableKeyword("FOG_EXP");
				}
			}
			else
			{
				material.EnableKeyword("FOG_LINEAR");
			}
			RenderTextureFormat renderTextureFormat = ((!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, renderTextureFormat);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, (!settings.excludeSkybox) ? 0 : 1);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x04000E58 RID: 3672
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x02000246 RID: 582
		private static class Uniforms
		{
			// Token: 0x04000E59 RID: 3673
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x04000E5A RID: 3674
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x04000E5B RID: 3675
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x04000E5C RID: 3676
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x04000E5D RID: 3677
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
