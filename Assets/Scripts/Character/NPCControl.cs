using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    private Vector3 mousePosition;
	List<PathNode> nodes;
	[SerializeField]
	private float speed;
    [SerializeField]
    NPCMove NPCMove;
    Vector3 nextPosition;
	private bool isMoving;
	Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = DealPosition.GetWorldMousePosition(transform.position.z);
        nodes = new List<PathNode>();
    }

    // Update is called once per frame
    void Update()
	{
		HandleInput();
		DealLogicMove();
		NPCMove.Move(direction, speed,isMoving);
	}
	private void DealLogicMove()
	{

		//如果当前节点的位置与本体的位置距离小于等于一帧移动的距离，则这一帧移动方向改为下个节点方向
		if (Vector3.Distance(transform.position,nextPosition) > speed*Time.deltaTime+0.01f)
		{
			isMoving = true;
		}
		else
		{
			transform.position = nextPosition;
			if (nodes.Count > 0)
			{
				nextPosition = nodes[0].WorldPosition;
				direction = (nextPosition - transform.position).normalized;
				nodes.RemoveAt(0);
			}
			else
			{
				isMoving = false;
			}
		}
	}

	private void HandleInput()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 currentMousePosition = DealPosition.GetWorldMousePosition(transform.position.z);
			if (!currentMousePosition.Equals(mousePosition))
			{
				GameManager.Instance.ClearColor();
				mousePosition = currentMousePosition;
				nodes = GameManager.Instance.GetPath(transform.position, currentMousePosition);
				if (nodes != null)
				{
					nextPosition = nodes[0].WorldPosition;
					direction = nextPosition - transform.position;
					nodes.RemoveAt(0);
				}
			}
		}
	}

}
