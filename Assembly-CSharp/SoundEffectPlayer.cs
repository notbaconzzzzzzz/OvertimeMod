using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class SoundEffectPlayer : MonoBehaviour
{
	// Token: 0x06002FE1 RID: 12257 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
	public SoundEffectPlayer()
	{
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x001443B0 File Offset: 0x001425B0
	private void Update()
	{
		if (this.onshot)
		{
			this.elapsedTime += Time.unscaledDeltaTime;
			if (this.elapsedTime > this.destroyTime)
			{
				this.Stop();
			}
		}
		else if (this.clips.Count != 0)
		{
			this.elapsedTime += Time.unscaledDeltaTime;
			if (this.elapsedTime >= this.destroyTime)
			{
				this.DeQueue();
			}
		}
	}

	// Token: 0x06002FE3 RID: 12259 RVA: 0x00144430 File Offset: 0x00142630
	public static bool DestroyPlayer(ref SoundEffectPlayer player)
	{
		try
		{
			if (player != null && player.gameObject != null)
			{
				UnityEngine.Object.Destroy(player.gameObject);
				player = null;
			}
		}
		catch (Exception ex)
		{
			player = null;
			return false;
		}
		return true;
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x00144490 File Offset: 0x00142690
	public static SoundEffectPlayer PlaySequence(Vector2 position, params string[] fileName)
	{
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		foreach (string str in fileName)
		{
			AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + str);
			if (!(audioClip == null))
			{
				component.clips.Enqueue(audioClip);
			}
		}
		component.DeQueue();
		component.onshot = false;
		gameObject.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
		return component;
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x00144550 File Offset: 0x00142750
	private void DeQueue()
	{
		if (this.clips.Count == 0)
		{
			this.Stop();
		}
		AudioClip audioClip = this.clips.Dequeue();
		if (audioClip == null)
		{
			this.Stop();
		}
		this.src.PlayOneShot(audioClip);
		this.destroyTime = audioClip.length;
		this.elapsedTime = 0f;
	}

	// Token: 0x06002FE6 RID: 12262 RVA: 0x001445B4 File Offset: 0x001427B4
	public static SoundEffectPlayer PlayOnce(string filename, Vector2 position)
	{
		AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + filename);
		if (audioClip == null)
		{
			return null;
		}
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		gameObject.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
		component2.clip = audioClip;
		component2.Play();
		component.destroyTime = audioClip.length;
		return component;
	}

	// Token: 0x06002FE7 RID: 12263 RVA: 0x0014464C File Offset: 0x0014284C
	public static SoundEffectPlayer PlayOnce(string filename, float pitch, Vector2 position)
	{
		AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + filename);
		if (audioClip == null)
		{
			return null;
		}
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		component2.pitch = pitch;
		gameObject.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
		component2.clip = audioClip;
		component2.Play();
		component.destroyTime = audioClip.length;
		return component;
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x001446E8 File Offset: 0x001428E8
	public static SoundEffectPlayer PlayOnce(string filename, Vector2 position, float volume)
	{
		AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + filename);
		if (audioClip == null)
		{
			return null;
		}
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		gameObject.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
		component2.clip = audioClip;
		component2.volume = volume;
		component2.Play();
		component.destroyTime = audioClip.length;
		return component;
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x00144784 File Offset: 0x00142984
	public static SoundEffectPlayer PlayOnce(string filename, Vector2 position, AudioRolloffMode mode)
	{
		AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + filename);
		if (audioClip == null)
		{
			return null;
		}
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		gameObject.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
		component2.rolloffMode = mode;
		component2.clip = audioClip;
		component2.Play();
		component.destroyTime = audioClip.length;
		return component;
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x00144820 File Offset: 0x00142A20
	public static SoundEffectPlayer Play(string filename, Transform transf)
	{
		AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + filename);
		if (audioClip == null)
		{
			return null;
		}
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		Vector2 vector = transf.position;
		gameObject.transform.SetParent(transf);
		gameObject.transform.localScale = Vector3.one;
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		component.onshot = false;
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		gameObject.transform.position = new Vector3(vector.x, vector.y, Camera.main.transform.position.z);
		component2.clip = audioClip;
		component2.loop = true;
		component2.Play();
		return component;
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x001448E4 File Offset: 0x00142AE4
	public static SoundEffectPlayer Play(string filename, Transform transf, float volume)
	{
		AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + filename);
		if (audioClip == null)
		{
			return null;
		}
		GameObject gameObject = Prefab.LoadPrefab("SoundEffectPlayer");
		Vector2 vector = transf.position;
		gameObject.transform.SetParent(transf);
		gameObject.transform.localScale = Vector3.one;
		SoundEffectPlayer component = gameObject.GetComponent<SoundEffectPlayer>();
		component.onshot = false;
		AudioSource component2 = gameObject.GetComponent<AudioSource>();
		gameObject.transform.position = new Vector3(vector.x, vector.y, Camera.main.transform.position.z);
		component2.clip = audioClip;
		component2.loop = true;
		component2.volume = volume;
		component2.Play();
		return component;
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x0002D212 File Offset: 0x0002B412
	public void AttachToCamera()
	{
		base.transform.SetParent(Camera.main.transform);
		base.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x0002D248 File Offset: 0x0002B448
	public void Halt()
	{
		this.src.Stop();
		this.halted = true;
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x0002D25C File Offset: 0x0002B45C
	public void ReStart()
	{
		this.src.Play();
		this.halted = false;
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x0002BE39 File Offset: 0x0002A039
	public void Stop()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x0002D270 File Offset: 0x0002B470
	public void SetDestroyTime(float destroyTime)
	{
		this.destroyTime = destroyTime;
	}

	// Token: 0x04002D63 RID: 11619
	private float destroyTime;

	// Token: 0x04002D64 RID: 11620
	private float elapsedTime;

	// Token: 0x04002D65 RID: 11621
	private bool onshot = true;

	// Token: 0x04002D66 RID: 11622
	public bool halted;

	// Token: 0x04002D67 RID: 11623
	public AudioSource src;

	// Token: 0x04002D68 RID: 11624
	private Queue<AudioClip> clips = new Queue<AudioClip>();
}
