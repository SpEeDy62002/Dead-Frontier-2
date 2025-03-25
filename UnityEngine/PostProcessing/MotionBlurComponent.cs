using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200024B RID: 587
	public sealed class MotionBlurComponent : PostProcessingComponentCommandBuffer<MotionBlurModel>
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00050ECF File Offset: 0x0004F2CF
		public MotionBlurComponent.ReconstructionFilter reconstructionFilter
		{
			get
			{
				if (this.m_ReconstructionFilter == null)
				{
					this.m_ReconstructionFilter = new MotionBlurComponent.ReconstructionFilter();
				}
				return this.m_ReconstructionFilter;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00050EED File Offset: 0x0004F2ED
		public MotionBlurComponent.FrameBlendingFilter frameBlendingFilter
		{
			get
			{
				if (this.m_FrameBlendingFilter == null)
				{
					this.m_FrameBlendingFilter = new MotionBlurComponent.FrameBlendingFilter();
				}
				return this.m_FrameBlendingFilter;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00050F0C File Offset: 0x0004F30C
		public override bool active
		{
			get
			{
				MotionBlurModel.Settings settings = base.model.settings;
				return base.model.enabled && ((settings.shutterAngle > 0f && this.reconstructionFilter.IsSupported()) || settings.frameBlending > 0f) && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && !this.context.interrupted;
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00050F83 File Offset: 0x0004F383
		public override string GetName()
		{
			return "Motion Blur";
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00050F8A File Offset: 0x0004F38A
		public void ResetHistory()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
			this.m_FrameBlendingFilter = null;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00050FA9 File Offset: 0x0004F3A9
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00050FAC File Offset: 0x0004F3AC
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.BeforeImageEffects;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00050FB0 File Offset: 0x0004F3B0
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00050FBC File Offset: 0x0004F3BC
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			if (this.m_FirstFrame)
			{
				this.m_FirstFrame = false;
				return;
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Motion Blur");
			Material material2 = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			MotionBlurModel.Settings settings = base.model.settings;
			RenderTextureFormat renderTextureFormat = ((!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			int tempRT = MotionBlurComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Point, renderTextureFormat);
			if (settings.shutterAngle > 0f && settings.frameBlending > 0f)
			{
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, BuiltinRenderTextureType.CameraTarget, tempRT, material);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, BuiltinRenderTextureType.CameraTarget, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			else if (settings.shutterAngle > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material2, 0);
				this.reconstructionFilter.ProcessImage(this.context, cb, ref settings, tempRT, BuiltinRenderTextureType.CameraTarget, material);
			}
			else if (settings.frameBlending > 0f)
			{
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, tempRT, material2, 0);
				this.frameBlendingFilter.BlendFrames(cb, settings.frameBlending, tempRT, BuiltinRenderTextureType.CameraTarget, material);
				this.frameBlendingFilter.PushFrame(cb, tempRT, this.context.width, this.context.height, material);
			}
			cb.ReleaseTemporaryRT(tempRT);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000511D1 File Offset: 0x0004F5D1
		public override void OnDisable()
		{
			if (this.m_FrameBlendingFilter != null)
			{
				this.m_FrameBlendingFilter.Dispose();
			}
		}

		// Token: 0x04000E65 RID: 3685
		private MotionBlurComponent.ReconstructionFilter m_ReconstructionFilter;

		// Token: 0x04000E66 RID: 3686
		private MotionBlurComponent.FrameBlendingFilter m_FrameBlendingFilter;

		// Token: 0x04000E67 RID: 3687
		private bool m_FirstFrame = true;

		// Token: 0x0200024C RID: 588
		private static class Uniforms
		{
			// Token: 0x04000E68 RID: 3688
			internal static readonly int _VelocityScale = Shader.PropertyToID("_VelocityScale");

			// Token: 0x04000E69 RID: 3689
			internal static readonly int _MaxBlurRadius = Shader.PropertyToID("_MaxBlurRadius");

			// Token: 0x04000E6A RID: 3690
			internal static readonly int _RcpMaxBlurRadius = Shader.PropertyToID("_RcpMaxBlurRadius");

			// Token: 0x04000E6B RID: 3691
			internal static readonly int _VelocityTex = Shader.PropertyToID("_VelocityTex");

			// Token: 0x04000E6C RID: 3692
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000E6D RID: 3693
			internal static readonly int _Tile2RT = Shader.PropertyToID("_Tile2RT");

			// Token: 0x04000E6E RID: 3694
			internal static readonly int _Tile4RT = Shader.PropertyToID("_Tile4RT");

			// Token: 0x04000E6F RID: 3695
			internal static readonly int _Tile8RT = Shader.PropertyToID("_Tile8RT");

			// Token: 0x04000E70 RID: 3696
			internal static readonly int _TileMaxOffs = Shader.PropertyToID("_TileMaxOffs");

			// Token: 0x04000E71 RID: 3697
			internal static readonly int _TileMaxLoop = Shader.PropertyToID("_TileMaxLoop");

			// Token: 0x04000E72 RID: 3698
			internal static readonly int _TileVRT = Shader.PropertyToID("_TileVRT");

			// Token: 0x04000E73 RID: 3699
			internal static readonly int _NeighborMaxTex = Shader.PropertyToID("_NeighborMaxTex");

			// Token: 0x04000E74 RID: 3700
			internal static readonly int _LoopCount = Shader.PropertyToID("_LoopCount");

			// Token: 0x04000E75 RID: 3701
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04000E76 RID: 3702
			internal static readonly int _History1LumaTex = Shader.PropertyToID("_History1LumaTex");

			// Token: 0x04000E77 RID: 3703
			internal static readonly int _History2LumaTex = Shader.PropertyToID("_History2LumaTex");

			// Token: 0x04000E78 RID: 3704
			internal static readonly int _History3LumaTex = Shader.PropertyToID("_History3LumaTex");

			// Token: 0x04000E79 RID: 3705
			internal static readonly int _History4LumaTex = Shader.PropertyToID("_History4LumaTex");

			// Token: 0x04000E7A RID: 3706
			internal static readonly int _History1ChromaTex = Shader.PropertyToID("_History1ChromaTex");

			// Token: 0x04000E7B RID: 3707
			internal static readonly int _History2ChromaTex = Shader.PropertyToID("_History2ChromaTex");

			// Token: 0x04000E7C RID: 3708
			internal static readonly int _History3ChromaTex = Shader.PropertyToID("_History3ChromaTex");

			// Token: 0x04000E7D RID: 3709
			internal static readonly int _History4ChromaTex = Shader.PropertyToID("_History4ChromaTex");

			// Token: 0x04000E7E RID: 3710
			internal static readonly int _History1Weight = Shader.PropertyToID("_History1Weight");

			// Token: 0x04000E7F RID: 3711
			internal static readonly int _History2Weight = Shader.PropertyToID("_History2Weight");

			// Token: 0x04000E80 RID: 3712
			internal static readonly int _History3Weight = Shader.PropertyToID("_History3Weight");

			// Token: 0x04000E81 RID: 3713
			internal static readonly int _History4Weight = Shader.PropertyToID("_History4Weight");
		}

		// Token: 0x0200024D RID: 589
		private enum Pass
		{
			// Token: 0x04000E83 RID: 3715
			VelocitySetup,
			// Token: 0x04000E84 RID: 3716
			TileMax1,
			// Token: 0x04000E85 RID: 3717
			TileMax2,
			// Token: 0x04000E86 RID: 3718
			TileMaxV,
			// Token: 0x04000E87 RID: 3719
			NeighborMax,
			// Token: 0x04000E88 RID: 3720
			Reconstruction,
			// Token: 0x04000E89 RID: 3721
			FrameCompression,
			// Token: 0x04000E8A RID: 3722
			FrameBlendingChroma,
			// Token: 0x04000E8B RID: 3723
			FrameBlendingRaw
		}

		// Token: 0x0200024E RID: 590
		public class ReconstructionFilter
		{
			// Token: 0x06000C17 RID: 3095 RVA: 0x0005137F File Offset: 0x0004F77F
			public ReconstructionFilter()
			{
				this.CheckTextureFormatSupport();
			}

			// Token: 0x06000C18 RID: 3096 RVA: 0x0005139C File Offset: 0x0004F79C
			private void CheckTextureFormatSupport()
			{
				if (!SystemInfo.SupportsRenderTextureFormat(this.m_PackedRTFormat))
				{
					this.m_PackedRTFormat = RenderTextureFormat.ARGB32;
				}
			}

			// Token: 0x06000C19 RID: 3097 RVA: 0x000513B5 File Offset: 0x0004F7B5
			public bool IsSupported()
			{
				return SystemInfo.supportsMotionVectors;
			}

			// Token: 0x06000C1A RID: 3098 RVA: 0x000513BC File Offset: 0x0004F7BC
			public void ProcessImage(PostProcessingContext context, CommandBuffer cb, ref MotionBlurModel.Settings settings, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				int num = (int)(5f * (float)context.height / 100f);
				int num2 = ((num - 1) / 8 + 1) * 8;
				float num3 = settings.shutterAngle / 360f;
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._VelocityScale, num3);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._MaxBlurRadius, (float)num);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._RcpMaxBlurRadius, 1f / (float)num);
				int velocityTex = MotionBlurComponent.Uniforms._VelocityTex;
				cb.GetTemporaryRT(velocityTex, context.width, context.height, 0, FilterMode.Point, this.m_PackedRTFormat, RenderTextureReadWrite.Linear);
				cb.Blit(null, velocityTex, material, 0);
				int tile2RT = MotionBlurComponent.Uniforms._Tile2RT;
				cb.GetTemporaryRT(tile2RT, context.width / 2, context.height / 2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, velocityTex);
				cb.Blit(velocityTex, tile2RT, material, 1);
				int tile4RT = MotionBlurComponent.Uniforms._Tile4RT;
				cb.GetTemporaryRT(tile4RT, context.width / 4, context.height / 4, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile2RT);
				cb.Blit(tile2RT, tile4RT, material, 2);
				cb.ReleaseTemporaryRT(tile2RT);
				int tile8RT = MotionBlurComponent.Uniforms._Tile8RT;
				cb.GetTemporaryRT(tile8RT, context.width / 8, context.height / 8, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile4RT);
				cb.Blit(tile4RT, tile8RT, material, 2);
				cb.ReleaseTemporaryRT(tile4RT);
				Vector2 vector = Vector2.one * ((float)num2 / 8f - 1f) * -0.5f;
				cb.SetGlobalVector(MotionBlurComponent.Uniforms._TileMaxOffs, vector);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._TileMaxLoop, (float)((int)((float)num2 / 8f)));
				int tileVRT = MotionBlurComponent.Uniforms._TileVRT;
				cb.GetTemporaryRT(tileVRT, context.width / num2, context.height / num2, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tile8RT);
				cb.Blit(tile8RT, tileVRT, material, 3);
				cb.ReleaseTemporaryRT(tile8RT);
				int neighborMaxTex = MotionBlurComponent.Uniforms._NeighborMaxTex;
				int num4 = context.width / num2;
				int num5 = context.height / num2;
				cb.GetTemporaryRT(neighborMaxTex, num4, num5, 0, FilterMode.Point, this.m_VectorRTFormat, RenderTextureReadWrite.Linear);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, tileVRT);
				cb.Blit(tileVRT, neighborMaxTex, material, 4);
				cb.ReleaseTemporaryRT(tileVRT);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._LoopCount, (float)Mathf.Clamp(settings.sampleCount / 2, 1, 64));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, 5);
				cb.ReleaseTemporaryRT(velocityTex);
				cb.ReleaseTemporaryRT(neighborMaxTex);
			}

			// Token: 0x04000E8C RID: 3724
			private RenderTextureFormat m_VectorRTFormat = RenderTextureFormat.RGHalf;

			// Token: 0x04000E8D RID: 3725
			private RenderTextureFormat m_PackedRTFormat = RenderTextureFormat.ARGB2101010;
		}

		// Token: 0x0200024F RID: 591
		public class FrameBlendingFilter
		{
			// Token: 0x06000C1B RID: 3099 RVA: 0x0005169E File Offset: 0x0004FA9E
			public FrameBlendingFilter()
			{
				this.m_UseCompression = MotionBlurComponent.FrameBlendingFilter.CheckSupportCompression();
				this.m_RawTextureFormat = MotionBlurComponent.FrameBlendingFilter.GetPreferredRenderTextureFormat();
				this.m_FrameList = new MotionBlurComponent.FrameBlendingFilter.Frame[4];
			}

			// Token: 0x06000C1C RID: 3100 RVA: 0x000516C8 File Offset: 0x0004FAC8
			public void Dispose()
			{
				foreach (MotionBlurComponent.FrameBlendingFilter.Frame frame in this.m_FrameList)
				{
					frame.Release();
				}
			}

			// Token: 0x06000C1D RID: 3101 RVA: 0x00051704 File Offset: 0x0004FB04
			public void PushFrame(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
			{
				int frameCount = Time.frameCount;
				if (frameCount == this.m_LastFrameCount)
				{
					return;
				}
				int num = frameCount % this.m_FrameList.Length;
				if (this.m_UseCompression)
				{
					this.m_FrameList[num].MakeRecord(cb, source, width, height, material);
				}
				else
				{
					this.m_FrameList[num].MakeRecordRaw(cb, source, width, height, this.m_RawTextureFormat);
				}
				this.m_LastFrameCount = frameCount;
			}

			// Token: 0x06000C1E RID: 3102 RVA: 0x0005177C File Offset: 0x0004FB7C
			public void BlendFrames(CommandBuffer cb, float strength, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material)
			{
				float time = Time.time;
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative = this.GetFrameRelative(-1);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative2 = this.GetFrameRelative(-2);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative3 = this.GetFrameRelative(-3);
				MotionBlurComponent.FrameBlendingFilter.Frame frameRelative4 = this.GetFrameRelative(-4);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1LumaTex, frameRelative.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2LumaTex, frameRelative2.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3LumaTex, frameRelative3.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4LumaTex, frameRelative4.lumaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History1ChromaTex, frameRelative.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History2ChromaTex, frameRelative2.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History3ChromaTex, frameRelative3.chromaTexture);
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._History4ChromaTex, frameRelative4.chromaTexture);
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History1Weight, frameRelative.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History2Weight, frameRelative2.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History3Weight, frameRelative3.CalculateWeight(strength, time));
				cb.SetGlobalFloat(MotionBlurComponent.Uniforms._History4Weight, frameRelative4.CalculateWeight(strength, time));
				cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
				cb.Blit(source, destination, material, (!this.m_UseCompression) ? 8 : 7);
			}

			// Token: 0x06000C1F RID: 3103 RVA: 0x000518E4 File Offset: 0x0004FCE4
			private static bool CheckSupportCompression()
			{
				return SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) && SystemInfo.supportedRenderTargetCount > 1;
			}

			// Token: 0x06000C20 RID: 3104 RVA: 0x00051900 File Offset: 0x0004FD00
			private static RenderTextureFormat GetPreferredRenderTextureFormat()
			{
				RenderTextureFormat[] array = new RenderTextureFormat[]
				{
					RenderTextureFormat.RGB565,
					RenderTextureFormat.ARGB1555,
					RenderTextureFormat.ARGB4444
				};
				foreach (RenderTextureFormat renderTextureFormat in array)
				{
					if (SystemInfo.SupportsRenderTextureFormat(renderTextureFormat))
					{
						return renderTextureFormat;
					}
				}
				return RenderTextureFormat.Default;
			}

			// Token: 0x06000C21 RID: 3105 RVA: 0x00051948 File Offset: 0x0004FD48
			private MotionBlurComponent.FrameBlendingFilter.Frame GetFrameRelative(int offset)
			{
				int num = (Time.frameCount + this.m_FrameList.Length + offset) % this.m_FrameList.Length;
				return this.m_FrameList[num];
			}

			// Token: 0x04000E8E RID: 3726
			private bool m_UseCompression;

			// Token: 0x04000E8F RID: 3727
			private RenderTextureFormat m_RawTextureFormat;

			// Token: 0x04000E90 RID: 3728
			private MotionBlurComponent.FrameBlendingFilter.Frame[] m_FrameList;

			// Token: 0x04000E91 RID: 3729
			private int m_LastFrameCount;

			// Token: 0x02000250 RID: 592
			private struct Frame
			{
				// Token: 0x06000C22 RID: 3106 RVA: 0x00051980 File Offset: 0x0004FD80
				public float CalculateWeight(float strength, float currentTime)
				{
					if (Mathf.Approximately(this.m_Time, 0f))
					{
						return 0f;
					}
					float num = Mathf.Lerp(80f, 16f, strength);
					return Mathf.Exp((this.m_Time - currentTime) * num);
				}

				// Token: 0x06000C23 RID: 3107 RVA: 0x000519C8 File Offset: 0x0004FDC8
				public void Release()
				{
					if (this.lumaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.lumaTexture);
					}
					if (this.chromaTexture != null)
					{
						RenderTexture.ReleaseTemporary(this.chromaTexture);
					}
					this.lumaTexture = null;
					this.chromaTexture = null;
				}

				// Token: 0x06000C24 RID: 3108 RVA: 0x00051A1C File Offset: 0x0004FE1C
				public void MakeRecord(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, Material material)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
					this.chromaTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
					this.lumaTexture.filterMode = FilterMode.Point;
					this.chromaTexture.filterMode = FilterMode.Point;
					if (this.m_MRT == null)
					{
						this.m_MRT = new RenderTargetIdentifier[2];
					}
					this.m_MRT[0] = this.lumaTexture;
					this.m_MRT[1] = this.chromaTexture;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.SetRenderTarget(this.m_MRT, this.lumaTexture);
					cb.DrawMesh(GraphicsUtils.quad, Matrix4x4.identity, material, 0, 6);
					this.m_Time = Time.time;
				}

				// Token: 0x06000C25 RID: 3109 RVA: 0x00051AFC File Offset: 0x0004FEFC
				public void MakeRecordRaw(CommandBuffer cb, RenderTargetIdentifier source, int width, int height, RenderTextureFormat format)
				{
					this.Release();
					this.lumaTexture = RenderTexture.GetTemporary(width, height, 0, format);
					this.lumaTexture.filterMode = FilterMode.Point;
					cb.SetGlobalTexture(MotionBlurComponent.Uniforms._MainTex, source);
					cb.Blit(source, this.lumaTexture);
					this.m_Time = Time.time;
				}

				// Token: 0x04000E92 RID: 3730
				public RenderTexture lumaTexture;

				// Token: 0x04000E93 RID: 3731
				public RenderTexture chromaTexture;

				// Token: 0x04000E94 RID: 3732
				private float m_Time;

				// Token: 0x04000E95 RID: 3733
				private RenderTargetIdentifier[] m_MRT;
			}
		}
	}
}
