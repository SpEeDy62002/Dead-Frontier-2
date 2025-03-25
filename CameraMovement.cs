using System;
using UnityEngine;

// Token: 0x02000BA3 RID: 2979
public class CameraMovement : MonoBehaviour
{
	// Token: 0x060072BA RID: 29370 RVA: 0x003A3453 File Offset: 0x003A1853
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
	}

	// Token: 0x060072BB RID: 29371 RVA: 0x003A3464 File Offset: 0x003A1864
	private void Update()
	{
		if (this.cam)
		{
			this.cam.fieldOfView += this.zoomSpeed * Time.deltaTime;
		}
		base.transform.Translate(this.translateSpeed * Time.deltaTime);
		base.transform.position = base.transform.position + this.positionSpeed * Time.deltaTime;
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x + Time.deltaTime * this.rotationSpeed.x, base.transform.eulerAngles.y + Time.deltaTime * this.rotationSpeed.y, base.transform.eulerAngles.z + Time.deltaTime * this.rotationSpeed.z);
	}

	// Token: 0x0400A44E RID: 42062
	public Vector3 translateSpeed;

	// Token: 0x0400A44F RID: 42063
	public Vector3 positionSpeed;

	// Token: 0x0400A450 RID: 42064
	public float zoomSpeed;

	// Token: 0x0400A451 RID: 42065
	public Vector3 rotationSpeed;

	// Token: 0x0400A452 RID: 42066
	private Camera cam;
}
