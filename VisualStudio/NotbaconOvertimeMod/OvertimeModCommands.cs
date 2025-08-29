using System;
using System.Collections.Generic;
using static NotbaconOvertimeMod.NotbaconTakeOne;

namespace NotbaconOvertimeMod
{
    public class OvertimeModCommands : ConsoleCommandsBase
    {
        public override void SetList()
        {
            base.SetList();
            agentCommand.Add("markfordeath");
            agentCommand.Add("happyhalloween");
        }

        public override void AgentCommandOperation(int index, params string[] param)
        {
            switch (index)
            {
                case 0:
                    {
                        foreach (WorkerModel worker in GetListOfWorkers(param[0]))
                        {
                            worker.AddUnitBuf(new NotbaconPaleHorseMarkedBuf());
                        }
                    }
                    break;
                case 1:
                    {
                        if (candyBufSprites == null) LoadCandyBufSprites();
                        string[] names = { "Strawberry", "Grape", "Rasberry", "Blueberry", "Cherry", "Peach", "Orange", "GreenApple", "Watermelon", "Banana", "BrownChoco", "WhiteChoco", "BlackChoco", "CookiesNCream", "CottonCandy", "CoffeeCandy", "Caramel", "Vanilla", "Peppermint", "Licorice", "Poison" };
                        if (param.Length <= 0 || param[0].Split(',').Length <= 0) param[0] = "a";
                        foreach (AgentModel agent in GetListOfAgents(param[0]))
                        {
                            if (param.Length <= 1 || param[1].Split(',').Length <= 0) param[1] = "0";
                            foreach (string str in param[1].Split(','))
                            {
                                string[] strs = str.ToLower().Split('.');
                                int type = 0;
                                if (strs.Length < 1) continue;
                                if (float.TryParse(strs[0], out float output))
                                {
                                    type = (int)output;
                                }
                                else if ("rand".StartsWith(strs[0]))
                                {
                                    type = -1;
                                }
                                else if ("all".StartsWith(strs[0]))
                                {
                                    type = 0;
                                }
                                else
                                {
                                    for (int i = 1; i - 1 < names.Length; i++)
                                    {
                                        if (names[i - 1].StartsWith(strs[0]))
                                        {
                                            type = i;
                                            break;
                                        }
                                    }
                                }
                                if (type == -1 || type == 0)
                                {
                                    CandyType[] candyPool;
                                    if (strs.Length < 2 || "all".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.GRAPE,
                                            CandyType.RASBERRY,
                                            CandyType.BLUEBERRY,
                                            CandyType.CHERRY,
                                            CandyType.PEACH,
                                            CandyType.ORANGE,
                                            CandyType.GREEN_APPLE,
                                            CandyType.WATERMELON,
                                            CandyType.BANANA,
                                            CandyType.BROWN_CHOCO,
                                            CandyType.WHITE_CHOCO,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.COTTON_CANDY,
                                            CandyType.COFFEE_CANDY,
                                            CandyType.CARAMEL,
                                            CandyType.VANILLA,
                                            CandyType.PEPPERMINT,
                                            CandyType.LICORICE,
                                        };
                                    }
                                    else if ("poisonall".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.GRAPE,
                                            CandyType.RASBERRY,
                                            CandyType.BLUEBERRY,
                                            CandyType.CHERRY,
                                            CandyType.PEACH,
                                            CandyType.ORANGE,
                                            CandyType.GREEN_APPLE,
                                            CandyType.WATERMELON,
                                            CandyType.BANANA,
                                            CandyType.BROWN_CHOCO,
                                            CandyType.WHITE_CHOCO,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.COTTON_CANDY,
                                            CandyType.COFFEE_CANDY,
                                            CandyType.CARAMEL,
                                            CandyType.VANILLA,
                                            CandyType.PEPPERMINT,
                                            CandyType.LICORICE,
                                            CandyType.POISON,
                                        };
                                    }
                                    else if ("body".StartsWith(strs[1]) || "primbody".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.RASBERRY,
                                            CandyType.CHERRY,
                                            CandyType.BROWN_CHOCO,
                                        };
                                    }
                                    else if ("mind".StartsWith(strs[1]) || "primmind".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.GRAPE,
                                            CandyType.BLUEBERRY,
                                            CandyType.PEACH,
                                            CandyType.WHITE_CHOCO,
                                        };
                                    }
                                    else if ("sugar".StartsWith(strs[1]) || "primsugar".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.COTTON_CANDY,
                                            CandyType.COFFEE_CANDY,
                                            CandyType.VANILLA,
                                        };
                                    }
                                    else if ("damage".StartsWith(strs[1]) || "primdamage".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.ORANGE,
                                            CandyType.GREEN_APPLE,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.LICORICE,
                                        };
                                    }
                                    else if ("defense".StartsWith(strs[1]) || "primdefense".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.WATERMELON,
                                            CandyType.BANANA,
                                            CandyType.CARAMEL,
                                            CandyType.PEPPERMINT,
                                        };
                                    }
                                    else if ("secbody".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.GRAPE,
                                            CandyType.ORANGE,
                                            CandyType.WATERMELON,
                                            CandyType.COTTON_CANDY,
                                        };
                                    }
                                    else if ("secmind".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.GREEN_APPLE,
                                            CandyType.BANANA,
                                            CandyType.COFFEE_CANDY,
                                        };
                                    }
                                    else if ("secsugar".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.BROWN_CHOCO,
                                            CandyType.WHITE_CHOCO,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.CARAMEL,
                                        };
                                    }
                                    else if ("secdamage".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.RASBERRY,
                                            CandyType.BLUEBERRY,
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.PEPPERMINT,
                                        };
                                    }
                                    else if ("secdefense".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.CHERRY,
                                            CandyType.PEACH,
                                            CandyType.VANILLA,
                                            CandyType.LICORICE,
                                        };
                                    }
                                    else if ("anybody".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.GRAPE,
                                            CandyType.RASBERRY,
                                            CandyType.CHERRY,
                                            CandyType.ORANGE,
                                            CandyType.WATERMELON,
                                            CandyType.BROWN_CHOCO,
                                            CandyType.COTTON_CANDY,
                                        };
                                    }
                                    else if ("anymind".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.GRAPE,
                                            CandyType.BLUEBERRY,
                                            CandyType.PEACH,
                                            CandyType.GREEN_APPLE,
                                            CandyType.BANANA,
                                            CandyType.WHITE_CHOCO,
                                            CandyType.COFFEE_CANDY,
                                        };
                                    }
                                    else if ("anysugar".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.BROWN_CHOCO,
                                            CandyType.WHITE_CHOCO,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.COTTON_CANDY,
                                            CandyType.COFFEE_CANDY,
                                            CandyType.CARAMEL,
                                            CandyType.VANILLA,
                                        };
                                    }
                                    else if ("anydamage".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.RASBERRY,
                                            CandyType.BLUEBERRY,
                                            CandyType.ORANGE,
                                            CandyType.GREEN_APPLE,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.PEPPERMINT,
                                            CandyType.LICORICE,
                                        };
                                    }
                                    else if ("anydefense".StartsWith(strs[1]))
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.CHERRY,
                                            CandyType.PEACH,
                                            CandyType.WATERMELON,
                                            CandyType.BANANA,
                                            CandyType.CARAMEL,
                                            CandyType.VANILLA,
                                            CandyType.PEPPERMINT,
                                            CandyType.LICORICE,
                                        };
                                    }
                                    else
                                    {
                                        candyPool = new CandyType[] {
                                            CandyType.STRAWBERRY,
                                            CandyType.GRAPE,
                                            CandyType.RASBERRY,
                                            CandyType.BLUEBERRY,
                                            CandyType.CHERRY,
                                            CandyType.PEACH,
                                            CandyType.ORANGE,
                                            CandyType.GREEN_APPLE,
                                            CandyType.WATERMELON,
                                            CandyType.BANANA,
                                            CandyType.BROWN_CHOCO,
                                            CandyType.WHITE_CHOCO,
                                            CandyType.BLACK_CHOCO,
                                            CandyType.COOKIE_N_CREAM,
                                            CandyType.COTTON_CANDY,
                                            CandyType.COFFEE_CANDY,
                                            CandyType.CARAMEL,
                                            CandyType.VANILLA,
                                            CandyType.PEPPERMINT,
                                            CandyType.LICORICE,
                                        };
                                    }
                                    if (type == -1)
                                    {
                                        agent.AddUnitBuf(new CandyBuf(candyPool[UnityEngine.Random.Range(0, candyPool.Length)]));
                                    }
                                    else if (type == 0)
                                    {
                                        foreach (CandyType candyType in candyPool)
                                        {
                                            agent.AddUnitBuf(new CandyBuf(candyType));
                                        }
                                    }
                                }
                                else if (type >= (int)CandyType.STRAWBERRY && type <= (int)CandyType.POISON)
                                {
                                    agent.AddUnitBuf(new CandyBuf((CandyType)type));
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
