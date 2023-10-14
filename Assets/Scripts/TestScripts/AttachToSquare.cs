using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToSquare : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidbody2d;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2d.SetRotation(rigidbody2d.rotation + 1);
    }
}
