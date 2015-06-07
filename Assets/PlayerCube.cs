using UnityEngine;
using System.Collections;

public class PlayerCube : MonoBehaviour {
	const int TOP = 1;
	const int UP = 2;
	const int LEFT = 3;
	const int RIGHT = 4;
	const int DOWN = 5;
	const int BOTTOM = 6;


	bool upflag = false;
	bool downflag = false;
	bool rightflag = false;
	bool leftflag = false;
	bool movingflag = false;
	float childSize;

	float x;
	float y;
	float z;

	float time;

	GameObject child;

	int[] dice = new int[7];

	int mapX;
	int mapY;

	public int myPositionX;
	public int myPositionZ;

	CreateMap createMap;
	PlayerDiceManager playerDiceManager;
	public int myNum;

	// Use this for initialization
	void Start () {
		//child = transform.FindChild("Dice").gameObject;
		child = transform.FindChild("d6").gameObject;
		childSize = child.transform.lossyScale.x;

		dice[1] = 5; //top
		dice[2] = 1; //up
		dice[3] = 3; //left
		dice[5] = 6; //down
		dice[4] = 4; //right
		dice[6] = 2; //bottom

		GameObject createMapObj = GameObject.Find("createMap");
		createMap = createMapObj.GetComponent<CreateMap>();
		mapX = createMap.mapX;
		mapY = createMap.mapY;

		GameObject diceManagerObj = GameObject.Find("diceManager");
		playerDiceManager = diceManagerObj.GetComponent<PlayerDiceManager>();
    }

	void diceManage(int downKey) {
		int oldcCenter = dice[1];
		dice[1] = 7 - dice[downKey];
		dice[downKey] = oldcCenter;
		dice[7-downKey] = 7 - oldcCenter;
		//Debug.Log(dice[TOP]);
	}

	void getMainDiceParam() {
		//contorolle dice
		myNum = playerDiceManager.controlleDiceNum;
		
		myPositionX = playerDiceManager.dicePosition[myNum,0];
		myPositionZ = playerDiceManager.dicePosition[myNum,1];
	}
	

	// Update is called once per frame
	void Update () {
		if(upflag == true) {
			transform.Rotate(new Vector3(90, 0, 0) * 0.01f * 10);
			time += 1;
			if(time >= 100.0f / 10){
				upflag = false;
				movingflag = false;
				if(playerDiceManager.checkBreakDice(myNum)){
					Debug.Log ("deleteDice");
				}
			}
		}
		if(downflag == true){
			transform.Rotate(new Vector3(-90, 0, 0) * 0.01f * 10);
			time += 1;
			if(time >= 100.0f / 10){
				downflag = false;
				movingflag = false;
				if(playerDiceManager.checkBreakDice(myNum)){
					Debug.Log ("deleteDice");
				}
			}
		}
		if(rightflag == true){
			transform.Rotate(new Vector3(-90, 0, 0) * 0.01f * 10);
			time += 1;
			if(time >= 100.0f / 10){
				rightflag = false;
				movingflag = false;
				if(playerDiceManager.checkBreakDice(myNum)){
					Debug.Log ("deleteDice");
				}
			}
		}
		if(leftflag == true){
			transform.Rotate(new Vector3(-90, 0, 0) * 0.01f * 10);
			time += 1;
			if(time >= 100.0f / 10){
				leftflag = false;
				movingflag = false;
				if(playerDiceManager.checkBreakDice(myNum)){
					Debug.Log ("deleteDice");
				}
			}
		}

		if (Input.GetKeyDown("up") && movingflag == false) {
			GameObject diceManagerObj = GameObject.Find("diceManager");
			playerDiceManager = diceManagerObj.GetComponent<PlayerDiceManager>();
			//Debug.Log(playerDiceManager.controlleDiceNum);
			if(playerDiceManager.controlleDiceNum == myNum){
				getMainDiceParam();
				if(mapX > myPositionZ + 1 && playerDiceManager.checkAroundDice(myPositionX,myPositionZ+1,dice[UP])){
					movingflag = true;
					x = child.transform.position.x;
					y = child.transform.position.y;
					z = child.transform.position.z;

					//親との関係を切る
					child.transform.parent = null;
					
					//親の移動などの処理を書く……
					transform.position = new Vector3(x, y - (childSize), z + (childSize));
					transform.eulerAngles = new Vector3(0,0,0);

					//処理が終わってからまた戻す
					child.transform.parent = transform;

					upflag = true;
					time = 0;
					diceManage(UP);

					//contorolle dice
					myNum = playerDiceManager.controlleDiceNum;

					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = 0;

					//dice position update
					myPositionZ += 1;

					playerDiceManager.dicePosition[myNum,0] = myPositionX;
					playerDiceManager.dicePosition[myNum,1] = myPositionZ;
					
					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = dice[TOP];
				}
			}
		}

		if (Input.GetKeyDown("down") && movingflag == false) {
			if(playerDiceManager.controlleDiceNum == myNum){
				getMainDiceParam();
				if(0 < myPositionZ && playerDiceManager.checkAroundDice(myPositionX,myPositionZ-1,dice[UP])){
					movingflag = true;
					x = child.transform.position.x;
					y = child.transform.position.y;
					z = child.transform.position.z;
					
					//親との関係を切る
					child.transform.parent = null;
					
					//親の移動などの処理を書く……
					transform.position = new Vector3(x, y - (childSize), z - (childSize));
					transform.eulerAngles = new Vector3(0,0,0);
					
					//処理が終わってからまた戻す
					child.transform.parent = transform;
					
					downflag = true;
					time = 0;
					diceManage(DOWN);
					
					//contorolle dice
					myNum = playerDiceManager.controlleDiceNum;
					
					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = 0;
					
					//dice position update
					myPositionZ -= 1;
					
					playerDiceManager.dicePosition[myNum,0] = myPositionX;
					playerDiceManager.dicePosition[myNum,1] = myPositionZ;
					
					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = dice[TOP];
				}
			}
		}

		if (Input.GetKeyDown("right")  && movingflag == false) {
			if(playerDiceManager.controlleDiceNum == myNum){
				getMainDiceParam();

				if(mapY > myPositionX + 1 && playerDiceManager.checkAroundDice(myPositionX+1,myPositionZ,dice[UP])){
					movingflag = true;
					x = child.transform.position.x;
					y = child.transform.position.y;
					z = child.transform.position.z;
					
					//親との関係を切る
					child.transform.parent = null;
					
					//親の移動などの処理を書く……
					transform.position = new Vector3(x + (childSize), y - (childSize), z);
					transform.eulerAngles = new Vector3(0,-90,0);
					
					//処理が終わってからまた戻す
					child.transform.parent = transform;

					rightflag = true;
					time = 0;
					diceManage(RIGHT);

					//contorolle dice
					myNum = playerDiceManager.controlleDiceNum;

					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = 0;
					
					//dice position update
					myPositionX += 1;

					playerDiceManager.dicePosition[myNum,0] = myPositionX;
					playerDiceManager.dicePosition[myNum,1] = myPositionZ;
					
					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = dice[TOP];
				}
			}
		}
		if (Input.GetKeyDown("left") && movingflag == false) {
			if(playerDiceManager.controlleDiceNum == myNum){
				getMainDiceParam();
				if(0 < myPositionX && playerDiceManager.checkAroundDice(myPositionX-1,myPositionZ,dice[UP])){
					movingflag = true;
					x = child.transform.position.x;
					y = child.transform.position.y;
					z = child.transform.position.z;
					
					//親との関係を切る
					child.transform.parent = null;
					
					//親の移動などの処理を書く……
					transform.position = new Vector3(x - (childSize), y - (childSize), z);
					transform.eulerAngles = new Vector3(0,90,0);
					
					//処理が終わってからまた戻す
					child.transform.parent = transform;

					leftflag = true;
					time = 0;
					diceManage(LEFT);

					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = 0;
					
					//dice position update
					myPositionX -= 1;

					playerDiceManager.dicePosition[myNum,0] = myPositionX;
					playerDiceManager.dicePosition[myNum,1] = myPositionZ;
					
					playerDiceManager.diceNumberPosition[myPositionX,myPositionZ] = dice[TOP];
				}
			}
		}
	}
}
