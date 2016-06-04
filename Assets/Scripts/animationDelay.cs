using UnityEngine;
using System.Collections;

public class animationDelay : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.StartPlayback ();
		StartCoroutine ("waitForSeconds");
		anim.StopPlayback ();

	}
	
	// Update is called once per frame
	void Update () {


	}

	IEnumerator waitForSeconds(){
		yield return new WaitForSeconds (Random.Range (0, 10));
		print ("Boom! We just waited man!");
	}
}
