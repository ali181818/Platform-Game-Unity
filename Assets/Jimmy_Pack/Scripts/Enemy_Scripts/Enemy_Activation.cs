using UnityEngine;
using System.Collections;

public class Enemy_Activation : MonoBehaviour {
	private Enemy_Attack_Controller Enemy;
	// Use this for initialization
	void Start () {
		Enemy = transform.parent.GetComponent<Enemy_Attack_Controller>();
		//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name=="Jimmy_body"){
			if(this.gameObject.name=="Area_In"){Enemy.has_target=true;}//Enemy will shoot
			if(this.gameObject.name=="Area_Out"){Enemy.has_target=false;}//Enemy will not shoot
		}
	}

	void Update () {

	}
}
