using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerHoldingType
{
	HoldingNothing,
	HoldingWeapon
}

public class PlayerMove : MonoBehaviour
{

    float horizontal;
    float vertical;
    [SerializeField]
    private float speed;
	//����ж��ķ���
    Vector2 direction;
	//�������ķ���
	Vector2 lookDirection;
    Rigidbody2D rgbody;
    Animator animator;
	//���λ��
	Vector3 mousePosition;
	//����ֳ���Ʒ������
	[SerializeField]
	PlayerHoldingType holdingType;
    void Start()
    {
        rgbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
	{
		HandleInput();
		Animate();

	}
	void TestPathFinding()
	{
		if (Input.GetMouseButtonDown(1))
		{
	
		}
	}
	private void HandleInput()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		direction = new Vector2(horizontal, vertical).normalized;

	}

	private void Animate()
	{
		switch (holdingType)
		{
			case PlayerHoldingType.HoldingNothing:
				lookDirection = direction;
				break;
			case PlayerHoldingType.HoldingWeapon:
				mousePosition = DealPosition.GetWorldMousePosition(transform.position.z);
				lookDirection = (mousePosition - transform.position).normalized;
				break;
		}
		if (lookDirection.magnitude > 0.1f)
		{
			animator.SetFloat("horizontal", lookDirection.x);
			animator.SetFloat("vertical", lookDirection.y);
		}
		animator.SetFloat("AxisSpeed", lookDirection.magnitude);
	}

	private void FixedUpdate()
	{
		PhysicMove();
	}

	private void PhysicMove()
	{
		rgbody.MovePosition(rgbody.position += direction * speed * Time.deltaTime);
	}
}
