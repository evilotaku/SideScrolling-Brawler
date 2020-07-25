using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class BasicMovementController : MonoBehaviour 
{

	int playerID = 0;
	public float deadzone = 0.1f;
	public float moveSpeed = 5f;

	//Player player;
	Vector3 moveVector, lookVector;
	CharacterController controller;
	Animator anim;

	public Vector3 heading;

	int Forward, Sideways;

	public bool isPaused = false;
	

	// Use this for initialization
	void Start () 
	{
		//if(!isLocalPlayer) return;
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		///player = ReInput.players.GetPlayer(playerID);	

		Forward = Animator.StringToHash("Forward");
		Sideways = Animator.StringToHash("Sideways");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isPaused) return;
		moveVector = new Vector3(Input.GetAxis("Horizontal"), 0.0f ,Input.GetAxis("Vertical"));
		lookVector = new Vector3(Input.GetAxis("Mouse X"), 0.0f , Input.GetAxis("Mouse Y"));

		if(moveVector.sqrMagnitude > deadzone) 
		{
			controller.Move(moveVector * moveSpeed * Time.deltaTime);
		}

		if(lookVector.sqrMagnitude > deadzone)
		{
			transform.rotation = Quaternion.LookRotation(lookVector);
		}

		heading = transform.InverseTransformDirection(controller.velocity);	
		anim.SetFloat(Forward, heading.z);
		anim.SetFloat(Sideways, heading.x);		

	}
}
