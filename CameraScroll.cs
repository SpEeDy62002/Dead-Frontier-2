using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003B RID: 59
public class CameraScroll : MonoBehaviour
{
	// Token: 0x06000190 RID: 400 RVA: 0x0000CA0F File Offset: 0x0000AE0F
	private void Start()
	{
		this.speedSlider.onValueChanged.AddListener(delegate
		{
			this.ChangeSpeed();
		});
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000CA30 File Offset: 0x0000AE30
	private void Update()
	{
		base.transform.Translate(Vector3.left * (Time.deltaTime * this.moveSpeed));
		if (base.transform.position.x > 110f)
		{
			base.transform.position = new Vector3(0f, base.transform.position.y, base.transform.position.z);
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000CAB6 File Offset: 0x0000AEB6
	private void ChangeSpeed()
	{
		this.moveSpeed = this.speedSlider.value;
	}

	// Token: 0x04000174 RID: 372
	public Slider speedSlider;

	// Token: 0x04000175 RID: 373
	public float moveSpeed = 0.5f;
}
