using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {

	public GameObject[] barrels;
	public bool hit = false;
	public bool broken = false;
	public float barrelDistanceToPlayer;


	Animator anim;
	public static int barrelTimer = 50;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (hit) {
			Debug.Log ("barrel was hit");
		}

		if (barrelTimer < 0) {
			anim.Play ("barrelBreak");
		}


	//	print ("the barrel's distance to player is " + distanceToPlayer);
	
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "weapon") {
			Debug.Log ("a hit");
//			Destroy(coll.gameObject);
//			Destroy(this.gameObject);

			hit = true;
		}
	}

	public static void barrelCountDown()
	{
		barrelTimer = barrelTimer - 5;
	}
	
}
