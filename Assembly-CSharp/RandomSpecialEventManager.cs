using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpecialEventManager// : IObserver
{
	public static RandomSpecialEventManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RandomSpecialEventManager();
			}
			return _instance;
		}
	}

    public void OnStageStart()
    {
        stageStarted = true;
        //Notice.instance.Observe(NoticeName.FixedUpdate, this);
        elapsed = 0f;
		int num = PlayerModel.instance.GetDay();
		if (num >= CreatureOverloadManager.instance.overflowValue.Length)
		{
			num = CreatureOverloadManager.instance.overflowValue.Length - 1;
		}
        overflowValue = CreatureOverloadManager.instance.overflowValue[num];
        waitingEvents.Clear();
        waitingIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            AddRandomWaiting();
        }
    }

    public void OnStageEnd()
    {
        stageStarted = false;
        activeEvents.Clear();
        //Notice.instance.Remove(NoticeName.FixedUpdate, this);
    }

    public void OnStageRelease()
    {
        stageStarted = false;
        activeEvents.Clear();
        //Notice.instance.Remove(NoticeName.FixedUpdate, this);
    }

    public void OnFixedUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= overflowValue)
        {
            elapsed = 0f;
            ActivateRandomWaiting();
        }
    }
    /*
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.FixedUpdate)
		{
			
		}
	}*/

    public void AddWaitingEvent(RandomSpecialEvent randomEvent)
    {
        waitingEvents.Add(randomEvent);
    }

    public void AddRandomWaiting()
    {
        RandomEventRank rank;
        int type;
        int level;
        if (waitingIndex % 4 <= 1)
        {
            rank = RandomEventRank.NOON;
        }
        if (waitingIndex % 4 == 2)
        {
            rank = RandomEventRank.DUSK;
        }
        if (waitingIndex % 4 == 3)
        {
            rank = RandomEventRank.DAWN;
        }
        level = waitingIndex / 4 + 1;
        type = UnityEngine.Random.Range(0, 7);
        AddWaitingEvent(new RandomSpecialEvent(rank, (RandomEventType)type, level));
        waitingIndex++;
    }

    public void ActivateEvent(RandomSpecialEvent randomEvent)
    {
        waitingEvents.Remove(randomEvent);
        activeEvents.Add(randomEvent);
    }

    public void ActivateRandomWaiting()
    {
        if (waitingEvents.Length <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                AddRandomWaiting();
            }
        }
        ActivateEvent(waitingEvents[UnityEngine.Random.Range(0, waitingEvents.Length)]);
        if (waitingEvents.Length < 3)
        {
            AddRandomWaiting();
        }
    }

    private static RandomSpecialEventManager _instance;

    private bool stageStarted;

    private float elapsed;

    private float overflowValue;

    public int waitingIndex;

    private List<RandomSpecialEvent> waitingEvents = new List<RandomSpecialEvent>();

    private List<RandomSpecialEvent> activeEvents = new List<RandomSpecialEvent>();
}