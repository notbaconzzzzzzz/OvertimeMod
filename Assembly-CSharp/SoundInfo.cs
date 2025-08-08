using System;
using UnityEngine;

// Token: 0x02000582 RID: 1410
public class SoundInfo
{
	// Token: 0x06003095 RID: 12437 RVA: 0x0014BDA4 File Offset: 0x00149FA4
	public static AudioClip LoadClip(SoundInfo sound)
	{
		return Resources.Load<AudioClip>(sound.soundSrc);
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x0002D875 File Offset: 0x0002BA75
	public static SoundEffectPlayer PlaySound(SoundInfo sound, Vector2 pos)
	{
		GameManager manager = GameManager.currentGameManager;
		if (manager != null)
		{
			if (manager.grunge > manager.grungeLimit && UnityEngine.Random.value > manager.grungeLimit / manager.grunge) return null;
		}
		return SoundEffectPlayer.PlayOnce(sound.soundSrc, pos);
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x0002D875 File Offset: 0x0002BA75
	public SoundEffectPlayer PlaySound(Vector2 pos)
	{ // <Patch>
        return PlaySound_Mod(string.Empty, pos);
		// return SoundEffectPlayer.PlayOnce(this.soundSrc, pos);
	}

    // <Patch>
    public SoundEffectPlayer PlaySound_Mod(string modid, Vector2 pos)
    {
		GameManager manager = GameManager.currentGameManager;
		if (manager != null)
		{
			manager.grunge += 1f;
			if (manager.grunge > manager.grungeLimit && UnityEngine.Random.value > manager.grungeLimit / manager.grunge) return null;
		}
        return SoundEffectPlayer.PlayOnce_Mod(modid, this.soundSrc, pos);
    }

	// Token: 0x04002E50 RID: 11856
	public DamageInfo_EffectType soundType;

	// Token: 0x04002E51 RID: 11857
	public string soundSrc = string.Empty;
}
