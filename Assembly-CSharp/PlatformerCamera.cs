using System;
using UnityEngine;

// Token: 0x02000BDB RID: 3035
public class PlatformerCamera : MonoBehaviour
{
	// Token: 0x06005B8F RID: 23439 RVA: 0x0000457C File Offset: 0x0000277C
	public PlatformerCamera()
	{
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x00207F84 File Offset: 0x00206184
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.paused)
			{
				this.escapeButton.SetActive(true);
				Time.timeScale = 0f;
				this.paused = false;
			}
			else
			{
				this.escapeButton.SetActive(false);
				Time.timeScale = 1f;
				this.paused = true;
			}
		}
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x00207FE8 File Offset: 0x002061E8
	public void introCameraWalk()
	{
		Vector3 localPosition = Camera.main.transform.localPosition;
		if ((double)localPosition.x <= -55.6)
		{
			localPosition.x += 0.5f;
			Camera.main.transform.localPosition = localPosition;
		}
		else
		{
			Camera.main.orthographicSize -= 0.1f;
			localPosition.x += 0.1f;
			Camera.main.transform.localPosition = localPosition;
			Debug.Log("사이즈 : " + Camera.main.orthographicSize);
			if (Camera.main.orthographicSize <= 10f && Camera.main.transform.localPosition.x >= -27.7f)
			{
				PlatformerCamera.introWalk = false;
			}
		}
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x002080D8 File Offset: 0x002062D8
	private void FixedUpdate()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (PlatformerCamera.introWalk)
		{
			this.introCameraWalk();
		}
		else
		{
			if (Input.GetKey(KeyCode.P))
			{
				Application.LoadLevel("Menu");
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				Vector3 localPosition = Camera.main.transform.localPosition;
				localPosition.x -= 0.1f;
				Camera.main.transform.localPosition = localPosition;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				Vector3 localPosition2 = Camera.main.transform.localPosition;
				localPosition2.x += 0.1f;
				Camera.main.transform.localPosition = localPosition2;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				Vector3 localPosition3 = Camera.main.transform.localPosition;
				localPosition3.y -= 0.1f;
				Camera.main.transform.localPosition = localPosition3;
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				Vector3 localPosition4 = Camera.main.transform.localPosition;
				localPosition4.y += 0.1f;
				Camera.main.transform.localPosition = localPosition4;
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 0.1f, 1.5f, 16.5f);
				this.lightCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 0.1f, 1.5f, 16.5f);
				this.frontCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 0.1f, 1.5f, 16.5f);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + 0.1f, 1.5f, 16.5f);
			}
			this.lightCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + 0.1f, 1.5f, 16.5f);
			this.frontCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + 0.1f, 1.5f, 16.5f);
		}
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x00048F06 File Offset: 0x00047106
	public void cameraZoomOut(float zoomOut)
	{
		Camera.main.orthographicSize += zoomOut;
		this.lightCamera.orthographicSize += zoomOut;
		this.frontCamera.orthographicSize += zoomOut;
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x00048F40 File Offset: 0x00047140
	public void cameraZoomIn(float zoomIn)
	{
		Camera.main.orthographicSize -= zoomIn;
		this.lightCamera.orthographicSize -= zoomIn;
		this.frontCamera.orthographicSize -= zoomIn;
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x00048F7A File Offset: 0x0004717A
	// Note: this type is marked as 'beforefieldinit'.
	static PlatformerCamera()
	{
	}

	// Token: 0x040053A9 RID: 21417
	public GameObject player;

	// Token: 0x040053AA RID: 21418
	public Camera lightCamera;

	// Token: 0x040053AB RID: 21419
	public Camera frontCamera;

	// Token: 0x040053AC RID: 21420
	public GameObject escapeButton;

	// Token: 0x040053AD RID: 21421
	public static bool introWalk = true;

	// Token: 0x040053AE RID: 21422
	private bool paused;
}
