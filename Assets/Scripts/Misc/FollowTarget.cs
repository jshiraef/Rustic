using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public GameObject target;

	public bool localize;
	public bool rotation;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update() {


		if (localize)
		{
			transform.position = target.transform.position;

			if (rotation)
			{
				transform.rotation = target.transform.rotation;
			}
		}
		else
		{
			transform.localPosition = target.transform.position;

			if (rotation)
			{
				transform.localRotation = target.transform.rotation;
			}

		}

	}
}
