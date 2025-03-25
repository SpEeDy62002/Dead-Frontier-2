using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000241 RID: 577
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x000501E3 File Offset: 0x0004E5E3
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00050206 File Offset: 0x0004E606
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00050210 File Offset: 0x0004E610
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i);
			}
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0005025C File Offset: 0x0004E65C
		public override void Prepare(Material uberMaterial)
		{
			if (++this.textureIndex >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x04000E42 RID: 3650
		private Texture2D[] noiseTextures;

		// Token: 0x04000E43 RID: 3651
		private int textureIndex;

		// Token: 0x04000E44 RID: 3652
		private const int k_TextureCount = 64;

		// Token: 0x02000242 RID: 578
		private static class Uniforms
		{
			// Token: 0x04000E45 RID: 3653
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x04000E46 RID: 3654
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
