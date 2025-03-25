using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class CasingScript : MonoBehaviour
{
	// Token: 0x060008B7 RID: 2231 RVA: 0x0003A73B File Offset: 0x00038B3B
	private void Awake()
	{
		base.Invoke("DetachParent", 0.3f);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0003A74D File Offset: 0x00038B4D
	private void DetachParent()
	{
		base.transform.SetParent(null);
	}
}
