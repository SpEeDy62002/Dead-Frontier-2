using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmplifyMotion;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x0200016A RID: 362
[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class AmplifyMotionEffectBase : MonoBehaviour
{
	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x0003162D File Offset: 0x0002FA2D
	// (set) Token: 0x0600078B RID: 1931 RVA: 0x00031635 File Offset: 0x0002FA35
	[Obsolete("workerThreads is deprecated, please use WorkerThreads instead.")]
	public int workerThreads
	{
		get
		{
			return this.WorkerThreads;
		}
		set
		{
			this.WorkerThreads = value;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x0003163E File Offset: 0x0002FA3E
	internal Material ReprojectionMaterial
	{
		get
		{
			return this.m_reprojectionMaterial;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600078D RID: 1933 RVA: 0x00031646 File Offset: 0x0002FA46
	internal Material SolidVectorsMaterial
	{
		get
		{
			return this.m_solidVectorsMaterial;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x0003164E File Offset: 0x0002FA4E
	internal Material SkinnedVectorsMaterial
	{
		get
		{
			return this.m_skinnedVectorsMaterial;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x00031656 File Offset: 0x0002FA56
	internal Material ClothVectorsMaterial
	{
		get
		{
			return this.m_clothVectorsMaterial;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x0003165E File Offset: 0x0002FA5E
	internal RenderTexture MotionRenderTexture
	{
		get
		{
			return this.m_motionRT;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x00031666 File Offset: 0x0002FA66
	public Dictionary<Camera, AmplifyMotionCamera> LinkedCameras
	{
		get
		{
			return this.m_linkedCameras;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x0003166E File Offset: 0x0002FA6E
	internal float MotionScaleNorm
	{
		get
		{
			return this.m_motionScaleNorm;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x00031676 File Offset: 0x0002FA76
	internal float FixedMotionScaleNorm
	{
		get
		{
			return this.m_fixedMotionScaleNorm;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x0003167E File Offset: 0x0002FA7E
	public AmplifyMotionCamera BaseCamera
	{
		get
		{
			return this.m_baseCamera;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00031686 File Offset: 0x0002FA86
	internal WorkerThreadPool WorkerPool
	{
		get
		{
			return this.m_workerThreadPool;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x0003168E File Offset: 0x0002FA8E
	public static bool IsD3D
	{
		get
		{
			return AmplifyMotionEffectBase.m_isD3D;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x00031695 File Offset: 0x0002FA95
	public bool CanUseGPU
	{
		get
		{
			return this.m_canUseGPU;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x0003169D File Offset: 0x0002FA9D
	public static bool IgnoreMotionScaleWarning
	{
		get
		{
			return AmplifyMotionEffectBase.m_ignoreMotionScaleWarning;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x000316A4 File Offset: 0x0002FAA4
	public static AmplifyMotionEffectBase FirstInstance
	{
		get
		{
			return AmplifyMotionEffectBase.m_firstInstance;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x000316AB File Offset: 0x0002FAAB
	public static AmplifyMotionEffectBase Instance
	{
		get
		{
			return AmplifyMotionEffectBase.m_firstInstance;
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x000316B4 File Offset: 0x0002FAB4
	private void Awake()
	{
		if (AmplifyMotionEffectBase.m_firstInstance == null)
		{
			AmplifyMotionEffectBase.m_firstInstance = this;
		}
		AmplifyMotionEffectBase.m_isD3D = SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D");
		this.m_globalObjectId = 1;
		this.m_width = (this.m_height = 0);
		if (this.ForceCPUOnly)
		{
			this.m_canUseGPU = false;
		}
		else
		{
			bool flag = SystemInfo.graphicsShaderLevel >= 30;
			bool flag2 = SystemInfo.SupportsTextureFormat(TextureFormat.RHalf);
			bool flag3 = SystemInfo.SupportsTextureFormat(TextureFormat.RGHalf);
			bool flag4 = SystemInfo.SupportsTextureFormat(TextureFormat.RGBAHalf);
			bool flag5 = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat);
			this.m_canUseGPU = flag && flag2 && flag3 && flag4 && flag5;
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0003176C File Offset: 0x0002FB6C
	internal void ResetObjectId()
	{
		this.m_globalObjectId = 1;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00031775 File Offset: 0x0002FB75
	internal int GenerateObjectId(GameObject obj)
	{
		if (obj.isStatic)
		{
			return 0;
		}
		this.m_globalObjectId++;
		if (this.m_globalObjectId > 254)
		{
			this.m_globalObjectId = 1;
		}
		return this.m_globalObjectId;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x000317AF File Offset: 0x0002FBAF
	private void SafeDestroyMaterial(ref Material mat)
	{
		if (mat != null)
		{
			global::UnityEngine.Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x000317C8 File Offset: 0x0002FBC8
	private bool CheckMaterialAndShader(Material material, string name)
	{
		bool flag = true;
		if (material == null || material.shader == null)
		{
			Debug.LogWarning("[AmplifyMotion] Error creating " + name + " material");
			flag = false;
		}
		else if (!material.shader.isSupported)
		{
			Debug.LogWarning("[AmplifyMotion] " + name + " shader not supported on this platform");
			flag = false;
		}
		return flag;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00031838 File Offset: 0x0002FC38
	private void DestroyMaterials()
	{
		this.SafeDestroyMaterial(ref this.m_blurMaterial);
		this.SafeDestroyMaterial(ref this.m_solidVectorsMaterial);
		this.SafeDestroyMaterial(ref this.m_skinnedVectorsMaterial);
		this.SafeDestroyMaterial(ref this.m_clothVectorsMaterial);
		this.SafeDestroyMaterial(ref this.m_reprojectionMaterial);
		this.SafeDestroyMaterial(ref this.m_combineMaterial);
		this.SafeDestroyMaterial(ref this.m_dilationMaterial);
		this.SafeDestroyMaterial(ref this.m_depthMaterial);
		this.SafeDestroyMaterial(ref this.m_debugMaterial);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000318B4 File Offset: 0x0002FCB4
	private bool CreateMaterials()
	{
		this.DestroyMaterials();
		int num = ((SystemInfo.graphicsShaderLevel < 30) ? 2 : 3);
		string text = "Hidden/Amplify Motion/MotionBlurSM" + num;
		string text2 = "Hidden/Amplify Motion/SolidVectors";
		string text3 = "Hidden/Amplify Motion/SkinnedVectors";
		string text4 = "Hidden/Amplify Motion/ClothVectors";
		string text5 = "Hidden/Amplify Motion/ReprojectionVectors";
		string text6 = "Hidden/Amplify Motion/Combine";
		string text7 = "Hidden/Amplify Motion/Dilation";
		string text8 = "Hidden/Amplify Motion/Depth";
		string text9 = "Hidden/Amplify Motion/Debug";
		try
		{
			this.m_blurMaterial = new Material(Shader.Find(text))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_solidVectorsMaterial = new Material(Shader.Find(text2))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_skinnedVectorsMaterial = new Material(Shader.Find(text3))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_clothVectorsMaterial = new Material(Shader.Find(text4))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_reprojectionMaterial = new Material(Shader.Find(text5))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_combineMaterial = new Material(Shader.Find(text6))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_dilationMaterial = new Material(Shader.Find(text7))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_depthMaterial = new Material(Shader.Find(text8))
			{
				hideFlags = HideFlags.DontSave
			};
			this.m_debugMaterial = new Material(Shader.Find(text9))
			{
				hideFlags = HideFlags.DontSave
			};
		}
		catch (Exception)
		{
		}
		bool flag = this.CheckMaterialAndShader(this.m_blurMaterial, text);
		flag = flag && this.CheckMaterialAndShader(this.m_solidVectorsMaterial, text2);
		flag = flag && this.CheckMaterialAndShader(this.m_skinnedVectorsMaterial, text3);
		flag = flag && this.CheckMaterialAndShader(this.m_clothVectorsMaterial, text4);
		flag = flag && this.CheckMaterialAndShader(this.m_reprojectionMaterial, text5);
		flag = flag && this.CheckMaterialAndShader(this.m_combineMaterial, text6);
		flag = flag && this.CheckMaterialAndShader(this.m_dilationMaterial, text7);
		flag = flag && this.CheckMaterialAndShader(this.m_depthMaterial, text8);
		return flag && this.CheckMaterialAndShader(this.m_debugMaterial, text9);
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00031B3C File Offset: 0x0002FF3C
	private RenderTexture CreateRenderTexture(string name, int depth, RenderTextureFormat fmt, RenderTextureReadWrite rw, FilterMode fm)
	{
		RenderTexture renderTexture = new RenderTexture(this.m_width, this.m_height, depth, fmt, rw);
		renderTexture.hideFlags = HideFlags.DontSave;
		renderTexture.name = name;
		renderTexture.wrapMode = TextureWrapMode.Clamp;
		renderTexture.filterMode = fm;
		renderTexture.Create();
		return renderTexture;
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00031B85 File Offset: 0x0002FF85
	private void SafeDestroyRenderTexture(ref RenderTexture rt)
	{
		if (rt != null)
		{
			RenderTexture.active = null;
			rt.Release();
			global::UnityEngine.Object.DestroyImmediate(rt);
			rt = null;
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00031BAB File Offset: 0x0002FFAB
	private void SafeDestroyTexture(ref Texture tex)
	{
		if (tex != null)
		{
			global::UnityEngine.Object.DestroyImmediate(tex);
			tex = null;
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00031BC4 File Offset: 0x0002FFC4
	private void DestroyRenderTextures()
	{
		RenderTexture.active = null;
		this.SafeDestroyRenderTexture(ref this.m_motionRT);
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00031BD8 File Offset: 0x0002FFD8
	private void UpdateRenderTextures(bool qualityChanged)
	{
		int num = Mathf.Max(Mathf.FloorToInt((float)this.m_camera.pixelWidth + 0.5f), 1);
		int num2 = Mathf.Max(Mathf.FloorToInt((float)this.m_camera.pixelHeight + 0.5f), 1);
		if (this.QualityLevel == Quality.Mobile)
		{
			num /= 2;
			num2 /= 2;
		}
		if (this.m_width != num || this.m_height != num2)
		{
			this.m_width = num;
			this.m_height = num2;
			this.DestroyRenderTextures();
		}
		if (this.m_motionRT == null)
		{
			this.m_motionRT = this.CreateRenderTexture("AM-MotionVectors", 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear, FilterMode.Point);
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00031C87 File Offset: 0x00030087
	public bool CheckSupport()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogError("[AmplifyMotion] Initialization failed. This plugin requires support for Image Effects and Render Textures.");
			return false;
		}
		return true;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00031CA0 File Offset: 0x000300A0
	private void InitializeThreadPool()
	{
		if (this.WorkerThreads <= 0)
		{
			this.WorkerThreads = Mathf.Max(Environment.ProcessorCount / 2, 1);
		}
		this.m_workerThreadPool = new WorkerThreadPool();
		this.m_workerThreadPool.InitializeAsyncUpdateThreads(this.WorkerThreads, this.SystemThreadPool);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00031CEE File Offset: 0x000300EE
	private void ShutdownThreadPool()
	{
		if (this.m_workerThreadPool != null)
		{
			this.m_workerThreadPool.FinalizeAsyncUpdateThreads();
			this.m_workerThreadPool = null;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00031D10 File Offset: 0x00030110
	private void InitializeCommandBuffers()
	{
		this.ShutdownCommandBuffers();
		this.m_updateCB = new CommandBuffer();
		this.m_updateCB.name = "AmplifyMotion.Update";
		this.m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m_updateCB);
		this.m_fixedUpdateCB = new CommandBuffer();
		this.m_fixedUpdateCB.name = "AmplifyMotion.FixedUpdate";
		this.m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m_fixedUpdateCB);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00031D80 File Offset: 0x00030180
	private void ShutdownCommandBuffers()
	{
		if (this.m_updateCB != null)
		{
			this.m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m_updateCB);
			this.m_updateCB.Release();
			this.m_updateCB = null;
		}
		if (this.m_fixedUpdateCB != null)
		{
			this.m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m_fixedUpdateCB);
			this.m_fixedUpdateCB.Release();
			this.m_fixedUpdateCB = null;
		}
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00031DF0 File Offset: 0x000301F0
	private void OnEnable()
	{
		this.m_camera = base.GetComponent<Camera>();
		if (!this.CheckSupport())
		{
			base.enabled = false;
			return;
		}
		this.InitializeThreadPool();
		this.m_starting = true;
		if (!this.CreateMaterials())
		{
			Debug.LogError("[AmplifyMotion] Failed loading or compiling necessary shaders. Please try reinstalling Amplify Motion or contact support@amplify.pt");
			base.enabled = false;
			return;
		}
		if (this.AutoRegisterObjs)
		{
			this.UpdateActiveObjects();
		}
		this.InitializeCameras();
		this.InitializeCommandBuffers();
		this.UpdateRenderTextures(true);
		this.m_linkedCameras.TryGetValue(this.m_camera, out this.m_baseCamera);
		if (this.m_baseCamera == null)
		{
			Debug.LogError("[AmplifyMotion] Failed setting up Base Camera. Please contact support@amplify.pt");
			base.enabled = false;
			return;
		}
		if (this.m_currentPostProcess != null)
		{
			this.m_currentPostProcess.enabled = true;
		}
		this.m_qualityLevel = this.QualityLevel;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00031ECE File Offset: 0x000302CE
	private void OnDisable()
	{
		if (this.m_currentPostProcess != null)
		{
			this.m_currentPostProcess.enabled = false;
		}
		this.ShutdownCommandBuffers();
		this.ShutdownThreadPool();
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00031EF9 File Offset: 0x000302F9
	private void Start()
	{
		this.UpdatePostProcess();
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x00031F01 File Offset: 0x00030301
	internal void RemoveCamera(Camera reference)
	{
		this.m_linkedCameras.Remove(reference);
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00031F10 File Offset: 0x00030310
	private void OnDestroy()
	{
		AmplifyMotionCamera[] array = this.m_linkedCameras.Values.ToArray<AmplifyMotionCamera>();
		foreach (AmplifyMotionCamera amplifyMotionCamera in array)
		{
			if (amplifyMotionCamera != null && amplifyMotionCamera.gameObject != base.gameObject)
			{
				Camera component = amplifyMotionCamera.GetComponent<Camera>();
				if (component != null)
				{
					component.targetTexture = null;
				}
				global::UnityEngine.Object.DestroyImmediate(amplifyMotionCamera);
			}
		}
		this.DestroyRenderTextures();
		this.DestroyMaterials();
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00031F9C File Offset: 0x0003039C
	private GameObject RecursiveFindCamera(GameObject obj, string auxCameraName)
	{
		GameObject gameObject = null;
		if (obj.name == auxCameraName)
		{
			gameObject = obj;
		}
		else
		{
			IEnumerator enumerator = obj.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					Transform transform = (Transform)obj2;
					gameObject = this.RecursiveFindCamera(transform.gameObject, auxCameraName);
					if (gameObject != null)
					{
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		return gameObject;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00032034 File Offset: 0x00030434
	private void InitializeCameras()
	{
		List<Camera> list = new List<Camera>(this.OverlayCameras.Length);
		for (int i = 0; i < this.OverlayCameras.Length; i++)
		{
			if (this.OverlayCameras[i] != null)
			{
				list.Add(this.OverlayCameras[i]);
			}
		}
		Camera[] array = new Camera[list.Count + 1];
		array[0] = this.m_camera;
		for (int j = 0; j < list.Count; j++)
		{
			array[j + 1] = list[j];
		}
		this.m_linkedCameras.Clear();
		for (int k = 0; k < array.Length; k++)
		{
			Camera camera = array[k];
			if (!this.m_linkedCameras.ContainsKey(camera))
			{
				AmplifyMotionCamera amplifyMotionCamera = camera.gameObject.GetComponent<AmplifyMotionCamera>();
				if (amplifyMotionCamera != null)
				{
					amplifyMotionCamera.enabled = false;
					amplifyMotionCamera.enabled = true;
				}
				else
				{
					amplifyMotionCamera = camera.gameObject.AddComponent<AmplifyMotionCamera>();
				}
				amplifyMotionCamera.LinkTo(this, k > 0);
				this.m_linkedCameras.Add(camera, amplifyMotionCamera);
				this.m_linkedCamerasChanged = true;
			}
		}
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0003215F File Offset: 0x0003055F
	public void UpdateActiveCameras()
	{
		this.InitializeCameras();
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00032168 File Offset: 0x00030568
	internal static void RegisterCamera(AmplifyMotionCamera cam)
	{
		if (!AmplifyMotionEffectBase.m_activeCameras.ContainsValue(cam))
		{
			AmplifyMotionEffectBase.m_activeCameras.Add(cam.GetComponent<Camera>(), cam);
		}
		foreach (AmplifyMotionObjectBase amplifyMotionObjectBase in AmplifyMotionEffectBase.m_activeObjects.Values)
		{
			amplifyMotionObjectBase.RegisterCamera(cam);
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000321EC File Offset: 0x000305EC
	internal static void UnregisterCamera(AmplifyMotionCamera cam)
	{
		foreach (AmplifyMotionObjectBase amplifyMotionObjectBase in AmplifyMotionEffectBase.m_activeObjects.Values)
		{
			amplifyMotionObjectBase.UnregisterCamera(cam);
		}
		AmplifyMotionEffectBase.m_activeCameras.Remove(cam.GetComponent<Camera>());
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00032260 File Offset: 0x00030660
	public void UpdateActiveObjects()
	{
		GameObject[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		for (int i = 0; i < array.Length; i++)
		{
			if (!AmplifyMotionEffectBase.m_activeObjects.ContainsKey(array[i]))
			{
				AmplifyMotionEffectBase.TryRegister(array[i], true);
			}
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x000322B4 File Offset: 0x000306B4
	internal static void RegisterObject(AmplifyMotionObjectBase obj)
	{
		AmplifyMotionEffectBase.m_activeObjects.Add(obj.gameObject, obj);
		foreach (AmplifyMotionCamera amplifyMotionCamera in AmplifyMotionEffectBase.m_activeCameras.Values)
		{
			obj.RegisterCamera(amplifyMotionCamera);
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00032328 File Offset: 0x00030728
	internal static void UnregisterObject(AmplifyMotionObjectBase obj)
	{
		foreach (AmplifyMotionCamera amplifyMotionCamera in AmplifyMotionEffectBase.m_activeCameras.Values)
		{
			obj.UnregisterCamera(amplifyMotionCamera);
		}
		AmplifyMotionEffectBase.m_activeObjects.Remove(obj.gameObject);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0003239C File Offset: 0x0003079C
	internal static bool FindValidTag(Material[] materials)
	{
		foreach (Material material in materials)
		{
			if (material != null)
			{
				string tag = material.GetTag("RenderType", false);
				if (tag == "Opaque" || tag == "TransparentCutout")
				{
					return !material.IsKeywordEnabled("_ALPHABLEND_ON") && !material.IsKeywordEnabled("_ALPHAPREMULTIPLY_ON");
				}
			}
		}
		return false;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00032420 File Offset: 0x00030820
	internal static bool CanRegister(GameObject gameObj, bool autoReg)
	{
		if (gameObj.isStatic)
		{
			return false;
		}
		Renderer component = gameObj.GetComponent<Renderer>();
		if (component == null || component.sharedMaterials == null || component.isPartOfStaticBatch)
		{
			return false;
		}
		if (!component.enabled)
		{
			return false;
		}
		if (component.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
		{
			return false;
		}
		if (component.GetType() == typeof(SpriteRenderer))
		{
			return false;
		}
		if (!AmplifyMotionEffectBase.FindValidTag(component.sharedMaterials))
		{
			return false;
		}
		Type type = component.GetType();
		if (type == typeof(MeshRenderer) || type == typeof(SkinnedMeshRenderer))
		{
			return true;
		}
		if (type == typeof(ParticleSystemRenderer) && !autoReg)
		{
			ParticleSystemRenderMode renderMode = (component as ParticleSystemRenderer).renderMode;
			return renderMode == ParticleSystemRenderMode.Mesh || renderMode == ParticleSystemRenderMode.Billboard;
		}
		return false;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00032504 File Offset: 0x00030904
	internal static void TryRegister(GameObject gameObj, bool autoReg)
	{
		if (AmplifyMotionEffectBase.CanRegister(gameObj, autoReg) && gameObj.GetComponent<AmplifyMotionObjectBase>() == null)
		{
			AmplifyMotionObjectBase.ApplyToChildren = false;
			gameObj.AddComponent<AmplifyMotionObjectBase>();
			AmplifyMotionObjectBase.ApplyToChildren = true;
		}
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00032538 File Offset: 0x00030938
	internal static void TryUnregister(GameObject gameObj)
	{
		AmplifyMotionObjectBase component = gameObj.GetComponent<AmplifyMotionObjectBase>();
		if (component != null)
		{
			global::UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0003255E File Offset: 0x0003095E
	public void Register(GameObject gameObj)
	{
		if (!AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryRegister(gameObj, false);
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00032577 File Offset: 0x00030977
	public static void RegisterS(GameObject gameObj)
	{
		if (!AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryRegister(gameObj, false);
		}
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00032590 File Offset: 0x00030990
	public void RegisterRecursively(GameObject gameObj)
	{
		if (!AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryRegister(gameObj, false);
		}
		IEnumerator enumerator = gameObj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				this.RegisterRecursively(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00032614 File Offset: 0x00030A14
	public static void RegisterRecursivelyS(GameObject gameObj)
	{
		if (!AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryRegister(gameObj, false);
		}
		IEnumerator enumerator = gameObj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				AmplifyMotionEffectBase.RegisterRecursivelyS(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00032694 File Offset: 0x00030A94
	public void Unregister(GameObject gameObj)
	{
		if (AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryUnregister(gameObj);
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000326AC File Offset: 0x00030AAC
	public static void UnregisterS(GameObject gameObj)
	{
		if (AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryUnregister(gameObj);
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000326C4 File Offset: 0x00030AC4
	public void UnregisterRecursively(GameObject gameObj)
	{
		if (AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryUnregister(gameObj);
		}
		IEnumerator enumerator = gameObj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				this.UnregisterRecursively(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00032744 File Offset: 0x00030B44
	public static void UnregisterRecursivelyS(GameObject gameObj)
	{
		if (AmplifyMotionEffectBase.m_activeObjects.ContainsKey(gameObj))
		{
			AmplifyMotionEffectBase.TryUnregister(gameObj);
		}
		IEnumerator enumerator = gameObj.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				AmplifyMotionEffectBase.UnregisterRecursivelyS(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000327C4 File Offset: 0x00030BC4
	private void UpdatePostProcess()
	{
		Camera camera = null;
		float num = float.MinValue;
		if (this.m_linkedCamerasChanged)
		{
			this.UpdateLinkedCameras();
		}
		for (int i = 0; i < this.m_linkedCameraKeys.Length; i++)
		{
			if (this.m_linkedCameraKeys[i] != null && this.m_linkedCameraKeys[i].isActiveAndEnabled && this.m_linkedCameraKeys[i].depth > num)
			{
				camera = this.m_linkedCameraKeys[i];
				num = this.m_linkedCameraKeys[i].depth;
			}
		}
		if (this.m_currentPostProcess != null && this.m_currentPostProcess.gameObject != camera.gameObject)
		{
			global::UnityEngine.Object.DestroyImmediate(this.m_currentPostProcess);
			this.m_currentPostProcess = null;
		}
		if (this.m_currentPostProcess == null && camera != null && camera != this.m_camera)
		{
			AmplifyMotionPostProcess[] components = base.gameObject.GetComponents<AmplifyMotionPostProcess>();
			if (components != null && components.Length > 0)
			{
				for (int j = 0; j < components.Length; j++)
				{
					global::UnityEngine.Object.DestroyImmediate(components[j]);
				}
			}
			this.m_currentPostProcess = camera.gameObject.AddComponent<AmplifyMotionPostProcess>();
			this.m_currentPostProcess.Instance = this;
		}
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00032918 File Offset: 0x00030D18
	private void LateUpdate()
	{
		if (this.m_baseCamera.AutoStep)
		{
			float num = ((!Application.isPlaying) ? Time.fixedDeltaTime : Time.unscaledDeltaTime);
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.m_deltaTime = ((num <= float.Epsilon) ? this.m_deltaTime : num);
			this.m_fixedDeltaTime = ((num <= float.Epsilon) ? this.m_fixedDeltaTime : fixedDeltaTime);
		}
		this.QualitySteps = Mathf.Clamp(this.QualitySteps, 0, 16);
		this.MotionScale = Mathf.Max(this.MotionScale, 0f);
		this.MinVelocity = Mathf.Min(this.MinVelocity, this.MaxVelocity);
		this.DepthThreshold = Mathf.Max(this.DepthThreshold, 0f);
		this.MinResetDeltaDist = Mathf.Max(this.MinResetDeltaDist, 0f);
		this.MinResetDeltaDistSqr = this.MinResetDeltaDist * this.MinResetDeltaDist;
		this.ResetFrameDelay = Mathf.Max(this.ResetFrameDelay, 0);
		this.UpdatePostProcess();
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00032A28 File Offset: 0x00030E28
	public void StopAutoStep()
	{
		foreach (AmplifyMotionCamera amplifyMotionCamera in this.m_linkedCameras.Values)
		{
			amplifyMotionCamera.StopAutoStep();
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00032A88 File Offset: 0x00030E88
	public void StartAutoStep()
	{
		foreach (AmplifyMotionCamera amplifyMotionCamera in this.m_linkedCameras.Values)
		{
			amplifyMotionCamera.StartAutoStep();
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00032AE8 File Offset: 0x00030EE8
	public void Step(float delta)
	{
		this.m_deltaTime = delta;
		this.m_fixedDeltaTime = delta;
		foreach (AmplifyMotionCamera amplifyMotionCamera in this.m_linkedCameras.Values)
		{
			amplifyMotionCamera.Step();
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00032B58 File Offset: 0x00030F58
	private void UpdateLinkedCameras()
	{
		Dictionary<Camera, AmplifyMotionCamera>.KeyCollection keys = this.m_linkedCameras.Keys;
		Dictionary<Camera, AmplifyMotionCamera>.ValueCollection values = this.m_linkedCameras.Values;
		if (this.m_linkedCameraKeys == null || keys.Count != this.m_linkedCameraKeys.Length)
		{
			this.m_linkedCameraKeys = new Camera[keys.Count];
		}
		if (this.m_linkedCameraValues == null || values.Count != this.m_linkedCameraValues.Length)
		{
			this.m_linkedCameraValues = new AmplifyMotionCamera[values.Count];
		}
		keys.CopyTo(this.m_linkedCameraKeys, 0);
		values.CopyTo(this.m_linkedCameraValues, 0);
		this.m_linkedCamerasChanged = false;
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00032BFC File Offset: 0x00030FFC
	private void FixedUpdate()
	{
		if (this.m_camera.enabled)
		{
			if (this.m_linkedCamerasChanged)
			{
				this.UpdateLinkedCameras();
			}
			this.m_fixedUpdateCB.Clear();
			for (int i = 0; i < this.m_linkedCameraValues.Length; i++)
			{
				if (this.m_linkedCameraValues[i] != null && this.m_linkedCameraValues[i].isActiveAndEnabled)
				{
					this.m_linkedCameraValues[i].FixedUpdateTransform(this, this.m_fixedUpdateCB);
				}
			}
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00032C88 File Offset: 0x00031088
	private void OnPreRender()
	{
		if (this.m_camera.enabled && (Time.frameCount == 1 || Mathf.Abs(Time.unscaledDeltaTime) > 1E-45f))
		{
			if (this.m_linkedCamerasChanged)
			{
				this.UpdateLinkedCameras();
			}
			this.m_updateCB.Clear();
			for (int i = 0; i < this.m_linkedCameraValues.Length; i++)
			{
				if (this.m_linkedCameraValues[i] != null && this.m_linkedCameraValues[i].isActiveAndEnabled)
				{
					this.m_linkedCameraValues[i].UpdateTransform(this, this.m_updateCB);
				}
			}
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00032D34 File Offset: 0x00031134
	private void OnPostRender()
	{
		bool flag = this.QualityLevel != this.m_qualityLevel;
		this.m_qualityLevel = this.QualityLevel;
		this.UpdateRenderTextures(flag);
		this.ResetObjectId();
		bool flag2 = this.CameraMotionMult > float.Epsilon;
		bool flag3 = !flag2 || this.m_starting;
		float num = ((this.DepthThreshold <= float.Epsilon) ? float.MaxValue : (1f / this.DepthThreshold));
		this.m_motionScaleNorm = ((this.m_deltaTime < float.Epsilon) ? 0f : (this.MotionScale * (1f / this.m_deltaTime)));
		this.m_fixedMotionScaleNorm = ((this.m_fixedDeltaTime < float.Epsilon) ? 0f : (this.MotionScale * (1f / this.m_fixedDeltaTime)));
		float num2 = (this.m_starting ? 0f : this.m_motionScaleNorm);
		float num3 = (this.m_starting ? 0f : this.m_fixedMotionScaleNorm);
		Shader.SetGlobalFloat("_AM_MIN_VELOCITY", this.MinVelocity);
		Shader.SetGlobalFloat("_AM_MAX_VELOCITY", this.MaxVelocity);
		Shader.SetGlobalFloat("_AM_RCP_TOTAL_VELOCITY", 1f / (this.MaxVelocity - this.MinVelocity));
		Shader.SetGlobalVector("_AM_DEPTH_THRESHOLD", new Vector2(this.DepthThreshold, num));
		this.m_motionRT.DiscardContents();
		this.m_baseCamera.PreRenderVectors(this.m_motionRT, flag3, num);
		for (int i = 0; i < this.m_linkedCameraValues.Length; i++)
		{
			AmplifyMotionCamera amplifyMotionCamera = this.m_linkedCameraValues[i];
			if (amplifyMotionCamera != null && amplifyMotionCamera.Overlay && amplifyMotionCamera.isActiveAndEnabled)
			{
				amplifyMotionCamera.PreRenderVectors(this.m_motionRT, flag3, num);
				amplifyMotionCamera.RenderVectors(num2, num3, this.QualityLevel);
			}
		}
		if (flag2)
		{
			float num4 = ((this.m_deltaTime < float.Epsilon) ? 0f : (this.MotionScale * this.CameraMotionMult * (1f / this.m_deltaTime)));
			float num5 = (this.m_starting ? 0f : num4);
			this.m_motionRT.DiscardContents();
			this.m_baseCamera.RenderReprojectionVectors(this.m_motionRT, num5);
		}
		this.m_baseCamera.RenderVectors(num2, num3, this.QualityLevel);
		for (int j = 0; j < this.m_linkedCameraValues.Length; j++)
		{
			AmplifyMotionCamera amplifyMotionCamera2 = this.m_linkedCameraValues[j];
			if (amplifyMotionCamera2 != null && amplifyMotionCamera2.Overlay && amplifyMotionCamera2.isActiveAndEnabled)
			{
				amplifyMotionCamera2.RenderVectors(num2, num3, this.QualityLevel);
			}
		}
		this.m_starting = false;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00033024 File Offset: 0x00031424
	private void ApplyMotionBlur(RenderTexture source, RenderTexture destination, Vector4 blurStep)
	{
		bool flag = this.QualityLevel == Quality.Mobile;
		int num = (int)(this.QualityLevel + ((!this.Noise) ? 0 : 4));
		RenderTexture renderTexture = null;
		if (flag)
		{
			renderTexture = RenderTexture.GetTemporary(this.m_width, this.m_height, 0, RenderTextureFormat.ARGB32);
			renderTexture.name = "AM-DepthTemp";
			renderTexture.wrapMode = TextureWrapMode.Clamp;
			renderTexture.filterMode = FilterMode.Point;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(this.m_width, this.m_height, 0, source.format);
		temporary.name = "AM-CombinedTemp";
		temporary.wrapMode = TextureWrapMode.Clamp;
		temporary.filterMode = FilterMode.Point;
		temporary.DiscardContents();
		this.m_combineMaterial.SetTexture("_MotionTex", this.m_motionRT);
		source.filterMode = FilterMode.Point;
		Graphics.Blit(source, temporary, this.m_combineMaterial, 0);
		this.m_blurMaterial.SetTexture("_MotionTex", this.m_motionRT);
		if (flag)
		{
			Graphics.Blit(null, renderTexture, this.m_depthMaterial, 0);
			this.m_blurMaterial.SetTexture("_DepthTex", renderTexture);
		}
		if (this.QualitySteps > 1)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(this.m_width, this.m_height, 0, source.format);
			temporary2.name = "AM-CombinedTemp2";
			temporary2.filterMode = FilterMode.Point;
			float num2 = 1f / (float)this.QualitySteps;
			float num3 = 1f;
			RenderTexture renderTexture2 = temporary;
			RenderTexture renderTexture3 = temporary2;
			for (int i = 0; i < this.QualitySteps; i++)
			{
				if (renderTexture3 != destination)
				{
					renderTexture3.DiscardContents();
				}
				this.m_blurMaterial.SetVector("_AM_BLUR_STEP", blurStep * num3);
				Graphics.Blit(renderTexture2, renderTexture3, this.m_blurMaterial, num);
				if (i < this.QualitySteps - 2)
				{
					RenderTexture renderTexture4 = renderTexture3;
					renderTexture3 = renderTexture2;
					renderTexture2 = renderTexture4;
				}
				else
				{
					renderTexture2 = renderTexture3;
					renderTexture3 = destination;
				}
				num3 -= num2;
			}
			RenderTexture.ReleaseTemporary(temporary2);
		}
		else
		{
			this.m_blurMaterial.SetVector("_AM_BLUR_STEP", blurStep);
			Graphics.Blit(temporary, destination, this.m_blurMaterial, num);
		}
		if (flag)
		{
			this.m_combineMaterial.SetTexture("_MotionTex", this.m_motionRT);
			Graphics.Blit(source, destination, this.m_combineMaterial, 1);
		}
		RenderTexture.ReleaseTemporary(temporary);
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00033271 File Offset: 0x00031671
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.m_currentPostProcess == null)
		{
			this.PostProcess(source, destination);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00033298 File Offset: 0x00031698
	public void PostProcess(RenderTexture source, RenderTexture destination)
	{
		Vector4 zero = Vector4.zero;
		zero.x = this.MaxVelocity / 1000f;
		zero.y = this.MaxVelocity / 1000f;
		RenderTexture renderTexture = null;
		if (QualitySettings.antiAliasing > 1)
		{
			renderTexture = RenderTexture.GetTemporary(this.m_width, this.m_height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			renderTexture.name = "AM-DilatedTemp";
			renderTexture.filterMode = FilterMode.Point;
			this.m_dilationMaterial.SetTexture("_MotionTex", this.m_motionRT);
			Graphics.Blit(this.m_motionRT, renderTexture, this.m_dilationMaterial, 0);
			this.m_dilationMaterial.SetTexture("_MotionTex", renderTexture);
			Graphics.Blit(renderTexture, this.m_motionRT, this.m_dilationMaterial, 1);
		}
		if (this.DebugMode)
		{
			this.m_debugMaterial.SetTexture("_MotionTex", this.m_motionRT);
			Graphics.Blit(source, destination, this.m_debugMaterial);
		}
		else
		{
			this.ApplyMotionBlur(source, destination, zero);
		}
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	// Token: 0x04000815 RID: 2069
	[Header("Motion Blur")]
	public Quality QualityLevel = Quality.Standard;

	// Token: 0x04000816 RID: 2070
	public int QualitySteps = 1;

	// Token: 0x04000817 RID: 2071
	public float MotionScale = 3f;

	// Token: 0x04000818 RID: 2072
	public float CameraMotionMult = 1f;

	// Token: 0x04000819 RID: 2073
	public float MinVelocity = 1f;

	// Token: 0x0400081A RID: 2074
	public float MaxVelocity = 10f;

	// Token: 0x0400081B RID: 2075
	public float DepthThreshold = 0.01f;

	// Token: 0x0400081C RID: 2076
	public bool Noise;

	// Token: 0x0400081D RID: 2077
	[Header("Camera")]
	public Camera[] OverlayCameras = new Camera[0];

	// Token: 0x0400081E RID: 2078
	public LayerMask CullingMask = -1;

	// Token: 0x0400081F RID: 2079
	[Header("Objects")]
	public bool AutoRegisterObjs = true;

	// Token: 0x04000820 RID: 2080
	public float MinResetDeltaDist = 1000f;

	// Token: 0x04000821 RID: 2081
	[NonSerialized]
	public float MinResetDeltaDistSqr;

	// Token: 0x04000822 RID: 2082
	public int ResetFrameDelay = 1;

	// Token: 0x04000823 RID: 2083
	[Header("Low-Level")]
	[FormerlySerializedAs("workerThreads")]
	public int WorkerThreads;

	// Token: 0x04000824 RID: 2084
	public bool SystemThreadPool;

	// Token: 0x04000825 RID: 2085
	public bool ForceCPUOnly;

	// Token: 0x04000826 RID: 2086
	public bool DebugMode;

	// Token: 0x04000827 RID: 2087
	private Camera m_camera;

	// Token: 0x04000828 RID: 2088
	private bool m_starting = true;

	// Token: 0x04000829 RID: 2089
	private int m_width;

	// Token: 0x0400082A RID: 2090
	private int m_height;

	// Token: 0x0400082B RID: 2091
	private RenderTexture m_motionRT;

	// Token: 0x0400082C RID: 2092
	private Material m_blurMaterial;

	// Token: 0x0400082D RID: 2093
	private Material m_solidVectorsMaterial;

	// Token: 0x0400082E RID: 2094
	private Material m_skinnedVectorsMaterial;

	// Token: 0x0400082F RID: 2095
	private Material m_clothVectorsMaterial;

	// Token: 0x04000830 RID: 2096
	private Material m_reprojectionMaterial;

	// Token: 0x04000831 RID: 2097
	private Material m_combineMaterial;

	// Token: 0x04000832 RID: 2098
	private Material m_dilationMaterial;

	// Token: 0x04000833 RID: 2099
	private Material m_depthMaterial;

	// Token: 0x04000834 RID: 2100
	private Material m_debugMaterial;

	// Token: 0x04000835 RID: 2101
	private Dictionary<Camera, AmplifyMotionCamera> m_linkedCameras = new Dictionary<Camera, AmplifyMotionCamera>();

	// Token: 0x04000836 RID: 2102
	internal Camera[] m_linkedCameraKeys;

	// Token: 0x04000837 RID: 2103
	internal AmplifyMotionCamera[] m_linkedCameraValues;

	// Token: 0x04000838 RID: 2104
	internal bool m_linkedCamerasChanged = true;

	// Token: 0x04000839 RID: 2105
	private AmplifyMotionPostProcess m_currentPostProcess;

	// Token: 0x0400083A RID: 2106
	private int m_globalObjectId = 1;

	// Token: 0x0400083B RID: 2107
	private float m_deltaTime;

	// Token: 0x0400083C RID: 2108
	private float m_fixedDeltaTime;

	// Token: 0x0400083D RID: 2109
	private float m_motionScaleNorm;

	// Token: 0x0400083E RID: 2110
	private float m_fixedMotionScaleNorm;

	// Token: 0x0400083F RID: 2111
	private Quality m_qualityLevel;

	// Token: 0x04000840 RID: 2112
	private AmplifyMotionCamera m_baseCamera;

	// Token: 0x04000841 RID: 2113
	private WorkerThreadPool m_workerThreadPool;

	// Token: 0x04000842 RID: 2114
	public static Dictionary<GameObject, AmplifyMotionObjectBase> m_activeObjects = new Dictionary<GameObject, AmplifyMotionObjectBase>();

	// Token: 0x04000843 RID: 2115
	public static Dictionary<Camera, AmplifyMotionCamera> m_activeCameras = new Dictionary<Camera, AmplifyMotionCamera>();

	// Token: 0x04000844 RID: 2116
	private static bool m_isD3D = false;

	// Token: 0x04000845 RID: 2117
	private bool m_canUseGPU;

	// Token: 0x04000846 RID: 2118
	private const CameraEvent m_updateCBEvent = CameraEvent.BeforeImageEffectsOpaque;

	// Token: 0x04000847 RID: 2119
	private CommandBuffer m_updateCB;

	// Token: 0x04000848 RID: 2120
	private const CameraEvent m_fixedUpdateCBEvent = CameraEvent.BeforeImageEffectsOpaque;

	// Token: 0x04000849 RID: 2121
	private CommandBuffer m_fixedUpdateCB;

	// Token: 0x0400084A RID: 2122
	private static bool m_ignoreMotionScaleWarning = false;

	// Token: 0x0400084B RID: 2123
	private static AmplifyMotionEffectBase m_firstInstance = null;
}
