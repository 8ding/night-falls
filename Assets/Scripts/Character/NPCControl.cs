using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Progress;

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
	private int isInLight;
	public System.Action<NPCControl> setInlight;
	public System.Action<NPCControl> setOutlight;
	Rigidbody2D rigidbody2;
    public int IsInLight { get => isInLight; 
		set 
		{ 
			isInLight = value;
			if (value > 0)
				setInlight?.Invoke(this);
			else
				setOutlight?.Invoke(this);
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		IsInLight = 0;
        mousePosition = DealPosition.GetWorldMousePosition(transform.position.z);
        nodes = new List<PathNode>();
		rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
	{

		HandleInput();
		DealLogicMove();

	}
    private void FixedUpdate()
    {
        NPCMove.Move(direction, speed, isMoving);
		
    }
    private void DealLogicMove()
	{

		if(isMoving)
        {
            //如果当前节点的位置与本体的位置距离小于等于一帧移动的距离，则这一帧移动方向改为下个节点方向
            if (Vector2.Distance(rigidbody2.position, nextPosition) < speed*Time.fixedDeltaTime+0.01)
            {
                rigidbody2.position = nextPosition;
                if (nodes.Count > 0)
                {

                    nextPosition = nodes[0].WorldPosition;
                    direction = ((Vector2)nextPosition - rigidbody2.position).normalized;
                    nodes.RemoveAt(0);
                }
                else
                {
                    isMoving = false;
                }
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
					direction = (Vector2)nextPosition - rigidbody2.position;
					nodes.RemoveAt(0);
					isMoving = true;
				}
			}
		}
	}

}
