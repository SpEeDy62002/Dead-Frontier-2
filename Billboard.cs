using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class Billboard : MonoBehaviour
{
	// Token: 0x060006F4 RID: 1780 RVA: 0x0002CDBC File Offset: 0x0002B1BC
	public Vector3 GetAxis(Billboard.Axis refAxis)
	{
		switch (refAxis)
		{
		case Billboard.Axis.down:
			return Vector3.down;
		case Billboard.Axis.left:
			return Vector3.left;
		case Billboard.Axis.right:
			return Vector3.right;
		case Billboard.Axis.forward:
			return Vector3.forward;
		case Billboard.Axis.back:
			return Vector3.back;
		default:
			return Vector3.up;
		}
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002CE0D File Offset: 0x0002B20D
	private void Awake()
	{
		if (!this.referenceCamera)
		{
			this.referenceCamera = Camera.main;
		}
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002CE2C File Offset: 0x0002B22C
	private void Update()
	{
		Vector3 vector = base.transform.position + this.referenceCamera.transform.rotation * ((!this.reverseFace) ? Vector3.back : Vector3.forward);
		Vector3 vector2 = this.referenceCamera.transform.rotation * this.GetAxis(this.axis);
		base.transform.LookAt(vector, vector2);
	}

	// Token: 0x04000738 RID: 1848
	public Camera referenceCamera;

	// Token: 0x04000739 RID: 1849
	public bool reverseFace;

	// Token: 0x0400073A RID: 1850
	public Billboard.Axis axis;

	// Token: 0x02000140 RID: 320
	public enum Axis
	{
		// Token: 0x0400073C RID: 1852
		up,
		// Token: 0x0400073D RID: 1853
		down,
		// Token: 0x0400073E RID: 1854
		left,
		// Token: 0x0400073F RID: 1855
		right,
		// Token: 0x04000740 RID: 1856
		forward,
		// Token: 0x04000741 RID: 1857
		back
	}
}
