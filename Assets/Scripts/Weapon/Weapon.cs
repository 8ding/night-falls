using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    Projectile bulletSpawn;
	int idCount = 0;
    private Vector3 pointDirection;
	private Rigidbody2D rigidbody2;

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
		rigidbody2 = GetComponent<Rigidbody2D>();
		
	}
	private Projectile OnCreateBullet()
	{
		Projectile res = Instantiate(bulletSpawn.gameObject, bulletPosition.position, Quaternion.identity).GetComponent<Projectile>();
		if (res == null)
		{
			return null;
		}
		res.id = idCount;
		idCount++;
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
	
		this.pointDirection = pointDirection;
		pointDirection.Normalize();
		//zÖáÐý×ªÖµ
		float zDegree = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
		rigidbody2.rotation = zDegree;
	}
	public void Fire()
	{
		Projectile bullets = ObjectPool.Get();
		bullets.SetPosition(bulletPosition.position);
		bullets.PointToDirection(pointDirection);

	}
}
