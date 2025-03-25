using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000249 RID: 585
	public sealed class GrainComponent : PostProcessingComponentRenderTexture<GrainModel>
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x00050C90 File Offset: 0x0004F090
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) && !this.context.interrupted;
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00050CE6 File Offset: 0x0004F0E6
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GrainLookupRT);
			this.m_GrainLookupRT = null;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00050CFC File Offset: 0x0004F0FC
		public override void Prepare(Material uberMaterial)
		{
			GrainModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("GRAIN");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float value = Random.value;
			float value2 = Random.value;
			if (this.m_GrainLookupRT == null || !this.m_GrainLookupRT.IsCreated())
			{
				GraphicsUtils.Destroy(this.m_GrainLookupRT);
				this.m_GrainLookupRT = new RenderTexture(192, 192, 0, RenderTextureFormat.ARGBHalf)
				{
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Repeat,
					anisoLevel = 0,
					name = "Grain Lookup Texture"
				};
				this.m_GrainLookupRT.Create();
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Grain Generator");
			material.SetFloat(GrainComponent.Uniforms._Phase, realtimeSinceStartup / 20f);
			Graphics.Blit(null, this.m_GrainLookupRT, material, (!settings.colored) ? 0 : 1);
			uberMaterial.SetTexture(GrainComponent.Uniforms._GrainTex, this.m_GrainLookupRT);
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params2, new Vector4((float)this.context.width / (float)this.m_GrainLookupRT.width / settings.size, (float)this.context.height / (float)this.m_GrainLookupRT.height / settings.size, value, value2));
		}

		// Token: 0x04000E60 RID: 3680
		private RenderTexture m_GrainLookupRT;

		// Token: 0x0200024A RID: 586
		private static class Uniforms
		{
			// Token: 0x04000E61 RID: 3681
			internal static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");

			// Token: 0x04000E62 RID: 3682
			internal static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");

			// Token: 0x04000E63 RID: 3683
			internal static readonly int _GrainTex = Shader.PropertyToID("_GrainTex");

			// Token: 0x04000E64 RID: 3684
			internal static readonly int _Phase = Shader.PropertyToID("_Phase");
		}
	}
}
