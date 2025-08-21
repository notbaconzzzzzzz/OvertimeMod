using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotbaconOvertimeMod
{
    public class NotbaconPaleHorseMarkedBuf : UnitBuf
    {
        public NotbaconPaleHorseMarkedBuf()
        {
            type = UnitBufType.NOTBACON_PALE_HORSE_MARKED;
            duplicateType = BufDuplicateType.ONLY_ONE;
        }

        public override void FixedUpdate()
        {
            model.TakeDamage(new DamageInfo(RwbpType.P, 0.1f));
            if (!model.IsAttackTargetable()) return;
            PassageObjectModel passage = model.GetMovableNode().currentPassage;
            if (passage != null && !previousPassage)
            {
                checkTimer.StartTimer(6f);
                checkDepth = 0;
                alreadyTargets.Clear();
                alreadyAttract.Clear();
                foreach (MovableObjectNode movableNode in passage.GetEnteredTargets())
                {
                    UnitModel unit = movableNode.GetUnit();
                    if (!(unit is CreatureModel)) continue;
                    CreatureModel creature = unit as CreatureModel;
                    creature.script.TryForceAggro(model);
                    alreadyTargets.Add(unit);
                }
            }
            if (checkTimer.RunTimer() && passage != null)
            {
                checkTimer.StartTimer(6f);
                foreach (MovableObjectNode movableNode in passage.GetEnteredTargets())
                {
                    UnitModel unit = movableNode.GetUnit();
                    if (!(unit is CreatureModel) || alreadyTargets.Contains(unit)) continue;
                    CreatureModel creature = unit as CreatureModel;
                    creature.script.TryForceAggro(model);
                    alreadyTargets.Add(unit);
                }
                if (checkDepth > 0)
                {
                    List<PassageObjectModel> curLayer = new List<PassageObjectModel>();
                    List<PassageObjectModel> nextLayer = new List<PassageObjectModel>();
                    List<PassageObjectModel> prevLayers = new List<PassageObjectModel>();
                    curLayer.Add(passage);
                    for (int i = 0; i < checkDepth; i++)
                    {
                        foreach (PassageObjectModel passage2 in curLayer)
                        {
                            MapNode node = passage2.GetLeft();
                            foreach (MapEdge edge in node.GetEdges())
                            {
                                PassageObjectModel passage3 = edge.ConnectedNode(node).GetAttachedPassage();
                                if (!prevLayers.Contains(passage3))
                                {
                                    if (passage3.type == PassageType.ISOLATEROOM) continue;
                                    prevLayers.Add(passage3);
                                    if (passage3.type == PassageType.VERTICAL)
                                    {
                                        foreach (MapNode node2 in passage3.GetNodeList())
                                        {
                                            foreach (MapEdge edge2 in node2.GetEdges())
                                            {
                                                PassageObjectModel passage4 = edge2.ConnectedNode(node2).GetAttachedPassage();
                                                if (!prevLayers.Contains(passage4))
                                                {
                                                    prevLayers.Add(passage4);
                                                    nextLayer.Add(passage4);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        nextLayer.Add(passage3);
                                    }
                                }
                            }
                            node = passage2.GetRight();
                            foreach (MapEdge edge in node.GetEdges())
                            {
                                PassageObjectModel passage3 = edge.ConnectedNode(node).GetAttachedPassage();
                                if (!prevLayers.Contains(passage3))
                                {
                                    if (passage3.type == PassageType.ISOLATEROOM) continue;
                                    prevLayers.Add(passage3);
                                    if (passage3.type == PassageType.VERTICAL)
                                    {
                                        foreach (MapNode node2 in passage3.GetNodeList())
                                        {
                                            foreach (MapEdge edge2 in node2.GetEdges())
                                            {
                                                PassageObjectModel passage4 = edge2.ConnectedNode(node2).GetAttachedPassage();
                                                if (!prevLayers.Contains(passage4))
                                                {
                                                    prevLayers.Add(passage4);
                                                    nextLayer.Add(passage4);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        nextLayer.Add(passage3);
                                    }
                                }
                            }
                            if (true)
                            {
                                MapNode[] mapNodes = passage2.GetNodeList();
                                node = mapNodes[mapNodes.Length / 2];
                                foreach (MapEdge edge in node.GetEdges())
                                {
                                    PassageObjectModel passage3 = edge.ConnectedNode(node).GetAttachedPassage();
                                    if (!prevLayers.Contains(passage3))
                                    {
                                        if (passage3.type == PassageType.ISOLATEROOM) continue;
                                        prevLayers.Add(passage3);
                                        if (passage3.type == PassageType.VERTICAL)
                                        {
                                            foreach (MapNode node2 in passage3.GetNodeList())
                                            {
                                                foreach (MapEdge edge2 in node2.GetEdges())
                                                {
                                                    PassageObjectModel passage4 = edge2.ConnectedNode(node2).GetAttachedPassage();
                                                    if (!prevLayers.Contains(passage4))
                                                    {
                                                        prevLayers.Add(passage4);
                                                        nextLayer.Add(passage4);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            nextLayer.Add(passage3);
                                        }
                                    }
                                }
                            }
                        }
                        curLayer = nextLayer;
                        nextLayer = new List<PassageObjectModel>();
                    }
                    foreach (PassageObjectModel passage2 in curLayer)
                    {
                        foreach (MovableObjectNode movableNode in passage2.GetEnteredTargets())
                        {
                            UnitModel unit = movableNode.GetUnit();
                            if (!(unit is CreatureModel) || alreadyAttract.Contains(unit)) continue;
                            CreatureModel creature = unit as CreatureModel;
                            creature.script.TryAttractAttention(passage, false);
                            alreadyAttract.Add(unit);
                        }
                    }
                }
                checkDepth++;
                if (checkDepth > 4)
                {
                    checkDepth = 1;
                    alreadyAttract.Clear();
                }
            }
            previousPassage = passage != null;
        }

        private bool previousPassage;

        private List<UnitModel> alreadyTargets = new List<UnitModel>();

        private List<UnitModel> alreadyAttract = new List<UnitModel>();

        private Timer checkTimer = new Timer();

        private int checkDepth;
    }
}
