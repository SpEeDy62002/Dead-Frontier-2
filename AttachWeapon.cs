using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class AttachWeapon : MonoBehaviour
{
	// Token: 0x06000198 RID: 408 RVA: 0x0000CAF1 File Offset: 0x0000AEF1
	private void Start()
	{
		this.Weapon.parent = this.attachPoint;
		this.Weapon.position = this.attachPoint.position;
		this.Weapon.rotation = this.attachPoint.rotation;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000CB30 File Offset: 0x0000AF30
	private void Update()
	{
	}

	// Token: 0x04000176 RID: 374
	public Transform attachPoint;

	// Token: 0x04000177 RID: 375
	public Transform Weapon;
}
