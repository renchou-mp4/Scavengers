using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject[] OutWallArray;
	public GameObject[] FloorArray;
	public GameObject[] WallArray;
	public GameObject[] FoodArray;
	public GameObject[] EnemyArray;
	public GameObject Exit;
	public int MinWall = 2;
	public int MaxWall = 8;
	private GameManager gameManager;
	private Transform Map;
	private List<Vector2> position = new List<Vector2>();
	private List<Vector2> ExitPosition = new List<Vector2>();

	public int rows = 10;
	public int cols = 10;
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitMap()
    {
		gameManager = this.GetComponent<GameManager>();
		//创建围墙和地板
		Map = new GameObject("map").transform;
		for(int i = 0; i < cols; i++)
        {
			for(int j = 0;j<rows; j++)
            {
				if(i==0||j==0||i==cols-1||j==rows-1)
                {
					int index = Random.Range(0, OutWallArray.Length);
					GameObject go = GameObject.Instantiate(OutWallArray[index], new Vector3(i, j, 0), Quaternion.identity) as GameObject;
					go.transform.SetParent(Map);
				}
                else
                {
                    int index = Random.Range(0, FloorArray.Length);
					GameObject go = GameObject.Instantiate(FloorArray[index], new Vector3(i, j, 0), Quaternion.identity) as GameObject;
					go.transform.SetParent(Map);
				}
            }
        }


		//物品取到的随机范围
		position.Clear();
		for(int i=2;i<cols -2;i++)
        {
			for(int j =2; j <rows - 2;j++)
            {
				position.Add(new Vector2(i, j));
            }
        }

		//出口取到的随机范围
		ExitPosition.Clear();
		for(int i = 1; i<cols-1;i++)
        {
			for(int j=1;j<rows-1;j++)
            {
				if(i==1||j==1||i==cols-2||j==rows-2)
                {
					ExitPosition.Add(new Vector2(i, j));
                }
            }
        }

		//创建障碍物		
		int WallCount = Random.Range(MinWall, MaxWall);//随机墙的数量
		for (int i = 0; i < WallCount; i++)
        {
			//随机墙的位置
			Vector2 WallPosition = RandomPosition();
			//随机墙的类型
			GameObject go = RandomObject(WallArray, WallPosition);
			go.transform.SetParent(Map);
        }

		//创建食物
		int foodCount = Random.Range(1, gameManager.level + 1);
		for(int i = 0; i < foodCount; i++)
        {
			Vector2 FoodPosition = RandomPosition();
			GameObject go = RandomObject(FoodArray, FoodPosition);
			go.transform.SetParent(Map);
        }

		//创建敌人
		int enemyCount = Random.Range(1, gameManager.level / 2);
		for(int i = 0; i < enemyCount; i++)
        {
			Vector2 EnemyPosition = RandomPosition();
			GameObject go = RandomObject(EnemyArray, EnemyPosition);
			go.transform.SetParent(Map);
        }

		//创建出口
		int ExitIndex = Random.Range(1, ExitPosition.Count);
		Vector2 ExitPos = ExitPosition[ExitIndex];
		GameObject c = GameObject.Instantiate(Exit, ExitPos, Quaternion.identity) as GameObject;
		c.transform.SetParent(Map);
    }


	//取得一个随机位置
	private Vector2 RandomPosition()
    {
		int index = Random.Range(0, position.Count);
		Vector2 a = position[index];
		position.RemoveAt(index);
		return a;
    }

	//取得一个随机物品
	private GameObject RandomObject(GameObject[] itemArray,Vector2 position)
    {
		int index = Random.Range(0, itemArray.Length);
		GameObject a = GameObject.Instantiate(itemArray[index], position, Quaternion.identity);
		return a;
    }
}
