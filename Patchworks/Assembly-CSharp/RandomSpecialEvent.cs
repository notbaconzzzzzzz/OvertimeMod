using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpecialEvent
{
    public RandomSpecialEvent(RandomEventRank _rank, RandomEventType _type, int _level)
    {
        rank = _rank;
        type = _type;
        level = _level;
    }

    public RandomEventRank rank;

	public RandomEventType type;

    public int level;
}