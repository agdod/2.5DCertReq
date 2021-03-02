using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//[SelectionBase]
public class Player : MonoBehaviour
{
	[Header("Movement Varibles")]
	[Range(5.0f, 20.0f)]
	[SerializeField] private float _speed;
	[Range(-1.0f, 30.0f)]
	[SerializeField] private float _gravityModifier;
	[Range(0f, 30.0f)]
	[SerializeField] private float _jumpHeight;
	


	private Vector3 _direction;
	private Vector3 _velcoity;
	private float _yVelocity;
	private bool _isJumping;
	private bool _isFlipped;
	private Vector3 _snapToPostClimb;
	[SerializeField]
	private bool _isHanging;

	// Components
	private Animator _anim;
	private CharacterController _cController;

	/* 
	*  if Grounded 
	*          Calcualte movement direction based on user input
	*  if Jump
	*          adjust Jump Mechanics
	*  Move
	*  
	*/

	private void Start()
	{
		_cController = GetComponent<CharacterController>();

		_anim = GetComponentInChildren<Animator>();
		if (_anim == null)
		{
			Debug.LogError("No animator found.");
		}
	}

	private void Update()
	{

		if (_cController != null)
		{
			if (_isHanging)
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					_anim.SetTrigger("Climb");
				}
			}
			GetMovement();
		}
	}

	private void GetMovement()
	{

		if (_cController.isGrounded)
		{
			_isHanging = false;
			// Checked has already jumped - if grounded then has landed.
			if (_isJumping)
			{
				_isJumping = false;
				_anim.SetBool("IsJump", false);
			}
			float horizontal = Input.GetAxisRaw("Horizontal");
			_anim.SetFloat("Speed", Mathf.Abs(horizontal));
			if (horizontal < 0 && !_isFlipped)
			{
				transform.Rotate(0, 180, 0, Space.Self);
				_isFlipped = true;
			}
			else if (horizontal > 0 && _isFlipped)
			{
				transform.Rotate(0, 180, 0, Space.Self);
				_isFlipped = false;
			}
			_direction = new Vector3(0, 0, horizontal);
			_velcoity = _direction * _speed;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				JumpAction();
			}
		}
		else
		{
			// Not grounded fall to ground, if not hanging.
			if (!_isHanging)
			{
				_yVelocity += Physics.gravity.y * _gravityModifier * Time.deltaTime;
			}
			else if (_isHanging)
			{
				// is hanging dont fall! Stop all movement.
				_yVelocity = 0;
				_velcoity = Vector3.zero;
			}
			
		}
		_velcoity.y = _yVelocity;
		_cController.Move(_velcoity * Time.deltaTime);
	}

	private void JumpAction()
	{
		_anim.SetBool("IsJump", true);
		//_anim.SetFloat("Speed", 0.0f);
		_isJumping = true;
		_yVelocity = _jumpHeight;
	}

	public void ActivateLedgeGrab(Vector3 snapTo, LedgeChecker ledge)
	{
		// trigger the ledge grab animation
		_anim.SetBool("LedgeGrab",true);
		// Snap to the correct position, disbale Character controller to enable snapping.
		_cController.enabled = false;
		transform.position = snapTo;
		_cController.enabled = true;
		// Freeze the gravity.
		_isHanging = true;

		// REset variable and animation bools
		_isJumping = false;
		
		_snapToPostClimb = ledge.ClimbSnapTo;
	}

	public void SnapToPostClimb()
	{
		// Reset Parameters after climbing up
		_anim.SetBool("IsJump", false);
		_anim.SetFloat("Speed", 0.0f);

		Debug.Log("Disabled CController.");
		_cController.enabled = false;
		Debug.Log("Snap to position.");
		transform.position = _snapToPostClimb;
		Debug.Log("REenabled CController.");
		_cController.enabled = true;
		_isHanging = false;
		_isJumping = false;
		_anim.SetBool("LedgeGrab", false);
		// Standup
		//_anim.SetTrigger("StandUp");
	}
}
