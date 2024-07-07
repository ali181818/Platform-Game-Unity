using UnityEngine;
using System.Collections;

public class Switch_Elevator_Controller : MonoBehaviour {
	Animator anim;
	public bool activated = false;
	public int delay_after_switch=1;
	private Elevator_Controller Elevator_Script;
	// Use this for initialization
	void Start () {
		GameObject Base_Elevator = GameObject.Find("Base");
		Elevator_Script = Base_Elevator.GetComponent<Elevator_Controller>();
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool("activated",false);
		anim.SetBool("press_f",false);
		anim.SetBool("go_out",false);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if((other.name=="Jimmy_body")){
			anim.SetBool("press_f",true);
			anim.SetBool("go_out",false);
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if((other.name=="Billy_body")){
			anim.SetBool("press_f",false);
			anim.SetBool("go_out",true);
		}
	}

	void Update () {
		if(activated==false){
			anim.SetBool("activated",false);
		}else{
			anim.SetBool("activated",true);
		}
		if ((Input.GetKeyDown ("f"))&&(anim.GetBool("press_f")==true)) {
			Invoke("Call_Elevator",delay_after_switch);
		}
	}

	void Call_Elevator(){
		Elevator_Script.activated=true;
		activated = true;
	}
}
