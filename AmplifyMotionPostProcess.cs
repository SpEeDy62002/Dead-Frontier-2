using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public sealed class AmplifyMotionPostProcess : MonoBehaviour
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000818 RID: 2072 RVA: 0x00034A42 File Offset: 0x00032E42
	// (set) Token: 0x06000819 RID: 2073 RVA: 0x00034A4A File Offset: 0x00032E4A
	public AmplifyMotionEffectBase Instance
	{
		get
		{
			return this.m_instance;
		}
		set
		{
			this.m_instance = value;
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00034A53 File Offset: 0x00032E53
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.m_instance != null)
		{
			this.m_instance.PostProcess(source, destination);
		}
	}

	// Token: 0x0400088A RID: 2186
	private AmplifyMotionEffectBase m_instance;
}
