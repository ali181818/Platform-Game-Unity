using UnityEngine;
using System.Collections;
//Example Script using the animations included in the pack
//If you want use this script, the name of prefab must be default (Jimmy / Jimmy_body / .....)
public class Jimmy_Example_Motion_Controller : MonoBehaviour {
	//--Character movement
	private float maxspeed; //walk speed
	private float spy_maxspeed; //walk in spy mode
	Animator anim;
	private bool faceright; //face side of sprite activated
	private bool jumping=false;
	private bool isdead=false;
	//-- 
	private float Step_After_Climb = 0.5f;//It is used to put the character in plataform, if the character not climb correctly you must increase it.
	//--
	//--"Walking and shooting"
	private int layer=0;//0 unarmed, 1 armed, 2 shooting
	//--
	private Vector2 Climb_point;
	//--Aux vars
	private bool right_point;//used climbing
	private bool is_static;//stop_mode
	private bool is_static_jump;//jump in stop_mode
	private bool is_armed;
	private bool is_spy;//spy mode
	private float prev_y=0f;
	private float move;
	private GameObject Drone_Prefab;
	private bool near_switch=false;
	private bool is_rifle=false;
	//--
	//Character body data
	private int actual_step=0;
	private float knee_feet_distance=0f;
	//--
	//--Bullet
	public Transform spawner_gun_up;//Gun point for instantiate bullets
	public Transform spawner_gun_down;//Gun point for instantiate bullets
	public Transform spawner_Rifle_up;//Gun point for instantiate bullets
	public Transform spawner_Rifle_down;//Gun point for instantiate bullets
	public GameObject Bullet_Prefab;
	public GameObject Grenade_Prefab;
	public Transform spawner_grenade;
	public int Bullet_speed = 15;
	//--
	//About progress-bar --
	private int life = 100;//Remember that must be multiple of 20
	private float progress_bar;//Relative width
	private float normalize_constant;//Initial concordance between actual progress bar size and duration.
	private float initial_progress_bar_size;//Initial Progress bar width
	private int drone_width=0;
	private int drone_height=0;
	//--OnGUI Interface--
	void OnGUI() {
		if (((int)progress_bar > 0)) {
			Render_Colored_Rectangle (Get_Turret_position("x") - (int)(drone_width/2), Screen.height - Get_Turret_position("y") - drone_height + (int)((drone_height/4)*1.8f), (int)initial_progress_bar_size, (int)(drone_height/20), 0, 0, 0);//Black Progress Bar
			Render_Colored_Rectangle (Get_Turret_position("x") - (int)(drone_width/2), Screen.height - Get_Turret_position("y") - drone_height + (int)((drone_height/4)*1.8f), (int)progress_bar, (int)(drone_height/20), 1, 1, 1);//White Progress Bar
		}
	}
	//--End OnGUI
	
	void Start () {
		Get_Normalize_Constant();
		Normalize_ ();
		drone_width=Get_Turret_size("x");
		drone_height=Get_Turret_size("y");
		Setup_Character ();
	}
	
	void Setup_Character(){
		prev_y = this.gameObject.transform.position.y;
		is_static = false;
		is_static_jump = false;
		is_armed = false;//Cambiar capa de animaciones
		is_spy = false;
		near_switch=false;
		//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
		//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Environment"), LayerMask.NameToLayer("Environment"), true);
		knee_feet_distance=GameObject.Find("Jimmy_knee").transform.position.y - GameObject.Find("Jimmy_feet").transform.position.y;//Real distance, used to climb
		maxspeed=3f;//Set walk speed
		spy_maxspeed = maxspeed / 5;
		faceright=true;//Default right side
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool ("walk", false);//Walking animation is deactivated
		anim.SetBool ("dead", false);//Dying animation is deactivated
		anim.SetBool ("jump", false);//Jumping animation is deactivated
		anim.SetBool ("attack", false);//Shooting animation is deactivated
		anim.SetBool ("climb", false);//used climbing
		anim.SetBool ("step2", false);//used climbing
		anim.SetBool ("step3", false);//used climbing
		anim.SetBool ("step4", false);//used climbing
		anim.SetBool ("static_jump", false);//used climbing
		anim.SetBool ("equip", false);//equip gun
		//anim.SetBool ("spy", false);//spy mode
		anim.SetBool ("down", false);
		anim.SetBool ("attack", false);
		//anim.SetBool ("pilot", false);//air drone
		//anim.SetBool("shield",false);
		anim.SetBool ("beaten", false);
		anim.SetBool("pressing",false);//interact
		anim.SetBool("grenade" , false);
		//--
	}
	//--#### About Climbing ####--
	void Steps_Climbing (){
		this.GetComponent<Rigidbody2D>().isKinematic = true;
		//2º_step
		actual_step = 2;
		anim.SetBool ("step2", true);
		this.gameObject.transform.position = new Vector3 (Climb_point.x,Climb_point.y,0);
		//--
	}
	void Step_Controller(){
		
		if(actual_step==4){
			anim.SetBool ("step2", false);
			anim.SetBool ("step3", false);
			anim.SetBool ("step4", false);
			anim.SetBool ("climb", false);
			actual_step=0;
		}
		if ((actual_step > 0)) {
			//3º_Step
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Climbing.Climb_Step2")) {
				if(actual_step==2){
					anim.SetBool ("step3", true);
					this.gameObject.transform.position = new Vector3 (Climb_point.x,Climb_point.y+knee_feet_distance,0);
					actual_step=3;
				}
			}
			//--
			//4º_Step
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Climbing.Climb_Step3")) {
				anim.SetBool ("step4", true);
				if(actual_step==3){
					//--
					switch(right_point){
					case true:
						this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x+Step_After_Climb,this.gameObject.transform.position.y+knee_feet_distance+(knee_feet_distance),0);
						break;
					case false:
						this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x-Step_After_Climb,this.gameObject.transform.position.y+knee_feet_distance+(knee_feet_distance),0);
						break;
					}
					
					//--
					actual_step=4;
					this.GetComponent<Rigidbody2D>().isKinematic = false;
					jumping=false;
					anim.SetBool ("jump", false);
					anim.SetBool ("static_jump", false);
					is_static=false;
					anim.SetBool ("down", false);
				}
			}
			//--End climb_steps
		}
	}//--End About Climbing		
	
	void OnTriggerEnter2D(Collider2D other) {
		
		if ((is_static == true)) {
			//--Left Climb Point
			if((other.name=="Left_climb_point")){
				right_point=false;
				Climb_point.x = other.transform.position.x;
				Climb_point.y = other.transform.position.y;
				if (is_static_jump == true) {
					if(faceright==true){Flip();}
					is_static_jump = false;
					anim.SetBool ("climb", true);
					Steps_Climbing ();
				}
			}
			//--Right Climb Point
			if((other.name=="Right_climb_point")){
				right_point=true;
				Climb_point.x = other.transform.position.x;
				Climb_point.y = other.transform.position.y;
				if (is_static_jump == true) {
					if(faceright==false){Flip();}
					is_static_jump = false;
					anim.SetBool ("climb", true);
					Steps_Climbing ();
				}
			}
		}
		//--Switch elevator
		if((other.name=="Switch")){
			near_switch=true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		//--Switch elevator
		if((other.name=="Switch")){
			near_switch=false;
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		jumping = false;
		anim.SetBool ("jump", false);
		anim.SetBool ("static_jump", false);
		anim.SetBool ("attack", false);
		if(coll.gameObject.name=="Bullet_enemy"){
			life = life - 20;
			anim.SetBool ("beaten", true);
			Vector3 aux_ = new Vector3(this.gameObject.transform.parent.position.x-0.3f,this.gameObject.transform.parent.position.y,this.gameObject.transform.parent.position.z);
			this.gameObject.transform.parent.position = aux_;
			if(faceright==false){
				Flip();
			}
		}
	}
	
	void Update () {
		//---
		if(life>=0){
			if(life<=0){
				anim.SetBool("dead",true);
				this.gameObject.GetComponent<Collider2D>().enabled=false;
				this.gameObject.GetComponent<Rigidbody2D>().isKinematic=true;
				isdead=true;
			}
			Normalize_ ();
			anim.SetBool("grenade" , false);
			anim.SetBool("attack" , false);
		}
		//---
		if ((isdead == false)) {
			if((anim.GetBool("beaten")==true)&&(anim.GetCurrentAnimatorStateInfo (0).IsName ("Beaten"))){anim.SetBool("beaten",false);}
			//if((anim.GetBool("pilot")==true)&&(anim.GetCurrentAnimatorStateInfo (0).IsName ("Piloting"))){anim.SetBool("pilot",false);}
			//if((anim.GetBool("pilot")==true)&&(anim.GetCurrentAnimatorStateInfo (1).IsName ("Piloting"))){anim.SetBool("pilot",false);}
			// if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Stop")) {is_climbing=false;}
			if (anim.GetCurrentAnimatorStateInfo (1).IsName ("Gun_Reload")) {anim.SetBool("reload",false);}
			if (anim.GetCurrentAnimatorStateInfo (1).IsName ("Shooting")||(anim.GetCurrentAnimatorStateInfo (1).IsName ("Shooting_Down"))) {anim.SetBool ("attack", false);}
			if (anim.GetBool ("equip") == true) {
				Play_Equip_Gun(1);
			}
			if ((anim.GetBool("step2") == true)) {
				Step_Controller ();//Steps_Climbing controller
			} else {
				if (is_armed == false) {is_spy = false;}
				if ((Input.GetKeyUp ("g")) && (In_Stop()==true)) {Play_Grenade();}//Launching grenade
				if ((Input.GetKeyUp ("r")) && (In_Stop()==true)) {Play_Reload();}//Launching grenade
				if ((Input.GetKeyUp ("q")) && (is_armed == true)) {Play_Spy_Mode ();}//spy mode, need armed
				if (Input.GetKeyUp ("1") && (In_Stop()==true)) {Play_Equip_Gun (0);}//equip_gun
				if ((Input.GetKeyUp ("2"))&&(In_Stop()==true)) {Play_Equip_Gun (2);}
				if (Input.GetKey ("k")) {Play_Dying ();}//Dead 
				if ((Input.GetKeyDown ("f"))&&(In_Stop()==true)&&(near_switch==true)) {anim.SetBool("pressing",true);}
				if ((Input.GetKeyUp ("f"))&&(near_switch==true)) {anim.SetBool("pressing",false);}
				if ((Input.GetKeyDown ("s"))) {Play_Down (0);}// 0 Down, 1 up
				if ((Input.GetKeyUp ("s"))) {Play_Down (1);}
				if (Input.GetButtonDown ("Jump")&& (is_spy == false)&&anim.GetBool ("down") == false){Play_Jump ();}
				if (Input.GetMouseButtonDown(0)&&(jumping==false)&&(is_armed==true)&&(anim.GetBool ("static_jump") == false)){//cant move
					if((anim.GetCurrentAnimatorStateInfo (1).IsName ("Gun_Idle"))||(anim.GetCurrentAnimatorStateInfo (1).IsName ("Gun_Stop"))||(anim.GetCurrentAnimatorStateInfo (1).IsName ("Gun_Down"))){
						Instantiate_Gun_Bullet();
					}
				}
				if (Input.GetMouseButtonDown(0)&&(jumping==false)&&(is_rifle==true)&&(anim.GetBool ("static_jump") == false)){//cant move
					if((anim.GetCurrentAnimatorStateInfo (2).IsName ("Rifle_Idle"))||(anim.GetCurrentAnimatorStateInfo (2).IsName ("Rifle_Stop"))||(anim.GetCurrentAnimatorStateInfo (2).IsName ("Rifle_Down"))){
						Instantiate_Rifle_Bullet();
					}
				}
				if (((Input.GetMouseButtonDown(1))&&(In_Stop()==true))||((Input.GetMouseButton(1)))){//shield
					//anim.SetBool("shield",true);
					//Play_Shield(true);
				}
				if (Input.GetMouseButtonUp(1)){//shield
					//anim.SetBool("shield",false);
					//Play_Shield(false);
				}
				//####--About Walking--####
				if((anim.GetBool ("down") == false)){Speed_Fixed();}//In down mode: cant move, but can jump, flip and shoot
				move = Input.GetAxis ("Horizontal");//direction
				if (move != 0){Play_Walk();}
				if (move == 0) {//Stop
					anim.SetBool ("walk", false);
					//anim.SetBool ("spy", false);
					is_static = true;
				}			
				//####--End About Walking--####
			}
		}
	}
	
	bool In_Stop(){//Is really at stop mode?
		bool aux_=false;
		if((anim.GetCurrentAnimatorStateInfo (0).IsName ("Stop"))||(anim.GetCurrentAnimatorStateInfo (1).IsName ("Gun_Stop"))||(anim.GetCurrentAnimatorStateInfo (1).IsName ("Rifle_Stop"))||(anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle_"))||(anim.GetCurrentAnimatorStateInfo (1).IsName ("Idle"))){
			aux_=true;
		}
		return aux_;
	}
	
	void Flip(){
		faceright=!faceright;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void Set_Layer(int layer_){
		switch(layer_){
		case 0:
			anim.SetLayerWeight(1, 0f);
			anim.SetLayerWeight(2, 0f);
			break;
		case 1:
			anim.SetLayerWeight(2, 0f);
			anim.SetBool ("equip", true);
			break;
		case 2:
			anim.SetLayerWeight(1, 0f);
			anim.SetBool ("equip", true);
			break;
		}
	}
	
	void Play_Grenade(){
		if(is_armed==true||is_rifle==true){
			anim.SetBool("grenade" , true);
			Invoke("Instantiate_Grenade",0.3f);
		}
	}
	
	void Instantiate_Grenade(){
		GameObject Grenade =null;
		if(anim.GetBool("down")==true){
			Grenade = Instantiate(Grenade_Prefab, new Vector3(spawner_grenade.position.x,spawner_grenade.position.y,spawner_grenade.position.z), Quaternion.identity)as GameObject;
		}else{
			Grenade = Instantiate(Grenade_Prefab, new Vector3(spawner_grenade.position.x,spawner_grenade.position.y,spawner_grenade.position.z), Quaternion.identity)as GameObject;
		}
		Grenade.name="Grenade";
		if(faceright==true){
			Grenade.GetComponent<Rigidbody2D>().velocity = new Vector2(Bullet_speed/3, 0f);
		}else{
			Grenade.GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_speed/3, 0f);
		}
	}
	
	void Instantiate_Gun_Bullet(){
		anim.SetBool ("attack", true);
		GameObject Bullet=null;
		if(anim.GetBool("down")==true){
			Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner_gun_down.position.x,spawner_gun_down.position.y,spawner_gun_down.position.z), Quaternion.identity)as GameObject;
		}else{
			Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner_gun_up.position.x,spawner_gun_up.position.y,spawner_gun_up.position.z), Quaternion.identity)as GameObject;
		}
		
		Bullet.name="Bullet";
		if(faceright==true){
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Bullet_speed, 0f);
		}else{
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_speed, 0f);
		}
	}
	
	void Instantiate_Rifle_Bullet(){
		anim.SetBool ("attack", true);
		GameObject Bullet=null;
		if(anim.GetBool("down")==true){
			Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner_Rifle_down.position.x,spawner_Rifle_down.position.y,spawner_Rifle_down.position.z), Quaternion.identity)as GameObject;
		}else{
			Bullet = Instantiate(Bullet_Prefab, new Vector3(spawner_Rifle_up.position.x,spawner_Rifle_up.position.y,spawner_Rifle_up.position.z), Quaternion.identity)as GameObject;
		}
		
		Bullet.name="Bullet";
		if(faceright==true){
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Bullet_speed +(Bullet_speed/2), 0f);
		}else{
			Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_speed -(Bullet_speed/2), 0f);
		}
	}
	
	void Play_Reload(){
		if((is_armed==true)||(is_rifle==true)){anim.SetBool("reload",true); }
	}
	
	void Play_Shield(bool act){
		
	}
	
	void Play_Walk(){
		is_static = false;
		if((anim.GetBool ("down") == false)){
			if ((is_spy == true) && (is_armed == true)) {
				//anim.SetBool ("spy", true);
			} else {
				anim.SetBool ("walk", true);//Walking animation is activated
			}
		}else{
			anim.SetBool ("walk", false);
			//anim.SetBool ("spy", false);
		}
		if((move<0)&&(faceright==true)){Flip ();}
		if((move>0)&&(faceright==false)){Flip ();}
	}
	
	void Play_Jump(){
		if((jumping==false)&&(anim.GetBool ("static_jump") == false)){//only once time each jump
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,200));
			if(is_static){
				is_static_jump=true;
				anim.SetBool ("static_jump", true);
			}else{
				jumping=true;
				anim.SetBool ("jump", true);
			}
		}
	}
	
	void Play_Spy_Mode(){
		is_spy=!is_spy;
		if(is_spy==false){
			anim.SetBool ("spy", false);
		}else{
			anim.SetBool ("walk", false);
		}
	}
	void Play_Equip_Gun(int a){
		switch (a) {
		case 0:
			if(is_armed==true){
				layer=0;
				is_armed=false;
				is_rifle=false;
			}else{
				layer=1;
				is_armed=true;
				is_rifle=false;
			}
			Set_Layer(layer);
			break;
		case 1:
			anim.SetBool ("equip", false);
			if(is_armed==true){
				anim.SetLayerWeight(1, 1f);
			}
			if(is_rifle==true){
				anim.SetLayerWeight(2, 1f);
			}
			break;
		case 2:
			if(is_rifle==true){
				layer=0;
				is_rifle=false;
				is_armed=false;
			}else{
				layer=2;
				is_armed=false;
				is_rifle=true;
			}
			Set_Layer(layer);
			break;
		}
	}
	void Play_Down(int a){
		Vector2 aux_size = new Vector2 (0,0);
		Vector2 aux_center = new Vector2 (0,0);
		BoxCollider2D b = this.gameObject.GetComponent<Collider2D>() as BoxCollider2D;//for change the collider size if is in down mode
		switch (a) {
		case 0:
			aux_size = new Vector2 (0.28f,0.73f);
			aux_center = new Vector2 (-0.05f,-0.11f);
			anim.SetBool ("down", true);
			anim.SetBool ("walk", false);
			//anim.SetBool ("spy", false);
			break;
		case 1:
			aux_size = new Vector2 (0.28f,0.89f);
			aux_center = new Vector2 (-0.05f,-0.03f);
			anim.SetBool ("down", false);
			anim.SetBool ("walk", false);
			//anim.SetBool ("spy", false);
			break;
		}
		b.size = aux_size;
		b.offset = aux_center;
	}
	void Play_Dying(){
		anim.SetBool ("dead", true);
		isdead=true;
	}
	void Speed_Fixed(){
		if ((is_spy == true) && (is_armed == true)&&(anim.GetBool("shield")==false)) {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (move * spy_maxspeed, GetComponent<Rigidbody2D>().velocity.y);
		} else {
			if((anim.GetCurrentAnimatorStateInfo (0).IsName ("Pressing")==false)){
				if(is_rifle==true){
					GetComponent<Rigidbody2D>().velocity = new Vector2 ((move * maxspeed)/3, GetComponent<Rigidbody2D>().velocity.y);
				}else{
					GetComponent<Rigidbody2D>().velocity = new Vector2 (move * maxspeed, GetComponent<Rigidbody2D>().velocity.y);
				}
			}
		}
	}
	//####About Progress bar life
	//Turret relative position and size
	int Get_Turret_size(string var){//Return the width ("x") or height ("y") of this sprite relative to the screen
		int aux = 0;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		Renderer mesh = gameObject.GetComponent<SpriteRenderer>();
		Vector3 posStart = cam.WorldToScreenPoint(new Vector3(mesh.bounds.min.x, mesh.bounds.min.y, mesh.bounds.min.z));
		Vector3 posEnd = cam.WorldToScreenPoint(new Vector3(mesh.bounds.max.x, mesh.bounds.max.y, mesh.bounds.min.z));
		if (var == "x") { aux = (int)((posEnd.x - posStart.x)/2);}//Gameobject width
		if (var == "y") { aux = (int)(posEnd.y - posStart.y);}//Gameobject height
		
		return aux;
	}
	
	int Get_Turret_position(string var){//Get the turret position relative to the screen ("x") or ("y") 
		
		int aux = 0;
		Vector3 pos;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		pos = cam.WorldToScreenPoint(this.gameObject.transform.position);
		if (var == "x") {aux = (int)pos.x;}//Gameobject width
		if (var == "y") {aux = (int)pos.y;}//Gameobject height
		
		return aux;
	}
	void Normalize_(){//Relative width size of the progress bar
		progress_bar = normalize_constant * life;	
	}
	void Get_Normalize_Constant(){
		initial_progress_bar_size = Get_Turret_size("x");
		normalize_constant = initial_progress_bar_size / life;
	}
	//--
	
	void Render_Colored_Rectangle(int x, int y, int w, int h, float r, float g, float b)
	{
		Texture2D rgb_texture = new Texture2D(w, h);
		Color rgb_color = new Color(r, g, b);
		int i, j;
		for(i = 0;i<w;i++)
		{
			for(j = 0;j<h;j++)
			{
				rgb_texture.SetPixel(i, j, rgb_color);
			}
		}
		rgb_texture.Apply();
		GUIStyle generic_style = new GUIStyle();
		GUI.skin.box = generic_style;
		GUI.Box (new Rect (x,y,w,h), rgb_texture);
	}
	
	private IEnumerator WaitForAnimation ( Animation animation )
	{
		do
		{
			yield return null;
		} while ( animation.isPlaying );
	}
}
/*void Get_IsFalling(){//Is falling is not activated if is jumping or climbing. 
		if (this.gameObject.transform.position.y < prev_y) {
			Debug.Log("is falling");
		}
		prev_y = this.gameObject.transform.position.y;
	}*/