using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponControl : MonoBehaviour
{
    [SerializeField]
    Projectile bullet;
    SpriteRenderer spriteRenderer;
    private Vector3 pointDirection;
    private Vector3 mousePosition;
    int parentSortingOrder;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentSortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;
		
	}
	private Projectile OnCreateBullet()
	{
		Projectile res = Instantiate(bullet.gameObject, bulletPosition.position, Quaternion.identity).GetComponent<Projectile>();
		if (res == null)
		{
			Debug.LogError("没得到");
			return null;
		}
		res.PlaceInPool(ObjectPool);
		return res;
	}
	private void OnGetBullet(Projectile bullet)
	{
		bullet.gameObject.SetActive(true);
		bullet.SetPosition(bulletPosition.position);
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
	void Update()
	{
		PointToMouse();
		CheckFire();
	}

	private void PointToMouse()
	{
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pointDirection = new Vector3(mousePosition.x, mousePosition.y, transform.position.z) - transform.position;
		pointDirection.Normalize();
		//z轴旋转值
		float zDegree = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
		if (zDegree > -90f && zDegree < 90f)
			spriteRenderer.flipY = false;
		else
			spriteRenderer.flipY = true;
		if (zDegree > 45 && zDegree < 135)
			spriteRenderer.sortingOrder = parentSortingOrder - 1;
		else
			spriteRenderer.sortingOrder = parentSortingOrder + 1;
		transform.rotation = Quaternion.Euler(0, 0, zDegree);
	}
	private void CheckFire()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Projectile bullets = ObjectPool.Get();
			bullets.PointToDirection(pointDirection);
		}
	}
}
