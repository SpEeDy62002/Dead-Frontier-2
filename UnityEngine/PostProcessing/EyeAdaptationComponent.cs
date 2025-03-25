using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000243 RID: 579
	public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0005033F File Offset: 0x0004E73F
		public override bool active
		{
			get
			{
				return base.model.enabled && SystemInfo.supportsComputeShaders && !this.context.interrupted;
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0005036C File Offset: 0x0004E76C
		public void ResetHistory()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00050375 File Offset: 0x0004E775
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00050380 File Offset: 0x0004E780
		public override void OnDisable()
		{
			foreach (RenderTexture renderTexture in this.m_AutoExposurePool)
			{
				GraphicsUtils.Destroy(renderTexture);
			}
			if (this.m_HistogramBuffer != null)
			{
				this.m_HistogramBuffer.Release();
			}
			this.m_HistogramBuffer = null;
			if (this.m_DebugHistogram != null)
			{
				this.m_DebugHistogram.Release();
			}
			this.m_DebugHistogram = null;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000503F4 File Offset: 0x0004E7F4
		private Vector4 GetHistogramScaleOffsetRes()
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			float num = (float)(settings.logMax - settings.logMin);
			float num2 = 1f / num;
			float num3 = (float)(-(float)settings.logMin) * num2;
			return new Vector4(num2, num3, Mathf.Floor((float)this.context.width / 2f), Mathf.Floor((float)this.context.height / 2f));
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00050468 File Offset: 0x0004E868
		public Texture Prepare(RenderTexture source, Material uberMaterial)
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			if (this.m_EyeCompute == null)
			{
				this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
			material.shaderKeywords = null;
			if (this.m_HistogramBuffer == null)
			{
				this.m_HistogramBuffer = new ComputeBuffer(64, 4);
			}
			if (EyeAdaptationComponent.s_EmptyHistogramBuffer == null)
			{
				EyeAdaptationComponent.s_EmptyHistogramBuffer = new uint[64];
			}
			Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
			RenderTexture renderTexture = this.context.renderTextureFactory.Get((int)histogramScaleOffsetRes.z, (int)histogramScaleOffsetRes.w, 0, source.format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture);
			if (this.m_AutoExposurePool[0] == null || !this.m_AutoExposurePool[0].IsCreated())
			{
				this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
			}
			if (this.m_AutoExposurePool[1] == null || !this.m_AutoExposurePool[1].IsCreated())
			{
				this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
			}
			this.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer);
			int num = this.m_EyeCompute.FindKernel("KEyeHistogram");
			this.m_EyeCompute.SetBuffer(num, "_Histogram", this.m_HistogramBuffer);
			this.m_EyeCompute.SetTexture(num, "_Source", renderTexture);
			this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
			this.m_EyeCompute.Dispatch(num, Mathf.CeilToInt((float)renderTexture.width / 16f), Mathf.CeilToInt((float)renderTexture.height / 16f), 1);
			this.context.renderTextureFactory.Release(renderTexture);
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
			material.SetBuffer("_Histogram", this.m_HistogramBuffer);
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, Mathf.Exp(settings.minLuminance * 0.6931472f), Mathf.Exp(settings.maxLuminance * 0.6931472f)));
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.keyValue);
			if (settings.dynamicKeyValue)
			{
				material.EnableKeyword("AUTO_KEY_VALUE");
			}
			if (this.m_FirstFrame || !Application.isPlaying)
			{
				this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
				Graphics.Blit(null, this.m_CurrentAutoExposure, material, 1);
				Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
			}
			else
			{
				int num2 = this.m_AutoExposurePingPing;
				RenderTexture renderTexture2 = this.m_AutoExposurePool[++num2 % 2];
				RenderTexture renderTexture3 = this.m_AutoExposurePool[++num2 % 2];
				Graphics.Blit(renderTexture2, renderTexture3, material, (int)settings.adaptationType);
				this.m_AutoExposurePingPing = (num2 + 1) % 2;
				this.m_CurrentAutoExposure = renderTexture3;
			}
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
				{
					this.m_DebugHistogram = new RenderTexture(256, 128, 0, RenderTextureFormat.ARGB32)
					{
						filterMode = FilterMode.Point,
						wrapMode = TextureWrapMode.Clamp
					};
				}
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, (float)this.m_DebugHistogram.width);
				Graphics.Blit(null, this.m_DebugHistogram, material, 2);
			}
			this.m_FirstFrame = false;
			return this.m_CurrentAutoExposure;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0005086C File Offset: 0x0004EC6C
		public void OnGUI()
		{
			if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
			{
				return;
			}
			Rect rect = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)this.m_DebugHistogram.width, (float)this.m_DebugHistogram.height);
			GUI.DrawTexture(rect, this.m_DebugHistogram);
		}

		// Token: 0x04000E47 RID: 3655
		private ComputeShader m_EyeCompute;

		// Token: 0x04000E48 RID: 3656
		private ComputeBuffer m_HistogramBuffer;

		// Token: 0x04000E49 RID: 3657
		private readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];

		// Token: 0x04000E4A RID: 3658
		private int m_AutoExposurePingPing;

		// Token: 0x04000E4B RID: 3659
		private RenderTexture m_CurrentAutoExposure;

		// Token: 0x04000E4C RID: 3660
		private RenderTexture m_DebugHistogram;

		// Token: 0x04000E4D RID: 3661
		private static uint[] s_EmptyHistogramBuffer;

		// Token: 0x04000E4E RID: 3662
		private bool m_FirstFrame = true;

		// Token: 0x04000E4F RID: 3663
		private const int k_HistogramBins = 64;

		// Token: 0x04000E50 RID: 3664
		private const int k_HistogramThreadX = 16;

		// Token: 0x04000E51 RID: 3665
		private const int k_HistogramThreadY = 16;

		// Token: 0x02000244 RID: 580
		private static class Uniforms
		{
			// Token: 0x04000E52 RID: 3666
			internal static readonly int _Params = Shader.PropertyToID("_Params");

			// Token: 0x04000E53 RID: 3667
			internal static readonly int _Speed = Shader.PropertyToID("_Speed");

			// Token: 0x04000E54 RID: 3668
			internal static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

			// Token: 0x04000E55 RID: 3669
			internal static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");

			// Token: 0x04000E56 RID: 3670
			internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			// Token: 0x04000E57 RID: 3671
			internal static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
		}
	}
}
