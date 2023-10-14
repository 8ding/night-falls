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
		//z����תֵ
		float zDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler( 0,0, zDegree);
        moveDirection = direction;
	}
    //ʹ��rigidbody2D�����ƶ�����Ϊ����ض���ʱ�Ͳ��ò���transform˲��,�����ڲ�ͬ֡�ͻ������˸
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
            //ʹ��transform����˲�ƾ�Ҫ��transform�����ж�
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
