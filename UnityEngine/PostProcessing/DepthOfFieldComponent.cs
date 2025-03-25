using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200023F RID: 575
	public sealed class DepthOfFieldComponent : PostProcessingComponentRenderTexture<DepthOfFieldModel>
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0004FD15 File Offset: 0x0004E115
		public override bool active
		{
			get
			{
				return base.model.enabled && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf) && !this.context.interrupted;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0004FD4F File Offset: 0x0004E14F
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0004FD54 File Offset: 0x0004E154
		private float CalculateFocalLength()
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			if (!settings.useCameraFov)
			{
				return settings.focalLength / 1000f;
			}
			float num = this.context.camera.fieldOfView * 0.017453292f;
			return 0.012f / Mathf.Tan(0.5f * num);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0004FDB0 File Offset: 0x0004E1B0
		private float CalculateMaxCoCRadius(int screenHeight)
		{
			float num = (float)base.model.settings.kernelSize * 4f + 6f;
			return Mathf.Min(0.05f, num / (float)screenHeight);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0004FDEC File Offset: 0x0004E1EC
		public void Prepare(RenderTexture source, Material uberMaterial, bool antialiasCoC)
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Depth Of Field");
			material.shaderKeywords = null;
			float num = settings.focusDistance;
			float num2 = this.CalculateFocalLength();
			num = Mathf.Max(num, num2);
			material.SetFloat(DepthOfFieldComponent.Uniforms._Distance, num);
			float num3 = num2 * num2 / (settings.aperture * (num - num2) * 0.024f * 2f);
			material.SetFloat(DepthOfFieldComponent.Uniforms._LensCoeff, num3);
			float num4 = this.CalculateMaxCoCRadius(source.height);
			material.SetFloat(DepthOfFieldComponent.Uniforms._MaxCoC, num4);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpMaxCoC, 1f / num4);
			float num5 = (float)source.height / (float)source.width;
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpAspect, num5);
			RenderTexture renderTexture = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			source.filterMode = FilterMode.Point;
			if (!antialiasCoC)
			{
				Graphics.Blit(source, renderTexture, material, 0);
			}
			else
			{
				bool flag = this.m_CoCHistory == null || !this.m_CoCHistory.IsCreated() || this.m_CoCHistory.width != this.context.width / 2 || this.m_CoCHistory.height != this.context.height / 2;
				RenderTexture temporary = RenderTexture.GetTemporary(this.context.width / 2, this.context.height / 2, 0, RenderTextureFormat.RHalf);
				temporary.filterMode = FilterMode.Point;
				temporary.name = "CoC History";
				this.m_MRT[0] = renderTexture.colorBuffer;
				this.m_MRT[1] = temporary.colorBuffer;
				material.SetTexture(DepthOfFieldComponent.Uniforms._MainTex, source);
				material.SetTexture(DepthOfFieldComponent.Uniforms._HistoryCoC, this.m_CoCHistory);
				material.SetFloat(DepthOfFieldComponent.Uniforms._HistoryWeight, (!flag) ? 0.5f : 0f);
				Graphics.SetRenderTarget(this.m_MRT, renderTexture.depthBuffer);
				GraphicsUtils.Blit(material, 1);
				RenderTexture.ReleaseTemporary(this.m_CoCHistory);
				this.m_CoCHistory = temporary;
			}
			RenderTexture renderTexture2 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(renderTexture, renderTexture2, material, (int)(2 + settings.kernelSize));
			Graphics.Blit(renderTexture2, renderTexture, material, 6);
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.FocusPlane))
			{
				uberMaterial.SetVector(DepthOfFieldComponent.Uniforms._DepthOfFieldParams, new Vector2(num, num3));
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD_COC_VIEW");
				this.context.Interrupt();
			}
			else
			{
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldTex, renderTexture);
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD");
			}
			this.context.renderTextureFactory.Release(renderTexture2);
			source.filterMode = FilterMode.Bilinear;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00050112 File Offset: 0x0004E512
		public override void OnDisable()
		{
			if (this.m_CoCHistory != null)
			{
				RenderTexture.ReleaseTemporary(this.m_CoCHistory);
			}
			this.m_CoCHistory = null;
		}

		// Token: 0x04000E34 RID: 3636
		private const string k_ShaderString = "Hidden/Post FX/Depth Of Field";

		// Token: 0x04000E35 RID: 3637
		private RenderTexture m_CoCHistory;

		// Token: 0x04000E36 RID: 3638
		private RenderBuffer[] m_MRT = new RenderBuffer[2];

		// Token: 0x04000E37 RID: 3639
		private const float k_FilmHeight = 0.024f;

		// Token: 0x02000240 RID: 576
		private static class Uniforms
		{
			// Token: 0x04000E38 RID: 3640
			internal static readonly int _DepthOfFieldTex = Shader.PropertyToID("_DepthOfFieldTex");

			// Token: 0x04000E39 RID: 3641
			internal static readonly int _Distance = Shader.PropertyToID("_Distance");

			// Token: 0x04000E3A RID: 3642
			internal static readonly int _LensCoeff = Shader.PropertyToID("_LensCoeff");

			// Token: 0x04000E3B RID: 3643
			internal static readonly int _MaxCoC = Shader.PropertyToID("_MaxCoC");

			// Token: 0x04000E3C RID: 3644
			internal static readonly int _RcpMaxCoC = Shader.PropertyToID("_RcpMaxCoC");

			// Token: 0x04000E3D RID: 3645
			internal static readonly int _RcpAspect = Shader.PropertyToID("_RcpAspect");

			// Token: 0x04000E3E RID: 3646
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000E3F RID: 3647
			internal static readonly int _HistoryCoC = Shader.PropertyToID("_HistoryCoC");

			// Token: 0x04000E40 RID: 3648
			internal static readonly int _HistoryWeight = Shader.PropertyToID("_HistoryWeight");

			// Token: 0x04000E41 RID: 3649
			internal static readonly int _DepthOfFieldParams = Shader.PropertyToID("_DepthOfFieldParams");
		}
	}
}
