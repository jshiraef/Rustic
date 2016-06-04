using UnityEngine;
using System.Collections;

public class CartController : MonoBehaviour {

    public bool isRunning;
    public bool grounded;
    public bool interact = false;
    public bool lockPosition;

    public float speed = 1f;
    public float v, h;

    public Direction direction;

    public Transform lineStart, lineEnd, groundedEnd;

    RaycastHit2D whatIHit;

    Animator anim;

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();

        this.direction = Direction.NULL;

    }

    // Update is called once per frame
    void Update () {

        Movement();
        Raycasting();

    }

    void Movement()
    {

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        isRunning = false;

 //       anim.SetFloat("VerticalAnalogAxis", (Input.GetAxis("Vertical")));
 //       anim.SetFloat("HorizontalAnalogAxis", (Input.GetAxis("Horizontal")));

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }

            if (!lockPosition)
            {
                isRunning = true;
                anim.Play("walkForward");

                transform.Translate(h * .015f, 0, 0);
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {

            if (!lockPosition)
            {
                isRunning = true;
                anim.Play("walkForward");


                transform.Translate(h * .015f, 0, 0);
            }
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }


            if (!lockPosition)
            {
                isRunning = true;
                anim.Play("walkForward");

                transform.Translate(0, v * .015f, 0);
            }
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }


            if (!lockPosition)
            {
                isRunning = true;
                anim.Play("Running");

                transform.Translate(0, v * .015f, 0);

            }
        }
    }

    void Raycasting()
    {
        if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Guard")))
        {
            whatIHit = Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer("Guard"));
            interact = true;
        }
        else
        {
            interact = false;
        }
    }

    public enum Direction
    {
        EAST, SouthEast10, SouthEast20, SouthEast30, SouthEast40, SouthEast50, SouthEast60, NULL
    }
}
