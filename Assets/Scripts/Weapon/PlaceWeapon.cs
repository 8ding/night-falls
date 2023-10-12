using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceWeapon : MonoBehaviour, IShootable
{
	private NPCControl targetEnemy;
	private Weapon weapon;
	private float fireTime;
	[SerializeField]
	private float timeFire;
	public void CheckPointTo()
	{
		if (targetEnemy != null)
		{
			Vector3 direction = targetEnemy.transform.position - transform.position;
			weapon.PointToDirection(direction.normalized);
		}
	}

	public void CheckShoot()
	{
		if (targetEnemy != null)
		{
			fireTime += Time.deltaTime;
			if (fireTime >= timeFire)
			{
				weapon.Fire();
				fireTime = 0;
			}
		}
		else
		{
			fireTime = timeFire;
		}

	}

	// Start is called before the first frame update
	void Start()
    {
		targetEnemy = null;
		weapon = GetComponent<Weapon>();
		fireTime = timeFire;

	}

	// Update is called once per frame
	void Update()
    {
		CheckPointTo();
		CheckShoot();
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (targetEnemy == null)
		{
			targetEnemy = collision.GetComponent<NPCControl>();
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (targetEnemy != null)
		{
			NPCControl nPCControl = targetEnemy.GetComponent<NPCControl>();
			if (nPCControl != null && nPCControl == targetEnemy)
			{
				targetEnemy = null;
			}
		}
	}

}
