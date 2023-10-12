using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public int id;
    private Vector3 moveDirection;
    private bool isSetable;
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigidbody2;
    private ObjectPool<Projectile> pool;
    [SerializeField]
    private float range;
    private Vector3 originPosition;
    // Start is called before the first frame update
    void Awake()
    {
    }
    public void PointToDirection(Vector3 direction)
    {
        direction.Normalize();
		//zÖáÐý×ªÖµ
		float zDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, zDegree);
        moveDirection = direction;
	}
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
        originPosition = transform.position;
    }

    private void FixedUpdate()
    {
		
		if (moveDirection != Vector3.zero)
        {
			transform.position = (transform.position + moveDirection * moveSpeed * Time.deltaTime);
            if ((transform.position - originPosition).magnitude >= range)
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
