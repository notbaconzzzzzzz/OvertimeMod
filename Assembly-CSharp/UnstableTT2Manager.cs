using System;
using System.Linq;
using UnityEngine;

public class UnstableTT2Manager
{
	public static UnstableTT2Manager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new UnstableTT2Manager();
			}
			return _instance;
		}
	}

	public static UnstableTT2Manager _instance;

	public void CheckActive()
	{
		isActive = SpecialModeConfig.instance.GetValue<bool>("UnstableTT2");
	}

    public void Init(int day)
    {
		if (day < 20)
		{
			float num = (day + 1) / 20f;
			speedUp = 1f * (1f - num) + 1.2f * num;
			drainRate = 0f * (1f - num) + 1f * num;
		}
		else if (day >= 20 && day < 35)
		{
			float num = (day - 19) / 15f;
			speedUp = 1.3f * (1f - num) + 1.5f * num;
			drainRate = 1f * (1f - num) + 2f * num;
		}
		else if (day >= 35 && day < 45)
		{
			float num = (day - 34) / 10f;
			speedUp = (5f / 3f) * (1f - num) + 2f * num;
			drainRate = 2f * (1f - num) + 4f * num;
		}
		else if (day >= 45 && day < 48)
		{
			float num = (day - 45) / 3f;
			speedUp = 2.5f * (1f - num) + 3f * num;
			drainRate = 5f * (1f - num) + 6f * num;
		}
		else
		{
			speedUp = 3f;
			drainRate = 6f;
		}
        maxPauseCharge = 60f;
        if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.MALKUT) &&
			MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.YESOD) &&
			MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.HOD) &&
			MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.NETZACH))
		{
			maxPauseCharge += 10f;
		}
        if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.TIPERERTH1) &&
			MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.GEBURAH) &&
			MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHESED))
		{
			maxPauseCharge += 10f;
		}
        if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.BINAH) &&
			MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHOKHMAH))
		{
			maxPauseCharge += 10f;
		}
        curPauseCharge = maxPauseCharge;
    }

	public void FixedUpdate()
	{
		if (GameManager.currentGameManager.state == GameState.PLAYING)
		{
            curPauseCharge += Time.deltaTime;
			if (curPauseCharge > maxPauseCharge)
			{
				curPauseCharge = maxPauseCharge;
			}
		}
	}

	public void Update()
	{
		if (GameManager.currentGameManager.state == GameState.PAUSE && GameManager.currentGameManager.GetCurrentPauseCaller() == PAUSECALL.STOPGAME)
		{
			curPauseCharge -= Time.unscaledDeltaTime * drainRate;
			if (curPauseCharge <= 0f)
			{
				curPauseCharge = 0f;
				PlaySpeedSettingUI.instance.OnResume(PAUSECALL.STOPGAME);
				GameManager.currentGameManager.SetPlaySpeed(1);
			}
		}
	}

    public float SpeedScaled(float speed)
    {
        if (!isActive) return speed;
        return Mathf.Min((speedUp + 3f) * (speed + 3f) / 4f - 3f, 6f);
    }

    public float speedUp = 1f;

    public float drainRate = 0f;

    public float maxPauseCharge = 60f;

    public float curPauseCharge = 60f;

	public bool isActive;
}