using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ParallaxScroll : MonoBehaviour
{

    public GameObject player;
    public float v, h;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        if (player.transform.position.y > -75)
        {

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.Translate(h * .01f, 0, 0);
            }

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.Translate(h * .01f, 0, 0);
            }

            if (Input.GetAxisRaw("Vertical") < 0)
            {
                transform.Translate(0, v * .01f, 0);
            }

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                transform.Translate(0, v * .01f, 0);
            }

        }
    }

}
