using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private Vector2 moveDirection;
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigidbody2;
    private ObjectPool<Projectile> pool;
    [SerializeField]
    private float range;
    private Vector2 originPosition;
    // Start is called before the first frame update
    void Awake()
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
        originPosition = rigidbody2.position;
	}
    public void SetPosition(Vector2 position)
    {
        rigidbody2.position = position;
    }
	private void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
			rigidbody2.MovePosition(rigidbody2.position + moveDirection * moveSpeed * Time.deltaTime);
            if ((rigidbody2.position - originPosition).magnitude >= range)
            {
                pool.Release(this);
            }
        }

    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision != null)
        {
            pool.Release(this);
        }
    }
    public void PlaceInPool(ObjectPool<Projectile> pool)
    {
        this.pool = pool;
    }
}
