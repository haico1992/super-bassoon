// Copyright (c) 2010 Bob Berkebile
// Please direct any bugs/comments/suggestions to http://www.pixelplacement.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("iTweenPath")]
public class iTweenPath : MonoBehaviour
{
	public string pathName ="";
	public Color pathColor = Color.cyan;
	public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};
	public int nodeCount;
	public static Dictionary<string, iTweenPath> paths = new Dictionary<string, iTweenPath>();
	public bool initialized = false;
	public string initialName = "";
	public bool pathVisible = true;
	public string[] handles = new string[]{"World", "Local"};
	public int handlesIndex = 0;
	static int handlesStaticIndex = 0;
		
	void OnEnable(){
		if(!paths.ContainsKey(pathName)){
			paths.Add(pathName.ToLower(), this);
		}
	}
	
	void OnDisable(){
		paths.Remove(pathName.ToLower());
	}
	
	
	void OnDrawGizmosSelected(){
		if(pathVisible){
			if(nodes.Count > 0){
				//Start edited by Duc Anh 20 Jan 2012
				if (handlesIndex == 0)
					iTween.DrawPath(nodes.ToArray(), pathColor);
				else
				{
					Vector3[] globalPoints = new Vector3[nodes.Count];
		            for (int i = 0; i < nodes.Count; i++)
		            {
		                globalPoints[i] = gameObject.transform.TransformPoint(nodes[i]);
		            }
		
		            iTween.DrawPath(globalPoints, pathColor);
				}
				//End edited by Duc Anh 20 Jan 2012
			}	
		}
	}
	
	/// <summary>
	/// Returns the visually edited path as a Vector3 array.
	/// </summary>
	/// <param name="requestedName">
	/// A <see cref="System.String"/> the requested name of a path.
	/// </param>
	/// <returns>
	/// A <see cref="Vector3[]"/>
	/// </returns>
	public static Vector3[] GetPath(string requestedName){
		requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
			if (handlesStaticIndex != 0)
			{
				iTweenPath path = paths[requestedName];
		        Vector3[] globalPoints = new Vector3[path.nodes.Count];
		        for (int i = 0; i < path.nodes.Count; i++)
		        {
		            globalPoints[i] = path.gameObject.transform.TransformPoint(path.nodes[i]);
		        }
		
		        return globalPoints;
			}
			else
			{
				return paths[requestedName].nodes.ToArray();
			}
		}else{
			Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}
	
	/// <summary>
	/// Returns the reversed visually edited path as a Vector3 array.
	/// </summary>
	/// <param name="requestedName">
	/// A <see cref="System.String"/> the requested name of a path.
	/// </param>
	/// <returns>
	/// A <see cref="Vector3[]"/>
	/// </returns>
	public static Vector3[] GetPathReversed(string requestedName){
		requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
			List<Vector3>  revNodes = paths[requestedName].nodes.GetRange(0,paths[requestedName].nodes.Count);
			revNodes.Reverse();
			if (handlesStaticIndex != 0)
			{
				iTweenPath path = paths[requestedName];
				Vector3[] globalPoints = new Vector3[revNodes.Count];
				for (int i = 0; i < revNodes.Count; i++)
				{
					globalPoints[i] = path.gameObject.transform.TransformPoint(revNodes[i]);
				}
				return globalPoints;
			}
			return revNodes.ToArray();
		}else{
			Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}
	
	void Update(){
		handlesStaticIndex = handlesIndex;	
	}
}