using UnityEngine;
using System.Collections;

public class Grenade_Example : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = this.gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if((anim.GetCurrentAnimatorStateInfo (0).IsName ("Explotion"))){
			this.gameObject.transform.rotation= Quaternion.identity;
			this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
		}
		if((anim.GetCurrentAnimatorStateInfo (0).IsName ("Destroy"))){
			Destroy (this.gameObject);
		}
	}
}
