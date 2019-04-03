using UnityEngine;
using System.Collections;

public class CartController : MonoBehaviour {

    public bool isRunning;
    public bool isJumping;
    public bool isIdle;
    public bool grounded;
    public bool interact = false;
    public bool lockPosition; 

    public float speed = 1f;
    public float v, h;

    public float jumpCoolDown;

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
        setRunDirection();
        animationSetter();

    }

    void Movement()
    {

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        isRunning = false;

        anim.SetFloat("VerticalAnalogAxis", (Input.GetAxis("Vertical")));
        anim.SetFloat("HorizontalAnalogAxis", (Input.GetAxis("Horizontal")));

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }

            if (!lockPosition)
            {
                if (!isJumping)
                {
                    isRunning = true;
                    anim.Play("gallop");
                }

                transform.Translate(h * .015f, 0, 0);
            }

            isIdle = false;

        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {

            if (!lockPosition)
            {
                if (!isJumping)
                {
                    isRunning = true;
                    anim.Play("gallop");

                }

                transform.Translate(h * .015f, 0, 0);
            }

            isIdle = false;
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }


            if (!lockPosition)
            {
                if (!isJumping)
                {
                    isRunning = true;
                    anim.Play("gallop");
                }

                transform.Translate(0, v * .015f, 0);
            }

            isIdle = false;
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (isRunning == false)
            {
                anim.StopPlayback();
            }


            if (!lockPosition)
            {
                if (!isJumping)
                {
                    isRunning = true;
                    anim.Play("Running");

                }

                transform.Translate(0, v * .015f, 0);
            }

            isIdle = false;
        }

        // jumping stuff

        if (Input.GetButton("PS4_X"))
        {
            if(!isJumping)
            {
                jumpCoolDown = 1.35f;
            }

            isJumping = true;
            isIdle = false;
        }

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            if (!isJumping && !isIdle)
            {               
                    anim.Play("Idle02");

                    isIdle = true;              
            }
           
        }

        if (isJumping)
        {
            anim.Play("Jumping");
        }

        if (jumpCoolDown > 0)
        {
            jumpCoolDown -= Time.deltaTime;
        }
        
        if(isJumping && jumpCoolDown <= 0)
        {
            isJumping = false;
        }

        if (Input.GetButton ("PS4_Triangle"))
        {
            anim.Play("death");
        }

        Debug.Log("the horsecart's direction is " + this.direction);



    }


    void animationSetter()
    {

        switch ((int)this.direction)
        {
            case 0:
                anim.SetFloat("direction(float)", 0f);
                break;
            case 1:
                anim.SetFloat("direction(float)", (1f / 6f) + .01f);
                break;
            case 2:
                anim.SetFloat("direction(float)", (2f / 6f) + .01f);
                break;
            case 3:
                anim.SetFloat("direction(float)", (3f / 6f) + .01f);
                break;
            case 4:
                anim.SetFloat("direction(float)", (4f / 6f) + .01f);
                break;
            case 5:
                anim.SetFloat("direction(float)", (5f / 6f) + .01f);
                break;
        }

//        Debug.Log("the horsecart's direction(float) is " + anim.GetFloat("direction(float)"));

//        Debug.Log("the horsecart's direction is " + this.direction);
    }

    void setRunDirection()
    {
        // Set SouthEast
        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            if (v > -.95 && v < -.83 || h > .05 && h < .17)
            {
                this.direction = Direction.SouthEast50;

            }

            if (v > -.83 && v < -.7 || h > .17 && h < .3)
            {
                this.direction = Direction.SouthEast40;

            }

            if (v > -.7 && v < -.58 || h > .3 && h < .42)
            {
                this.direction = Direction.SouthEast30;

            }

            if (v > -.58 && v < -.45 || h > .42 && h < .55)
            {
                this.direction = Direction.SouthEast20;

            }

            if (v > -.45 && v < -.32 || h > .55 && h < .67)
            {
                this.direction = Direction.SouthEast10;

            }

            //if (v > -.32 && v < -.2 || h > .67 && h < .78)
            //{
            //    this.direction = Direction.SouthEast60;

            //    Debug.Log("it is now southeast 60");
            //}

            //if (v > -.2 && v < 0 || h > .78 && h < .9)
            //{
            //    this.direction = Direction.SouthEast60;

            //    Debug.Log("it is still southeast 60");
            //}

        }
    }

    void coolDownMaker(bool coolDownTrigger, float coolDown, float coolDownTime)
    {

        if (!coolDownTrigger)
            return;

        if (coolDown <= 0)
        {
            coolDown = coolDownTime;

            Debug.Log("at least this is happening");
        }

        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }

        //if (coolDown <= 0)
        //{
        //    coolDownTrigger = false;
        //}

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
