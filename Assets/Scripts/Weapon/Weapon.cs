using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    Projectile bulletSpawn;
    private Vector3 pointDirection;

	[SerializeField]
	private Transform bulletPosition;
	private ObjectPool<Projectile> objectPool;
	public ObjectPool<Projectile> ObjectPool
	{
		get
		{
			if (objectPool == null)
			{
				objectPool = new ObjectPool<Projectile>(OnCreateBullet, OnGetBullet, OnTakeBullet, OnDestroyBullet);
			}
			return objectPool;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		pointDirection = Vector2.right;
	}
	private Projectile OnCreateBullet()
	{
		Projectile res = Instantiate(bulletSpawn.gameObject, bulletPosition.position, Quaternion.identity).GetComponent<Projectile>();
		if (res == null)
		{
			return null;
		}
		res.PlaceInPool(ObjectPool);
		return res;
	}
	private void OnGetBullet(Projectile bullet)
	{
		bullet.gameObject.SetActive(true);

	}
	private void OnTakeBullet(Projectile bullet)
	{
		bullet.gameObject.SetActive(false);
	}
	private void OnDestroyBullet(Projectile bullet)
	{
		Destroy(bullet.gameObject);
	}
	// Update is called once per frame


	public void PointToDirection(Vector3 pointDirection)
	{
		if(Vector3.Angle(this.pointDirection,pointDirection)> 2f)
		{
            this.pointDirection = pointDirection;
            pointDirection.Normalize();
            //z����תֵ
            float zDegree = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,zDegree);
        }
	}
	public void Fire()
	{
		Projectile bullets = ObjectPool.Get();
		bullets.SetPosition(bulletPosition.position);
		bullets.PointToDirection(pointDirection);

	}
}
