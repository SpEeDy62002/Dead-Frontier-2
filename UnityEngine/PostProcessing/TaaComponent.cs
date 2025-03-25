using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000254 RID: 596
	public sealed class TaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00052574 File Offset: 0x00050974
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Taa && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) && SystemInfo.supportsMotionVectors && !this.context.interrupted;
			}
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000525D0 File Offset: 0x000509D0
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000525D3 File Offset: 0x000509D3
		public void ResetHistory()
		{
			this.m_ResetHistory = true;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000525DC File Offset: 0x000509DC
		public void SetProjectionMatrix(Func<Vector2, Matrix4x4> jitteredFunc)
		{
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			Vector2 vector = this.GenerateRandomOffset();
			vector *= taaSettings.jitterSpread;
			this.context.camera.nonJitteredProjectionMatrix = this.context.camera.projectionMatrix;
			if (jitteredFunc != null)
			{
				this.context.camera.projectionMatrix = jitteredFunc(vector);
			}
			else
			{
				this.context.camera.projectionMatrix = ((!this.context.camera.orthographic) ? this.GetPerspectiveProjectionMatrix(vector) : this.GetOrthographicProjectionMatrix(vector));
			}
			this.context.camera.useJitteredProjectionMatrixForTransparentRendering = false;
			vector.x /= (float)this.context.width;
			vector.y /= (float)this.context.height;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.SetVector(TaaComponent.Uniforms._Jitter, vector);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000526F8 File Offset: 0x00050AF8
		public void Render(RenderTexture source, RenderTexture destination)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.shaderKeywords = null;
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			if (this.m_ResetHistory || this.m_HistoryTexture == null || this.m_HistoryTexture.width != source.width || this.m_HistoryTexture.height != source.height)
			{
				if (this.m_HistoryTexture)
				{
					RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
				}
				this.m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				this.m_HistoryTexture.name = "TAA History";
				Graphics.Blit(source, this.m_HistoryTexture, material, 2);
			}
			material.SetVector(TaaComponent.Uniforms._SharpenParameters, new Vector4(taaSettings.sharpen, 0f, 0f, 0f));
			material.SetVector(TaaComponent.Uniforms._FinalBlendParameters, new Vector4(taaSettings.stationaryBlending, taaSettings.motionBlending, 6000f, 0f));
			material.SetTexture(TaaComponent.Uniforms._MainTex, source);
			material.SetTexture(TaaComponent.Uniforms._HistoryTex, this.m_HistoryTexture);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			temporary.name = "TAA History";
			this.m_MRT[0] = destination.colorBuffer;
			this.m_MRT[1] = temporary.colorBuffer;
			Graphics.SetRenderTarget(this.m_MRT, source.depthBuffer);
			GraphicsUtils.Blit(material, (!this.context.camera.orthographic) ? 0 : 1);
			RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			this.m_HistoryTexture = temporary;
			this.m_ResetHistory = false;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x000528E0 File Offset: 0x00050CE0
		private float GetHaltonValue(int index, int radix)
		{
			float num = 0f;
			float num2 = 1f / (float)radix;
			while (index > 0)
			{
				num += (float)(index % radix) * num2;
				index /= radix;
				num2 /= (float)radix;
			}
			return num;
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0005291C File Offset: 0x00050D1C
		private Vector2 GenerateRandomOffset()
		{
			Vector2 vector = new Vector2(this.GetHaltonValue(this.m_SampleIndex & 1023, 2), this.GetHaltonValue(this.m_SampleIndex & 1023, 3));
			if (++this.m_SampleIndex >= 8)
			{
				this.m_SampleIndex = 0;
			}
			return vector;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00052978 File Offset: 0x00050D78
		private Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset)
		{
			float num = Mathf.Tan(0.008726646f * this.context.camera.fieldOfView);
			float num2 = num * this.context.camera.aspect;
			offset.x *= num2 / (0.5f * (float)this.context.width);
			offset.y *= num / (0.5f * (float)this.context.height);
			float num3 = (offset.x - num2) * this.context.camera.nearClipPlane;
			float num4 = (offset.x + num2) * this.context.camera.nearClipPlane;
			float num5 = (offset.y + num) * this.context.camera.nearClipPlane;
			float num6 = (offset.y - num) * this.context.camera.nearClipPlane;
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x[0, 0] = 2f * this.context.camera.nearClipPlane / (num4 - num3);
			matrix4x[0, 1] = 0f;
			matrix4x[0, 2] = (num4 + num3) / (num4 - num3);
			matrix4x[0, 3] = 0f;
			matrix4x[1, 0] = 0f;
			matrix4x[1, 1] = 2f * this.context.camera.nearClipPlane / (num5 - num6);
			matrix4x[1, 2] = (num5 + num6) / (num5 - num6);
			matrix4x[1, 3] = 0f;
			matrix4x[2, 0] = 0f;
			matrix4x[2, 1] = 0f;
			matrix4x[2, 2] = -(this.context.camera.farClipPlane + this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			matrix4x[2, 3] = -(2f * this.context.camera.farClipPlane * this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			matrix4x[3, 0] = 0f;
			matrix4x[3, 1] = 0f;
			matrix4x[3, 2] = -1f;
			matrix4x[3, 3] = 0f;
			return matrix4x;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00052C08 File Offset: 0x00051008
		private Matrix4x4 GetOrthographicProjectionMatrix(Vector2 offset)
		{
			float orthographicSize = this.context.camera.orthographicSize;
			float num = orthographicSize * this.context.camera.aspect;
			offset.x *= num / (0.5f * (float)this.context.width);
			offset.y *= orthographicSize / (0.5f * (float)this.context.height);
			float num2 = offset.x - num;
			float num3 = offset.x + num;
			float num4 = offset.y + orthographicSize;
			float num5 = offset.y - orthographicSize;
			return Matrix4x4.Ortho(num2, num3, num5, num4, this.context.camera.nearClipPlane, this.context.camera.farClipPlane);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00052CD2 File Offset: 0x000510D2
		public override void OnDisable()
		{
			if (this.m_HistoryTexture != null)
			{
				RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			}
			this.m_HistoryTexture = null;
			this.m_SampleIndex = 0;
			this.ResetHistory();
		}

		// Token: 0x04000EC8 RID: 3784
		private const string k_ShaderString = "Hidden/Post FX/Temporal Anti-aliasing";

		// Token: 0x04000EC9 RID: 3785
		private const int k_SampleCount = 8;

		// Token: 0x04000ECA RID: 3786
		private readonly RenderBuffer[] m_MRT = new RenderBuffer[2];

		// Token: 0x04000ECB RID: 3787
		private int m_SampleIndex;

		// Token: 0x04000ECC RID: 3788
		private bool m_ResetHistory = true;

		// Token: 0x04000ECD RID: 3789
		private RenderTexture m_HistoryTexture;

		// Token: 0x02000255 RID: 597
		private static class Uniforms
		{
			// Token: 0x04000ECE RID: 3790
			internal static int _Jitter = Shader.PropertyToID("_Jitter");

			// Token: 0x04000ECF RID: 3791
			internal static int _SharpenParameters = Shader.PropertyToID("_SharpenParameters");

			// Token: 0x04000ED0 RID: 3792
			internal static int _FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");

			// Token: 0x04000ED1 RID: 3793
			internal static int _HistoryTex = Shader.PropertyToID("_HistoryTex");

			// Token: 0x04000ED2 RID: 3794
			internal static int _MainTex = Shader.PropertyToID("_MainTex");
		}
	}
}
