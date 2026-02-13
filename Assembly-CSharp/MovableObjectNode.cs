/*
private void ProcessMoveByDistance(float distance) // 
public static float GetDistance(MovableObjectNode node1, MovableObjectNode node2) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000376 RID: 886
public class MovableObjectNode
{
	// Token: 0x06001B15 RID: 6933 RVA: 0x0001DE22 File Offset: 0x0001C022
	public MovableObjectNode(bool active)
	{
		this.unpassableList = new List<PassType>();
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x0001DE47 File Offset: 0x0001C047
	public MovableObjectNode(UnitModel model)
	{
		this.model = model;
		this.unpassableList = new List<PassType>();
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0001DE73 File Offset: 0x0001C073
	public bool isActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06001B18 RID: 6936 RVA: 0x0001DE7B File Offset: 0x0001C07B
	public MapNode currentNode
	{
		get
		{
			return this._currentNode;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0001DE83 File Offset: 0x0001C083
	public MapEdge currentEdge
	{
		get
		{
			return this._currentEdge;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0001DE8B File Offset: 0x0001C08B
	// (set) Token: 0x06001B1B RID: 6939 RVA: 0x0001DE93 File Offset: 0x0001C093
	public PassageObjectModel currentPassage
	{
		get
		{
			return this._currentPassage;
		}
		set
		{
			this._currentPassage = value;
			if (this._currentPassage != null)
			{
				this.notNullPassage = this._currentPassage;
			}
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06001B1C RID: 6940 RVA: 0x0001DEB3 File Offset: 0x0001C0B3
	public bool IsNextElevator
	{
		get
		{
			return this._isNextElevator;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001B1D RID: 6941 RVA: 0x0001DEBB File Offset: 0x0001C0BB
	public bool isBlocked
	{
		get
		{
			return this.blockedTimer > 0f;
		}
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x000E2CAC File Offset: 0x000E0EAC
	public void SetActive(bool active)
	{
		if (this._isActive)
		{
			if (this.currentPassage != null)
			{
				this.currentPassage.ExitUnit(this);
			}
			if (this.notNullPassage != null && this.notNullPassage != null && this.notNullPassage.GetSefiraEnum() != SefiraEnum.DUMMY)
			{
				this.notNullPassage.GetSefira().ExitAgent(this);
			}
			this._isActive = active;
		}
		else
		{
			if (this.currentPassage != null)
			{
				this.currentPassage.EnterUnit(this);
			}
			if (this.notNullPassage != null && this.notNullPassage.GetSefiraEnum() != SefiraEnum.DUMMY)
			{
				this.notNullPassage.GetSefira().EnterAgent(this);
			}
			this._isActive = active;
		}
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x0001DECA File Offset: 0x0001C0CA
	public void SetPassageChangedParam(object target)
	{
		this.passageChangedParam = target;
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x000E2D6C File Offset: 0x000E0F6C
	public static Vector3 GetViewPositionInEdge(MapEdge edge, EdgeDirection edgeDirection, float edgePosRate)
	{
		Vector3 result = new Vector3(0f, 0f, 0f);
		MapNode node = edge.node1;
		MapNode node2 = edge.node2;
		Vector3 position = node.GetPosition();
		Vector3 position2 = node2.GetPosition();
		if (edgeDirection == EdgeDirection.FORWARD)
		{
			result.x = Mathf.Lerp(position.x, position2.x, edgePosRate);
			result.y = Mathf.Lerp(position.y, position2.y, edgePosRate);
			result.z = Mathf.Lerp(position.z, position2.z, edgePosRate);
		}
		else
		{
			result.x = Mathf.Lerp(position.x, position2.x, 1f - edgePosRate);
			result.y = Mathf.Lerp(position.y, position2.y, 1f - edgePosRate);
			result.z = Mathf.Lerp(position.z, position2.z, 1f - edgePosRate);
		}
		return result;
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000E2E6C File Offset: 0x000E106C
	public Vector3 GetCurrentViewPosition()
	{
		if (this.viewPositionSet)
		{
			return this.viewPosition;
		}
		Vector3 viewPositionInEdge = new Vector3(0f, 0f, 0f);
		if (this.currentNode != null)
		{
			Vector3 position = this.currentNode.GetPosition();
			viewPositionInEdge.x = position.x;
			viewPositionInEdge.y = position.y;
			viewPositionInEdge.z = position.z;
		}
		else if (this.currentEdge != null)
		{
			viewPositionInEdge = MovableObjectNode.GetViewPositionInEdge(this.currentEdge, this.edgeDirection, this.edgePosRate);
		}
		return viewPositionInEdge;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x0001DED3 File Offset: 0x0001C0D3
	public void EnablePositionSetter(Vector3 position)
	{
		this.viewPositionSet = true;
		this.viewPosition = position;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x0001DEE3 File Offset: 0x0001C0E3
	public void DisablePositionSetter()
	{
		this.viewPositionSet = false;
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x0001DEEC File Offset: 0x0001C0EC
	public PassageObjectModel GetPassage()
	{
		return this.currentPassage;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
	public PassageObjectModel GetPassageCheckPrev()
	{
		if (this.currentPassage != null)
		{
			return this.currentPassage;
		}
		if (this.notNullPassage == null)
		{
			Debug.LogError("Cannot find pasage");
			return null;
		}
		return this.notNullPassage;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0001DF25 File Offset: 0x0001C125
	public void AddUnpassableType(PassType pass)
	{
		if (!this.unpassableList.Contains(pass))
		{
			this.unpassableList.Add(pass);
		}
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x0001DF44 File Offset: 0x0001C144
	public void RemoveUnpassableType(PassType pass)
	{
		if (this.unpassableList.Contains(pass))
		{
			this.unpassableList.Remove(pass);
		}
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x0001DF64 File Offset: 0x0001C164
	public void SetTeleportable(bool b)
	{
		this._teleportable = b;
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000E2F0C File Offset: 0x000E110C
	private bool CheckPassable(MapEdge edge, EdgeDirection edgeDir, float oldEdgePosRate, float newEdgePosRate)
	{
		if (this.currentPassage == null)
		{
			return true;
		}
		Vector3 viewPositionInEdge = MovableObjectNode.GetViewPositionInEdge(edge, edgeDir, Mathf.Clamp01(newEdgePosRate));
		foreach (PassType passType in this.unpassableList)
		{
			if (passType == PassType.SHIELDBEARER)
			{
				List<AgentModel> list = new List<AgentModel>();
				foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
				{
					if (agentModel.IsShieldBlock())
					{
						list.Add(agentModel);
					}
				}
				foreach (AgentModel agentModel2 in list)
				{
					Vector3 currentViewPosition = agentModel2.GetCurrentViewPosition();
					float num = currentViewPosition.x - 1f;
					float num2 = currentViewPosition.x + 1f;
					if (num <= viewPositionInEdge.x && viewPositionInEdge.x <= num2)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x0001DF6D File Offset: 0x0001C16D
	public void ProcessMoveNode(float movement)
	{
		this.ProcessMoveByDistance(Time.deltaTime * DistanceUnitUtil.GameToUnity(movement) * this.currentScale);
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000E307C File Offset: 0x000E127C
	private void ProcessMoveByDistance(float distance)
	{ // <Mod> Modified the behavior of the Tiphereth elevator; IgnoreDoors()
		if (this.blockedTimer > 0f)
		{
			this.blockedTimer -= Time.deltaTime;
		}
		float num = -1f;
		for (;;)
		{
			if (this._isNextElevator)
			{
				this._elevatorWaitElapsedTime += Time.deltaTime;
			}
			else
			{
				this._elevatorWaitElapsedTime = 0f;
			}
			this._isNextElevator = false;
			if (this.currentNode != null)
			{
				if (this.currentNode.GetDoor() != null)
				{
					this.currentNode.GetDoor().OnObjectPassed();
				}
			}
			else if (this.currentEdge != null)
			{
				if (this.currentEdge.node1.GetDoor() != null)
				{
					this.currentEdge.node1.GetDoor().OnObjectPassed();
				}
				if (this.currentEdge.node2.GetDoor() != null)
				{
					this.currentEdge.node2.GetDoor().OnObjectPassed();
				}
			}
			if (this.state != MovableState.MOVE)
			{
				goto IL_7E3;
			}
			if (this.pathMoveBy == null)
			{
				goto IL_2AC;
			}
			this.unitDirection = this.pathMoveBy.direction;
			if (this.moveDistance >= this.pathMoveBy.value)
			{
				goto IL_2A7;
			}
			if (this.currentNode != null)
			{
				MapEdge mapEdge = MovableObjectNode.MoveBy_GetNextEdge(this.currentNode, this.pathMoveBy.direction);
				if (mapEdge == null)
				{
					break;
				}
				if (mapEdge.node1 == this.currentNode)
				{
					this.edgeDirection = EdgeDirection.FORWARD;
				}
				else
				{
					this.edgeDirection = EdgeDirection.BACKWARD;
				}
				this.edgePosRate = 0f;
				this.UpdateNodeEdge(null, mapEdge);
			}
			else
			{
				if (this.currentEdge == null)
				{
					goto IL_29D;
				}
				float cost = this.currentEdge.cost;
				num = distance / cost;
				if (!this.CheckPassable(this.currentEdge, this.edgeDirection, this.edgePosRate, this.edgePosRate + num))
				{
					goto Block_15;
				}
				this.moveDistance += distance;
				this.edgePosRate += num;
				float num2 = 0f;
				if (this.edgePosRate >= 1f)
				{
					num2 = this.edgePosRate - 1f;
					if (this.edgeDirection == EdgeDirection.FORWARD)
					{
						this.UpdateNodeEdge(this.currentEdge.node2, null);
					}
					else
					{
						this.UpdateNodeEdge(this.currentEdge.node1, null);
					}
				}
				if (this.moveDistance >= this.pathMoveBy.value)
				{
					goto Block_18;
				}
				if (num2 <= 0f)
				{
					goto IL_298;
				}
				distance = num2 * cost;
			}
		}
		this.StopMoving();
		goto IL_2A7;
		Block_15:
		this.blockedTimer = 1f;
		this.StopMoving();
		return;
		Block_18:
		this.StopMoving();
		IL_298:
		goto IL_2A7;
		IL_29D:
		Debug.Log("invalid");
		IL_2A7:
		goto IL_7E3;
		IL_2AC:
		if (this.pathInfo != null)
		{
			if (this.currentNode != null)
			{
				if (this.pathIndex >= this.pathInfo.pathEdges.Length)
				{
					this.StopMoving();
				}
				else
				{
					MapEdge mapEdge2 = this.pathInfo.pathEdges[this.pathIndex];
					EdgeDirection direction = this.pathInfo.edgeDirections[this.pathIndex];
					MapNode goalNode = mapEdge2.GetGoalNode(direction);
					if (mapEdge2.node1 != this.currentNode && mapEdge2.node2 != this.currentNode)
					{
						Debug.LogError("Invalid Movable State.. Please report this.");
						this.StopMoving();
					}
					else if (goalNode.closed && goalNode.GetDoor() != null && (model == null || !model.IgnoreDoors()))
					{
						if (this.model != null && this.model.CanOpenDoor())
						{
							goalNode.GetDoor().TryOpen();
						}
					}
					else if (goalNode.closed && goalNode.GetDoor() == null)
					{
						this.StopMoving();
					}
					else if (goalNode.GetElevator() != null && mapEdge2.type != "road") // 
					{
						this._isNextElevator = true;
						if (this.pathIndex + 1 >= this.pathInfo.pathEdges.Length)
						{
							Debug.Log("Elevator.. .....");
							this.StopMoving();
							throw new MovableObjectNode.MovableElevatorStuckException();
						}
						MapNode teleportNode;
						if (this._teleportable && this._currentNode != null && (teleportNode = this._currentNode.GetTeleportNode(this, true)) != null)
						{
							this._isNextElevator = false;
							this.TrySetCurrentNode(teleportNode);
						}
						else
						{
							MapEdge mapEdge3 = this.pathInfo.pathEdges[this.pathIndex + 1];
							EdgeDirection direction2 = this.pathInfo.edgeDirections[this.pathIndex + 1];
							MapNode goalNode2 = mapEdge3.GetGoalNode(direction2); //>
							if (mapEdge3.type == "road")
							{
								_elevatorWaitElapsedTime = 0f;
								SetCurrentNode(goalNode);
							} //<
							else if (this._elevatorWaitElapsedTime >= 0.5f)
							{
								this._elevatorWaitElapsedTime = 0f;
								this.SetCurrentNode(goalNode2);
							}
						}
					} //>
					else if (mapEdge2.type == "door" && mapEdge2.GetGoalNode((EdgeDirection)(EdgeDirection.BACKWARD - direction)).GetElevator() != null)
					{
						_elevatorWaitElapsedTime = 0f;
						SetCurrentNode(goalNode);
					} //<
					else
					{
                        bool flag = true;
                        if (goalNode.GetElevator() != null)
                        {
                            MapNode teleportNode;
                            if (this._teleportable && this._currentNode != null && (teleportNode = this._currentNode.GetTeleportNode(this, true)) != null)
                            {
                                this._isNextElevator = false;
                                this.TrySetCurrentNode(teleportNode);
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            this.edgeDirection = direction;
                            this.edgePosRate = 0f;
                            this.UpdateNodeEdge(null, mapEdge2);
                            if (float.IsInfinity(num))
                            {
                                Debug.LogError("1");
                            }
                            this.ProcessMoveByDistance(distance);
                        }
					}
				}
			}
			else if (this.currentEdge != null && this.pathInfo.pathEdges != null)
			{
				float cost = this.currentEdge.cost;
				num = distance / cost;
				if (num < 0f)
				{
					Debug.LogError("1");
				}
				if (!this.CheckPassable(this.currentEdge, this.edgeDirection, this.edgePosRate, this.edgePosRate + num))
				{
					this.blockedTimer = 1f;
					this.StopMoving();
					return;
				}
				this.edgePosRate += num;
				if (this.edgeDirection == EdgeDirection.FORWARD)
				{
					if (this.currentEdge.node1.GetPosition().x < this.currentEdge.node2.GetPosition().x)
					{
						this.unitDirection = UnitDirection.RIGHT;
					}
					else if (this.currentEdge.node1.GetPosition().x > this.currentEdge.node2.GetPosition().x)
					{
						this.unitDirection = UnitDirection.LEFT;
					}
				}
				else if (this.currentEdge.node1.GetPosition().x > this.currentEdge.node2.GetPosition().x)
				{
					this.unitDirection = UnitDirection.RIGHT;
				}
				else if (this.currentEdge.node1.GetPosition().x < this.currentEdge.node2.GetPosition().x)
				{
					this.unitDirection = UnitDirection.LEFT;
				}
				if (this.pathIndex >= this.pathInfo.pathEdges.Length - 1)
				{
					if (this.edgePosRate >= this.edgePosRateGoal)
					{
						this.edgePosRate = this.edgePosRateGoal;
						if (this.edgePosRateGoal == 1f)
						{
							if (this.edgeDirection == EdgeDirection.FORWARD)
							{
								this.UpdateNodeEdge(this.currentEdge.node2, null);
							}
							else
							{
								this.UpdateNodeEdge(this.currentEdge.node1, null);
							}
						}
						MapNode teleportNode2;
						if (this._teleportable && this.currentNode != null && (teleportNode2 = this.currentNode.GetTeleportNode(this, false)) != null)
						{
							this.TrySetCurrentNode(teleportNode2);
						}
						else
						{
							this.StopMoving();
						}
					}
				}
				else if (this.edgePosRate >= 1f)
				{
					float num3 = this.edgePosRate - 1f;
					if (this.edgeDirection == EdgeDirection.FORWARD)
					{
						this.UpdateNodeEdge(this.currentEdge.node2, null);
					}
					else
					{
						this.UpdateNodeEdge(this.currentEdge.node1, null);
					}
					MapNode teleportNode3;
					if (this._teleportable && this.currentNode != null && (teleportNode3 = this.currentNode.GetTeleportNode(this, false)) != null)
					{
						this.TrySetCurrentNode(teleportNode3);
					}
					else
					{
						this.edgePosRate = 0f;
						this.pathIndex++;
						if (float.IsInfinity(num))
						{
							Debug.LogError("1");
						}
						this.ProcessMoveByDistance(num3 * cost);
					}
				}
			}
		}
		IL_7E3:
		if (float.IsNaN(this.edgePosRate))
		{
			Debug.LogError("aaa");
		}
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000E3890 File Offset: 0x000E1A90
	private static MapEdge MoveBy_GetNextEdge(MapNode node, UnitDirection direction)
	{
		List<MapEdge> list = new List<MapEdge>();
		float x = node.GetPosition().x;
		foreach (MapEdge mapEdge in node.GetEdges())
		{
			if (mapEdge.ConnectedNode(node) != null)
			{
				if (mapEdge.ConnectedNode(node).GetPosition().x < x && direction == UnitDirection.LEFT)
				{
					list.Add(mapEdge);
				}
				else if (mapEdge.ConnectedNode(node).GetPosition().x > x && direction == UnitDirection.RIGHT)
				{
					list.Add(mapEdge);
				}
			}
		}
		MapEdge result = null;
		foreach (MapEdge mapEdge2 in list)
		{
			if (!(mapEdge2.type == "door"))
			{
				Vector3 vector = mapEdge2.ConnectedNode(node).GetPosition() - node.GetPosition();
				vector.Normalize();
				if (Mathf.Abs(vector.y / vector.magnitude) < 0.2f)
				{
					result = mapEdge2;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x00014079 File Offset: 0x00012279
	private bool CheckPassInNode()
	{
		return true;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000E39F0 File Offset: 0x000E1BF0
	public bool Equal(MovableObjectNode src)
	{
		return (this.currentEdge == src.currentEdge && this.edgePosRate == src.edgePosRate && this.edgeDirection == src.edgeDirection) || this.currentNode == src.currentNode;
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x0001DF88 File Offset: 0x0001C188
	public MapNode GetCurrentNode()
	{
		return this.currentNode;
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x000E3A48 File Offset: 0x000E1C48
	private void TrySetCurrentNode(MapNode node)
	{
		if (this.model is CreatureModel)
		{
			CreatureModel creatureModel = this.model as CreatureModel;
			if (creatureModel.script.TryRabbitTeleport(node))
			{
				this.SetCurrentNode(node);
			}
		}
		else
		{
			this.SetCurrentNode(node);
		}
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x0001DF90 File Offset: 0x0001C190
	public void SetCurrentNode(MapNode node)
	{
		this.pathInfo = null;
		this._isNextElevator = false;
		this.state = MovableState.STOP;
		this.UpdateNodeEdge(node, null);
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x0001DFAF File Offset: 0x0001C1AF
	public void SetCurrentEdge(MapEdge srcEdge, float srcEdgePosRate, EdgeDirection srcDirection)
	{
		this.pathInfo = null;
		this.state = MovableState.STOP;
		this.edgePosRate = srcEdgePosRate;
		this.edgeDirection = srcDirection;
		this.UpdateNodeEdge(null, srcEdge);
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000E3A98 File Offset: 0x000E1C98
	public void SetCurrentEdge(MovableObjectNode mov)
	{
		MapEdge currentEdge = mov.GetCurrentEdge();
		if (currentEdge != null)
		{
			float srcEdgePosRate = mov.edgePosRate;
			EdgeDirection srcDirection = mov.edgeDirection;
			this.SetCurrentEdge(currentEdge, srcEdgePosRate, srcDirection);
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x0001DFD5 File Offset: 0x0001C1D5
	public UnitDirection GetDirection()
	{
		return this.unitDirection;
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x0001DFDD File Offset: 0x0001C1DD
	public void SetDirection(UnitDirection direction)
	{
		this.unitDirection = direction;
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000E3ACC File Offset: 0x000E1CCC
	public void Assign(MovableObjectNode src)
	{
		this.UpdateNodeEdge(src.currentNode, src.currentEdge);
		if (src.pathInfo != null)
		{
			this.pathInfo = new PathResult((MapEdge[])src.pathInfo.pathEdges.Clone(), (EdgeDirection[])src.pathInfo.edgeDirections.Clone(), src.pathInfo.totalCost);
		}
		else
		{
			this.pathInfo = null;
		}
		this.state = src.state;
		this.pathIndex = src.pathIndex;
		this.edgePosRate = src.edgePosRate;
		this.edgeDirection = src.edgeDirection;
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x0001DFE6 File Offset: 0x0001C1E6
	public MapEdge GetCurrentEdge()
	{
		return this.currentEdge;
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x0001DFEE File Offset: 0x0001C1EE
	public EdgeDirection GetEdgeDirection()
	{
		return this.edgeDirection;
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x0001DFF6 File Offset: 0x0001C1F6
	public bool IsMoving()
	{
		return this.state == MovableState.MOVE;
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000E3B74 File Offset: 0x000E1D74
	public MovableState GetState()
	{
		return this.state;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x0001E001 File Offset: 0x0001C201
	public void Wait()
	{
		this.state = MovableState.WAIT;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x0001E00A File Offset: 0x0001C20A
	public void StopMoving()
	{
		this.pathInfo = null;
		this.pathMoveBy = null;
		this.state = MovableState.STOP;
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x0001E021 File Offset: 0x0001C221
	private void InteractWithDoor(DoorObjectModel door)
	{
		this.model.InteractWithDoor(door);
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x0001E02F File Offset: 0x0001C22F
	public bool CheckInRange(MovableObjectNode other)
	{
		return this.CheckInRange(other, 3f);
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000E3B8C File Offset: 0x000E1D8C
	public bool CheckInRange(MovableObjectNode other, float range)
	{
		float distance = this.GetDistance(other, range);
		return distance != -1f && distance < range;
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x000E3BB4 File Offset: 0x000E1DB4
	public float GetDistance(MapNode other, float limit)
	{
		MovableObjectNode movableObjectNode = new MovableObjectNode(false);
		movableObjectNode.SetCurrentNode(other);
		return this.GetDistance(movableObjectNode, limit);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000E3BD8 File Offset: 0x000E1DD8
	public float GetDistance(MovableObjectNode other, float limit)
	{
		float result = -1f;
		if (this.currentNode != null && other.currentNode != null)
		{
			result = GraphAstar.Distance(this.currentNode, other.currentNode, limit + 5f);
		}
		else if (this.currentNode == null && this.currentEdge != null && other.currentNode != null)
		{
			MapNode mapNode = new MapNode("-1", this.GetCurrentViewPosition(), this.currentEdge.node1.GetAreaName());
			MapEdge edge = new MapEdge(mapNode, this.currentEdge.node1, this.currentEdge.type);
			MapEdge edge2 = new MapEdge(mapNode, this.currentEdge.node2, this.currentEdge.type);
			mapNode.AddEdge(edge);
			mapNode.AddEdge(edge2);
			result = GraphAstar.Distance(mapNode, other.currentNode, limit + 5f);
		}
		else if (this.currentNode != null && other.currentNode == null && other.currentEdge != null)
		{
			MapNode mapNode2 = new MapNode("-1", other.GetCurrentViewPosition(), other.currentEdge.node1.GetAreaName());
			MapEdge edge3 = new MapEdge(mapNode2, other.currentEdge.node1, other.currentEdge.type);
			MapEdge edge4 = new MapEdge(mapNode2, other.currentEdge.node2, other.currentEdge.type);
			mapNode2.AddEdge(edge3);
			mapNode2.AddEdge(edge4);
			result = GraphAstar.Distance(this.currentNode, mapNode2, limit + 5f);
		}
		else if (this.currentNode == null && this.currentEdge != null && other.currentNode == null && other.currentEdge != null)
		{
			MapNode mapNode3 = new MapNode("-1", this.GetCurrentViewPosition(), this.currentEdge.node1.GetAreaName());
			MapEdge edge5 = new MapEdge(mapNode3, this.currentEdge.node1, this.currentEdge.type);
			MapEdge edge6 = new MapEdge(mapNode3, this.currentEdge.node2, this.currentEdge.type);
			mapNode3.AddEdge(edge5);
			mapNode3.AddEdge(edge6);
			MapNode mapNode4 = new MapNode("-1", other.GetCurrentViewPosition(), this.currentEdge.node1.GetAreaName());
			MapEdge edge7 = new MapEdge(other.currentEdge.node1, mapNode4, this.currentEdge.type);
			MapEdge edge8 = new MapEdge(other.currentEdge.node2, mapNode4, this.currentEdge.type);
			other.currentEdge.node1.AddEdge(edge7);
			other.currentEdge.node1.AddEdge(edge8);
			result = GraphAstar.Distance(mapNode3, mapNode4, limit + 5f);
			other.currentEdge.node1.RemoveEdge(edge7);
			other.currentEdge.node1.RemoveEdge(edge8);
		}
		else
		{
			Debug.Log("Current State invalid");
		}
		return result;
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x000E3ED8 File Offset: 0x000E20D8
	public static bool CheckConnectedInPassage(MovableObjectNode n1, MovableObjectNode n2)
	{
		MovableObjectNode movableObjectNode = new MovableObjectNode(false);
		movableObjectNode.Assign(n1);
		movableObjectNode.MoveToMovableNode(n2, true);
		return movableObjectNode.IsMoving();
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000E3F04 File Offset: 0x000E2104
	public void MoveToMovableNode(MovableObjectNode targetNode, bool checkRabbit = false)
	{
		this.StopMoving();
		if (targetNode.currentNode != null)
		{
			this.MoveToNode(targetNode.currentNode, checkRabbit);
			return;
		}
		if (this.currentNode != null)
		{
			MapNode mapNode = new MapNode("-1", this.GetCurrentViewPosition(), targetNode.currentEdge.node1.GetAreaName());
			MapEdge edge = new MapEdge(targetNode.currentEdge.node1, mapNode, targetNode.currentEdge.type);
			MapEdge edge2 = new MapEdge(targetNode.currentEdge.node2, mapNode, targetNode.currentEdge.type);
			mapNode.isTemporary = true;
			targetNode.currentEdge.node1.AddEdge(edge);
			targetNode.currentEdge.node2.AddEdge(edge2);
			PathResult pathResult = GraphAstar.SearchPath(this.currentNode, mapNode, checkRabbit);
			if (pathResult == null)
			{
				return;
			}
			MapEdge[] pathEdges = pathResult.pathEdges;
			float num = 0f;
			if (pathEdges.Length > 0)
			{
				MapEdge mapEdge = pathEdges[pathEdges.Length - 1];
				if (mapEdge.node1 == targetNode.currentEdge.node1)
				{
					pathEdges[pathEdges.Length - 1] = targetNode.currentEdge;
					pathResult.edgeDirections[pathEdges.Length - 1] = EdgeDirection.FORWARD;
					if (targetNode.edgeDirection == EdgeDirection.FORWARD)
					{
						num = targetNode.edgePosRate;
					}
					else
					{
						num = 1f - targetNode.edgePosRate;
					}
				}
				else if (mapEdge.node1 == targetNode.currentEdge.node2)
				{
					pathEdges[pathEdges.Length - 1] = targetNode.currentEdge;
					pathResult.edgeDirections[pathEdges.Length - 1] = EdgeDirection.BACKWARD;
					if (targetNode.edgeDirection == EdgeDirection.FORWARD)
					{
						num = 1f - targetNode.edgePosRate;
					}
					else
					{
						num = targetNode.edgePosRate;
					}
				}
				else
				{
					Debug.LogError("UNKNOWN ERROR : ???");
				}
				this.edgePosRateGoal = num;
				this.pathInfo = pathResult;
				this.state = MovableState.MOVE;
				this.pathIndex = 0;
			}
			targetNode.currentEdge.node1.RemoveEdge(edge);
			targetNode.currentEdge.node2.RemoveEdge(edge2);
		}
		else if (this.currentEdge != null)
		{
			if (this.currentEdge == targetNode.currentEdge)
			{
				PathResult pathResult2 = new PathResult(new MapEdge[1], new EdgeDirection[1], this.currentEdge.cost);
				float num2;
				if (this.edgeDirection == EdgeDirection.FORWARD)
				{
					num2 = this.edgePosRate;
				}
				else
				{
					num2 = 1f - this.edgePosRate;
				}
				float num3;
				if (targetNode.edgeDirection == EdgeDirection.FORWARD)
				{
					num3 = targetNode.edgePosRate;
				}
				else
				{
					num3 = 1f - targetNode.edgePosRate;
				}
				if (num2 > num3)
				{
					this.edgeDirection = EdgeDirection.BACKWARD;
					this.edgePosRate = 1f - num2;
					this.edgePosRateGoal = 1f - num3;
				}
				else
				{
					this.edgeDirection = EdgeDirection.FORWARD;
					this.edgePosRate = num2;
					this.edgePosRateGoal = num3;
				}
				this.pathInfo = pathResult2;
				this.pathInfo.pathEdges[0] = this.currentEdge;
				this.pathInfo.edgeDirections[0] = this.edgeDirection;
				this.pathIndex = 0;
				this.state = MovableState.MOVE;
			}
			else
			{
				MapNode mapNode2 = new MapNode("-1", this.GetCurrentViewPosition(), this.currentEdge.node1.GetAreaName());
				MapEdge mapEdge2 = new MapEdge(mapNode2, this.currentEdge.node1, this.currentEdge.type);
				MapEdge mapEdge3 = new MapEdge(mapNode2, this.currentEdge.node2, this.currentEdge.type);
				mapNode2.isTemporary = true;
				mapNode2.AddEdge(mapEdge2);
				mapNode2.AddEdge(mapEdge3);
				MovableObjectNode movableObjectNode = new MovableObjectNode(false);
				movableObjectNode.Assign(this);
				this.UpdateNodeEdge(mapNode2, null);
				this.MoveToMovableNode(targetNode, checkRabbit);
				mapNode2.RemoveEdge(mapEdge2);
				mapNode2.RemoveEdge(mapEdge3);
				this.pathIndex = 0;
				if (this.pathInfo != null && this.pathInfo.pathEdges != null && this.pathInfo.pathEdges.Length > 0)
				{
					if (this.pathInfo.pathEdges[0] == mapEdge2)
					{
						this.pathInfo.pathEdges[0] = movableObjectNode.currentEdge;
						this.pathInfo.edgeDirections[0] = EdgeDirection.BACKWARD;
						this.edgeDirection = EdgeDirection.BACKWARD;
						if (movableObjectNode.edgeDirection == EdgeDirection.FORWARD)
						{
							this.edgePosRate = 1f - movableObjectNode.edgePosRate;
						}
						else
						{
							this.edgePosRate = movableObjectNode.edgePosRate;
						}
						this.UpdateNodeEdge(null, movableObjectNode.currentEdge);
					}
					else if (this.pathInfo.pathEdges[0] == mapEdge3)
					{
						this.pathInfo.pathEdges[0] = movableObjectNode.currentEdge;
						this.pathInfo.edgeDirections[0] = EdgeDirection.FORWARD;
						this.edgeDirection = EdgeDirection.FORWARD;
						if (movableObjectNode.edgeDirection == EdgeDirection.FORWARD)
						{
							this.edgePosRate = movableObjectNode.edgePosRate;
						}
						else
						{
							this.edgePosRate = 1f - movableObjectNode.edgePosRate;
						}
						this.UpdateNodeEdge(null, movableObjectNode.currentEdge);
					}
					else
					{
						Debug.LogError("MovableObjectNode : unknown error");
					}
				}
				else
				{
					this.Assign(movableObjectNode);
				}
			}
		}
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x000E4410 File Offset: 0x000E2610
	public void MoveToNode(MapNode targetNode, bool checkRabbit = false)
	{
		this.StopMoving();
		if (this.currentNode != null)
		{
			PathResult pathResult = GraphAstar.SearchPath(this.currentNode, targetNode, checkRabbit);
			if (pathResult == null)
			{
				return;
			}
			this.pathInfo = pathResult;
			this.state = MovableState.MOVE;
			this.pathIndex = 0;
			this.edgePosRateGoal = 1f;
		}
		else if (this.currentEdge != null)
		{
			PathResult pathResult2 = GraphAstar.SearchPath(this.currentEdge.node1, targetNode, checkRabbit);
			PathResult pathResult3 = GraphAstar.SearchPath(this.currentEdge.node2, targetNode, checkRabbit);
			float num = 0f;
			if (pathResult2 != null)
			{
				if (this.edgeDirection == EdgeDirection.FORWARD)
				{
					num = this.currentEdge.cost * this.edgePosRate + pathResult2.totalCost;
				}
				else
				{
					num = this.currentEdge.cost * (1f - this.edgePosRate) + pathResult2.totalCost;
				}
			}
			float num2 = 0f;
			if (pathResult3 != null)
			{
				if (this.edgeDirection == EdgeDirection.FORWARD)
				{
					num2 = this.currentEdge.cost * (1f - this.edgePosRate) + pathResult3.totalCost;
				}
				else
				{
					num2 = this.currentEdge.cost * this.edgePosRate + pathResult3.totalCost;
				}
			}
			if (pathResult2 != null && (pathResult3 == null || num <= num2))
			{
				List<EdgeDirection> list = new List<EdgeDirection>(pathResult2.edgeDirections);
				List<MapEdge> list2 = new List<MapEdge>(pathResult2.pathEdges);
				list.Insert(0, EdgeDirection.BACKWARD);
				list2.Insert(0, this.currentEdge);
				pathResult2.pathEdges = list2.ToArray();
				pathResult2.edgeDirections = list.ToArray();
				if (this.edgeDirection == EdgeDirection.FORWARD)
				{
					this.edgeDirection = EdgeDirection.BACKWARD;
					this.edgePosRate = 1f - this.edgePosRate;
				}
				this.pathInfo = pathResult2;
			}
			else
			{
				if (pathResult3 == null || (pathResult2 != null && num2 > num))
				{
					return;
				}
				List<EdgeDirection> list3 = new List<EdgeDirection>(pathResult3.edgeDirections);
				List<MapEdge> list4 = new List<MapEdge>(pathResult3.pathEdges);
				list3.Insert(0, EdgeDirection.FORWARD);
				list4.Insert(0, this.currentEdge);
				pathResult3.pathEdges = list4.ToArray();
				pathResult3.edgeDirections = list3.ToArray();
				if (this.edgeDirection == EdgeDirection.BACKWARD)
				{
					this.edgeDirection = EdgeDirection.FORWARD;
					this.edgePosRate = 1f - this.edgePosRate;
				}
				this.pathInfo = pathResult3;
			}
			this.pathIndex = 0;
			this.state = MovableState.MOVE;
			this.edgePosRateGoal = 1f;
		}
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x000E4688 File Offset: 0x000E2888
	public bool CanMoveBy(UnitDirection direction)
	{
		return this.currentEdge != null || (this.currentNode != null && MovableObjectNode.MoveBy_GetNextEdge(this.currentNode, direction) != null);
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000E46C8 File Offset: 0x000E28C8
	public void MoveBy(UnitDirection direction, float value)
	{
		this.StopMoving();
		this.pathMoveBy = new PathMoveBy();
		this.pathMoveBy.direction = direction;
		this.pathMoveBy.value = value;
		this.state = MovableState.MOVE;
		this.moveDistance = 0f;
		if (this.currentEdge != null)
		{
			if (direction == UnitDirection.RIGHT)
			{
				if (this.edgeDirection == EdgeDirection.FORWARD && this.currentEdge.node1.GetPosition().x > this.currentEdge.node2.GetPosition().x)
				{
					this.edgeDirection = EdgeDirection.BACKWARD;
					this.edgePosRate = 1f - this.edgePosRate;
				}
				else if (this.edgeDirection == EdgeDirection.BACKWARD && this.currentEdge.node1.GetPosition().x < this.currentEdge.node2.GetPosition().x)
				{
					this.edgeDirection = EdgeDirection.FORWARD;
					this.edgePosRate = 1f - this.edgePosRate;
				}
			}
			if (direction == UnitDirection.LEFT)
			{
				if (this.edgeDirection == EdgeDirection.FORWARD && this.currentEdge.node1.GetPosition().x < this.currentEdge.node2.GetPosition().x)
				{
					this.edgeDirection = EdgeDirection.BACKWARD;
					this.edgePosRate = 1f - this.edgePosRate;
				}
				else if (this.edgeDirection == EdgeDirection.BACKWARD && this.currentEdge.node1.GetPosition().x > this.currentEdge.node2.GetPosition().x)
				{
					this.edgeDirection = EdgeDirection.FORWARD;
					this.edgePosRate = 1f - this.edgePosRate;
				}
			}
		}
		if (this.currentNode != null && MovableObjectNode.MoveBy_GetNextEdge(this.currentNode, this.pathMoveBy.direction) == null)
		{
			this.StopMoving();
		}
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x000E48C8 File Offset: 0x000E2AC8
	public MovableObjectNode GetSideMovableNode(UnitDirection direction, float distance)
	{
		MapNode mapNode = this.currentNode;
		MapEdge mapEdge = this.currentEdge;
		float num = this.edgePosRate;
		EdgeDirection srcDirection = this.edgeDirection;
		for (;;)
		{
			if (mapNode != null)
			{
				MapEdge mapEdge2 = MovableObjectNode.MoveBy_GetNextEdge(mapNode, direction);
				if (mapEdge2 == null)
				{
					break;
				}
				if (mapEdge2.node1 == mapNode)
				{
					srcDirection = EdgeDirection.FORWARD;
				}
				else
				{
					srcDirection = EdgeDirection.BACKWARD;
				}
				num = 0f;
				mapNode = null;
				mapEdge = mapEdge2;
			}
			else if (mapEdge != null)
			{
				float cost = mapEdge.cost;
				float num2 = distance / cost;
				num += num2;
				float num3 = 0f;
				if (num >= 1f)
				{
					num3 = num - 1f;
					if (this.edgeDirection == EdgeDirection.FORWARD)
					{
						mapNode = mapEdge.node2;
					}
					else
					{
						mapNode = mapEdge.node1;
					}
					mapEdge = null;
				}
				if (num3 <= 0f)
				{
					break;
				}
				distance = num3 * cost;
			}
			else
			{
				Debug.Log("invalid");
			}
		}
		MovableObjectNode movableObjectNode = new MovableObjectNode(false);
		if (mapNode != null)
		{
			movableObjectNode.SetCurrentNode(mapNode);
		}
		else
		{
			movableObjectNode.SetCurrentEdge(mapEdge, num, srcDirection);
		}
		return movableObjectNode;
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x0001E03D File Offset: 0x0001C23D
	public bool EqualPosition(MapNode node)
	{
		return this.GetCurrentNode() != null && this.GetCurrentNode().GetId() == node.GetId();
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x0001E068 File Offset: 0x0001C268
	private void UpdateNodeEdge(MapNode node, MapEdge edge)
	{
		this._currentNode = node;
		this._currentEdge = edge;
		if (node != null && !node.isTemporary)
		{
			this.lastNode = node;
		}
		this.UpdateCurrentPassage();
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x000E49EC File Offset: 0x000E2BEC
	private void UpdateCurrentPassage()
	{
		PassageObjectModel passageObjectModel = this.notNullPassage;
		PassageObjectModel currentPassage = this.currentPassage;
		if (this.currentNode != null)
		{
			this.currentPassage = this.currentNode.GetAttachedPassage();
		}
		else if (this.currentEdge != null)
		{
			PassageObjectModel attachedPassage = this.currentEdge.node1.GetAttachedPassage();
			PassageObjectModel attachedPassage2 = this.currentEdge.node2.GetAttachedPassage();
			if (attachedPassage != null && attachedPassage2 != null)
			{
				if (attachedPassage == attachedPassage2)
				{
					this.currentPassage = attachedPassage;
				}
				else
				{
					this.currentPassage = null;
				}
			}
			else
			{
				this.currentPassage = null;
			}
		}
		else
		{
			Debug.Log("ERROR : invalid node state");
		}
		if (currentPassage != this.currentPassage)
		{
			if (currentPassage != null)
			{
				currentPassage.ExitUnit(this);
			}
			if (this.currentPassage != null && this.isActive)
			{
				this.currentPassage.EnterUnit(this);
			}
		}
		if (passageObjectModel != this.notNullPassage)
		{
			if (passageObjectModel != null && passageObjectModel.GetSefiraEnum() != SefiraEnum.DUMMY)
			{
				passageObjectModel.GetSefira().ExitAgent(this);
			}
			if (this.notNullPassage != null && this.isActive && this.notNullPassage.GetSefiraEnum() != SefiraEnum.DUMMY)
			{
				this.notNullPassage.GetSefira().EnterAgent(this);
			}
		}
		if (this.currentPassage != null)
		{
			this.currentScale = this.currentPassage.scaleFactor;
		}
		else
		{
			this.currentScale = 1f;
		}
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x0001E096 File Offset: 0x0001C296
	public bool InElevator()
	{
		return this.currentPassage != null && this.currentPassage.type == PassageType.VERTICAL;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000E4B64 File Offset: 0x000E2D64
	public void EnterElevator(MapNode elevatorNode, MapNode nextNode)
	{
		if (this.currentNode != null && elevatorNode.GetElevator() != null)
		{
			bool flag = false;
			foreach (MapNode mapNode in elevatorNode.GetElevator().GetCurrentFloorNodes())
			{
				if (mapNode == this.currentNode)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.currentElevator = elevatorNode.GetElevator();
				this.currentElevator.OnUnitEnter(this, nextNode);
			}
			else
			{
				elevatorNode.GetElevator().ClickButton(this.currentNode);
			}
		}
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x0001E0B7 File Offset: 0x0001C2B7
	public void ExitElevator(MapNode node)
	{
		if (this.currentElevator != null)
		{
			this.SetCurrentNode(node);
		}
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x000E4BF8 File Offset: 0x000E2DF8
	public static float GetDistance(MovableObjectNode node1, MovableObjectNode node2)
	{ // <Mod>
		if (node1.GetPassage() == null)
		{
			return 100000f;
		}
		if (node1.GetPassage() != node2.GetPassage())
		{
			return 100000f;
		}
		/*
		if (!MovableObjectNode.CheckConnectedInPassage(node1, node2))
		{
			return 100000f;
		}*/
		Vector3 currentViewPosition = node1.GetCurrentViewPosition();
		Vector3 currentViewPosition2 = node2.GetCurrentViewPosition();
		return DistanceUnitUtil.UnityToGame(Vector3.Distance(currentViewPosition, currentViewPosition2) / node1.currentScale);
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000E4C60 File Offset: 0x000E2E60
	public void ReportUnitName()
	{
		if (this.model is WorkerModel)
		{
			Debug.Log((this.model as WorkerModel).name);
		}
		else if (this.model is CreatureModel)
		{
			Debug.Log((this.model as CreatureModel).metaInfo.name);
		}
		else if (this.model is StandingItemModel)
		{
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x0001E0CB File Offset: 0x0001C2CB
	public UnitModel GetUnit()
	{
		return this.model;
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000E4CD8 File Offset: 0x000E2ED8
	public float GetDistanceDouble(MovableObjectNode mov)
	{
		Vector2 vector = this.GetCurrentViewPosition();
		Vector2 vector2 = mov.GetCurrentViewPosition();
		float num = vector2.x - vector.x;
		float num2 = vector2.y - vector.y;
		float num3 = num * num + num2 * num2;
		float num4 = 1.3333334f;
		return num3 * num4 * num4;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x000E4D38 File Offset: 0x000E2F38
	public Sefira GetCurrentStandingSefira()
	{
		PassageObjectModel passageObjectModel = this.currentPassage;
		Sefira result;
		try
		{
			if (passageObjectModel == null)
			{
				Sefira sefira = null;
				MapEdge currentEdge = this.GetCurrentEdge();
				if (currentEdge == null)
				{
					result = null;
				}
				else
				{
					MapNode mapNode = currentEdge.node1;
					passageObjectModel = mapNode.GetAttachedPassage();
					if (passageObjectModel != null)
					{
						sefira = SefiraManager.instance.GetSefira(passageObjectModel.GetSefiraName());
					}
					else
					{
						mapNode = currentEdge.node2;
						passageObjectModel = mapNode.GetAttachedPassage();
						if (passageObjectModel != null)
						{
							sefira = SefiraManager.instance.GetSefira(passageObjectModel.GetSefiraName());
						}
					}
					result = sefira;
				}
			}
			else
			{
				result = SefiraManager.instance.GetSefira(passageObjectModel.GetSefiraName());
			}
		}
		catch (Exception ex)
		{
			result = null;
		}
		return result;
	}

	// Token: 0x04001BB8 RID: 7096
	private UnitModel model;

	// Token: 0x04001BB9 RID: 7097
	private bool _isActive;

	// Token: 0x04001BBA RID: 7098
	private List<PassType> unpassableList;

	// Token: 0x04001BBB RID: 7099
	private bool _teleportable = true;

	// Token: 0x04001BBC RID: 7100
	private MovableState state;

	// Token: 0x04001BBD RID: 7101
	private MapNode lastNode;

	// Token: 0x04001BBE RID: 7102
	private MapNode _currentNode;

	// Token: 0x04001BBF RID: 7103
	private MapEdge _currentEdge;

	// Token: 0x04001BC0 RID: 7104
	private UnitDirection unitDirection;

	// Token: 0x04001BC1 RID: 7105
	private ElevatorPassageModel currentElevator;

	// Token: 0x04001BC2 RID: 7106
	private object passageChangedParam;

	// Token: 0x04001BC3 RID: 7107
	private PassageObjectModel _currentPassage;

	// Token: 0x04001BC4 RID: 7108
	private PassageObjectModel notNullPassage;

	// Token: 0x04001BC5 RID: 7109
	public float currentZValue;

	// Token: 0x04001BC6 RID: 7110
	public bool isIgnoreZValue;

	// Token: 0x04001BC7 RID: 7111
	public float edgePosRate;

	// Token: 0x04001BC8 RID: 7112
	public EdgeDirection edgeDirection;

	// Token: 0x04001BC9 RID: 7113
	private PathResult pathInfo;

	// Token: 0x04001BCA RID: 7114
	private float edgePosRateGoal;

	// Token: 0x04001BCB RID: 7115
	private MapNode destinationNode;

	// Token: 0x04001BCC RID: 7116
	private MovableObjectNode destinationNode2;

	// Token: 0x04001BCD RID: 7117
	private bool _isNextElevator;

	// Token: 0x04001BCE RID: 7118
	private float _elevatorWaitElapsedTime;

	// Token: 0x04001BCF RID: 7119
	public const float _elevatorWaitTime = 0.5f;

	// Token: 0x04001BD0 RID: 7120
	private int pathIndex;

	// Token: 0x04001BD1 RID: 7121
	private PathMoveBy pathMoveBy;

	// Token: 0x04001BD2 RID: 7122
	private float moveDistance;

	// Token: 0x04001BD3 RID: 7123
	public float currentScale = 1f;

	// Token: 0x04001BD4 RID: 7124
	public float blockedTimer;

	// Token: 0x04001BD5 RID: 7125
	private bool viewPositionSet;

	// Token: 0x04001BD6 RID: 7126
	private Vector3 viewPosition;

	// Token: 0x02000377 RID: 887
	public class MovableElevatorStuckException : Exception
	{
		// Token: 0x06001B53 RID: 6995 RVA: 0x0001E0D3 File Offset: 0x0001C2D3
		public MovableElevatorStuckException()
		{
		}
	}
}
