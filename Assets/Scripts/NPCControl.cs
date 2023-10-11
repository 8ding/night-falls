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
		//HandleInput();
		DealLogicMove();
	}
	private void FixedUpdate()
	{
		NPCMove.Move(direction);
	}
	private void DealLogicMove()
	{
		if (nodes.Count > 0)
		{
			if ((nodes[0].WorldPosition - transform.position).magnitude > 0.1)
			{
				direction = nodes[0].WorldPosition - transform.position;
				direction.Normalize();
			}
			else
			{
				nodes.RemoveAt(0);
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
