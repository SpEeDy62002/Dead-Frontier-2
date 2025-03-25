using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000295 RID: 661
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("Effects/Post-Processing Behaviour", -1)]
	public class PostProcessingBehaviour : MonoBehaviour
	{
		// Token: 0x06000CA7 RID: 3239 RVA: 0x0005419C File Offset: 0x0005259C
		private void OnEnable()
		{
			this.m_CommandBuffers = new Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>>();
			this.m_MaterialFactory = new MaterialFactory();
			this.m_RenderTextureFactory = new RenderTextureFactory();
			this.m_Context = new PostProcessingContext();
			this.m_Components = new List<PostProcessingComponentBase>();
			this.m_DebugViews = this.AddComponent<BuiltinDebugViewsComponent>(new BuiltinDebugViewsComponent());
			this.m_AmbientOcclusion = this.AddComponent<AmbientOcclusionComponent>(new AmbientOcclusionComponent());
			this.m_ScreenSpaceReflection = this.AddComponent<ScreenSpaceReflectionComponent>(new ScreenSpaceReflectionComponent());
			this.m_FogComponent = this.AddComponent<FogComponent>(new FogComponent());
			this.m_MotionBlur = this.AddComponent<MotionBlurComponent>(new MotionBlurComponent());
			this.m_Taa = this.AddComponent<TaaComponent>(new TaaComponent());
			this.m_EyeAdaptation = this.AddComponent<EyeAdaptationComponent>(new EyeAdaptationComponent());
			this.m_DepthOfField = this.AddComponent<DepthOfFieldComponent>(new DepthOfFieldComponent());
			this.m_Bloom = this.AddComponent<BloomComponent>(new BloomComponent());
			this.m_ChromaticAberration = this.AddComponent<ChromaticAberrationComponent>(new ChromaticAberrationComponent());
			this.m_ColorGrading = this.AddComponent<ColorGradingComponent>(new ColorGradingComponent());
			this.m_UserLut = this.AddComponent<UserLutComponent>(new UserLutComponent());
			this.m_Grain = this.AddComponent<GrainComponent>(new GrainComponent());
			this.m_Vignette = this.AddComponent<VignetteComponent>(new VignetteComponent());
			this.m_Dithering = this.AddComponent<DitheringComponent>(new DitheringComponent());
			this.m_Fxaa = this.AddComponent<FxaaComponent>(new FxaaComponent());
			this.m_ComponentStates = new Dictionary<PostProcessingComponentBase, bool>();
			foreach (PostProcessingComponentBase postProcessingComponentBase in this.m_Components)
			{
				this.m_ComponentStates.Add(postProcessingComponentBase, false);
			}
			base.useGUILayout = false;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00054358 File Offset: 0x00052758
		private void OnPreCull()
		{
			this.m_Camera = base.GetComponent<Camera>();
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			PostProcessingContext postProcessingContext = this.m_Context.Reset();
			postProcessingContext.profile = this.profile;
			postProcessingContext.renderTextureFactory = this.m_RenderTextureFactory;
			postProcessingContext.materialFactory = this.m_MaterialFactory;
			postProcessingContext.camera = this.m_Camera;
			this.m_DebugViews.Init(postProcessingContext, this.profile.debugViews);
			this.m_AmbientOcclusion.Init(postProcessingContext, this.profile.ambientOcclusion);
			this.m_ScreenSpaceReflection.Init(postProcessingContext, this.profile.screenSpaceReflection);
			this.m_FogComponent.Init(postProcessingContext, this.profile.fog);
			this.m_MotionBlur.Init(postProcessingContext, this.profile.motionBlur);
			this.m_Taa.Init(postProcessingContext, this.profile.antialiasing);
			this.m_EyeAdaptation.Init(postProcessingContext, this.profile.eyeAdaptation);
			this.m_DepthOfField.Init(postProcessingContext, this.profile.depthOfField);
			this.m_Bloom.Init(postProcessingContext, this.profile.bloom);
			this.m_ChromaticAberration.Init(postProcessingContext, this.profile.chromaticAberration);
			this.m_ColorGrading.Init(postProcessingContext, this.profile.colorGrading);
			this.m_UserLut.Init(postProcessingContext, this.profile.userLut);
			this.m_Grain.Init(postProcessingContext, this.profile.grain);
			this.m_Vignette.Init(postProcessingContext, this.profile.vignette);
			this.m_Dithering.Init(postProcessingContext, this.profile.dithering);
			this.m_Fxaa.Init(postProcessingContext, this.profile.antialiasing);
			if (this.m_PreviousProfile != this.profile)
			{
				this.DisableComponents();
				this.m_PreviousProfile = this.profile;
			}
			this.CheckObservers();
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			foreach (PostProcessingComponentBase postProcessingComponentBase in this.m_Components)
			{
				if (postProcessingComponentBase.active)
				{
					depthTextureMode |= postProcessingComponentBase.GetCameraFlags();
				}
			}
			postProcessingContext.camera.depthTextureMode = depthTextureMode;
			if (!this.m_RenderingInSceneView && this.m_Taa.active && !this.profile.debugViews.willInterrupt)
			{
				this.m_Taa.SetProjectionMatrix(this.jitteredMatrixFunc);
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0005461C File Offset: 0x00052A1C
		private void OnPreRender()
		{
			if (this.profile == null)
			{
				return;
			}
			this.TryExecuteCommandBuffer<BuiltinDebugViewsModel>(this.m_DebugViews);
			this.TryExecuteCommandBuffer<AmbientOcclusionModel>(this.m_AmbientOcclusion);
			this.TryExecuteCommandBuffer<ScreenSpaceReflectionModel>(this.m_ScreenSpaceReflection);
			this.TryExecuteCommandBuffer<FogModel>(this.m_FogComponent);
			if (!this.m_RenderingInSceneView)
			{
				this.TryExecuteCommandBuffer<MotionBlurModel>(this.m_MotionBlur);
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00054684 File Offset: 0x00052A84
		private void OnPostRender()
		{
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			if (!this.m_RenderingInSceneView && this.m_Taa.active && !this.profile.debugViews.willInterrupt)
			{
				this.m_Context.camera.ResetProjectionMatrix();
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x000546F4 File Offset: 0x00052AF4
		[ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.profile == null || this.m_Camera == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			bool flag = false;
			bool active = this.m_Fxaa.active;
			bool flag2 = this.m_Taa.active && !this.m_RenderingInSceneView;
			bool flag3 = this.m_DepthOfField.active && !this.m_RenderingInSceneView;
			Material material = this.m_MaterialFactory.Get("Hidden/Post FX/Uber Shader");
			material.shaderKeywords = null;
			RenderTexture renderTexture = source;
			if (flag2)
			{
				RenderTexture renderTexture2 = this.m_RenderTextureFactory.Get(renderTexture);
				this.m_Taa.Render(renderTexture, renderTexture2);
				renderTexture = renderTexture2;
			}
			Texture texture = GraphicsUtils.whiteTexture;
			if (this.m_EyeAdaptation.active)
			{
				flag = true;
				texture = this.m_EyeAdaptation.Prepare(renderTexture, material);
			}
			material.SetTexture("_AutoExposure", texture);
			if (flag3)
			{
				flag = true;
				this.m_DepthOfField.Prepare(renderTexture, material, flag2);
			}
			if (this.m_Bloom.active)
			{
				flag = true;
				this.m_Bloom.Prepare(renderTexture, material, texture);
			}
			flag |= this.TryPrepareUberImageEffect<ChromaticAberrationModel>(this.m_ChromaticAberration, material);
			flag |= this.TryPrepareUberImageEffect<ColorGradingModel>(this.m_ColorGrading, material);
			flag |= this.TryPrepareUberImageEffect<VignetteModel>(this.m_Vignette, material);
			flag |= this.TryPrepareUberImageEffect<UserLutModel>(this.m_UserLut, material);
			Material material2 = ((!active) ? null : this.m_MaterialFactory.Get("Hidden/Post FX/FXAA"));
			if (active)
			{
				material2.shaderKeywords = null;
				this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material2);
				this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material2);
				if (flag)
				{
					RenderTexture renderTexture3 = this.m_RenderTextureFactory.Get(renderTexture);
					Graphics.Blit(renderTexture, renderTexture3, material, 0);
					renderTexture = renderTexture3;
				}
				this.m_Fxaa.Render(renderTexture, destination);
			}
			else
			{
				flag |= this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material);
				flag |= this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material);
				if (flag)
				{
					if (!GraphicsUtils.isLinearColorSpace)
					{
						material.EnableKeyword("UNITY_COLORSPACE_GAMMA");
					}
					Graphics.Blit(renderTexture, destination, material, 0);
				}
			}
			if (!flag && !active)
			{
				Graphics.Blit(renderTexture, destination);
			}
			this.m_RenderTextureFactory.ReleaseAll();
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00054960 File Offset: 0x00052D60
		private void OnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			if (this.m_EyeAdaptation.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				this.m_EyeAdaptation.OnGUI();
			}
			else if (this.m_ColorGrading.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut))
			{
				this.m_ColorGrading.OnGUI();
			}
			else if (this.m_UserLut.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut))
			{
				this.m_UserLut.OnGUI();
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00054A40 File Offset: 0x00052E40
		private void OnDisable()
		{
			foreach (KeyValuePair<CameraEvent, CommandBuffer> keyValuePair in this.m_CommandBuffers.Values)
			{
				this.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
				keyValuePair.Value.Dispose();
			}
			this.m_CommandBuffers.Clear();
			if (this.profile != null)
			{
				this.DisableComponents();
			}
			this.m_Components.Clear();
			if (this.m_Camera != null)
			{
				this.m_Camera.depthTextureMode = DepthTextureMode.None;
			}
			this.m_MaterialFactory.Dispose();
			this.m_RenderTextureFactory.Dispose();
			GraphicsUtils.Dispose();
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00054B24 File Offset: 0x00052F24
		public void ResetTemporalEffects()
		{
			this.m_Taa.ResetHistory();
			this.m_MotionBlur.ResetHistory();
			this.m_EyeAdaptation.ResetHistory();
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00054B48 File Offset: 0x00052F48
		private void CheckObservers()
		{
			foreach (KeyValuePair<PostProcessingComponentBase, bool> keyValuePair in this.m_ComponentStates)
			{
				PostProcessingComponentBase key = keyValuePair.Key;
				bool enabled = key.GetModel().enabled;
				if (enabled != keyValuePair.Value)
				{
					if (enabled)
					{
						this.m_ComponentsToEnable.Add(key);
					}
					else
					{
						this.m_ComponentsToDisable.Add(key);
					}
				}
			}
			for (int i = 0; i < this.m_ComponentsToDisable.Count; i++)
			{
				PostProcessingComponentBase postProcessingComponentBase = this.m_ComponentsToDisable[i];
				this.m_ComponentStates[postProcessingComponentBase] = false;
				postProcessingComponentBase.OnDisable();
			}
			for (int j = 0; j < this.m_ComponentsToEnable.Count; j++)
			{
				PostProcessingComponentBase postProcessingComponentBase2 = this.m_ComponentsToEnable[j];
				this.m_ComponentStates[postProcessingComponentBase2] = true;
				postProcessingComponentBase2.OnEnable();
			}
			this.m_ComponentsToDisable.Clear();
			this.m_ComponentsToEnable.Clear();
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00054C80 File Offset: 0x00053080
		private void DisableComponents()
		{
			foreach (PostProcessingComponentBase postProcessingComponentBase in this.m_Components)
			{
				PostProcessingModel model = postProcessingComponentBase.GetModel();
				if (model != null && model.enabled)
				{
					postProcessingComponentBase.OnDisable();
				}
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00054CF4 File Offset: 0x000530F4
		private CommandBuffer AddCommandBuffer<T>(CameraEvent evt, string name) where T : PostProcessingModel
		{
			CommandBuffer commandBuffer = new CommandBuffer
			{
				name = name
			};
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair = new KeyValuePair<CameraEvent, CommandBuffer>(evt, commandBuffer);
			this.m_CommandBuffers.Add(typeof(T), keyValuePair);
			this.m_Camera.AddCommandBuffer(evt, keyValuePair.Value);
			return keyValuePair.Value;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00054D4C File Offset: 0x0005314C
		private void RemoveCommandBuffer<T>() where T : PostProcessingModel
		{
			Type typeFromHandle = typeof(T);
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair;
			if (!this.m_CommandBuffers.TryGetValue(typeFromHandle, out keyValuePair))
			{
				return;
			}
			this.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
			this.m_CommandBuffers.Remove(typeFromHandle);
			keyValuePair.Value.Dispose();
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00054DAC File Offset: 0x000531AC
		private CommandBuffer GetCommandBuffer<T>(CameraEvent evt, string name) where T : PostProcessingModel
		{
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair;
			CommandBuffer commandBuffer;
			if (!this.m_CommandBuffers.TryGetValue(typeof(T), out keyValuePair))
			{
				commandBuffer = this.AddCommandBuffer<T>(evt, name);
			}
			else if (keyValuePair.Key != evt)
			{
				this.RemoveCommandBuffer<T>();
				commandBuffer = this.AddCommandBuffer<T>(evt, name);
			}
			else
			{
				commandBuffer = keyValuePair.Value;
			}
			return commandBuffer;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00054E10 File Offset: 0x00053210
		private void TryExecuteCommandBuffer<T>(PostProcessingComponentCommandBuffer<T> component) where T : PostProcessingModel
		{
			if (component.active)
			{
				CommandBuffer commandBuffer = this.GetCommandBuffer<T>(component.GetCameraEvent(), component.GetName());
				commandBuffer.Clear();
				component.PopulateCommandBuffer(commandBuffer);
			}
			else
			{
				this.RemoveCommandBuffer<T>();
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00054E53 File Offset: 0x00053253
		private bool TryPrepareUberImageEffect<T>(PostProcessingComponentRenderTexture<T> component, Material material) where T : PostProcessingModel
		{
			if (!component.active)
			{
				return false;
			}
			component.Prepare(material);
			return true;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00054E6A File Offset: 0x0005326A
		private T AddComponent<T>(T component) where T : PostProcessingComponentBase
		{
			this.m_Components.Add(component);
			return component;
		}

		// Token: 0x04000FA8 RID: 4008
		public PostProcessingProfile profile;

		// Token: 0x04000FA9 RID: 4009
		public Func<Vector2, Matrix4x4> jitteredMatrixFunc;

		// Token: 0x04000FAA RID: 4010
		private Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>> m_CommandBuffers;

		// Token: 0x04000FAB RID: 4011
		private List<PostProcessingComponentBase> m_Components;

		// Token: 0x04000FAC RID: 4012
		private Dictionary<PostProcessingComponentBase, bool> m_ComponentStates;

		// Token: 0x04000FAD RID: 4013
		private MaterialFactory m_MaterialFactory;

		// Token: 0x04000FAE RID: 4014
		private RenderTextureFactory m_RenderTextureFactory;

		// Token: 0x04000FAF RID: 4015
		private PostProcessingContext m_Context;

		// Token: 0x04000FB0 RID: 4016
		private Camera m_Camera;

		// Token: 0x04000FB1 RID: 4017
		private PostProcessingProfile m_PreviousProfile;

		// Token: 0x04000FB2 RID: 4018
		private bool m_RenderingInSceneView;

		// Token: 0x04000FB3 RID: 4019
		private BuiltinDebugViewsComponent m_DebugViews;

		// Token: 0x04000FB4 RID: 4020
		private AmbientOcclusionComponent m_AmbientOcclusion;

		// Token: 0x04000FB5 RID: 4021
		private ScreenSpaceReflectionComponent m_ScreenSpaceReflection;

		// Token: 0x04000FB6 RID: 4022
		private FogComponent m_FogComponent;

		// Token: 0x04000FB7 RID: 4023
		private MotionBlurComponent m_MotionBlur;

		// Token: 0x04000FB8 RID: 4024
		private TaaComponent m_Taa;

		// Token: 0x04000FB9 RID: 4025
		private EyeAdaptationComponent m_EyeAdaptation;

		// Token: 0x04000FBA RID: 4026
		private DepthOfFieldComponent m_DepthOfField;

		// Token: 0x04000FBB RID: 4027
		private BloomComponent m_Bloom;

		// Token: 0x04000FBC RID: 4028
		private ChromaticAberrationComponent m_ChromaticAberration;

		// Token: 0x04000FBD RID: 4029
		private ColorGradingComponent m_ColorGrading;

		// Token: 0x04000FBE RID: 4030
		private UserLutComponent m_UserLut;

		// Token: 0x04000FBF RID: 4031
		private GrainComponent m_Grain;

		// Token: 0x04000FC0 RID: 4032
		private VignetteComponent m_Vignette;

		// Token: 0x04000FC1 RID: 4033
		private DitheringComponent m_Dithering;

		// Token: 0x04000FC2 RID: 4034
		private FxaaComponent m_Fxaa;

		// Token: 0x04000FC3 RID: 4035
		private List<PostProcessingComponentBase> m_ComponentsToEnable = new List<PostProcessingComponentBase>();

		// Token: 0x04000FC4 RID: 4036
		private List<PostProcessingComponentBase> m_ComponentsToDisable = new List<PostProcessingComponentBase>();
	}
}
