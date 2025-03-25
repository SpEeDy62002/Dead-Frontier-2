using System;
using System.Collections;
using System.Collections.Generic;
using AmplifyMotion;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000172 RID: 370
[AddComponentMenu("")]
public class AmplifyMotionObjectBase : MonoBehaviour
{
	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000805 RID: 2053 RVA: 0x00033ACD File Offset: 0x00031ECD
	internal bool FixedStep
	{
		get
		{
			return this.m_fixedStep;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000806 RID: 2054 RVA: 0x00033AD5 File Offset: 0x00031ED5
	internal int ObjectId
	{
		get
		{
			return this.m_objectId;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000807 RID: 2055 RVA: 0x00033ADD File Offset: 0x00031EDD
	public ObjectType Type
	{
		get
		{
			return this.m_type;
		}
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00033AE8 File Offset: 0x00031EE8
	internal void RegisterCamera(AmplifyMotionCamera camera)
	{
		Camera component = camera.GetComponent<Camera>();
		if ((component.cullingMask & (1 << base.gameObject.layer)) != 0 && !this.m_states.ContainsKey(component))
		{
			MotionState motionState;
			switch (this.m_type)
			{
			case ObjectType.Solid:
				motionState = new SolidState(camera, this);
				break;
			case ObjectType.Skinned:
				motionState = new SkinnedState(camera, this);
				break;
			case ObjectType.Cloth:
				motionState = new ClothState(camera, this);
				break;
			case ObjectType.Particle:
				motionState = new ParticleState(camera, this);
				break;
			default:
				throw new Exception("[AmplifyMotion] Invalid object type.");
			}
			camera.RegisterObject(this);
			this.m_states.Add(component, motionState);
		}
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00033BA4 File Offset: 0x00031FA4
	internal void UnregisterCamera(AmplifyMotionCamera camera)
	{
		Camera component = camera.GetComponent<Camera>();
		MotionState motionState;
		if (this.m_states.TryGetValue(component, out motionState))
		{
			camera.UnregisterObject(this);
			if (this.m_states.TryGetValue(component, out motionState))
			{
				motionState.Shutdown();
			}
			this.m_states.Remove(component);
		}
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00033BF8 File Offset: 0x00031FF8
	private bool InitializeType()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (AmplifyMotionEffectBase.CanRegister(base.gameObject, false))
		{
			ParticleSystem component2 = base.GetComponent<ParticleSystem>();
			if (component2 != null)
			{
				this.m_type = ObjectType.Particle;
				AmplifyMotionEffectBase.RegisterObject(this);
			}
			else if (component != null)
			{
				if (component.GetType() == typeof(MeshRenderer))
				{
					this.m_type = ObjectType.Solid;
				}
				else if (component.GetType() == typeof(SkinnedMeshRenderer))
				{
					if (base.GetComponent<Cloth>() != null)
					{
						this.m_type = ObjectType.Cloth;
					}
					else
					{
						this.m_type = ObjectType.Skinned;
					}
				}
				AmplifyMotionEffectBase.RegisterObject(this);
			}
		}
		return component != null;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00033CB8 File Offset: 0x000320B8
	private void OnEnable()
	{
		bool flag = this.InitializeType();
		if (flag)
		{
			if (this.m_type == ObjectType.Cloth)
			{
				this.m_fixedStep = false;
			}
			else if (this.m_type == ObjectType.Solid)
			{
				Rigidbody component = base.GetComponent<Rigidbody>();
				if (component != null && component.interpolation == RigidbodyInterpolation.None && !component.isKinematic)
				{
					this.m_fixedStep = true;
				}
			}
		}
		if (this.m_applyToChildren)
		{
			IEnumerator enumerator = base.gameObject.transform.GetEnumerator();
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
		if (!flag)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00033DA4 File Offset: 0x000321A4
	private void OnDisable()
	{
		AmplifyMotionEffectBase.UnregisterObject(this);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00033DAC File Offset: 0x000321AC
	private void TryInitializeStates()
	{
		foreach (KeyValuePair<Camera, MotionState> keyValuePair in this.m_states)
		{
			MotionState value = keyValuePair.Value;
			if (value.Owner.Initialized && !value.Error && !value.Initialized)
			{
				value.Initialize();
			}
		}
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00033E12 File Offset: 0x00032212
	private void Start()
	{
		if (AmplifyMotionEffectBase.Instance != null)
		{
			this.TryInitializeStates();
		}
		this.m_lastPosition = base.transform.position;
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x00033E3B File Offset: 0x0003223B
	private void Update()
	{
		if (AmplifyMotionEffectBase.Instance != null)
		{
			this.TryInitializeStates();
		}
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00033E54 File Offset: 0x00032254
	private static void RecursiveResetMotionAtFrame(Transform transform, AmplifyMotionObjectBase obj, int frame)
	{
		if (obj != null)
		{
			obj.m_resetAtFrame = frame;
		}
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				Transform transform2 = (Transform)obj2;
				AmplifyMotionObjectBase.RecursiveResetMotionAtFrame(transform2, transform2.GetComponent<AmplifyMotionObjectBase>(), frame);
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

	// Token: 0x06000811 RID: 2065 RVA: 0x00033ED0 File Offset: 0x000322D0
	public void ResetMotionNow()
	{
		AmplifyMotionObjectBase.RecursiveResetMotionAtFrame(base.transform, this, Time.frameCount);
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00033EE3 File Offset: 0x000322E3
	public void ResetMotionAtFrame(int frame)
	{
		AmplifyMotionObjectBase.RecursiveResetMotionAtFrame(base.transform, this, frame);
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00033EF2 File Offset: 0x000322F2
	private void CheckTeleportReset(AmplifyMotionEffectBase inst)
	{
		if (Vector3.SqrMagnitude(base.transform.position - this.m_lastPosition) > inst.MinResetDeltaDistSqr)
		{
			AmplifyMotionObjectBase.RecursiveResetMotionAtFrame(base.transform, this, Time.frameCount + inst.ResetFrameDelay);
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00033F34 File Offset: 0x00032334
	internal void OnUpdateTransform(AmplifyMotionEffectBase inst, Camera camera, CommandBuffer updateCB, bool starting)
	{
		MotionState motionState;
		if (this.m_states.TryGetValue(camera, out motionState) && !motionState.Error)
		{
			this.CheckTeleportReset(inst);
			bool flag = this.m_resetAtFrame > 0 && Time.frameCount >= this.m_resetAtFrame;
			motionState.UpdateTransform(updateCB, starting || flag);
		}
		this.m_lastPosition = base.transform.position;
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00033FAC File Offset: 0x000323AC
	internal void OnRenderVectors(Camera camera, CommandBuffer renderCB, float scale, Quality quality)
	{
		MotionState motionState;
		if (this.m_states.TryGetValue(camera, out motionState) && !motionState.Error)
		{
			motionState.RenderVectors(camera, renderCB, scale, quality);
			if (this.m_resetAtFrame > 0 && Time.frameCount >= this.m_resetAtFrame)
			{
				this.m_resetAtFrame = -1;
			}
		}
	}

	// Token: 0x0400087D RID: 2173
	internal static bool ApplyToChildren = true;

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private bool m_applyToChildren = AmplifyMotionObjectBase.ApplyToChildren;

	// Token: 0x0400087F RID: 2175
	private ObjectType m_type;

	// Token: 0x04000880 RID: 2176
	private Dictionary<Camera, MotionState> m_states = new Dictionary<Camera, MotionState>();

	// Token: 0x04000881 RID: 2177
	private bool m_fixedStep;

	// Token: 0x04000882 RID: 2178
	private int m_objectId;

	// Token: 0x04000883 RID: 2179
	private Vector3 m_lastPosition = Vector3.zero;

	// Token: 0x04000884 RID: 2180
	private int m_resetAtFrame = -1;

	// Token: 0x02000173 RID: 371
	public enum MinMaxCurveState
	{
		// Token: 0x04000886 RID: 2182
		Scalar,
		// Token: 0x04000887 RID: 2183
		Curve,
		// Token: 0x04000888 RID: 2184
		TwoCurves,
		// Token: 0x04000889 RID: 2185
		TwoScalars
	}
}
