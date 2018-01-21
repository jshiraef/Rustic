using UnityEngine;
using System.Collections;

public class SmartDustTrail : MonoBehaviour {

    private Vector3 playerPosition;
    private Vector3 iconicPosition;
    public Direction playersDirection;
    public GameObject dustTrail;

	// Use this for initialization
	void Start () {
        //        playersDirection = GameObject.Find("player").GetComponent<PlayerControl>().getDirection();
        playerPosition = GameObject.Find("player").transform.localPosition;
//            dustTrail.transform.localPosition = iconicPosition;
	}
	
	// Update is called once per frame
	void Update () {
        //       dustTrail.transform.localPosition = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
//        dustTrail.transform.localPosition = new Vector3(playerPosition.x + 2, playerPosition.y + 5, playerPosition.z);
//        Debug.Log(playerPosition);
//        Debug.Log(dustTrail.transform.localPosition);
	}

    public enum Direction
    {
        EAST, NORTHEAST30, NORTHEAST50, NORTHEAST70, NORTH, NORTHWEST110, NORTHWEST130, NORTHWEST150, WEST, SOUTHWEST210, SOUTHWEST230, SOUTHWEST250, SOUTH, SOUTHEAST290, SOUTHEAST310, SOUTHEAST330, NULL
    }
}
