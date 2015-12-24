using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public Transform particle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collide){
		if (collide.gameObject.CompareTag ("Weapon")) {

			Debug.Log (collide.gameObject.tag);
			this.GetComponent<Rigidbody> ().mass = 1;
			particle.gameObject.SetActive (true);
		};
	
	}
}
