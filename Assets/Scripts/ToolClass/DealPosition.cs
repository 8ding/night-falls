using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DealPosition 
{
	/// <summary>
	/// 获取与游戏物体同一平面的鼠标世界坐标位置
	/// </summary>
	/// <param name="_z">游戏物体的z坐标值，这样才能在同一x,y平面</param>
	/// <returns></returns>
	static public Vector3 GetWorldMousePosition(float _z)
	{
		Vector3 mousePositionCamera = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector3(mousePositionCamera.x, mousePositionCamera.y, _z);
	}
}
