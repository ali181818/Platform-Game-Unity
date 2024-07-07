using UnityEngine;
using System.Collections;

public class Enemy_Attack_Controller : MonoBehaviour {
	Animator anim;
	public Transform spawner;//Gun point for instantiate bullets
	public GameObject Bullet_Prefab;
	public int Bullet_speed = 10;
	public int Shoot_Delay = 50;//Time between shots
	private Vector2 LocalPos;
	private bool Idle_sw=false;
	private int cont=0;
	private int b_count=0;
	public bool has_target=false;

	void Start () {
		Init ();
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.name=="Bullet"){anim.SetBool("destroy",true);}
	}

	void FixedUpdate () {
		cont++;
		if(has_target==true){
			LaunchProjectile();
		}
		if ((anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))&&(cont>=20)) {
			cont=0;
			Idle_();
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Destroy")) {
			//Destroy (this.gameObject.transform.parent.gameObject);
			Destroy (this.gameObject);
		}
	}

	void Init(){
		LocalPos = new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.y);
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool("attack",false);
		anim.SetBool("destroy",false);
	}

	void Idle_(){
		Vector3 aux_ = new Vector3(this.gameObject.transform.position.x,0,0);
		Idle_sw = !Idle_sw;
		if(Idle_sw==true){
			aux_.y = LocalPos.y + 0.02f;
		}else{
			aux_.y = LocalPos.y - 0.02f;
		}
		this.gameObject.transform.position = aux_;
	}

	void LaunchProjectile(){
		if (b_count < Shoot_Delay) {
			b_count++;
			anim.SetBool ("attack", false);
		} else {
			anim.SetBool ("attack", true);
			Instantiate_Bullet();
			b_count = 0;
		}
	}

	void Instantiate_Bullet(){
		GameObject Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner.position.x,spawner.position.y,spawner.position.z), Quaternion.identity)as GameObject;
		Bullet.name="Bullet_enemy";
		Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_speed, 0f);
	}
}
