using UnityEngine;
using System.Collections;

public class Elevator_Controller : MonoBehaviour {
	public bool up=false;
	public bool down=false;
	private bool active=false;
	public int delay=2;//Delay to start down
	private bool stop=true;
	public bool activated=false;//Used by switch controller
	private Switch_Elevator_Controller Switch_Script;
	// Use this for initialization
	void Start () {
		GameObject Sw_Base_Elevator = GameObject.Find("Switch");
		Switch_Script = Sw_Base_Elevator.GetComponent<Switch_Elevator_Controller>();
	}
	void OnTriggerEnter2D(Collider2D other) {
			if((other.name=="Limit_down")){
				active=false;
				down=false;
				stop=true;
				activated=false;
				Switch_Script.activated=false;
			}
			if((other.name=="Limit_up")){
				active=false;
				up=false;
				Invoke ("Down_",delay);
			}
	}

	void Update () {
		if ((activated==true)&&(stop==true)) {
			active=true;
			up=true;
			stop=false;
		}
		if(active==true){
			Move();
		}
	}

	void Move (){
		if(up==true){
			transform.Translate(0, Time.deltaTime, 0);
		}
		if(down==true){
			transform.Translate(0, -Time.deltaTime, 0);
		}
	}

	void Down_(){
		active=true;
		down=true;
	}
}
