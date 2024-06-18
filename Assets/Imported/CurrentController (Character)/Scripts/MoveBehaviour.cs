using System;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using UnityEngine.InputSystem;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 1.0f;                   // Default run speed.
	public float sprintSpeed = 2.0f;                // Default sprint speed.
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
	private string _jumpButton = "Jump";              // Default jump button.
	private string _rollButton = "Roll";
	public float jumpHeight = 1.5f;                 // Default jump height.
	public float jumpIntertialForce = 10f;          // Default horizontal inertial force when jumping.

	public float _speed, _speedSeeker;               // Moving speed.
	
	private PhotonView _view;
	private int _jumpBool;                           // Animator variable related to jumping.
	private int _rollBool;
	private int _groundedBool;                       // Animator variable related to whether or not the player is on ground.
	private bool _jump;                              // Boolean to determine whether or not the player started a jump.
	private bool _roll;
	private bool _isColliding;                       // Boolean to determine if the player has collided with an obstacle.
	private bool _slippery;
	public bool canMove = true;
	
	// Start is always called after any Awake functions.
	void Start()
	{
		// Set up the references.
		_jumpBool = Animator.StringToHash("Jump");
		_groundedBool = Animator.StringToHash("Grounded");
		_rollBool = Animator.StringToHash("Roll");
		behaviourManager.GetAnim.SetBool(_groundedBool, true);

		// Subscribe and register this behaviour as the default behaviour.
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(behaviourCode);
		_speedSeeker = runSpeed;
		_view = GetComponent<PhotonView>();
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		SoundEffect();
		
		if (_view.IsMine && !GetComponent<Player>().IsInAction)
		{
			// Get jump input.
			if (Input.GetButtonDown(_jumpButton) && !_roll && !_jump && behaviourManager.IsCurrentBehaviour(behaviourCode) )
			{
				//SoundLibrary.Instance.PlaySound("Saut");
				
				_jump = true;
			}
			if ( !_roll && !_jump && Input.GetButtonDown(_rollButton)) 
			{
				_roll = true;
			}
		}
	}
	
	private void SoundEffect()
	{
		if (!IsGrounded() || _speed == 0)
		{
			AudioManager.Instance.Stop("Walk");
			AudioManager.Instance.Stop("Run");
			return;
		}
		
		if (_speed is > 0 and < 2f && !AudioManager.Instance.IsPlaying("Walk"))
		{
			AudioManager.Instance.Play("Walk"); 
			AudioManager.Instance.Stop("Run");
		}
		else if (_speed >= 2 && !AudioManager.Instance.IsPlaying("Run"))
		{
			AudioManager.Instance.Play("Run");
			AudioManager.Instance.Stop("Walk");
		}
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		if (_view.IsMine)
		{
			// Call the basic movement manager.
			MovementManagement(behaviourManager.GetH, behaviourManager.GetV);

			// Call the jump manager.
			JumpManagement();

			DoRoll();
		}
	}
	void DoRoll()
    {
		if (_roll)
		{
			behaviourManager.LockTempBehaviour(this.behaviourCode);
			behaviourManager.GetAnim.SetBool(_rollBool, true);
			_roll=false;
		} 
    }

	public void Bounce(float bounceForce)
	{
		behaviourManager.LockTempBehaviour(this.behaviourCode);
		behaviourManager.GetAnim.SetBool(_jumpBool, true);
		RemoveVerticalVelocity();
		float velocity = 2f * Mathf.Abs(Physics.gravity.y) * bounceForce;
		velocity = Mathf.Sqrt(velocity);
		behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
	}

	public bool Slippery { get; set; }
	
	// Execute the idle and walk/run jump movements.
	// ReSharper disable Unity.PerformanceAnalysis
	private void JumpManagement()
	{
		// Start a new jump.
		if (_jump && !behaviourManager.GetAnim.GetBool(_jumpBool) && behaviourManager.IsGrounded())
		{
			// Set jump related parameters.
			behaviourManager.LockTempBehaviour(this.behaviourCode);
			behaviourManager.GetAnim.SetBool(_jumpBool, true);
			// Is a locomotion jump?
			//if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
			//{
            // Temporarily change player friction to pass through obstacles.
            // GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
            // GetComponent<CapsuleCollider>().material.staticFriction = 0f;
            // Remove vertical velocity to avoid "super jumps" on slope ends.
            RemoveVerticalVelocity();
            // Set jump vertical impulse velocity.
            float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
            velocity = Mathf.Sqrt(velocity);
            behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
			//}
		}
		// Is already jumping?
		else if (behaviourManager.GetAnim.GetBool(_jumpBool))
		{
			// Keep forward movement while in the air.
			if (!behaviourManager.IsGrounded() && !_isColliding && behaviourManager.GetTempLockStatus())
			{
				behaviourManager.GetRigidBody.AddForce(transform.forward * (jumpIntertialForce * Physics.gravity.magnitude *  _speed * 1.33f), ForceMode.Acceleration);
			}
			// Has landed?
			if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
			{
				behaviourManager.GetAnim.SetBool(_groundedBool, true);
				// Change back player friction to default.
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
				// Set jump related parameters.
				_jump = false;
				behaviourManager.GetAnim.SetBool(_jumpBool, false);
				behaviourManager.UnlockTempBehaviour(this.behaviourCode);
			}
		}
	}

	public bool IsGrounded() => behaviourManager.IsGrounded();

	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical)
	{
		if (!canMove || !enabled)
		{
			return;
		}

		// On ground, obey gravity.
		if (behaviourManager.IsGrounded())
			behaviourManager.GetRigidBody.useGravity = true;

		// Avoid takeoff when reached a slope end.
		else if (!behaviourManager.GetAnim.GetBool(_jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
		{
			RemoveVerticalVelocity();
		}

		// Call function that deals with player orientation.
		Rotating(horizontal, vertical);

		if (GetComponent<Player>().IsAiming && horizontal == 0 && vertical == 0)
		{
			Rotating(0, 1);
		}

		// Set proper speed.
		Vector2 dir = new Vector2(horizontal, vertical);
		_speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		// This is for PC only, gamepads control speed via analog stick.
		_speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		_speedSeeker = Mathf.Clamp(_speedSeeker, walkSpeed, runSpeed);
		_speed *= _speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			//SoundLibrary.Instance.PlaySound("courir");
			_speed = sprintSpeed;
		}
		
		if (Slippery) behaviourManager.GetAnim.SetFloat(speedFloat, _speed*2, speedDampTime*5, Time.deltaTime);
		else behaviourManager.GetAnim.SetFloat(speedFloat, _speed, speedDampTime, Time.deltaTime);
	}

	// Remove vertical rigidbody velocity.
	private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
		horizontalVelocity.y = 0;
		behaviourManager.GetRigidBody.velocity = horizontalVelocity;
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = Camera.main.transform.forward;
        
		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if (/*behaviourManager.IsMoving() && */targetDirection != Vector3.zero)
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

	// Collision detection.
	private void OnCollisionStay(Collision collision)
	{
		_isColliding = true;
		// Slide on vertical obstacles
		// if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		_isColliding = false;
		GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
		GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
	}
}