using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000289 RID: 649
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00053F6A File Offset: 0x0005236A
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x00053F72 File Offset: 0x00052372
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00053F7B File Offset: 0x0005237B
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x04000F81 RID: 3969
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x0200028A RID: 650
		public enum SSRResolution
		{
			// Token: 0x04000F83 RID: 3971
			High,
			// Token: 0x04000F84 RID: 3972
			Low = 2
		}

		// Token: 0x0200028B RID: 651
		public enum SSRReflectionBlendType
		{
			// Token: 0x04000F86 RID: 3974
			PhysicallyBased,
			// Token: 0x04000F87 RID: 3975
			Additive
		}

		// Token: 0x0200028C RID: 652
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04000F88 RID: 3976
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04000F89 RID: 3977
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04000F8A RID: 3978
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04000F8B RID: 3979
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x0200028D RID: 653
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x04000F8C RID: 3980
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x04000F8D RID: 3981
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x04000F8E RID: 3982
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x04000F8F RID: 3983
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x04000F90 RID: 3984
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x04000F91 RID: 3985
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x04000F92 RID: 3986
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x04000F93 RID: 3987
			[Tooltip("Enable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x0200028E RID: 654
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x04000F94 RID: 3988
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x0200028F RID: 655
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700018D RID: 397
			// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00053F88 File Offset: 0x00052388
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x04000F95 RID: 3989
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x04000F96 RID: 3990
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x04000F97 RID: 3991
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
