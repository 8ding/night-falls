using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    Rigidbody2D rgBody2D;
    [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
        rgBody2D = GetComponent<Rigidbody2D>();
    }
    public void Move(Vector2 direction,float speed,bool isMoving)
    {
        if (isMoving)
        {
            rgBody2D.MovePosition(rgBody2D.position + direction * speed * Time.deltaTime);
		}
    }
    // Update is called once per frame

}
