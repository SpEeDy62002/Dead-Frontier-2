using System;
using UnityEngine;

namespace Xml.Examples.WorldSpace
{
	// Token: 0x02000C18 RID: 3096
	public class WSExampleCameraController : MonoBehaviour
	{
		// Token: 0x06007558 RID: 30040 RVA: 0x003B124C File Offset: 0x003AF64C
		private void Start()
		{
			this.mouseX = base.transform.rotation.eulerAngles.y;
			this.mouseY = base.transform.rotation.eulerAngles.x;
			this.mouseZ = base.transform.rotation.eulerAngles.z;
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x003B12C0 File Offset: 0x003AF6C0
		private void Update()
		{
			if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				this.mouseX += Input.GetAxis("Mouse X") * this.xRotationSpeed;
				if (this.mouseX <= -180f)
				{
					this.mouseX += 360f;
				}
				else if (this.mouseX > 180f)
				{
					this.mouseX -= 360f;
				}
				this.mouseY -= Input.GetAxis("Mouse Y") * this.yRotationSpeed;
				if (this.mouseY <= -180f)
				{
					this.mouseY += 360f;
				}
				else if (this.mouseY > 180f)
				{
					this.mouseY -= 360f;
				}
			}
			base.transform.rotation = Quaternion.Euler(this.mouseY, this.mouseX, this.mouseZ);
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				base.transform.position += base.transform.forward * (Time.deltaTime * this.moveSpeed);
			}
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				base.transform.position -= base.transform.forward * (Time.deltaTime * this.moveSpeed);
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.position -= base.transform.right * (Time.deltaTime * this.moveSpeed);
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.position += base.transform.right * (Time.deltaTime * this.moveSpeed);
			}
		}

		// Token: 0x0400A67A RID: 42618
		public float xRotationSpeed = 5f;

		// Token: 0x0400A67B RID: 42619
		public float yRotationSpeed = 2.5f;

		// Token: 0x0400A67C RID: 42620
		public float moveSpeed = 10f;

		// Token: 0x0400A67D RID: 42621
		private float mouseX;

		// Token: 0x0400A67E RID: 42622
		private float mouseY;

		// Token: 0x0400A67F RID: 42623
		private float mouseZ;
	}
}
