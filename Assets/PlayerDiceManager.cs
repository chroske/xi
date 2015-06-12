using UnityEngine;
using System.Collections;

public class PlayerDiceManager : MonoBehaviour {

	public int controlleDiceNum;
	Component rotateDiceComponent;
	GameObject addComponentDice;
	GameObject removeComponentDice;
	
	GameObject cubeRotate;
	PlayerCube playerCube;
	CreateMap createMap;

	int mapX;
	int mapY;

	public int[,] dicePosition;
	public int[,] diceNumberPosition;

	// Use this for initialization
	void Start () {
		GameObject createMapObj = GameObject.Find("createMap");
		createMap = createMapObj.GetComponent<CreateMap>();
		mapX = createMap.mapX ;
		mapY = createMap.mapY;
		//0=X 1=Y
		dicePosition = new int[mapX * mapY,2];
		diceNumberPosition = new int[mapX,mapY];

		//demo dice position
		//dice1
		dicePosition[1,0] = 0;
		dicePosition[1,1] = 0;
		//dice2
		dicePosition[2,0] = 1;
		dicePosition[2,1] = 0;
		//dice3
		dicePosition[3,0] = 1;
		dicePosition[3,1] = 0;
		//dice4
		dicePosition[4,0] = 1;
		dicePosition[4,1] = 1;


		//dice1
		addComponentDice = GameObject.Find("cubeRotate1");
		rotateDiceComponent = addComponentDice.AddComponent<PlayerCube>();

		cubeRotate = GameObject.Find("cubeRotate1");
		playerCube = cubeRotate.GetComponent<PlayerCube>();
		playerCube.myPositionX = dicePosition[1,0];
		playerCube.myPositionZ = dicePosition[1,1];

		controlleDiceNum = 1;
		playerCube.myNum = 1;


		//dice2
		addComponentDice = GameObject.Find("cubeRotate2");
		rotateDiceComponent = addComponentDice.AddComponent<PlayerCube>();
		
		cubeRotate = GameObject.Find("cubeRotate2");
		playerCube = cubeRotate.GetComponent<PlayerCube>();
		playerCube.myPositionX = dicePosition[2,0];
		playerCube.myPositionZ = dicePosition[2,1];

		playerCube.myNum = 2;


		//dice4
		makeDice(1,1);
	}

	public void makeDice(int positionX,int positionY){
		if(diceNumberPosition[positionX,positionY] == 0){
			// プレハブを取得
			GameObject prefab = (GameObject)Resources.Load ("Prefabs/dice6");
			// プレハブからインスタンスを生成
			GameObject createdChildDice = Instantiate (prefab, new Vector3(4*positionX, 2.5f, 4+4*positionY), new Quaternion(0f,0f,0f,0f)) as GameObject;
			createdChildDice.name = "d6";

			GameObject cubeRotateObj = new GameObject();
			cubeRotateObj.name = "cubeRotate4";
			cubeRotateObj.transform.position = new Vector3(0, 0, 0);
			cubeRotateObj.transform.eulerAngles = new Vector3(0,0,0);

			//addComponent
			Component createdDiceComponent = cubeRotateObj.AddComponent<PlayerCube>();
			
			PlayerCube playerCube = createdDiceComponent.GetComponent<PlayerCube>();
			
			playerCube.myPositionX = dicePosition[4/*kahen*/,1];
			playerCube.myPositionZ = dicePosition[4/*kahen*/,1];

			//オブジェクトに親子関係を生成
			createdChildDice.transform.parent = cubeRotateObj.transform;
			
			playerCube.myNum = 4;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			//removeComponent
			//Object.Destroy(rotateDiceComponent);
			//addComponent
			//addComponentDice = GameObject.Find("cubeRotate1");
			//sousa key set
			controlleDiceNum = 1;

			//rotateDiceComponent = addComponentDice.AddComponent<PlayerCube>();
		}
		if (Input.GetKeyDown("2")) {
			//removeComponent
			//Object.Destroy(rotateDiceComponent);
			//addComponent
			//addComponentDice = GameObject.Find("cubeRotate2");
			//sousa key set
			controlleDiceNum = 2;

			//rotateDiceComponent = addComponentDice.AddComponent<PlayerCube>();

		}
		if (Input.GetKeyDown("3")) {
			//removeComponent
			//Object.Destroy(rotateDiceComponent);
			//addComponent
			//addComponentDice = GameObject.Find("cubeRotate2");
			//sousa key set
			controlleDiceNum = 3;
			
			//rotateDiceComponent = addComponentDice.AddComponent<PlayerCube>();
			
		}
		if (Input.GetKeyDown("4")) {
			//removeComponent
			//Object.Destroy(rotateDiceComponent);
			//addComponent
			//addComponentDice = GameObject.Find("cubeRotate2");
			//sousa key set
			controlleDiceNum = 4;
			
			//rotateDiceComponent = addComponentDice.AddComponent<PlayerCube>();
			
		}
	}

	//操作サイコロの前後左右チェック
	public bool checkBreakDice (int myNum){
		int playerDiceTopNum = diceNumberPosition[dicePosition[myNum,0],dicePosition[myNum,1]];

		if(diceNumberPosition.GetLength(0) > dicePosition[myNum,0]+1 && playerDiceTopNum == diceNumberPosition[dicePosition[myNum,0]+1,dicePosition[myNum,1]]) {
			return true;
		}
		if(dicePosition[myNum,0]-1 > 0 && playerDiceTopNum == diceNumberPosition[dicePosition[myNum,0]-1,dicePosition[myNum,1]]) {
			return true;
		}
		if(diceNumberPosition.GetLength(1) > dicePosition[myNum,1]+1 && playerDiceTopNum == diceNumberPosition[dicePosition[myNum,0],dicePosition[myNum,1]+1]) {
			return true;
		}
		if(dicePosition[myNum,1]-1 > 0 && playerDiceTopNum == diceNumberPosition[dicePosition[myNum,0],dicePosition[myNum,1]-1]) {
			return true;
		}

		return false;
	}

	//hokano saikoro naika check
	public bool checkAroundDice (int mainDicePositionX, int mainDicePositionY, int mainDiceNum){
		if(diceNumberPosition[mainDicePositionX,mainDicePositionY] == 0){
			return true;
		} else {
			return false;
		}
	}
}