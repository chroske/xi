using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MiniJSON;


public class EditorExWindow : EditorWindow
{
	// 変数
	string textArea = "";
	
	// method
	[MenuItem("Window/EditorEx")]
	static void Open()
	{
		EditorWindow.GetWindow<EditorExWindow>( "EditorEx" );
	}
	
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
		Dictionary<string,List<Dictionary<string,Dictionary<string, string>>>> objDic = new Dictionary<string,List<Dictionary<string,Dictionary<string, string>>>>();
		
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
		
		string beforeObjectName = null;
		List<Dictionary<string,Dictionary<string, string>>> compList = new List<Dictionary<string,Dictionary<string, string>>>();
		
		foreach(var componentData in allMonobehaviourList){
			
			Dictionary<string,Dictionary<string, string>> compDic = new Dictionary<string,Dictionary<string, string>>();
			Dictionary<string, string> valueDic = new Dictionary<string,string>();
			
			foreach(MemberInfo mi in componentData.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance |  BindingFlags.DeclaredOnly)){
				FieldInfo field = componentData.GetType().GetField(mi.Name);
				if(field != null){
					var value = field.GetValue(componentData);
					
					if(value != null){
						value = Json.Serialize(value);
						if(value != null){
							valueDic.Add(mi.Name.ToString(),value.ToString());
						}
					}
					
					//textArea += componentData.gameObject.name + " " + componentData.GetType() + " " + mi.Name + "=" + value + "\n";
				}
			}
			
			//コンポーネントを配列に突っ込んでからオブジェクトごとにディクショナリに突っ込む
			if(componentData.GetType() != null && valueDic != null){
				compDic.Add(componentData.GetType().ToString(),valueDic);
				
				if(beforeObjectName == null || componentData.gameObject.name.ToString() == beforeObjectName){
					compList.Add(compDic);
					objDic[componentData.gameObject.name.ToString()] = compList;
				} else {
					//初期化
					compList = new List<Dictionary<string,Dictionary<string, string>>>();
					compList.Add(compDic);
					
					objDic[componentData.gameObject.name.ToString()] = compList;
				}
				
				beforeObjectName = componentData.gameObject.name.ToString();
			}
		}
		
		textArea += Json.Serialize (objDic);
		//Debug.Log(Json.Serialize(objDic));
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