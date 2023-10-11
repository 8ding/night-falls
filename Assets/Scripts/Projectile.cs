using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 moveDirection;
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigidbody2;
    // Start is called before the first frame update
    void Start()
    {
		rigidbody2 = GetComponent<Rigidbody2D>();
    }
    public void PointToDirection(Vector2 direction)
    {
        direction.Normalize();
		//zÖáÐý×ªÖµ
		float zDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, zDegree);
        moveDirection = direction;
	}
    // Update is called once per frame
    void Update()
    {
        
    }
	private void FixedUpdate()
    {
        if(moveDirection != Vector2.zero)
            rigidbody2.MovePosition(rigidbody2.position + moveDirection * moveSpeed * Time.deltaTime);

    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision != null)
        {
            Destroy(gameObject);
        }
    }
}
