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
		NPCMove.Move(direction, speed);
	}
	private void DealLogicMove()
	{
		if (nodes.Count > 0)
		{
			//�����ǰ�ڵ��λ���뱾���λ�þ���С�ڵ���һ֡�ƶ��ľ��룬����һ֡�ƶ������Ϊ�¸��ڵ㷽��
			if ((nodes[0].WorldPosition - transform.position).magnitude <= (direction* speed *Time.deltaTime).magnitude+0.01f)
			{
				transform.position = nodes[0].WorldPosition;
				nodes.RemoveAt(0);
				if (nodes.Count > 0)
				{
					direction = nodes[0].WorldPosition - transform.position;
					direction.Normalize();
				}
				else
					direction = Vector2.zero;
			}
		}
		else
		{
			direction = Vector2.zero;
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
			}
		}
	}

}
