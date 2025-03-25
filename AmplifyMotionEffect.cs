using System;
using UnityEngine;

// Token: 0x0200016C RID: 364
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Amplify Motion")]
public class AmplifyMotionEffect : AmplifyMotionEffectBase
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060007ED RID: 2029 RVA: 0x00033A85 File Offset: 0x00031E85
	public new static AmplifyMotionEffect FirstInstance
	{
		get
		{
			return (AmplifyMotionEffect)AmplifyMotionEffectBase.FirstInstance;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060007EE RID: 2030 RVA: 0x00033A91 File Offset: 0x00031E91
	public new static AmplifyMotionEffect Instance
	{
		get
		{
			return (AmplifyMotionEffect)AmplifyMotionEffectBase.Instance;
		}
	}
}
