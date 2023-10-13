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
    public void Move(Vector3 position,float speed,bool isMoving)
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
		}
    }
    // Update is called once per frame

}
