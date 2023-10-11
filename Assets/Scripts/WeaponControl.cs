using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentSortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;
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
		//zÖáÐý×ªÖµ
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
			Projectile bullets = Instantiate(bullet.gameObject, bulletPosition.position, Quaternion.identity).GetComponent<Projectile>();
			bullets.PointToDirection(pointDirection);

		}
	}
}
