using UnityEngine;
using System.Collections;

public class selector : MonoBehaviour {

	public bool selected = false;
	private Vector3 moveToDest = Vector3.zero;
	public float floorOffset = 1;
	public float speed = 10;
	public float stopDistOffset = 0.5f;
	
	// Update is called once per frame
	void Update () 
	{
	
		if(renderer.isVisible && Input.GetMouseButtonUp(0))
		{
			Vector3 camPos = Camera.mainCamera.WorldToScreenPoint(transform.position);
			camPos.y = RTSCam.InvertMouseY(camPos.y);
			selected = RTSCam.selection.Contains(camPos);
			if(selected)
			renderer.material.color = Color.red;
			else
				renderer.material.color = Color.white;
		}
		if(selected && Input.GetMouseButtonUp(1))
		{
			Vector3 destination = RTSCam.getDestination();
			
			if (destination != Vector3.zero)
			{
				moveToDest = destination;
				moveToDest.y += floorOffset;
			}
		}
		updateMove();
		
	}
	
	private void updateMove()
	{
		if(moveToDest != Vector3.zero && transform.position != moveToDest)
		{
			Vector3 direction = (moveToDest - transform.position).normalized;
			direction.y = 0;
			transform.rigidbody.velocity = direction * speed;
			
			if(Vector3.Distance(transform.position, moveToDest) < stopDistOffset)
			moveToDest = Vector3.zero;
			
		}
		else
			transform.rigidbody.velocity = Vector3.zero;
		
	}
}