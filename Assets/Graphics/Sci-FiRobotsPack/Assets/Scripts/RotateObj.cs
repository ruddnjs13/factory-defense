using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour {

	public GameObject rotateObj;
	public Vector3 rotateSpeed = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		rotateObj.gameObject.transform.Rotate (rotateSpeed * Time.deltaTime);
		
	}
}
