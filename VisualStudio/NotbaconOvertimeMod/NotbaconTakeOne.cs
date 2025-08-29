using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconTakeOne : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            if (candyBufSprites == null)
            {
                LoadCandyBufSprites();
            }
        }

        public override void OnStageStart()
        {
            deck.Clear();
            for (int i = 1; i <= 20; i++)
            {
                deck.Add((CandyType)i);
            }
            for (int i = 0; i < 21; i++)
            {
                candyCounts[i] = 0;
            }
            candyCounts[20] = 1;
            for (int i = 0; i < 3 + PlayerModel.instance.GetDay() / 10; i++)
            {
                AddRandomCandyToPool();
            }
        }

        public override void OnEnterRoom(UseSkill skill)
        {
            AgentModel agent = skill.agent;
            List<CandyType> choices = new List<CandyType>();
            int num = 0;
            for (int i = 1; i <= 21; i++)
            {
                if (agent.HasUnitBuf((UnitBufType)((int)UnitBufType.NOTBACON_TAKE_ONE_STATBUF + i)))
                {
                    num++;
                }
                else if (candyCounts[i - 1] > 0)
                {
                    if (i == 21 && !(choices.Count <= 0 && num > 0))
                    {
                        continue;
                    }
                    choices.Add((CandyType)i);
                }
            }
            if (choices.Count <= 0) return;
            CandyType type = choices[UnityEngine.Random.Range(0, choices.Count)];
            candyCounts[(int)type - 1]--;
            agent.AddUnitBuf(new CandyBuf(type));
            num++;
            if (num > 1)
            {
                CandyGreedBuf buf = null;
                buf = (CandyGreedBuf)agent.GetUnitBufByName("CandyGreedBuf");
                if (buf == null)
                {
                    buf = new CandyGreedBuf(this);
                    agent.AddUnitBuf(buf);
                }
                else
                {
                    buf.EatCandy(num);
                }
            }
        }

        public CandyType AddRandomCandyToPool()
        {
            CandyType type = deck[UnityEngine.Random.Range(0, deck.Count)];
            deck.Remove(type);
            candyCounts[(int)type - 1]++;
            if (deck.Count <= 0)
            {
                for (int i = 1; i <= 20; i++)
                {
                    deck.Add((CandyType)i);
                }
            }
            return type;
        }

        public int[] candyCounts = new int[21];

        public List<CandyType> deck = new List<CandyType>();

        public static Sprite[] candyBufSprites = null;

        public static Sprite GetCandyBufSprite(CandyType _candy)
        {
            if (candyBufSprites == null)
            {
                LoadCandyBufSprites();
            }
            if (_candy == CandyType.NONE) return candyBufSprites[0];
            return candyBufSprites[(int)_candy - 1];
        }

        public static void LoadCandyBufSprites()
        {
            candyBufSprites = new Sprite[21];
            string[] names = { "Strawberry", "Grape", "Rasberry", "Blueberry", "Cherry", "Peach", "Orange", "GreenApple", "Watermelon", "Banana", "BrownChoco", "WhiteChoco", "BlackChoco", "CookiesNCream", "CottonCandy", "CoffeeCandy", "Caramel", "Vanilla", "Peppermint", "Licorice", "Poison" };
            for (int i = 0; i < 21; i++)
            {
                byte[] data = File.ReadAllBytes(Application.dataPath + "/BaseMods/NotbaconOvertimeMod/Sprites/CandyBufs/" + names[i] + ".png");
                Texture2D texture2D = new Texture2D(300, 300);
                texture2D.LoadImage(data);
                candyBufSprites[i] = Sprite.Create(texture2D, new Rect(0, 0, 300, 300), new Vector2(150, 150), 50, 0U, SpriteMeshType.FullRect, new Vector4());
            }
        }

        public enum CandyType
        {
            NONE,
            STRAWBERRY,
            GRAPE,
            RASBERRY,
            BLUEBERRY,
            CHERRY,
            PEACH,
            ORANGE,
            GREEN_APPLE,
            WATERMELON,
            BANANA,
            BROWN_CHOCO,
            WHITE_CHOCO,
            BLACK_CHOCO,
            COOKIE_N_CREAM,
            COTTON_CANDY,
            COFFEE_CANDY,
            CARAMEL,
            VANILLA,
            PEPPERMINT,
            LICORICE,
            POISON,
        }

        public class CandyBuf : UnitBuf
        {
            public CandyBuf(CandyType _candy)
            {
                candy = _candy;
                type = (UnitBufType)((int)UnitBufType.NOTBACON_TAKE_ONE_STATBUF + (int)candy);
                duplicateType = BufDuplicateType.ONLY_ONE;
                BodyMod = 0;
                MindMod = 0;
                SugarMod = 0;
                DamageMod = 0;
                DefenseMod = 0;
                switch (candy)
                {
                    case CandyType.STRAWBERRY:
                        BodyMod = 2;
                        MindMod = 1;
                        break;
                    case CandyType.GRAPE:
                        MindMod = 2;
                        BodyMod = 1;
                        break;
                    case CandyType.RASBERRY:
                        BodyMod = 2;
                        DamageMod = 1;
                        break;
                    case CandyType.BLUEBERRY:
                        MindMod = 2;
                        DamageMod = 1;
                        break;
                    case CandyType.CHERRY:
                        BodyMod = 2;
                        DefenseMod = 1;
                        break;
                    case CandyType.PEACH:
                        MindMod = 2;
                        DefenseMod = 1;
                        break;
                    case CandyType.ORANGE:
                        DamageMod = 2;
                        BodyMod = 1;
                        break;
                    case CandyType.GREEN_APPLE:
                        DamageMod = 2;
                        MindMod = 1;
                        break;
                    case CandyType.WATERMELON:
                        DefenseMod = 2;
                        BodyMod = 1;
                        break;
                    case CandyType.BANANA:
                        DefenseMod = 2;
                        MindMod = 1;
                        break;
                    case CandyType.BROWN_CHOCO:
                        BodyMod = 2;
                        SugarMod = 1;
                        break;
                    case CandyType.WHITE_CHOCO:
                        MindMod = 2;
                        SugarMod = 1;
                        break;
                    case CandyType.BLACK_CHOCO:
                        DamageMod = 2;
                        SugarMod = 1;
                        break;
                    case CandyType.COOKIE_N_CREAM:
                        SugarMod = 2;
                        DamageMod = 1;
                        break;
                    case CandyType.COTTON_CANDY:
                        SugarMod = 2;
                        BodyMod = 1;
                        break;
                    case CandyType.COFFEE_CANDY:
                        SugarMod = 2;
                        MindMod = 1;
                        break;
                    case CandyType.CARAMEL:
                        DefenseMod = 2;
                        SugarMod = 1;
                        break;
                    case CandyType.VANILLA:
                        SugarMod = 2;
                        DefenseMod = 1;
                        break;
                    case CandyType.PEPPERMINT:
                        DefenseMod = 2;
                        DamageMod = 1;
                        break;
                    case CandyType.LICORICE:
                        DamageMod = 2;
                        DefenseMod = 1;
                        break;
                    case CandyType.POISON:
                        BodyMod = -1;
                        MindMod = -1;
                        SugarMod = 2;
                        DamageMod = 2;
                        DefenseMod = 2;
                        break;
                }
            }

            public override void Init(UnitModel model)
            {
                base.Init(model);
                agent = model as AgentModel;
                if (DefenseMod > 0)
                {
                    float factor = 1f + 0.1f * DefenseMod;
                    model.additionalDef.R /= factor;
                    model.additionalDef.W /= factor;
                    model.additionalDef.B /= factor;
                    model.additionalDef.P /= factor;
                }
                statbuf = new UnitStatBuf(float.PositiveInfinity, UnitBufType.NOTBACON_TAKE_ONE_STATBUF);
                statbuf.duplicateType = BufDuplicateType.UNLIMIT;
                statbuf.maxHp = BodyMod * (agent.fortitudeStat + 150) / 40;
                statbuf.maxMental = MindMod * (agent.prudenceStat + 150) / 40;
                statbuf.attackSpeed = SugarMod * (agent.justiceStat * 3 + 750) / 100;
                statbuf.movementSpeed = SugarMod * (agent.justiceStat * 3 + 750) / 100;
                model.AddUnitBuf(statbuf);
                agent.GetUnit().AddUnitBuf(this, GetCandyBufSprite(candy));
            }

            public override void FixedUpdate()
            {
                if (BodyMod > 0 || MindMod > 0)
                {
                    if (recoverTimer.started)
                    {
                        if (recoverTimer.RunTimer())
                        {
                            if (BodyMod > 0)
                            {
                                agent.RecoverHPv2((float)BodyMod * 2f);
                            }
                            if (MindMod > 0)
                            {
                                agent.RecoverMentalv2((float)MindMod * 2f);
                            }
                            recoverTimer.StartTimer(4f);
                        }
                    }
                    else
                    {
                        recoverTimer.StartTimer(4f);
                    }
                }
            }

            public override void OnDestroy()
            {
                if (DefenseMod > 0)
                {
                    float factor = 1f + 0.1f * DefenseMod;
                    model.additionalDef.R *= factor;
                    model.additionalDef.W *= factor;
                    model.additionalDef.B *= factor;
                    model.additionalDef.P *= factor;
                }
                model.RemoveUnitBuf(statbuf);
                agent.GetUnit().RemoveUnitBuf(this);
            }

            public override float GetDamageFactor()
            {
                return 1f + 0.1f * DamageMod;
            }

            public CandyType candy;

            public int BodyMod;

            public int MindMod;

            public int SugarMod;

            public int DamageMod;

            public int DefenseMod;

            public UnitStatBuf statbuf;

            public Timer recoverTimer = new Timer();

            private AgentModel agent;
        }

        public class CandyGreedBuf : UnitBuf
        {
            public CandyGreedBuf(NotbaconTakeOne _takeOne)
            {
                takeOne = _takeOne;
            }

            public override void Init(UnitModel model)
            {
                base.Init(model);
                if (!(model is AgentModel))
                {
                    return;
                }
                agent = model as AgentModel;
                _remainDeathTime = 10f + PlayerModel.emergencyController.GetCurrentScore();
                _remainStallTime = 60f;
                numOfCandy = 2;
                rate = 1f;
            }

            public override void FixedUpdate()
            {
                if (agent.IsSuppressing())
                {
                    _remainStallTime -= Time.deltaTime * rate;
                    if (_remainStallTime > 0f)
                    {
                        return;
                    }
                    _remainDeathTime -= Time.deltaTime * 0.25f * rate;
                }
                else
                {
                    _remainDeathTime -= Time.deltaTime * rate;
                }
                if (_remainDeathTime <= 0f)
                {
                    agent.Die();
                    int num = numOfCandy;
                    switch (agent.level)
                    {
                        case 1:
                            num += 1;
                            break;
                        case 2:
                            num += 2;
                            break;
                        case 3:
                            num += 3;
                            break;
                        case 4:
                            num += 5;
                            break;
                        case 5:
                            num += 8;
                            break;
                    }
                    for (int i = 0; i < num; i++)
                    {
                        takeOne.AddRandomCandyToPool();
                    }
                }
            }

            public void EatCandy(int num)
            {
                numOfCandy++;
                rate *= num;
                _remainDeathTime = 10f + PlayerModel.emergencyController.GetCurrentScore();
                _remainStallTime = 60f;
            }

            private float _remainDeathTime;

            private float _remainStallTime;

            private AgentModel agent;

            public int numOfCandy;

            public float rate;

            private NotbaconTakeOne takeOne;
        }
    }
}
