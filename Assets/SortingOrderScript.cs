using UnityEngine;
using System.Collections;

public class SortingOrderScript : MonoBehaviour 
{
	public const string OverlapLayer = "Overlap";
	public string currentLayerName;
	public int sortingOrder = 0;
	private SpriteRenderer sprite;
	public GameObject player;


	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<SpriteRenderer> ();
		player = GameObject.Find ("player");

		currentLayerName = sprite.sortingLayerName.ToString ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.transform.position.y > -9.5) 
		{
//			sprite.sortingOrder = sortingOrder;
			sprite.sortingLayerName = OverlapLayer;
		} 
		else
			sprite.sortingLayerName = currentLayerName;

		Debug.Log ("the sprite's current sorting layer is" + sprite.sortingLayerName);
		Debug.Log ("the player's x & y are" + player.transform.position.x + " , " + player.transform.position.y);
	}
}
