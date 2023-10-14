using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public int id;
    private Vector2 moveDirection;
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
        rigidbody2 = GetComponent<Rigidbody2D>();
    }
    public void PointToDirection(Vector3 direction)
    {
        direction.Normalize();
		//z轴旋转值
		float zDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler( 0,0, zDegree);
        moveDirection = direction;
	}
    //使用rigidbody2D进行移动，作为对象池对象时就不得不用transform瞬移,否则在不同帧就会出现闪烁
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
        originPosition = transform.position;
    }

    private void FixedUpdate()
    {
		
		if (moveDirection != Vector2.zero)
        {
            rigidbody2.MovePosition(rigidbody2.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
            //使用transform进行瞬移就要用transform进行判断
            if (Vector2.Distance(originPosition,transform.position) >= range)
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
