using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class CameraMode : MonoBehaviour
{
	// Token: 0x060008C6 RID: 2246 RVA: 0x0003ADE4 File Offset: 0x000391E4
	public void ChangeMode(CameraMode.Mode mode)
	{
		base.gameObject.SetActive(this.mode == mode);
	}

	// Token: 0x04000974 RID: 2420
	public CameraMode.Mode mode;

	// Token: 0x02000194 RID: 404
	public enum Mode
	{
		// Token: 0x04000976 RID: 2422
		orbit = -1,
		// Token: 0x04000977 RID: 2423
		main,
		// Token: 0x04000978 RID: 2424
		cam1,
		// Token: 0x04000979 RID: 2425
		cam2,
		// Token: 0x0400097A RID: 2426
		cam3,
		// Token: 0x0400097B RID: 2427
		cam4,
		// Token: 0x0400097C RID: 2428
		cam5,
		// Token: 0x0400097D RID: 2429
		cam6,
		// Token: 0x0400097E RID: 2430
		cam7,
		// Token: 0x0400097F RID: 2431
		cam8,
		// Token: 0x04000980 RID: 2432
		cam9
	}
}
