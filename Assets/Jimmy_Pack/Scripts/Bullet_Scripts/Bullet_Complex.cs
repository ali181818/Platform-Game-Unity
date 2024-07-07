using UnityEngine;
using System.Collections;

public class Bullet_Complex : MonoBehaviour {
	Animator anim;
	private bool faceright=false;//This example the Enemy is only at right of the screen.

	void Start () {
		Flip ();//This example the Enemy is only at right of the screen.
		anim = this.gameObject.GetComponent<Animator> ();
		Invoke ("AutoDestruct",3);//Bullet time life, default 3
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Disable_Collision ();
		Patch_position ();//Explotion animation need this patch
		//--
		anim.Play ("Explotion", -1, 0f);

	}
	void AutoDestruct(){
		if (this.gameObject != null) {
						Destroy (this.gameObject);
				}
	}
	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Destroy")) {
			if (this.gameObject != null) {
				Destroy (this.gameObject);
			}
		}
	}

	void Flip(){
		faceright=!faceright;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	float Get_Bullet_width(){//Return the width ("x") or height ("y") of this sprite relative to the screen
		float aux = 0;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		Renderer mesh = gameObject.GetComponent<SpriteRenderer>();
		aux = mesh.bounds.max.x -  mesh.bounds.min.x;

		return aux;
	}

	void Disable_Collision(){
		if (this.GetComponent<Rigidbody2D>().isKinematic == false) {
			this.GetComponent<Rigidbody2D>().isKinematic = !this.GetComponent<Rigidbody2D>().isKinematic;
			this.gameObject.GetComponent<Collider2D>().enabled = !this.gameObject.GetComponent<Collider2D>().enabled;
		}
	}

	void Patch_position(){//The animation is moved (sprite-width/2) to the left or right according to "faceright"
		Vector3 aux = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y,0);
		if (faceright) {
			aux.x = aux.x + (float)(Get_Bullet_width()/2);	
		} else {
			aux.x = aux.x - (float)(Get_Bullet_width()/2);	
		}
		this.gameObject.transform.position = aux;
	}
}
