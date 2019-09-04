using UnityEngine;
using System.Collections;

public class SmartDustTrail : MonoBehaviour {

    private Vector3 playerPosition;
    private Vector3 iconicPosition;
    public Direction playersDirection;
    public GameObject dustTrail;

    private string angleString;
    private Vector2 playerAngle;

    private ParticleSystem dustEmitter;
    

	// Use this for initialization
	void Start () {
        playersDirection = (Direction)transform.parent.GetComponent<PlayerControl>().getDirection();
        playerPosition = transform.parent.transform.localPosition;
        iconicPosition = dustTrail.transform.localPosition;
        dustTrail = GameObject.Find("dustTrail");
        dustEmitter = dustTrail.GetComponent<ParticleSystem>();
        
    }
	
	// Update is called once per frame
	void Update () {

        //playersDirection = (Direction) transform.parent.GetComponent<PlayerControl>().getDirection();
        //if (playersDirection == Direction.SOUTH)
        //{
//            dustTrail.transform.localPosition = new Vector3(iconicPosition.x + Mathf.Cos(transform.GetComponentInParent<PlayerControl>().getAngularDirection()) * 2, iconicPosition.y - Mathf.Sin(transform.GetComponentInParent<PlayerControl>().getAngularDirection()) * 2);




        if (GetComponentInParent<PlayerControl>().getIsRunning())
        {
            if(!dustEmitter.isPlaying)
            {
                dustEmitter.Play();
            }
        }
        else
        {
            if(dustEmitter.isPlaying)
            {
                dustEmitter.Stop();
            }
            
        }

        //}

        //         dustTrail.transform.localPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
        //        dustTrail.transform.localPosition = new Vector3(playerPosition.x + 2, playerPosition.y + 5, playerPosition.z);
        //        Debug.Log(playerPosition);
        //        Debug.Log(dustTrail.transform.localPosition);
    }

    public enum Direction
    {
        EAST, NORTHEAST30, NORTHEAST50, NORTHEAST70, NORTH, NORTHWEST110, NORTHWEST130, NORTHWEST150, WEST, SOUTHWEST210, SOUTHWEST230, SOUTHWEST250, SOUTH, SOUTHEAST290, SOUTHEAST310, SOUTHEAST330, NULL
    }
}
