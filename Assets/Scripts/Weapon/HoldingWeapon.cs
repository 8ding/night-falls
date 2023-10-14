using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HoldingWeapon : MonoBehaviour,IShootable
{
    private Weapon weapon;
	private Vector3 pointDirection;
	private SpriteRenderer spriteRenderer;
	private int parentSortingOrder;
    private void Start()
    {
        weapon = GetComponent<Weapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
		parentSortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;
        
    }
    public void CheckPointTo()
	{
		Vector3 mousePosition = DealPosition.GetWorldMousePosition(0);
		pointDirection = mousePosition - transform.position;
		weapon.PointToDirection(pointDirection);
	}
	/// <summary>
	/// 根据角度决定武器渲染层级
	/// </summary>
	private void CheckOrder()
	{
		float zDegree = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
		if (zDegree > -90f && zDegree < 90f)
			spriteRenderer.flipY = false;
		else
			spriteRenderer.flipY = true;
		if (zDegree > 45 && zDegree < 135)
			spriteRenderer.sortingOrder = parentSortingOrder - 1;
		else
			spriteRenderer.sortingOrder = parentSortingOrder + 1;
	}

	void Update()
	{
		CheckPointTo();
		CheckOrder();
		CheckShoot();

	}
    /// <summary>
    /// 检测是否射击
    /// </summary>
    public void CheckShoot()
	{
		if (Input.GetMouseButtonDown(0))
		{
			weapon.Fire();
		}
	}



}
