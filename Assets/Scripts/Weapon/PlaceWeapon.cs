using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceWeapon : MonoBehaviour, IShootable
{
	private List<NPCControl> targetEnemys;
	private Weapon weapon;
	private float fireTime;
	[SerializeField]
	private float timeFire;
	public void CheckPointTo()
	{
		if (targetEnemys.Count > 0)
		{
			Vector3 direction = targetEnemys[0].transform.position - transform.position;
			weapon.PointToDirection(direction.normalized);
		}
	}

	public void CheckShoot()
	{
		if (targetEnemys.Count>0)
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
		targetEnemys = new List<NPCControl>();
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
		NPCControl nPCControl = collision.GetComponent<NPCControl>();
		if (nPCControl != null) 
		{
			//����������Χ��ע���������¼���һ��������վͼ���Ŀ���
			nPCControl.setInlight += setTarget;
            nPCControl.setOutlight += ReleaseTarget;
            if (nPCControl.IsInLight > 0)
			{ 
				//�������������Χ��Ҳͬʱ�Ѿ��ڹ��շ�Χ�������Ŀ���
				setTarget(nPCControl);
            }
        }

	}
	private void setTarget(NPCControl enemy)
	{
		if(!targetEnemys.Contains(enemy))
			targetEnemys.Add(enemy);
	}
	private void ReleaseTarget(NPCControl enemy)
	{
		if(targetEnemys.Contains(enemy))
			targetEnemys.Remove(enemy);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
        NPCControl nPCControl = collision.GetComponent<NPCControl>();
		//�е����뿪������Χ,�����֮ǰ�Ѿ���Ŀ��������Ƴ�ע���¼������Ƴ�Ŀ���
		if(nPCControl != null)
		{
            nPCControl.setInlight -= setTarget;
            nPCControl.setOutlight -= ReleaseTarget;
            ReleaseTarget(nPCControl);
		}
	}

}
