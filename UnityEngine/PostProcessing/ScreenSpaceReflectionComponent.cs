﻿using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000251 RID: 593
	public sealed class ScreenSpaceReflectionComponent : PostProcessingComponentCommandBuffer<ScreenSpaceReflectionModel>
	{
		// Token: 0x06000C27 RID: 3111 RVA: 0x00051B77 File Offset: 0x0004FF77
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00051B7A File Offset: 0x0004FF7A
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && !this.context.interrupted;
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00051BB0 File Offset: 0x0004FFB0
		public override void OnEnable()
		{
			this.m_ReflectionTextures[0] = Shader.PropertyToID("_ReflectionTexture0");
			this.m_ReflectionTextures[1] = Shader.PropertyToID("_ReflectionTexture1");
			this.m_ReflectionTextures[2] = Shader.PropertyToID("_ReflectionTexture2");
			this.m_ReflectionTextures[3] = Shader.PropertyToID("_ReflectionTexture3");
			this.m_ReflectionTextures[4] = Shader.PropertyToID("_ReflectionTexture4");
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00051C17 File Offset: 0x00050017
		public override string GetName()
		{
			return "Screen Space Reflection";
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00051C1E File Offset: 0x0005001E
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterFinalPass;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00051C24 File Offset: 0x00050024
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			ScreenSpaceReflectionModel.Settings settings = base.model.settings;
			Camera camera = this.context.camera;
			int num = ((settings.reflection.reflectionQuality != ScreenSpaceReflectionModel.SSRResolution.High) ? 2 : 1);
			int num2 = this.context.width / num;
			int num3 = this.context.height / num;
			float num4 = (float)this.context.width;
			float num5 = (float)this.context.height;
			float num6 = num4 / 2f;
			float num7 = num5 / 2f;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Screen Space Reflection");
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._RayStepSize, settings.reflection.stepSize);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AdditiveReflection, (settings.reflection.blendType != ScreenSpaceReflectionModel.SSRReflectionBlendType.Additive) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._BilateralUpsampling, (!this.k_BilateralUpsample) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TreatBackfaceHitAsMiss, (!this.k_TreatBackfaceHitAsMiss) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._AllowBackwardsRays, (!settings.reflection.reflectBackfaces) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._TraceBehindObjects, (!this.k_TraceBehindObjects) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._MaxSteps, settings.reflection.iterationCount);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._FullResolutionFiltering, 0);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HalfResolution, (settings.reflection.reflectionQuality == ScreenSpaceReflectionModel.SSRResolution.High) ? 0 : 1);
			material.SetInt(ScreenSpaceReflectionComponent.Uniforms._HighlightSuppression, (!this.k_HighlightSuppression) ? 0 : 1);
			float num8 = num4 / (-2f * Mathf.Tan(camera.fieldOfView / 180f * 3.1415927f * 0.5f));
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._PixelsPerMeterAtOneMeter, num8);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ScreenEdgeFading, settings.screenEdgeMask.intensity);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._ReflectionBlur, settings.reflection.reflectionBlur);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._MaxRayTraceDistance, settings.reflection.maxDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FadeDistance, settings.intensity.fadeDistance);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._LayerThickness, settings.reflection.widthModifier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._SSRMultiplier, settings.intensity.reflectionMultiplier);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFade, settings.intensity.fresnelFade);
			material.SetFloat(ScreenSpaceReflectionComponent.Uniforms._FresnelFadePower, settings.intensity.fresnelFadePower);
			Matrix4x4 projectionMatrix = camera.projectionMatrix;
			Vector4 vector = new Vector4(-2f / (num4 * projectionMatrix[0]), -2f / (num5 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			Vector3 vector2 = ((!float.IsPositiveInfinity(camera.farClipPlane)) ? new Vector3(camera.nearClipPlane * camera.farClipPlane, camera.nearClipPlane - camera.farClipPlane, camera.farClipPlane) : new Vector3(camera.nearClipPlane, -1f, 1f));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ReflectionBufferSize, new Vector2((float)num2, (float)num3));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ScreenSize, new Vector2(num4, num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._InvScreenSize, new Vector2(1f / num4, 1f / num5));
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._ProjInfo, vector);
			material.SetVector(ScreenSpaceReflectionComponent.Uniforms._CameraClipInfo, vector2);
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.SetRow(0, new Vector4(num6, 0f, 0f, num6));
			matrix4x.SetRow(1, new Vector4(0f, num7, 0f, num7));
			matrix4x.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
			matrix4x.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
			Matrix4x4 matrix4x2 = matrix4x * projectionMatrix;
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._ProjectToPixelMatrix, matrix4x2);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._WorldToCameraMatrix, camera.worldToCameraMatrix);
			material.SetMatrix(ScreenSpaceReflectionComponent.Uniforms._CameraToWorldMatrix, camera.worldToCameraMatrix.inverse);
			RenderTextureFormat renderTextureFormat = ((!this.context.isHdr) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf);
			int normalAndRoughnessTexture = ScreenSpaceReflectionComponent.Uniforms._NormalAndRoughnessTexture;
			int hitPointTexture = ScreenSpaceReflectionComponent.Uniforms._HitPointTexture;
			int blurTexture = ScreenSpaceReflectionComponent.Uniforms._BlurTexture;
			int filteredReflections = ScreenSpaceReflectionComponent.Uniforms._FilteredReflections;
			int finalReflectionTexture = ScreenSpaceReflectionComponent.Uniforms._FinalReflectionTexture;
			int tempTexture = ScreenSpaceReflectionComponent.Uniforms._TempTexture;
			cb.GetTemporaryRT(normalAndRoughnessTexture, -1, -1, 0, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			cb.GetTemporaryRT(hitPointTexture, num2, num3, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			for (int i = 0; i < 5; i++)
			{
				cb.GetTemporaryRT(this.m_ReflectionTextures[i], num2 >> i, num3 >> i, 0, FilterMode.Bilinear, renderTextureFormat);
			}
			cb.GetTemporaryRT(filteredReflections, num2, num3, 0, (!this.k_BilateralUpsample) ? FilterMode.Bilinear : FilterMode.Point, renderTextureFormat);
			cb.GetTemporaryRT(finalReflectionTexture, num2, num3, 0, FilterMode.Point, renderTextureFormat);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, normalAndRoughnessTexture, material, 6);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, hitPointTexture, material, 0);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, filteredReflections, material, 5);
			cb.Blit(filteredReflections, this.m_ReflectionTextures[0], material, 8);
			for (int j = 1; j < 5; j++)
			{
				int num9 = this.m_ReflectionTextures[j - 1];
				int num10 = j;
				cb.GetTemporaryRT(blurTexture, num2 >> num10, num3 >> num10, 0, FilterMode.Bilinear, renderTextureFormat);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(1f, 0f, 0f, 0f));
				cb.SetGlobalFloat(ScreenSpaceReflectionComponent.Uniforms._CurrentMipLevel, (float)j - 1f);
				cb.Blit(num9, blurTexture, material, 2);
				cb.SetGlobalVector(ScreenSpaceReflectionComponent.Uniforms._Axis, new Vector4(0f, 1f, 0f, 0f));
				num9 = this.m_ReflectionTextures[j];
				cb.Blit(blurTexture, num9, material, 2);
				cb.ReleaseTemporaryRT(blurTexture);
			}
			cb.Blit(this.m_ReflectionTextures[0], finalReflectionTexture, material, 3);
			cb.GetTemporaryRT(tempTexture, camera.pixelWidth, camera.pixelHeight, 0, FilterMode.Bilinear, renderTextureFormat);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, tempTexture, material, 1);
			cb.Blit(tempTexture, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(tempTexture);
		}

		// Token: 0x04000E96 RID: 3734
		private bool k_HighlightSuppression;

		// Token: 0x04000E97 RID: 3735
		private bool k_TraceBehindObjects = true;

		// Token: 0x04000E98 RID: 3736
		private bool k_TreatBackfaceHitAsMiss;

		// Token: 0x04000E99 RID: 3737
		private bool k_BilateralUpsample = true;

		// Token: 0x04000E9A RID: 3738
		private readonly int[] m_ReflectionTextures = new int[5];

		// Token: 0x02000252 RID: 594
		private static class Uniforms
		{
			// Token: 0x04000E9B RID: 3739
			internal static readonly int _RayStepSize = Shader.PropertyToID("_RayStepSize");

			// Token: 0x04000E9C RID: 3740
			internal static readonly int _AdditiveReflection = Shader.PropertyToID("_AdditiveReflection");

			// Token: 0x04000E9D RID: 3741
			internal static readonly int _BilateralUpsampling = Shader.PropertyToID("_BilateralUpsampling");

			// Token: 0x04000E9E RID: 3742
			internal static readonly int _TreatBackfaceHitAsMiss = Shader.PropertyToID("_TreatBackfaceHitAsMiss");

			// Token: 0x04000E9F RID: 3743
			internal static readonly int _AllowBackwardsRays = Shader.PropertyToID("_AllowBackwardsRays");

			// Token: 0x04000EA0 RID: 3744
			internal static readonly int _TraceBehindObjects = Shader.PropertyToID("_TraceBehindObjects");

			// Token: 0x04000EA1 RID: 3745
			internal static readonly int _MaxSteps = Shader.PropertyToID("_MaxSteps");

			// Token: 0x04000EA2 RID: 3746
			internal static readonly int _FullResolutionFiltering = Shader.PropertyToID("_FullResolutionFiltering");

			// Token: 0x04000EA3 RID: 3747
			internal static readonly int _HalfResolution = Shader.PropertyToID("_HalfResolution");

			// Token: 0x04000EA4 RID: 3748
			internal static readonly int _HighlightSuppression = Shader.PropertyToID("_HighlightSuppression");

			// Token: 0x04000EA5 RID: 3749
			internal static readonly int _PixelsPerMeterAtOneMeter = Shader.PropertyToID("_PixelsPerMeterAtOneMeter");

			// Token: 0x04000EA6 RID: 3750
			internal static readonly int _ScreenEdgeFading = Shader.PropertyToID("_ScreenEdgeFading");

			// Token: 0x04000EA7 RID: 3751
			internal static readonly int _ReflectionBlur = Shader.PropertyToID("_ReflectionBlur");

			// Token: 0x04000EA8 RID: 3752
			internal static readonly int _MaxRayTraceDistance = Shader.PropertyToID("_MaxRayTraceDistance");

			// Token: 0x04000EA9 RID: 3753
			internal static readonly int _FadeDistance = Shader.PropertyToID("_FadeDistance");

			// Token: 0x04000EAA RID: 3754
			internal static readonly int _LayerThickness = Shader.PropertyToID("_LayerThickness");

			// Token: 0x04000EAB RID: 3755
			internal static readonly int _SSRMultiplier = Shader.PropertyToID("_SSRMultiplier");

			// Token: 0x04000EAC RID: 3756
			internal static readonly int _FresnelFade = Shader.PropertyToID("_FresnelFade");

			// Token: 0x04000EAD RID: 3757
			internal static readonly int _FresnelFadePower = Shader.PropertyToID("_FresnelFadePower");

			// Token: 0x04000EAE RID: 3758
			internal static readonly int _ReflectionBufferSize = Shader.PropertyToID("_ReflectionBufferSize");

			// Token: 0x04000EAF RID: 3759
			internal static readonly int _ScreenSize = Shader.PropertyToID("_ScreenSize");

			// Token: 0x04000EB0 RID: 3760
			internal static readonly int _InvScreenSize = Shader.PropertyToID("_InvScreenSize");

			// Token: 0x04000EB1 RID: 3761
			internal static readonly int _ProjInfo = Shader.PropertyToID("_ProjInfo");

			// Token: 0x04000EB2 RID: 3762
			internal static readonly int _CameraClipInfo = Shader.PropertyToID("_CameraClipInfo");

			// Token: 0x04000EB3 RID: 3763
			internal static readonly int _ProjectToPixelMatrix = Shader.PropertyToID("_ProjectToPixelMatrix");

			// Token: 0x04000EB4 RID: 3764
			internal static readonly int _WorldToCameraMatrix = Shader.PropertyToID("_WorldToCameraMatrix");

			// Token: 0x04000EB5 RID: 3765
			internal static readonly int _CameraToWorldMatrix = Shader.PropertyToID("_CameraToWorldMatrix");

			// Token: 0x04000EB6 RID: 3766
			internal static readonly int _Axis = Shader.PropertyToID("_Axis");

			// Token: 0x04000EB7 RID: 3767
			internal static readonly int _CurrentMipLevel = Shader.PropertyToID("_CurrentMipLevel");

			// Token: 0x04000EB8 RID: 3768
			internal static readonly int _NormalAndRoughnessTexture = Shader.PropertyToID("_NormalAndRoughnessTexture");

			// Token: 0x04000EB9 RID: 3769
			internal static readonly int _HitPointTexture = Shader.PropertyToID("_HitPointTexture");

			// Token: 0x04000EBA RID: 3770
			internal static readonly int _BlurTexture = Shader.PropertyToID("_BlurTexture");

			// Token: 0x04000EBB RID: 3771
			internal static readonly int _FilteredReflections = Shader.PropertyToID("_FilteredReflections");

			// Token: 0x04000EBC RID: 3772
			internal static readonly int _FinalReflectionTexture = Shader.PropertyToID("_FinalReflectionTexture");

			// Token: 0x04000EBD RID: 3773
			internal static readonly int _TempTexture = Shader.PropertyToID("_TempTexture");
		}

		// Token: 0x02000253 RID: 595
		private enum PassIndex
		{
			// Token: 0x04000EBF RID: 3775
			RayTraceStep,
			// Token: 0x04000EC0 RID: 3776
			CompositeFinal,
			// Token: 0x04000EC1 RID: 3777
			Blur,
			// Token: 0x04000EC2 RID: 3778
			CompositeSSR,
			// Token: 0x04000EC3 RID: 3779
			MinMipGeneration,
			// Token: 0x04000EC4 RID: 3780
			HitPointToReflections,
			// Token: 0x04000EC5 RID: 3781
			BilateralKeyPack,
			// Token: 0x04000EC6 RID: 3782
			BlitDepthAsCSZ,
			// Token: 0x04000EC7 RID: 3783
			PoissonBlur
		}
	}
}
