using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceCollision : MonoBehaviour {

    private Collider2D fenceCollider;

	// Use this for initialization
	void Start () {
        fenceCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "Player")
        {
            Debug.Log("this is happening now");
        }
    }
}
