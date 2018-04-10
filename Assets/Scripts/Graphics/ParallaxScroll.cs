using UnityEngine;
using System.Collections;
//using XInputDotNetPure;
using Cinemachine;

public class ParallaxScroll : MonoBehaviour
{

    public GameObject player;
    public GameObject Camera;
    public CinemachineVirtualCamera CinemachineCamera;
    public float v, h;

    public bool foreground;

    public bool background;

    private bool toggle;

    private Vector3 camLastPosition;
    private Vector3 camNextPosition;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("player");
        Camera = GameObject.Find("CM vcam1");
        
    }

    // Update is called once per frame
    void Update()
    {

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        toggle = !toggle;



        // check to see if the camera is moving
        if(toggle)
        camLastPosition = Camera.GetComponent<CinemachineVirtualCamera>().transform.position;

        if(!toggle)
        camNextPosition = Camera.GetComponent<CinemachineVirtualCamera>().transform.position;          


        if (player.transform.position.x < 70 && !(camLastPosition == camNextPosition))
        {

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.Translate(h * -.005f, 0, 0);
            }

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.Translate(h * -.005f, 0, 0);
            }

            if (Input.GetAxisRaw("Vertical") < 0)
            {
                transform.Translate(0, v * -.005f, 0);
            }

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                transform.Translate(0, v * -.005f, 0);
            }

        }
    }

}
