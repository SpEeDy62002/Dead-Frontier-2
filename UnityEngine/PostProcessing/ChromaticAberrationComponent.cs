using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200023B RID: 571
	public sealed class ChromaticAberrationComponent : PostProcessingComponentRenderTexture<ChromaticAberrationModel>
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0004EBA0 File Offset: 0x0004CFA0
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0004EBEB File Offset: 0x0004CFEB
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_SpectrumLut);
			this.m_SpectrumLut = null;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0004EC00 File Offset: 0x0004D000
		public override void Prepare(Material uberMaterial)
		{
			ChromaticAberrationModel.Settings settings = base.model.settings;
			Texture2D texture2D = settings.spectralTexture;
			if (texture2D == null)
			{
				if (this.m_SpectrumLut == null)
				{
					this.m_SpectrumLut = new Texture2D(3, 1, TextureFormat.RGB24, false)
					{
						name = "Chromatic Aberration Spectrum Lookup",
						filterMode = FilterMode.Bilinear,
						wrapMode = TextureWrapMode.Clamp,
						anisoLevel = 0,
						hideFlags = HideFlags.DontSave
					};
					Color[] array = new Color[]
					{
						new Color(1f, 0f, 0f),
						new Color(0f, 1f, 0f),
						new Color(0f, 0f, 1f)
					};
					this.m_SpectrumLut.SetPixels(array);
					this.m_SpectrumLut.Apply();
				}
				texture2D = this.m_SpectrumLut;
			}
			uberMaterial.EnableKeyword("CHROMATIC_ABERRATION");
			uberMaterial.SetFloat(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Amount, settings.intensity * 0.03f);
			uberMaterial.SetTexture(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Spectrum, texture2D);
		}

		// Token: 0x04000E19 RID: 3609
		private Texture2D m_SpectrumLut;

		// Token: 0x0200023C RID: 572
		private static class Uniforms
		{
			// Token: 0x04000E1A RID: 3610
			internal static readonly int _ChromaticAberration_Amount = Shader.PropertyToID("_ChromaticAberration_Amount");

			// Token: 0x04000E1B RID: 3611
			internal static readonly int _ChromaticAberration_Spectrum = Shader.PropertyToID("_ChromaticAberration_Spectrum");
		}
	}
}
