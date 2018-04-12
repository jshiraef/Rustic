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

    private Vector3 originalPosition;

    private SpriteRenderer sprite;

    public double threshold = 0;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("player");
        Camera = GameObject.Find("CM vcam1");

        originalPosition = this.transform.position;

        sprite = this.GetComponent<SpriteRenderer>();

        if (threshold == 0)
        {
            threshold = this.transform.position.y + 1.25f;
        }

    }

    // Update is called once per frame
    void Update()
    {

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        toggle = !toggle;

        //if (player.transform.position.y > threshold)
        //{
            foreground = true;
        //}
        //else foreground = false;

        // check to see if the camera is moving
        if(toggle)
        camLastPosition = Camera.GetComponent<CinemachineVirtualCamera>().transform.position;

        if(!toggle)
        camNextPosition = Camera.GetComponent<CinemachineVirtualCamera>().transform.position;          


        if (!(camLastPosition == camNextPosition) && sprite.isVisible && foreground)
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

        if (!sprite.isVisible && !(this.transform.position == originalPosition))
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime);
        }

        //if(this.name == "BigVillageHouse")
        //{
        //    Debug.Log("the original position is " + originalPosition);
        //    Debug.Log("the current position of the house is " + transform.position);
        //}
        

        //if (!sprite.isVisible)
        //{
        //    transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime);
        //}
    }

}
