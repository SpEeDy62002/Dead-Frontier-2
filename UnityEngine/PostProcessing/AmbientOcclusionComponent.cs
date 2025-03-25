using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000232 RID: 562
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0004DA54 File Offset: 0x0004BE54
		private AmbientOcclusionComponent.OcclusionSource occlusionSource
		{
			get
			{
				if (this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility)
				{
					return AmbientOcclusionComponent.OcclusionSource.GBuffer;
				}
				if (base.model.settings.highPrecision && (!this.context.isGBufferAvailable || base.model.settings.forceForwardCompatibility))
				{
					return AmbientOcclusionComponent.OcclusionSource.DepthTexture;
				}
				return AmbientOcclusionComponent.OcclusionSource.DepthNormalsTexture;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0004DAD0 File Offset: 0x0004BED0
		private bool ambientOnlySupported
		{
			get
			{
				return this.context.isHdr && base.model.settings.ambientOnly && this.context.isGBufferAvailable && !base.model.settings.forceForwardCompatibility;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0004DB30 File Offset: 0x0004BF30
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0004DB7C File Offset: 0x0004BF7C
		public override DepthTextureMode GetCameraFlags()
		{
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (this.occlusionSource == AmbientOcclusionComponent.OcclusionSource.DepthTexture)
			{
				depthTextureMode |= DepthTextureMode.Depth;
			}
			if (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer)
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0004DBAB File Offset: 0x0004BFAB
		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0004DBB2 File Offset: 0x0004BFB2
		public override CameraEvent GetCameraEvent()
		{
			return (!this.ambientOnlySupported || this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion)) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeReflections;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0004DBE4 File Offset: 0x0004BFE4
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material2.shaderKeywords = null;
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material2.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, (!settings.downsampling) ? 1f : 0.5f);
			material2.SetInt(AmbientOcclusionComponent.Uniforms._SampleCount, (int)settings.sampleCount);
			int width = this.context.width;
			int height = this.context.height;
			int num = ((!settings.downsampling) ? 1 : 2);
			int num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture1;
			cb.GetTemporaryRT(num2, width / num, height / num, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.Blit(null, num2, material2, (int)this.occlusionSource);
			int occlusionTexture = AmbientOcclusionComponent.Uniforms._OcclusionTexture2;
			cb.GetTemporaryRT(occlusionTexture, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
			cb.Blit(num2, occlusionTexture, material2, (this.occlusionSource != AmbientOcclusionComponent.OcclusionSource.GBuffer) ? 3 : 4);
			cb.ReleaseTemporaryRT(num2);
			num2 = AmbientOcclusionComponent.Uniforms._OcclusionTexture;
			cb.GetTemporaryRT(num2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, occlusionTexture);
			cb.Blit(occlusionTexture, num2, material2, 5);
			cb.ReleaseTemporaryRT(occlusionTexture);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.AmbientOcclusion))
			{
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, num2);
				cb.Blit(num2, BuiltinRenderTextureType.CameraTarget, material2, 8);
				this.context.Interrupt();
			}
			else if (this.ambientOnlySupported)
			{
				cb.SetRenderTarget(this.m_MRT, BuiltinRenderTextureType.CameraTarget);
				cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material2, 0, 7);
			}
			else
			{
				RenderTextureFormat renderTextureFormat = ((!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
				int tempRT = AmbientOcclusionComponent.Uniforms._TempRT;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear, renderTextureFormat);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material, 0);
				cb.SetGlobalTexture(AmbientOcclusionComponent.Uniforms._MainTex, tempRT);
				cb.Blit(tempRT, BuiltinRenderTextureType.CameraTarget, material2, 6);
				cb.ReleaseTemporaryRT(tempRT);
			}
			cb.ReleaseTemporaryRT(num2);
		}

		// Token: 0x04000DEA RID: 3562
		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x04000DEB RID: 3563
		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		// Token: 0x04000DEC RID: 3564
		private readonly RenderTargetIdentifier[] m_MRT = new RenderTargetIdentifier[]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.CameraTarget
		};

		// Token: 0x02000233 RID: 563
		private static class Uniforms
		{
			// Token: 0x04000DED RID: 3565
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04000DEE RID: 3566
			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			// Token: 0x04000DEF RID: 3567
			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			// Token: 0x04000DF0 RID: 3568
			internal static readonly int _SampleCount = Shader.PropertyToID("_SampleCount");

			// Token: 0x04000DF1 RID: 3569
			internal static readonly int _OcclusionTexture1 = Shader.PropertyToID("_OcclusionTexture1");

			// Token: 0x04000DF2 RID: 3570
			internal static readonly int _OcclusionTexture2 = Shader.PropertyToID("_OcclusionTexture2");

			// Token: 0x04000DF3 RID: 3571
			internal static readonly int _OcclusionTexture = Shader.PropertyToID("_OcclusionTexture");

			// Token: 0x04000DF4 RID: 3572
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000DF5 RID: 3573
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		// Token: 0x02000234 RID: 564
		private enum OcclusionSource
		{
			// Token: 0x04000DF7 RID: 3575
			DepthTexture,
			// Token: 0x04000DF8 RID: 3576
			DepthNormalsTexture,
			// Token: 0x04000DF9 RID: 3577
			GBuffer
		}
	}
}
