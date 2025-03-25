using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000256 RID: 598
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00052D64 File Offset: 0x00051164
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00052DE8 File Offset: 0x000511E8
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00052E70 File Offset: 0x00051270
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			Rect rect = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height);
			GUI.DrawTexture(rect, settings.lut);
		}

		// Token: 0x02000257 RID: 599
		private static class Uniforms
		{
			// Token: 0x04000ED3 RID: 3795
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04000ED4 RID: 3796
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
