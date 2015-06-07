using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateMap : MonoBehaviour {
	public int mapX;
	public int mapY;

	// Use this for initialization
	void Start () {
		mapX = 5;
		mapY = 5;

		int mapPositionY = 0;
		int mapPositionZ = 0;

		for (int i = 0; i < mapX; i++) {
			int mapPositionX = 0;
			mapPositionZ += 4;
			for (int j = 0; j < mapY; j++) {
				GameObject floor = GameObject.CreatePrimitive (PrimitiveType.Cube);
				floor.name  = "floor" + i + "_" + j;
				floor.transform.Translate(mapPositionX, mapPositionY, mapPositionZ);
				floor.transform.localScale = new Vector3 (4, 1, 4);
				floor.GetComponent<Renderer>().material.color = Color.green;
				mapPositionX += 4;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
