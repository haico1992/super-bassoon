#warning This thirdparty program contains old method call. Warning CS0618 is suppressed.
#pragma warning disable 618

//by Bob Berkebile : Pixelplacement : http://www.pixelplacement.com

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(iTweenPath))]
public class iTweenPathEditor : Editor
{
	iTweenPath _target;
	GUIStyle style = new GUIStyle();
	public static int count = 0;
	
	void OnEnable(){
		//i like bold handle labels since I'm getting old:
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		_target = (iTweenPath)target;
		
		//lock in a default path name:
		if(!_target.initialized){
			_target.initialized = true;
			_target.pathName = "New Path " + ++count;
			_target.initialName = _target.pathName;
		}
	}
	
	public override void OnInspectorGUI(){
		//draw the path
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Visible");
		_target.pathVisible = EditorGUILayout.Toggle(_target.pathVisible);
		EditorGUILayout.EndHorizontal();
		
		//Edit by Duc Anh 20 Jan 2012
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Handles In");
		_target.handlesIndex = EditorGUILayout.Popup(_target.handlesIndex,_target.handles);
		EditorGUILayout.EndHorizontal();
		//Edit by Duc Anh 20 Jan 2012
		
		//path name:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Name");
		_target.pathName = EditorGUILayout.TextField(_target.pathName);
		EditorGUILayout.EndHorizontal();
		
		if(_target.pathName == ""){
			_target.pathName = _target.initialName;
		}
		
		//path color:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Color");
		_target.pathColor = EditorGUILayout.ColorField(_target.pathColor);
		EditorGUILayout.EndHorizontal();
		
		//exploration segment count control:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Node Count");
		_target.nodeCount =  Mathf.Clamp(EditorGUILayout.IntSlider(_target.nodeCount, 2, 100), 2,100);
		EditorGUILayout.EndHorizontal();
		
		//add node?
		if(_target.nodeCount > _target.nodes.Count){
			for (int i = 0; i < _target.nodeCount - _target.nodes.Count; i++) {
				_target.nodes.Add(_target.nodes[_target.nodes.Count - 1]);	
			}
		}
	
		//remove node?
		if(_target.nodeCount < _target.nodes.Count){
			if(EditorUtility.DisplayDialog("Remove path node?","Shortening the node list will permantently destory parts of your path. This operation cannot be undone.", "OK", "Cancel")){
				int removeCount = _target.nodes.Count - _target.nodeCount;
				_target.nodes.RemoveRange(_target.nodes.Count-removeCount,removeCount);
			}else{
				_target.nodeCount = _target.nodes.Count;	
			}
		}
				
		//node display:
		EditorGUI.indentLevel = 4;
		for (int i = 0; i < _target.nodes.Count; i++) {
			_target.nodes[i] = EditorGUILayout.Vector3Field("Node " + (i+1), _target.nodes[i]);
		}
		
		//update and redraw:
		if(GUI.changed){
			EditorUtility.SetDirty(_target);			
		}
	}
	
	void OnSceneGUI(){
//		Debug.Log(Handles.);
		if(_target.pathVisible){			
			if(_target.nodes.Count > 0){
				//allow path adjustment undo:
				Undo.SetSnapshotTarget(_target,"Adjust iTween Path");
				
				//node handle display:
				for (int i = 0; i < _target.nodes.Count; i++) {
					//Start edited by Duc Anh 20 Jan 2012
					//world
					if (_target.handlesIndex == 0)
						_target.nodes[i] = Handles.PositionHandle(_target.nodes[i], Quaternion.identity);
					//local
					else
					{
						_target.nodes[i] = _target.transform.InverseTransformPoint(
	                    Handles.PositionHandle(_target.transform.TransformPoint(_target.nodes[i]), Quaternion.identity));
					}
					//End edited by Duc Anh 20 Jan 2012
				}
				
				//path begin and end labels:
				//world
				if (_target.handlesIndex == 0)
				{
					Handles.Label(_target.nodes[0], "'" + _target.pathName + "' Begin", style);
					Handles.Label(_target.nodes[_target.nodes.Count-1], "'" + _target.pathName + "' End", style);
				}
				//local
				else
				{
					Handles.Label(_target.transform.TransformPoint(_target.nodes[0]), "'" + _target.pathName + "' Begin", style);
					Handles.Label(_target.transform.TransformPoint(_target.nodes[_target.nodes.Count-1]), "'" + _target.pathName + "' End", style);
				}
			}	
		}
	}
}