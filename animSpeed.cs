using System;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class animSpeed : MonoBehaviour
{
	// Token: 0x0600072C RID: 1836 RVA: 0x0002E26E File Offset: 0x0002C66E
	private void Start()
	{
		base.gameObject.GetComponent<Animation>()[this.clipName].speed = this.s;
	}

	// Token: 0x0400078F RID: 1935
	public float s = 0.2f;

	// Token: 0x04000790 RID: 1936
	public string clipName = string.Empty;
}
