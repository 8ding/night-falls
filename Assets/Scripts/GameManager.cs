using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
	
	public class GameManager : Singleton<GameManager>
	{
		private Dictionary<Vector3Int, Transform> cellposTransDec;
		public Transform obstacle;
		[SerializeField]
		private Tile wallTile;
		[SerializeField]
		private Tilemap baseTileMap;
		[SerializeField]
		private Transform shadowParent;
		//寻路器
		private PathFinder pathFinder;
		private int sizeHorizontal;
		private int sizeVerical;

		public int SizeVerical
		{
			get => sizeVerical;
		}
		public int SizeHorizontal
		{
			get => sizeHorizontal;
		}

		void Start()
		{
			sizeHorizontal = 50;
			sizeVerical = 50;
			pathFinder = new PathFinder();
			cellposTransDec = new Dictionary<Vector3Int, Transform>();
		}


		void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				Vector3Int cellPosition = GetCellFromWorld(DealPosition.GetWorldMousePosition(0));
				SetObstacle(cellPosition, wallTile);
			}
		}
		/// <summary>
		/// 获取路径
		/// </summary>
		/// <param name="startPosition"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public List<PathNode> GetPath(Vector3 startPosition, Vector3 targetPosition)
		{
			if (baseTileMap != null)
				return pathFinder.GetPath(startPosition, targetPosition);
			else
			{
				Debug.LogError("GetPath---TileMap数据丢失-----------------------------");
				return null;
			}
		}
		/// <summary>
		/// 从网格坐标转为世界坐标
		/// </summary>
		/// <param name="cellPosition"></param>
		/// <returns></returns>
		public Vector3 GetWorldFromCell(Vector3Int cellPosition)
		{
			return baseTileMap.GetCellCenterWorld(cellPosition);
		}
		/// <summary>
		/// 把世界坐标转为网格坐标
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public Vector3Int GetCellFromWorld(Vector3 worldPosition)
		{
			return baseTileMap.WorldToCell(worldPosition);
		}
		/// <summary>
		/// 清除地图颜色
		/// </summary>
		public void ClearColor()
		{
			for (int i = -1 * sizeHorizontal; i < sizeHorizontal; i++)
			{
				for (int j = -1 * sizeVerical; j < sizeVerical; j++)
				{
					Vector3Int cellPosition = new Vector3Int(i, j, 0);
					UnlockSetColor(cellPosition, Color.white);
				}
			}
		}
		/// <summary>
		///设置地图边界
		/// </summary>
		public void SetBounds()
		{
			for (int i = -1 * sizeHorizontal; i < sizeHorizontal; i++)
			{
				for (int j = -1 * sizeVerical; j < sizeVerical; j++)
				{
					Vector3Int cellPosition = new Vector3Int(i, j, 0);
					UnlockSetColor(cellPosition, Color.red);
					var res = baseTileMap.GetColor(cellPosition);
				}
			}
		}
		/// <summary>
		/// 解锁并设置地图瓦片颜色
		/// </summary>
		/// <param name="cellPosition">要设置的地图瓦片的网格位置</param>
		/// <param name="color">要设置的颜色</param>
		public void UnlockSetColor(Vector3Int cellPosition, Color color)
		{
			baseTileMap.RemoveTileFlags(cellPosition, TileFlags.LockColor);
			baseTileMap.SetColor(cellPosition, color);
		}
		/// <summary>
		/// 获取网格位置是否有障碍
		/// </summary>
		/// <param name="cellPostion"></param>
		/// <returns></returns>
		public bool IsHaveObstacle(Vector3Int cellPostion)
		{
			return cellposTransDec.ContainsKey(cellPostion);
		}
		/// <summary>
		/// 在网格位置设置障碍物
		/// </summary>
		/// <param name="cellPosition"></param>
		/// <param name="tile"></param>
		private void SetObstacle(Vector3Int cellPosition, Tile tile)
		{
			if (!cellposTransDec.ContainsKey(cellPosition))
			{
				Transform shadowObstacle = Instantiate(obstacle, GetWorldFromCell(cellPosition), Quaternion.identity);
				shadowObstacle.SetParent(shadowParent);
				cellposTransDec.Add(cellPosition, shadowObstacle);

			}

		}
	}
}