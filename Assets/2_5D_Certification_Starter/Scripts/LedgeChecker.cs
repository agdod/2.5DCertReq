using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeChecker : MonoBehaviour
{
	[Header("Snap To Position")]
	[SerializeField] private Transform _handSnapToPostion;
	[SerializeField] private Transform _postClimbSnapTo;

	public Vector3 ClimbSnapTo
	{
		get { return _postClimbSnapTo.position; }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag== "LedgeGrab")
		{
			// Activate Ledge grab animation.
			// Player is Parent of ledge Grab Checker
			Player player = other.GetComponentInParent<Player>();
			if(player != null)
			{
				player.ActivateLedgeGrab(_handSnapToPostion.position,this);
			}
			
		}
	}
}
