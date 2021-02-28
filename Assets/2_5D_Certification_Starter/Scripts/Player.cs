using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[SelectionBase]
public class Player : MonoBehaviour
{
    /* Custom Character controller Script 
     * Vars : 
     *      Speed, Gravity,Direction,Jump Height
     *      
     * In Update 
     *  if Grounded 
     *          Calcualte movement direction based on user input
     *  if Jump
     *          adjust Jump Mechanics
     *  Move
     *  
     */

    [Header("Movement Varibles")]
    [Range(0.0f,10.0f)]
    [SerializeField] private float _speed;
    [Range(-1.0f,2.0f)]
    [SerializeField] private float _gravityModifier;
    [Range(0f, 30.0f)]
    [SerializeField] private float _jumpHeight;

    private Vector3 _direction;
    private Vector3 _velcoity;
    private float _yVelocity;
    private CharacterController _cController;


	private void Update()
	{
        _cController = GetComponent<CharacterController>();
        if (_cController != null)
		{
            GetMovement();
		}
	}

    private void GetMovement()
	{
        if (_cController.isGrounded)
		{
            float horizontal = Input.GetAxis("Horizontal");
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
            _yVelocity -= _gravityModifier;
		}
        _velcoity.y = _yVelocity;
        _cController.Move(_velcoity * Time.deltaTime);
	}

    private void JumpAction()
	{
        _yVelocity = _jumpHeight;
	}
}
