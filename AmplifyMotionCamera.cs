using System;
using System.Collections.Generic;
using AmplifyMotion;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200016B RID: 363
[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public class AmplifyMotionCamera : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000333F0 File Offset: 0x000317F0
	public bool Initialized
	{
		get
		{
			return this.m_initialized;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000333F8 File Offset: 0x000317F8
	public bool AutoStep
	{
		get
		{
			return this.m_autoStep;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00033400 File Offset: 0x00031800
	public bool Overlay
	{
		get
		{
			return this.m_overlay;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060007D6 RID: 2006 RVA: 0x00033408 File Offset: 0x00031808
	public Camera Camera
	{
		get
		{
			return this.m_camera;
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00033410 File Offset: 0x00031810
	public void RegisterObject(AmplifyMotionObjectBase obj)
	{
		this.m_affectedObjectsTable.Add(obj);
		this.m_affectedObjectsChanged = true;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00033426 File Offset: 0x00031826
	public void UnregisterObject(AmplifyMotionObjectBase obj)
	{
		this.m_affectedObjectsTable.Remove(obj);
		this.m_affectedObjectsChanged = true;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0003343C File Offset: 0x0003183C
	private void UpdateAffectedObjects()
	{
		if (this.m_affectedObjects == null || this.m_affectedObjectsTable.Count != this.m_affectedObjects.Length)
		{
			this.m_affectedObjects = new AmplifyMotionObjectBase[this.m_affectedObjectsTable.Count];
		}
		this.m_affectedObjectsTable.CopyTo(this.m_affectedObjects);
		this.m_affectedObjectsChanged = false;
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x0003349A File Offset: 0x0003189A
	public void LinkTo(AmplifyMotionEffectBase instance, bool overlay)
	{
		this.Instance = instance;
		this.m_camera = base.GetComponent<Camera>();
		this.m_camera.depthTextureMode |= DepthTextureMode.Depth;
		this.InitializeCommandBuffers();
		this.m_overlay = overlay;
		this.m_linked = true;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x000334D6 File Offset: 0x000318D6
	public void Initialize()
	{
		this.m_step = false;
		this.UpdateMatrices();
		this.m_initialized = true;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x000334EC File Offset: 0x000318EC
	private void InitializeCommandBuffers()
	{
		this.ShutdownCommandBuffers();
		this.m_renderCB = new CommandBuffer();
		this.m_renderCB.name = "AmplifyMotion.Render";
		this.m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, this.m_renderCB);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00033522 File Offset: 0x00031922
	private void ShutdownCommandBuffers()
	{
		if (this.m_renderCB != null)
		{
			this.m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, this.m_renderCB);
			this.m_renderCB.Release();
			this.m_renderCB = null;
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00033554 File Offset: 0x00031954
	private void Awake()
	{
		this.Transform = base.transform;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00033562 File Offset: 0x00031962
	private void OnEnable()
	{
		AmplifyMotionEffectBase.RegisterCamera(this);
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x0003356A File Offset: 0x0003196A
	private void OnDisable()
	{
		this.m_initialized = false;
		this.ShutdownCommandBuffers();
		AmplifyMotionEffectBase.UnregisterCamera(this);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0003357F File Offset: 0x0003197F
	private void OnDestroy()
	{
		if (this.Instance != null)
		{
			this.Instance.RemoveCamera(this.m_camera);
		}
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x000335A3 File Offset: 0x000319A3
	public void StopAutoStep()
	{
		if (this.m_autoStep)
		{
			this.m_autoStep = false;
			this.m_step = true;
		}
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x000335BE File Offset: 0x000319BE
	public void StartAutoStep()
	{
		this.m_autoStep = true;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x000335C7 File Offset: 0x000319C7
	public void Step()
	{
		this.m_step = true;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x000335D0 File Offset: 0x000319D0
	private void Update()
	{
		if (!this.m_linked || !this.Instance.isActiveAndEnabled)
		{
			return;
		}
		if (!this.m_initialized)
		{
			this.Initialize();
		}
		if ((this.m_camera.depthTextureMode & DepthTextureMode.Depth) == DepthTextureMode.None)
		{
			this.m_camera.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00033630 File Offset: 0x00031A30
	private void UpdateMatrices()
	{
		if (!this.m_starting)
		{
			this.PrevViewProjMatrix = this.ViewProjMatrix;
			this.PrevViewProjMatrixRT = this.ViewProjMatrixRT;
		}
		Matrix4x4 worldToCameraMatrix = this.m_camera.worldToCameraMatrix;
		Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this.m_camera.projectionMatrix, false);
		this.ViewProjMatrix = gpuprojectionMatrix * worldToCameraMatrix;
		this.InvViewProjMatrix = Matrix4x4.Inverse(this.ViewProjMatrix);
		Matrix4x4 gpuprojectionMatrix2 = GL.GetGPUProjectionMatrix(this.m_camera.projectionMatrix, true);
		this.ViewProjMatrixRT = gpuprojectionMatrix2 * worldToCameraMatrix;
		if (this.m_starting)
		{
			this.PrevViewProjMatrix = this.ViewProjMatrix;
			this.PrevViewProjMatrixRT = this.ViewProjMatrixRT;
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x000336E0 File Offset: 0x00031AE0
	public void FixedUpdateTransform(AmplifyMotionEffectBase inst, CommandBuffer updateCB)
	{
		if (!this.m_initialized)
		{
			this.Initialize();
		}
		if (this.m_affectedObjectsChanged)
		{
			this.UpdateAffectedObjects();
		}
		for (int i = 0; i < this.m_affectedObjects.Length; i++)
		{
			if (this.m_affectedObjects[i].FixedStep)
			{
				this.m_affectedObjects[i].OnUpdateTransform(inst, this.m_camera, updateCB, this.m_starting);
			}
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00033758 File Offset: 0x00031B58
	public void UpdateTransform(AmplifyMotionEffectBase inst, CommandBuffer updateCB)
	{
		if (!this.m_initialized)
		{
			this.Initialize();
		}
		if (Time.frameCount > this.m_prevFrameCount && (this.m_autoStep || this.m_step))
		{
			this.UpdateMatrices();
			if (this.m_affectedObjectsChanged)
			{
				this.UpdateAffectedObjects();
			}
			for (int i = 0; i < this.m_affectedObjects.Length; i++)
			{
				if (!this.m_affectedObjects[i].FixedStep)
				{
					this.m_affectedObjects[i].OnUpdateTransform(inst, this.m_camera, updateCB, this.m_starting);
				}
			}
			this.m_starting = false;
			this.m_step = false;
			this.m_prevFrameCount = Time.frameCount;
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00033814 File Offset: 0x00031C14
	public void RenderReprojectionVectors(RenderTexture destination, float scale)
	{
		this.m_renderCB.SetGlobalMatrix("_AM_MATRIX_CURR_REPROJ", this.PrevViewProjMatrix * this.InvViewProjMatrix);
		this.m_renderCB.SetGlobalFloat("_AM_MOTION_SCALE", scale);
		RenderTexture renderTexture = null;
		this.m_renderCB.Blit(new RenderTargetIdentifier(renderTexture), destination, this.Instance.ReprojectionMaterial);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00033878 File Offset: 0x00031C78
	public void PreRenderVectors(RenderTexture motionRT, bool clearColor, float rcpDepthThreshold)
	{
		this.m_renderCB.Clear();
		this.m_renderCB.SetGlobalFloat("_AM_MIN_VELOCITY", this.Instance.MinVelocity);
		this.m_renderCB.SetGlobalFloat("_AM_MAX_VELOCITY", this.Instance.MaxVelocity);
		this.m_renderCB.SetGlobalFloat("_AM_RCP_TOTAL_VELOCITY", 1f / (this.Instance.MaxVelocity - this.Instance.MinVelocity));
		this.m_renderCB.SetGlobalVector("_AM_DEPTH_THRESHOLD", new Vector2(this.Instance.DepthThreshold, rcpDepthThreshold));
		this.m_renderCB.SetRenderTarget(motionRT);
		this.m_renderCB.ClearRenderTarget(true, clearColor, Color.black);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0003393C File Offset: 0x00031D3C
	public void RenderVectors(float scale, float fixedScale, Quality quality)
	{
		if (!this.m_initialized)
		{
			this.Initialize();
		}
		float nearClipPlane = this.m_camera.nearClipPlane;
		float farClipPlane = this.m_camera.farClipPlane;
		Vector4 vector;
		if (AmplifyMotionEffectBase.IsD3D)
		{
			vector.x = 1f - farClipPlane / nearClipPlane;
			vector.y = farClipPlane / nearClipPlane;
		}
		else
		{
			vector.x = (1f - farClipPlane / nearClipPlane) / 2f;
			vector.y = (1f + farClipPlane / nearClipPlane) / 2f;
		}
		vector.z = vector.x / farClipPlane;
		vector.w = vector.y / farClipPlane;
		this.m_renderCB.SetGlobalVector("_AM_ZBUFFER_PARAMS", vector);
		if (this.m_affectedObjectsChanged)
		{
			this.UpdateAffectedObjects();
		}
		for (int i = 0; i < this.m_affectedObjects.Length; i++)
		{
			if ((this.m_camera.cullingMask & (1 << this.m_affectedObjects[i].gameObject.layer)) != 0)
			{
				this.m_affectedObjects[i].OnRenderVectors(this.m_camera, this.m_renderCB, (!this.m_affectedObjects[i].FixedStep) ? scale : fixedScale, quality);
			}
		}
	}

	// Token: 0x0400084C RID: 2124
	internal AmplifyMotionEffectBase Instance;

	// Token: 0x0400084D RID: 2125
	internal Matrix4x4 PrevViewProjMatrix;

	// Token: 0x0400084E RID: 2126
	internal Matrix4x4 ViewProjMatrix;

	// Token: 0x0400084F RID: 2127
	internal Matrix4x4 InvViewProjMatrix;

	// Token: 0x04000850 RID: 2128
	internal Matrix4x4 PrevViewProjMatrixRT;

	// Token: 0x04000851 RID: 2129
	internal Matrix4x4 ViewProjMatrixRT;

	// Token: 0x04000852 RID: 2130
	internal Transform Transform;

	// Token: 0x04000853 RID: 2131
	private bool m_linked;

	// Token: 0x04000854 RID: 2132
	private bool m_initialized;

	// Token: 0x04000855 RID: 2133
	private bool m_starting = true;

	// Token: 0x04000856 RID: 2134
	private bool m_autoStep = true;

	// Token: 0x04000857 RID: 2135
	private bool m_step;

	// Token: 0x04000858 RID: 2136
	private bool m_overlay;

	// Token: 0x04000859 RID: 2137
	private Camera m_camera;

	// Token: 0x0400085A RID: 2138
	private int m_prevFrameCount;

	// Token: 0x0400085B RID: 2139
	private HashSet<AmplifyMotionObjectBase> m_affectedObjectsTable = new HashSet<AmplifyMotionObjectBase>();

	// Token: 0x0400085C RID: 2140
	private AmplifyMotionObjectBase[] m_affectedObjects;

	// Token: 0x0400085D RID: 2141
	private bool m_affectedObjectsChanged = true;

	// Token: 0x0400085E RID: 2142
	private const CameraEvent m_renderCBEvent = CameraEvent.BeforeImageEffects;

	// Token: 0x0400085F RID: 2143
	private CommandBuffer m_renderCB;
}
