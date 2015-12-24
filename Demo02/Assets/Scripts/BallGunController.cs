using UnityEngine;
using System.Collections;

public class BallGunController : MonoBehaviour {


	public Transform BallPrefab;
	public float power;
	public Transform[] targets;
	public float BallPerSec;
	private Transform[] BallInstantList =  new Transform[25];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	double timer;

	void Update () {
		if (timer < Time.realtimeSinceStartup) {
			ShootBall ();
			timer = Time.realtimeSinceStartup + (1 / BallPerSec);
		}
	}

	int BallRotationCount=0;
	void ShootBall(){
		int ballIndex = BallRotationCount % BallInstantList.Length;
		if (BallInstantList [ballIndex] == null) {
			Transform ball=Instantiate (BallPrefab) as Transform;
			BallInstantList [ballIndex] = ball;
			ball.SetParent (this.transform);
		}
	
		BallInstantList [ballIndex].localPosition = Vector3.zero;
		int targetIndex = Random.Range (0, targets.Length);
		var target= BallInstantList [ballIndex].position-targets [Random.Range (0, targets.Length)].position;
		Debug.Log("target: "+ targetIndex);
		BallInstantList [ballIndex].GetComponent<BallScript> ().particle.gameObject.SetActive (false);
		BallInstantList [ballIndex].GetComponent<Rigidbody> ().mass = 0.1f;
		BallInstantList [ballIndex].GetComponent<Rigidbody> ().velocity = Vector3.zero;
		BallInstantList [ballIndex].GetComponent<Rigidbody> ().AddForce (-target.normalized * power);
	
		BallRotationCount++;
	}
}
