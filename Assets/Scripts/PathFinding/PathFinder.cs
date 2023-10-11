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

    //���д���
    public int Gcost =-1;
    //Ԥ������
    public int Hcost = -1;
    //�ܴ���
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
    /// ���ø��ڵ�,����·�ߵĻ���
    /// </summary>
    /// <param name="parrentNode">���ڵ㣬Ҳ����·���е���һ��λ��</param>
    public void SetParrentNode(PathNode parrentNode)
    {
		this.parrentNode = parrentNode;
    }


}
public class PathFinder 
{

    /// <summary>
    /// ��ȡ�ƶ�·��
    /// </summary>
    /// <param name="_startPosition">������ʼ��������λ��</param>
    /// <param name="_targetPosition">����Ŀ����������λ��</param>
    /// <returns>����·���ڵ��б�</returns>
    public List<PathNode> GetPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        List<PathNode> resNodes = new List<PathNode> ();
        Dictionary<Vector3Int,PathNode> toSearchNode = new Dictionary<Vector3Int, PathNode> ();
        Dictionary<Vector3Int, PathNode> processNode = new Dictionary<Vector3Int, PathNode>();
		//Ŀ��ڵ�
		PathNode endNode = new(GameManager.Instance.GetCellFromWorld(_targetPosition), _targetPosition);
        endNode.SetHcost(endNode);
        //��ʼ�ڵ�
        PathNode beginNode = new(GameManager.Instance.GetCellFromWorld(_startPosition), _startPosition);
        beginNode.SetGcost(0);
        beginNode.SetHcost(endNode);

		PathNode currentNode = beginNode;
        toSearchNode.Add(beginNode.CellPosition, beginNode);
        float startTime = Time.fixedTime;
        //��·�߻��Ѿ��ҵ��յ�ʱ����
        while (toSearchNode.Count > 0 )
        {
            //��ȡ����Ѱ����Fcost��͵Ľڵ���Ϊ��ǰ�ڵ�
            currentNode = GetLowestFCostNode(toSearchNode);
            //�ҵ�Ŀ��ڵ㼴�˳�ѭ��
			if (currentNode.CellPosition == endNode.CellPosition)
            {
                break;
            }
			//��ʾ�Ѵ������ɫ
			GameManager.Instance.UnlockSetColor(currentNode.CellPosition, Color.red);
			//�Ѿ�����Ľڵ�ӵ��Ѵ���ڵ���ȥ
			processNode.Add(currentNode.CellPosition, currentNode);
            //����ǰ�ڵ�Ӵ���Ѱ�ڵ����Ƴ�
            toSearchNode.Remove(currentNode.CellPosition);
            List<PathNode> neighbors = GetNeighbors(currentNode, toSearchNode);
            for (int i = 0; i < neighbors.Count; i++)
            {
                //�����ھӽڵ��Ƿ��Ѽ������Ѱ��
                bool isInSearch = toSearchNode.ContainsKey(neighbors[i].CellPosition);
                bool isInProcess = processNode.ContainsKey(neighbors[i].CellPosition);
                int updateCost = currentNode.Gcost + GetNeighborGcost(currentNode, neighbors[i]);
                if (!isInProcess)
                {
					//����ھӽڵ�δ�������Ѱ�����Gcost�ܱ�����,�����Gcost���߼������Ѱ��
					if (!isInSearch || updateCost < neighbors[i].Gcost)
                    {
						//����Gcost,����·����һ�ڵ�
						neighbors[i].SetGcost(updateCost);
						neighbors[i].SetParrentNode(currentNode);
						//δ������Ѱ���������Ѱ��
						if (!isInSearch)
                        {
                            toSearchNode.Add(neighbors[i].CellPosition, neighbors[i]);
                            //��ʾ���������ɫ
							GameManager.Instance.UnlockSetColor(neighbors[i].CellPosition, Color.blue);
                            neighbors[i].SetHcost(endNode);
                        }
                    }
                }
            }
        }
        //��·���ڵ�������ڵ��б�
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
    /// ��ȡ����Ѱ����Fcost��С�Ľڵ�
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
	/// ��ȡ�ڵ㵽�ھӽڵ�ľ��룬���ǶԽ��߽ڵ���������
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
    /// ��ȡ�ýڵ�������ھӽڵ㣬��������ھӽڵ��Hcost
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
    /// ��⴫������λ���Ƿ��ܹ�����ڵ���ھӽڵ�
    /// </summary>
    /// <param name="testNode"></param>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    private bool CanAsNeighborNode(Vector3Int cellPosition, PathNode currentNode)
    {
        if (GameManager.Instance.IsHaveObstacle(cellPosition))    //��λ�ò������ϰ���
        {
            return false;
        }
        else if (Mathf.Abs(cellPosition.x) >= GameManager.Instance.SizeHorizontal || Mathf.Abs(cellPosition.y) >= GameManager.Instance.SizeVerical) //λ�ò��ܳ����߽�
        {
            return false;
        }
        else //���ϡ����¡����ϡ�����λ�ò��ܱ��ϰ���߰�Χ
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
