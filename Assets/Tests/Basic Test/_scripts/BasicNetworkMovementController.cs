using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
//using Rewired;

[RequireComponent(typeof(CharacterController))]
public class BasicNetworkMovementController : MonoBehaviour
{
	public float deadzone = 0.1f;
	public float moveSpeed = 5f;

	//Player player;
	Vector3 moveVector, lookVector;
	CharacterController controller;
	Animator anim;

	public Vector3 heading;

	int Forward, Sideways;

	Transform transform;
	

	// Use this for initialization
	void Start () 
	{
		//if(!isLocalPlayer) return;
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		transform = GetComponent<Transform>();
		//player = ReInput.players.GetPlayer(playerControllerId);	

		Forward = Animator.StringToHash("Forward");
		Sideways = Animator.StringToHash("Sideways");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if(!isLocalPlayer) return;
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
