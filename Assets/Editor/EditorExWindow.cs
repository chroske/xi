﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Json;  //json生成用


public class EditorExWindow : EditorWindow
{
	
	
	[MenuItem("Window/EditorEx")]
	static void Open()
	{
		EditorWindow.GetWindow<EditorExWindow>( "EditorEx" );
	}
	
	string textArea = "";
	private Vector2 scrollPosition = Vector2.zero;
	
	void OnGUI()
	{
		if( GUILayout.Button("Caputure") )
			Find();
		
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		
		GUILayout.Label( "TextArea" );
		textArea = GUILayout.TextArea( textArea );
		
		GUILayout.EndScrollView();	// スクロールバー終了
		
	}	 
	
	private void Find (){
		// TextAreaクリア
		textArea = "";
		
		List<GameObject> AllObjects =  new List<GameObject>();
		List<MonoBehaviour> allMonobehaviourList = new List<MonoBehaviour> ();
		
		foreach (GameObject obj in UnityEngine.Resources.FindObjectsOfTypeAll(typeof(GameObject)))
		{
			// アセットからパスを取得.シーン上に存在するオブジェクトの場合,シーンファイル（.unity）のパスを取得.
			string path = AssetDatabase.GetAssetOrScenePath(obj);
			// シーン上に存在するオブジェクトかどうか文字列で判定.
			bool isScene = path.Contains(".unity");
			// シーン上に存在するオブジェクトならば処理.
			if (isScene)
			{
				// GameObjectの名前を表示.
				AllObjects.Add (obj);
			}
		}
		
		allMonobehaviourList.AddRange (GetComponentsInList<MonoBehaviour> (AllObjects));
		
		foreach(var componentData in allMonobehaviourList){
			foreach(MemberInfo mi in componentData.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance |  BindingFlags.DeclaredOnly)){
				
				FieldInfo field = componentData.GetType().GetField(mi.Name);
				if(field != null){
					var value = field.GetValue(componentData);
					textArea +=  componentData.gameObject.name + " " + componentData.GetType() + " " + mi.Name + "=" + value + "\n";
				}
			}
		}
	}
	
	public static IEnumerable<T> GetComponentsInList<T> (IEnumerable<GameObject> gameObjects ) where T : Component 
	{
		var componentList = new List<T> ();
		
		foreach (var obj in gameObjects) {
			var components = obj.GetComponents<T> ();
			if (components != null) {
				foreach(var component in components){
					componentList.Add (component);
				}
			}
		}
		return componentList;
	}
}