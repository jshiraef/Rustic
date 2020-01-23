using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script will warp the player or object to another location in world space
public class WarpController : MonoBehaviour
{

    public GameObject newPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.transform.position = newPosition.transform.position;
    }

}
