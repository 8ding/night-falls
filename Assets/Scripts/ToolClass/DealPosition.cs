using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DealPosition 
{
	/// <summary>
	/// ��ȡ����Ϸ����ͬһƽ��������������λ��
	/// </summary>
	/// <param name="_z">��Ϸ�����z����ֵ������������ͬһx,yƽ��</param>
	/// <returns></returns>
	static public Vector3 GetWorldMousePosition(float _z)
	{
		Vector3 mousePositionCamera = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector3(mousePositionCamera.x, mousePositionCamera.y, _z);
	}
}
