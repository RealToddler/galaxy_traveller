﻿using UnityEngine;
using Photon.Pun;

// FlyBehaviour inherits from GenericBehaviour. This class corresponds to the flying behaviour.
public class FlyBehaviour : GenericBehaviour
{
	public PhotonView view;
	public string flyButton = "Fly";              // Default fly button.
	public float flySpeed = 4.0f;                 // Default flying speed.
	public float sprintFactor = 2.0f;             // How much sprinting affects fly speed.
	public float flyMaxVerticalAngle = 60f;       // Angle to clamp camera vertical movement when flying.

	private int flyBool;                          // Animator variable related to flying.
	private bool fly = false;                     // Boolean to determine whether or not the player activated fly mode.
	private CapsuleCollider col;                  // Reference to the player capsulle collider.

	// Start is always called after any Awake functions.
	void Start()
	{
		// Set up the references.
		flyBool = Animator.StringToHash("Fly");
		col = this.GetComponent<CapsuleCollider>();
		// Subscribe this behaviour on the manager.
		behaviourManager.SubscribeBehaviour(this);
		view = GetComponent<PhotonView>();
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		if (view.IsMine /*&& Debug.isDebugBuild*/ && !GetComponent<Player>().IsInAction)
		{
			// Toggle fly by input, only if there is no overriding state or temporary transitions.
			if (Input.GetButtonDown(flyButton) && !behaviourManager.IsOverriding() 
			                                   && !behaviourManager.GetTempLockStatus(behaviourManager.GetDefaultBehaviour))
			{
				fly = !fly;

				// Force end jump transition.
				behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);

				// Obey gravity. It's the law!
				behaviourManager.GetRigidBody.useGravity = !fly;

				// Player is flying.
				if (fly)
				{
					// Register this behaviour.
					behaviourManager.RegisterBehaviour(this.behaviourCode);
				}
				else
				{
					// Set collider direction to vertical.
					col.direction = 1;

					// Unregister this behaviour and set current behaviour to the default one.
					behaviourManager.UnregisterBehaviour(this.behaviourCode);
				}
			}

			// Assert this is the active behaviour
			fly = fly && behaviourManager.IsCurrentBehaviour(this.behaviourCode);

			// Set fly related variables on the Animator Controller.
			behaviourManager.GetAnim.SetBool(flyBool, fly);
		}
	}

	// This function is called when another behaviour overrides the current one.
	public override void OnOverride()
	{
		// Ensure the collider will return to vertical position when behaviour is overriden.
		col.direction = 1;
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		if (view.IsMine)
		{

			// Call the fly manager.
			FlyManagement(behaviourManager.GetH, behaviourManager.GetV);
		}
	}
	// Deal with the player movement when flying.
	void FlyManagement(float horizontal, float vertical)
	{
		// Add a force player's rigidbody according to the fly direction.
		Vector3 direction = Rotating(horizontal, vertical);
		behaviourManager.GetRigidBody.AddForce((direction * flySpeed * 100 * (behaviourManager.IsSprinting() ? sprintFactor : 1)), ForceMode.Acceleration);
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = Camera.main.transform.forward;
        
		// Player is moving on ground, Y component of camera facing is not relevant.
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, forward.y, -forward.x);
		Vector3 targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if (behaviourManager.IsMoving() && targetDirection != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, 0.1f);
			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		else if (!(Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}
}
