using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PathNode
{
    public static int RegularDis = 10;
    public static int DiagnolDis = 14;
	public Vector3Int CellPosition;
    public Vector3 WorldPosition;
    public PathNode parrentNode;

    //已有代价
    public int Gcost =-1;
    //预估代价
    public int Hcost = -1;
    //总代价
    public int Fcost = -1;
    public PathNode(Vector3Int cellPosition, Vector3 worldPosition,int Hcost)
	{
		CellPosition = cellPosition;
		WorldPosition = worldPosition;
        this.Hcost = Hcost;
        parrentNode = null;
	}
    public PathNode(Vector3Int cellPosition, Vector3 worldPosition)
    {
		CellPosition = cellPosition;
		WorldPosition = worldPosition;
        parrentNode = null;
    }
    public void SetHcost(PathNode endNode)
    {
        this.Hcost = Mathf.Abs(CellPosition.x - endNode.CellPosition.x) * RegularDis + Mathf.Abs(CellPosition.y - endNode.CellPosition.y) * RegularDis;
		if (Gcost != -1)
        {
            Fcost = Hcost + Gcost;
        }
    }
    public void SetGcost(int Gcost)
    {
        this.Gcost = Gcost;
        if (Hcost != -1)
        {
            Fcost = Hcost + this.Gcost;
        }
        
    }

    /// <summary>
    /// 设置父节点,用于路线的回溯
    /// </summary>
    /// <param name="parrentNode">父节点，也就是路线中的上一个位置</param>
    public void SetParrentNode(PathNode parrentNode)
    {
		this.parrentNode = parrentNode;
    }


}
public class PathFinder 
{

    /// <summary>
    /// 获取移动路线
    /// </summary>
    /// <param name="_startPosition">传入起始世界坐标位置</param>
    /// <param name="_targetPosition">传入目标世界坐标位置</param>
    /// <returns>返回路径节点列表</returns>
    public List<PathNode> GetPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        List<PathNode> resNodes = new List<PathNode> ();
        Dictionary<Vector3Int,PathNode> toSearchNode = new Dictionary<Vector3Int, PathNode> ();
        Dictionary<Vector3Int, PathNode> processNode = new Dictionary<Vector3Int, PathNode>();
		//目标节点
		PathNode endNode = new(GameManager.Instance.GetCellFromWorld(_targetPosition), _targetPosition);
        endNode.SetHcost(endNode);
        //初始节点
        PathNode beginNode = new(GameManager.Instance.GetCellFromWorld(_startPosition), _startPosition);
        beginNode.SetGcost(0);
        beginNode.SetHcost(endNode);

		PathNode currentNode = beginNode;
        toSearchNode.Add(beginNode.CellPosition, beginNode);
        float startTime = Time.fixedTime;
        //无路线或已经找到终点时结束
        while (toSearchNode.Count > 0 )
        {
            //获取待搜寻库中Fcost最低的节点作为当前节点
            currentNode = GetLowestFCostNode(toSearchNode);
            //找到目标节点即退出循环
			if (currentNode.CellPosition == endNode.CellPosition)
            {
                break;
            }
			//显示已处理点颜色
			GameManager.Instance.UnlockSetColor(currentNode.CellPosition, Color.red);
			//已经处理的节点加到已处理节点中去
			processNode.Add(currentNode.CellPosition, currentNode);
            //将当前节点从待搜寻节点中移除
            toSearchNode.Remove(currentNode.CellPosition);
            List<PathNode> neighbors = GetNeighbors(currentNode, toSearchNode);
            for (int i = 0; i < neighbors.Count; i++)
            {
                //检验邻居节点是否已加入待搜寻库
                bool isInSearch = toSearchNode.ContainsKey(neighbors[i].CellPosition);
                bool isInProcess = processNode.ContainsKey(neighbors[i].CellPosition);
                int updateCost = currentNode.Gcost + GetNeighborGcost(currentNode, neighbors[i]);
                if (!isInProcess)
                {
					//如果邻居节点未加入待搜寻库或者Gcost能被更新,则更新Gcost或者加入待搜寻库
					if (!isInSearch || updateCost < neighbors[i].Gcost)
                    {
						//更新Gcost,设置路径上一节点
						neighbors[i].SetGcost(updateCost);
						neighbors[i].SetParrentNode(currentNode);
						//未加入搜寻库则加入搜寻库
						if (!isInSearch)
                        {
                            toSearchNode.Add(neighbors[i].CellPosition, neighbors[i]);
                            //显示待处理点颜色
							GameManager.Instance.UnlockSetColor(neighbors[i].CellPosition, Color.blue);
                            neighbors[i].SetHcost(endNode);
                        }
                    }
                }
            }
        }
        //将路径节点加入结果节点列表
        if (toSearchNode.Count > 0)
        {
            resNodes.Add(endNode);
            currentNode = currentNode.parrentNode;
            while (currentNode != null)
            {
                resNodes.Add(currentNode);
                currentNode = currentNode.parrentNode;
            }
        }
		resNodes.Reverse();
        return resNodes;
    }

    /// <summary>
    /// 获取待搜寻库中Fcost最小的节点
    /// </summary>
    /// <param name="curretNode"></param>
    /// <param name="pathNodes"></param>
    /// <returns></returns>
	private PathNode GetLowestFCostNode(Dictionary<Vector3Int,PathNode> pathNodes)
	{
        int lowest = int.MaxValue;
        PathNode resNode = null;
        foreach (var item in pathNodes)
        {
            if (item.Value.Fcost < lowest)
            {
                lowest = item.Value.Fcost;
                resNode = item.Value;
            }
        }
        return resNode;
	}
	/// <summary>
	/// 获取节点到邻居节点的距离，若是对角线节点则距离更大
	/// </summary>
	/// <param name="node"></param>
	/// <param name="anotherNode"></param>
	/// <returns></returns>
	private int GetNeighborGcost(PathNode node, PathNode anotherNode)
    {
        if (Mathf.Abs(node.CellPosition.x - anotherNode.CellPosition.x) > 0 && Mathf.Abs(node.CellPosition.y - anotherNode.CellPosition.y) > 0)
        {
            return PathNode.DiagnolDis ;
        }
        else
            return PathNode.RegularDis;
    }
    /// <summary>
    /// 获取该节点的所有邻居节点，并计算出邻居节点的Hcost
    /// </summary>
    /// <param name="node"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    private List<PathNode> GetNeighbors(PathNode node,Dictionary<Vector3Int, PathNode> toSearchNodes)
    {
        List<PathNode> neighbors = new List<PathNode> ();
        PathNode pathNode = null;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                
                Vector3Int cellPosition = new Vector3Int(node.CellPosition.x+i, node.CellPosition.y+j, node.CellPosition.z);
                if (CanAsNeighborNode(cellPosition, node))
                {
					if (toSearchNodes.ContainsKey(cellPosition))
					{
						pathNode = toSearchNodes[cellPosition];
					}
					else
					{
						pathNode = new(cellPosition, GameManager.Instance.GetWorldFromCell(cellPosition));
					}
					if (pathNode != node)
					{
						neighbors.Add(pathNode);
					}
				}
            }
        }
        return neighbors;
    }
    /// <summary>
    /// 检测传入网格位置是否能够加入节点的邻居节点
    /// </summary>
    /// <param name="testNode"></param>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    private bool CanAsNeighborNode(Vector3Int cellPosition, PathNode currentNode)
    {
        if (GameManager.Instance.IsHaveObstacle(cellPosition))    //该位置不能有障碍物
        {
            return false;
        }
        else if (Mathf.Abs(cellPosition.x) >= GameManager.Instance.SizeHorizontal || Mathf.Abs(cellPosition.y) >= GameManager.Instance.SizeVerical) //位置不能超出边界
        {
            return false;
        }
        else //左上、左下、右上、右下位置不能被障碍半边包围
        {
            if (cellPosition.x < currentNode.CellPosition.x && cellPosition.y < currentNode.CellPosition.y)
                return !(IsRightObstacle(cellPosition) && IsUpObstacle(cellPosition));
            else if (cellPosition.x < currentNode.CellPosition.x && cellPosition.y > currentNode.CellPosition.y)
                return !(IsRightObstacle(cellPosition) && IsDownObstacle(cellPosition));
            else if (cellPosition.x > currentNode.CellPosition.x && cellPosition.y > currentNode.CellPosition.y)
                return !(IsLeftObstacle(cellPosition) && IsDownObstacle(cellPosition));
            else if (cellPosition.x > currentNode.CellPosition.x && cellPosition.y < currentNode.CellPosition.y)
                return !(IsLeftObstacle(cellPosition) && IsUpObstacle(cellPosition));
            else
                return true;
		}
    }
    private bool IsRightObstacle(Vector3Int cellPosition)
    {
        return GameManager.Instance.IsHaveObstacle(new Vector3Int(cellPosition.x + 1, cellPosition.y));
    }
	private bool IsLeftObstacle(Vector3Int cellPosition)
	{
        return GameManager.Instance.IsHaveObstacle(new Vector3Int(cellPosition.x - 1, cellPosition.y));
	}
	private bool IsUpObstacle(Vector3Int cellPosition)
	{
		return GameManager.Instance.IsHaveObstacle(new Vector3Int(cellPosition.x, cellPosition.y+1));
	}
	private bool IsDownObstacle(Vector3Int cellPosition)
	{
		return GameManager.Instance.IsHaveObstacle(new Vector3Int(cellPosition.x, cellPosition.y - 1));
	}
}
