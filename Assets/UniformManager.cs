using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformManager : MonoBehaviour {

    private Material mat;
    private bool isInteractive = true;
    private GameObject player;
    private GameObject camera;
    private Camera mainCamera;


	// Use this for initialization
	void Start () {
        mat = this.gameObject.GetComponent<Renderer>().material;
        player = GameObject.Find("player");
        camera = GameObject.Find("Main Camera");
        mainCamera = camera.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        //this is how you get an object's screen coordinates
        Vector3 playerScreenPosition = mainCamera.WorldToScreenPoint(player.transform.position);

        Vector3 mousePos = Input.mousePosition;
        float zValue = (isInteractive)  ? 1.0f : 0.0f;
        mat.SetVector("handPos1", new Vector4(playerScreenPosition.x, playerScreenPosition.y, zValue, 0.0f));
		
	}
}
