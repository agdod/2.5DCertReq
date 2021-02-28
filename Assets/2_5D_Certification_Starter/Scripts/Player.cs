using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//[SelectionBase]
public class Player : MonoBehaviour
{
	[Header("Movement Varibles")]
	[Range(10.0f, 20.0f)]
	[SerializeField] private float _speed;
	[Range(-1.0f, 2.0f)]
	[SerializeField] private float _gravityModifier;
	[Range(0f, 30.0f)]
	[SerializeField] private float _jumpHeight;


	private Vector3 _direction;
	private Vector3 _velcoity;
	private float _yVelocity;
	private bool _isJumping;
	private bool _isFlipped;

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
			GetMovement();
		}
	}

	private void GetMovement()
	{

		if (_cController.isGrounded)
		{
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
			// Not grounded fall to ground
			_yVelocity += Physics.gravity.y * _gravityModifier * Time.deltaTime;
		}
		_velcoity.y = _yVelocity;
		_cController.Move(_velcoity * Time.deltaTime);
	}

	private void JumpAction()
	{
		_anim.SetBool("IsJump", true);
		_isJumping = true;
		_yVelocity = _jumpHeight;
	}
}
