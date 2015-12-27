using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : NetworkBehaviour {


	public UILabel debugText01;
	public UILabel debugText02;
	public UILabel debugText03;
	public Transform avatar;
	public Transform avatar2;
	public Transform UIcamera;
	public float Ratio=10;
	private List<AccelerationEvent> sampleList =  new List<AccelerationEvent>();

	private Vector3 Origin;
	private Quaternion originRotation;
	private Vector3 output;
	public Quaternion deviceRotation;

	private NetworkClient myClient;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		GetMotionFixedUpdate();
	}

//	IEnumerator Stablelize(int frame){
//		int frameTime = frame;
//		for (int i = 0; i < frameTime; i++) {
//			var samples = Input.accelerationEvents;
//			sampleList.AddRange (samples);
//			yield return new WaitForEndOfFrame ();
//		}
//		Vector3 output = Vector3.zero;
//		for (int i = 0; i < sampleList.Count; i++) {
//			output += sampleList [i].acceleration;
//		}
//		output.x = output.x / sampleList.Count;
//		output.y = output.y / sampleList.Count;
//		output.z = output.z / sampleList.Count;
//		Origin = output;
//		ShowOrigin ();
//	}

	public void GetRootPosition( Quaternion root){
		avatar.transform.localRotation = root;
		avatar.parent.localRotation= Quaternion.Inverse(avatar.localRotation);
	}

//	void ShowOrigin ()
//	{
//		debugText01.text = "x= " + Origin.x;
//		debugText02.text = "y= " + Origin.y;
//		debugText03.text = "z= " + Origin.z;
//	}

//	public void OnClickStablelize(){
//		StartCoroutine (Stablelize(15));
//	}

	void GetMotionFixedUpdate(){		
		avatar.transform.localRotation = deviceRotation;
	}



}
