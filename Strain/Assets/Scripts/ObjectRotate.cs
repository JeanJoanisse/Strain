using UnityEngine;
using System.Collections;

public class ObjectRotate : MonoBehaviour {

	public float speed = 1;
	
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAroundLocal(Vector3.right, Time.deltaTime * speed);
	}
}
