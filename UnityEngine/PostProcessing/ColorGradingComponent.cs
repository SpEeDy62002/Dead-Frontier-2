using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200023D RID: 573
	public sealed class ColorGradingComponent : PostProcessingComponentRenderTexture<ColorGradingModel>
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0004ED53 File Offset: 0x0004D153
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0004ED76 File Offset: 0x0004D176
		private float StandardIlluminantY(float x)
		{
			return 2.87f * x - 3f * x * x - 0.27509508f;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0004ED90 File Offset: 0x0004D190
		private Vector3 CIExyToLMS(float x, float y)
		{
			float num = 1f;
			float num2 = num * x / y;
			float num3 = num * (1f - x - y) / y;
			float num4 = 0.7328f * num2 + 0.4296f * num - 0.1624f * num3;
			float num5 = -0.7036f * num2 + 1.6975f * num + 0.0061f * num3;
			float num6 = 0.003f * num2 + 0.0136f * num + 0.9834f * num3;
			return new Vector3(num4, num5, num6);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0004EE0C File Offset: 0x0004D20C
		private Vector3 CalculateColorBalance(float temperature, float tint)
		{
			float num = temperature / 55f;
			float num2 = tint / 55f;
			float num3 = 0.31271f - num * ((num >= 0f) ? 0.05f : 0.1f);
			float num4 = this.StandardIlluminantY(num3) + num2 * 0.05f;
			Vector3 vector = new Vector3(0.949237f, 1.03542f, 1.08728f);
			Vector3 vector2 = this.CIExyToLMS(num3, num4);
			return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0004EEB0 File Offset: 0x0004D2B0
		private static Color NormalizeColor(Color c)
		{
			float num = (c.r + c.g + c.b) / 3f;
			if (Mathf.Approximately(num, 0f))
			{
				return new Color(1f, 1f, 1f, c.a);
			}
			return new Color
			{
				r = c.r / num,
				g = c.g / num,
				b = c.b / num,
				a = c.a
			};
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0004EF4E File Offset: 0x0004D34E
		private static Vector3 ClampVector(Vector3 v, float min, float max)
		{
			return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0004EF80 File Offset: 0x0004D380
		public static Vector3 GetLiftValue(Color lift)
		{
			Color color = ColorGradingComponent.NormalizeColor(lift);
			float num = (color.r + color.g + color.b) / 3f;
			float num2 = (color.r - num) * 0.1f + lift.a;
			float num3 = (color.g - num) * 0.1f + lift.a;
			float num4 = (color.b - num) * 0.1f + lift.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), -1f, 1f);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0004F014 File Offset: 0x0004D414
		public static Vector3 GetGammaValue(Color gamma)
		{
			Color color = ColorGradingComponent.NormalizeColor(gamma);
			float num = (color.r + color.g + color.b) / 3f;
			gamma.a *= ((gamma.a >= 0f) ? 5f : 0.8f);
			float num2 = Mathf.Pow(2f, (color.r - num) * 0.5f) + gamma.a;
			float num3 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gamma.a;
			float num4 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gamma.a;
			float num5 = 1f / Mathf.Max(0.01f, num2);
			float num6 = 1f / Mathf.Max(0.01f, num3);
			float num7 = 1f / Mathf.Max(0.01f, num4);
			return ColorGradingComponent.ClampVector(new Vector3(num5, num6, num7), 0f, 5f);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0004F130 File Offset: 0x0004D530
		public static Vector3 GetGainValue(Color gain)
		{
			Color color = ColorGradingComponent.NormalizeColor(gain);
			float num = (color.r + color.g + color.b) / 3f;
			gain.a *= ((gain.a <= 0f) ? 1f : 3f);
			float num2 = Mathf.Pow(2f, (color.r - num) * 0.5f) + gain.a;
			float num3 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gain.a;
			float num4 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gain.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), 0f, 4f);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0004F20F File Offset: 0x0004D60F
		public static void CalculateLiftGammaGain(Color lift, Color gamma, Color gain, out Vector3 outLift, out Vector3 outGamma, out Vector3 outGain)
		{
			outLift = ColorGradingComponent.GetLiftValue(lift);
			outGamma = ColorGradingComponent.GetGammaValue(gamma);
			outGain = ColorGradingComponent.GetGainValue(gain);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0004F238 File Offset: 0x0004D638
		public static Vector3 GetSlopeValue(Color slope)
		{
			Color color = ColorGradingComponent.NormalizeColor(slope);
			float num = (color.r + color.g + color.b) / 3f;
			slope.a *= 0.5f;
			float num2 = (color.r - num) * 0.1f + slope.a + 1f;
			float num3 = (color.g - num) * 0.1f + slope.a + 1f;
			float num4 = (color.b - num) * 0.1f + slope.a + 1f;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), 0f, 2f);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0004F2F0 File Offset: 0x0004D6F0
		public static Vector3 GetPowerValue(Color power)
		{
			Color color = ColorGradingComponent.NormalizeColor(power);
			float num = (color.r + color.g + color.b) / 3f;
			power.a *= 0.5f;
			float num2 = (color.r - num) * 0.1f + power.a + 1f;
			float num3 = (color.g - num) * 0.1f + power.a + 1f;
			float num4 = (color.b - num) * 0.1f + power.a + 1f;
			float num5 = 1f / Mathf.Max(0.01f, num2);
			float num6 = 1f / Mathf.Max(0.01f, num3);
			float num7 = 1f / Mathf.Max(0.01f, num4);
			return ColorGradingComponent.ClampVector(new Vector3(num5, num6, num7), 0.5f, 2.5f);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0004F3E4 File Offset: 0x0004D7E4
		public static Vector3 GetOffsetValue(Color offset)
		{
			Color color = ColorGradingComponent.NormalizeColor(offset);
			float num = (color.r + color.g + color.b) / 3f;
			offset.a *= 0.5f;
			float num2 = (color.r - num) * 0.05f + offset.a;
			float num3 = (color.g - num) * 0.05f + offset.a;
			float num4 = (color.b - num) * 0.05f + offset.a;
			return ColorGradingComponent.ClampVector(new Vector3(num2, num3, num4), -0.8f, 0.8f);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0004F48A File Offset: 0x0004D88A
		public static void CalculateSlopePowerOffset(Color slope, Color power, Color offset, out Vector3 outSlope, out Vector3 outPower, out Vector3 outOffset)
		{
			outSlope = ColorGradingComponent.GetSlopeValue(slope);
			outPower = ColorGradingComponent.GetPowerValue(power);
			outOffset = ColorGradingComponent.GetOffsetValue(offset);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0004F4B4 File Offset: 0x0004D8B4
		private Texture2D GetCurveTexture()
		{
			if (this.m_GradingCurves == null)
			{
				this.m_GradingCurves = new Texture2D(128, 2, TextureFormat.RGBAHalf, false, true)
				{
					name = "Internal Curves Texture",
					hideFlags = HideFlags.DontSave,
					anisoLevel = 0,
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
			}
			Color[] array = new Color[256];
			ColorGradingModel.CurvesSettings curves = base.model.settings.curves;
			curves.hueVShue.Cache();
			curves.hueVSsat.Cache();
			for (int i = 0; i < 128; i++)
			{
				float num = (float)i * 0.0078125f;
				float num2 = curves.hueVShue.Evaluate(num);
				float num3 = curves.hueVSsat.Evaluate(num);
				float num4 = curves.satVSsat.Evaluate(num);
				float num5 = curves.lumVSsat.Evaluate(num);
				array[i] = new Color(num2, num3, num4, num5);
				float num6 = curves.master.Evaluate(num);
				float num7 = curves.red.Evaluate(num);
				float num8 = curves.green.Evaluate(num);
				float num9 = curves.blue.Evaluate(num);
				array[i + 128] = new Color(num7, num8, num9, num6);
			}
			this.m_GradingCurves.SetPixels(array);
			this.m_GradingCurves.Apply(false, false);
			return this.m_GradingCurves;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0004F646 File Offset: 0x0004DA46
		private bool IsLogLutValid(RenderTexture lut)
		{
			return lut != null && lut.IsCreated() && lut.height == 32;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0004F66C File Offset: 0x0004DA6C
		private void GenerateLut()
		{
			ColorGradingModel.Settings settings = base.model.settings;
			if (!this.IsLogLutValid(base.model.bakedLut))
			{
				GraphicsUtils.Destroy(base.model.bakedLut);
				base.model.bakedLut = new RenderTexture(1024, 32, 0, RenderTextureFormat.ARGBHalf)
				{
					name = "Color Grading Log LUT",
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					anisoLevel = 0
				};
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Lut Generator");
			material.SetVector(ColorGradingComponent.Uniforms._LutParams, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			material.shaderKeywords = null;
			ColorGradingModel.TonemappingSettings tonemapping = settings.tonemapping;
			ColorGradingModel.Tonemapper tonemapper = tonemapping.tonemapper;
			if (tonemapper != ColorGradingModel.Tonemapper.Neutral)
			{
				if (tonemapper == ColorGradingModel.Tonemapper.ACES)
				{
					material.EnableKeyword("TONEMAPPING_FILMIC");
				}
			}
			else
			{
				material.EnableKeyword("TONEMAPPING_NEUTRAL");
				float num = tonemapping.neutralBlackIn * 20f + 1f;
				float num2 = tonemapping.neutralBlackOut * 10f + 1f;
				float num3 = tonemapping.neutralWhiteIn / 20f;
				float num4 = 1f - tonemapping.neutralWhiteOut / 20f;
				float num5 = num / num2;
				float num6 = num3 / num4;
				float num7 = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, num5));
				float num8 = Mathf.LerpUnclamped(0.01f, 0.24f, num6);
				float num9 = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, num5));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams1, new Vector4(0.2f, num7, num8, num9));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams2, new Vector4(0.02f, 0.3f, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10f));
			}
			material.SetFloat(ColorGradingComponent.Uniforms._HueShift, settings.basic.hueShift / 360f);
			material.SetFloat(ColorGradingComponent.Uniforms._Saturation, settings.basic.saturation);
			material.SetFloat(ColorGradingComponent.Uniforms._Contrast, settings.basic.contrast);
			material.SetVector(ColorGradingComponent.Uniforms._Balance, this.CalculateColorBalance(settings.basic.temperature, settings.basic.tint));
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			ColorGradingComponent.CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, out vector, out vector2, out vector3);
			material.SetVector(ColorGradingComponent.Uniforms._Lift, vector);
			material.SetVector(ColorGradingComponent.Uniforms._InvGamma, vector2);
			material.SetVector(ColorGradingComponent.Uniforms._Gain, vector3);
			Vector3 vector4;
			Vector3 vector5;
			Vector3 vector6;
			ColorGradingComponent.CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, out vector4, out vector5, out vector6);
			material.SetVector(ColorGradingComponent.Uniforms._Slope, vector4);
			material.SetVector(ColorGradingComponent.Uniforms._Power, vector5);
			material.SetVector(ColorGradingComponent.Uniforms._Offset, vector6);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerRed, settings.channelMixer.red);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerGreen, settings.channelMixer.green);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerBlue, settings.channelMixer.blue);
			material.SetTexture(ColorGradingComponent.Uniforms._Curves, this.GetCurveTexture());
			Graphics.Blit(null, base.model.bakedLut, material, 0);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0004FA40 File Offset: 0x0004DE40
		public override void Prepare(Material uberMaterial)
		{
			if (base.model.isDirty || !this.IsLogLutValid(base.model.bakedLut))
			{
				this.GenerateLut();
				base.model.isDirty = false;
			}
			uberMaterial.EnableKeyword((!this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog)) ? "COLOR_GRADING" : "COLOR_GRADING_LOG_VIEW");
			RenderTexture bakedLut = base.model.bakedLut;
			uberMaterial.SetTexture(ColorGradingComponent.Uniforms._LogLut, bakedLut);
			uberMaterial.SetVector(ColorGradingComponent.Uniforms._LogLut_Params, new Vector3(1f / (float)bakedLut.width, 1f / (float)bakedLut.height, (float)bakedLut.height - 1f));
			float num = Mathf.Exp(base.model.settings.basic.postExposure * 0.6931472f);
			uberMaterial.SetFloat(ColorGradingComponent.Uniforms._ExposureEV, num);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0004FB3C File Offset: 0x0004DF3C
		public void OnGUI()
		{
			RenderTexture bakedLut = base.model.bakedLut;
			Rect rect = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)bakedLut.width, (float)bakedLut.height);
			GUI.DrawTexture(rect, bakedLut);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0004FB96 File Offset: 0x0004DF96
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GradingCurves);
			GraphicsUtils.Destroy(base.model.bakedLut);
			this.m_GradingCurves = null;
			base.model.bakedLut = null;
		}

		// Token: 0x04000E1C RID: 3612
		private const int k_InternalLogLutSize = 32;

		// Token: 0x04000E1D RID: 3613
		private const int k_CurvePrecision = 128;

		// Token: 0x04000E1E RID: 3614
		private const float k_CurveStep = 0.0078125f;

		// Token: 0x04000E1F RID: 3615
		private Texture2D m_GradingCurves;

		// Token: 0x0200023E RID: 574
		private static class Uniforms
		{
			// Token: 0x04000E20 RID: 3616
			internal static readonly int _LutParams = Shader.PropertyToID("_LutParams");

			// Token: 0x04000E21 RID: 3617
			internal static readonly int _NeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");

			// Token: 0x04000E22 RID: 3618
			internal static readonly int _NeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");

			// Token: 0x04000E23 RID: 3619
			internal static readonly int _HueShift = Shader.PropertyToID("_HueShift");

			// Token: 0x04000E24 RID: 3620
			internal static readonly int _Saturation = Shader.PropertyToID("_Saturation");

			// Token: 0x04000E25 RID: 3621
			internal static readonly int _Contrast = Shader.PropertyToID("_Contrast");

			// Token: 0x04000E26 RID: 3622
			internal static readonly int _Balance = Shader.PropertyToID("_Balance");

			// Token: 0x04000E27 RID: 3623
			internal static readonly int _Lift = Shader.PropertyToID("_Lift");

			// Token: 0x04000E28 RID: 3624
			internal static readonly int _InvGamma = Shader.PropertyToID("_InvGamma");

			// Token: 0x04000E29 RID: 3625
			internal static readonly int _Gain = Shader.PropertyToID("_Gain");

			// Token: 0x04000E2A RID: 3626
			internal static readonly int _Slope = Shader.PropertyToID("_Slope");

			// Token: 0x04000E2B RID: 3627
			internal static readonly int _Power = Shader.PropertyToID("_Power");

			// Token: 0x04000E2C RID: 3628
			internal static readonly int _Offset = Shader.PropertyToID("_Offset");

			// Token: 0x04000E2D RID: 3629
			internal static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

			// Token: 0x04000E2E RID: 3630
			internal static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

			// Token: 0x04000E2F RID: 3631
			internal static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

			// Token: 0x04000E30 RID: 3632
			internal static readonly int _Curves = Shader.PropertyToID("_Curves");

			// Token: 0x04000E31 RID: 3633
			internal static readonly int _LogLut = Shader.PropertyToID("_LogLut");

			// Token: 0x04000E32 RID: 3634
			internal static readonly int _LogLut_Params = Shader.PropertyToID("_LogLut_Params");

			// Token: 0x04000E33 RID: 3635
			internal static readonly int _ExposureEV = Shader.PropertyToID("_ExposureEV");
		}
	}
}
