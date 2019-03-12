using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    public bool interact = false;
    protected bool lockPosition = false;
    protected bool grounded;
    protected bool falling;
    

    // collision
    protected BoxCollider2D boxCollider2D;
    protected Rigidbody2D body;

    protected int collisionCount;
    protected int environmentCount;

    // movement
    protected bool left;
    protected bool right;
    protected bool up;
    protected bool down;
    protected bool southEast;
    protected bool southWest;
    protected bool northEast;
    protected bool northWest;

    // movement attributes
    protected float moveSpeed;
    protected float moveForce;
    protected float maxSpeed;
    protected float stopSpeed;
    protected float fallSpeed;
    protected float fallDistance;
    protected float maxVelocity;

    // animation
    protected Animator anim;
    protected int currentAction;
    protected int previousAction;
    protected bool facingRight;

    // direction
    public Direction direction;
    protected float rigidbodyAngularDirection;

    

    

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		

	}

    public enum Direction
    {
        EAST, NORTHEAST30, NORTHEAST50, NORTHEAST70, NORTH, NORTHWEST110, NORTHWEST130, NORTHWEST150, WEST, SOUTHWEST210, SOUTHWEST230, SOUTHWEST250, SOUTH, SOUTHEAST290, SOUTHEAST310, SOUTHEAST330, NULL
    }

    public void animationDirectionSetter()
    {

        switch ((int)this.direction)
        {
            case 0:
                anim.SetFloat("direction(float)", 0f);
                break;
            case 1:
                anim.SetFloat("direction(float)", (1f / 16f) + .01f);
                break;
            case 2:
                anim.SetFloat("direction(float)", (2f / 16f) + .01f);
                break;
            case 3:
                anim.SetFloat("direction(float)", (3f / 16f) + .01f);
                break;
            case 4:
                anim.SetFloat("direction(float)", (4f / 16f) + .01f);
                break;
            case 5:
                anim.SetFloat("direction(float)", (5f / 16f) + .01f);
                break;
            case 6:
                anim.SetFloat("direction(float)", (6f / 16f) + .01f);
                break;
            case 7:
                anim.SetFloat("direction(float)", (7f / 16f) + .01f);
                break;
            case 8:
                anim.SetFloat("direction(float)", (8f / 16f) + .01f);
                break;
            case 9:
                anim.SetFloat("direction(float)", (9f / 16f) + .01f);
                break;
            case 10:
                anim.SetFloat("direction(float)", (10f / 16f) + .01f);
                break;
            case 11:
                anim.SetFloat("direction(float)", (11f / 16f) + .01f);
                break;
            case 12:
                anim.SetFloat("direction(float)", (12f / 16f) + .01f);
                break;
            case 13:
                anim.SetFloat("direction(float)", (13f / 16f) + .01f);
                break;
            case 14:
                anim.SetFloat("direction(float)", (14f / 16f) + .01f);
                break;
            case 15:
                anim.SetFloat("direction(float)", (15f / 16f) + .01f);
                break;
        }
    }

    public enum RunDirection
    {
        EAST,
        NORTHEAST20, NORTHEAST30, NORTHEAST40, NORTHEAST50, NORTHEAST60, NORTHEAST70, NORTHEAST80,
        NORTH,
        NORTHWEST110, NORTHWEST120, NORTHWEST130, NORTHWEST140, NORTHWEST150, NORTHWEST160, NORTHWEST170,
        WEST,
        SOUTHWEST200, SOUTHWEST210, SOUTHWEST220, SOUTHWEST230, SOUTHWEST240, SOUTHWEST250, SOUTHWEST260,
        SOUTH,
        SOUTHEAST290, SOUTHEAST300, SOUTHEAST310, SOUTHEAST320, SOUTHEAST330, SOUTHEAST340, SOUTHEAST350,
        NULL
    }

    public void getNextPosition(float v, float h)
    {
        Vector3 rightMovement = Vector3.right * moveSpeed * Time.deltaTime * h;
        Vector3 upMovement = Vector3.up * moveSpeed * Time.deltaTime * v;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        if (!lockPosition)
        {
            transform.position += rightMovement;
            transform.position += upMovement;
        }
    }

    public void setKinematic()
    {
        this.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void setNonKinematic()
    {
        this.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public void setSpriteFlipX(bool b)
    {
        this.GetComponent < SpriteRenderer>().flipX = b;
    }

    public bool animatorIsPlaying(string stateName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public bool animationHasPlayedOnce()
    {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0);
    }

    public float getAnimatorNormalizedTime()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime - (int)anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public float getRigidbodyAngularDirection()
    {
        Vector2 moveDirection = body.velocity;
        if (moveDirection != Vector2.zero)
        {
            rigidbodyAngularDirection = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        }
        return rigidbodyAngularDirection;
    }

    public string getSpriteLayerName(SpriteRenderer sprite)
    {
        return sprite.sortingLayerName.ToString();
    }

    public int getSpriteSortingOrder(SpriteRenderer sprite)
    {
        return sprite.sortingOrder;
    }

    public Direction getDirection()
    {

        return direction;
    }

    public Direction getDirectionNSEW()
    {
        if (direction == Direction.SOUTHEAST290 || direction == Direction.SOUTHWEST250 || direction == Direction.SOUTH)
        {
            return Direction.SOUTH;
        }
        else if (direction == Direction.SOUTHEAST310 || direction == Direction.SOUTHEAST330 || direction == Direction.NORTHEAST30 || direction == Direction.NORTHEAST50 || direction == Direction.EAST)
        {
            return Direction.EAST;
        }
        else if (direction == Direction.NORTHWEST130 || direction == Direction.NORTHWEST150 || direction == Direction.SOUTHWEST210 || direction == Direction.SOUTHWEST230 || direction == Direction.WEST)
        {
            return Direction.WEST;
        }
        else if (direction == Direction.NORTHEAST70 || direction == Direction.NORTHWEST110 || direction == Direction.NORTH)
        {
            return Direction.NORTH;
        }
        else return Direction.NULL;

    }

    public void setDirection8()
    {
        up = direction == Direction.NORTH;

        down =  direction == Direction.SOUTH;

        right = direction == Direction.EAST;

        left =  direction == Direction.WEST;

        southEast = direction == Direction.SOUTHEAST310 || direction == Direction.SOUTHEAST290 || direction == Direction.SOUTHEAST330;

        southWest = direction == Direction.SOUTHWEST230 || direction == Direction.SOUTHWEST250 || direction == Direction.SOUTHWEST210;

        northWest = direction == Direction.NORTHWEST130 || direction == Direction.NORTHWEST110 || direction == Direction.NORTHWEST150;

        northEast = direction == Direction.NORTHEAST50 || direction == Direction.NORTHEAST70 || direction == Direction.NORTHEAST30;
    }

    public Direction getDirection8()
    {
        if(direction == Direction.SOUTHEAST310 || direction == Direction.SOUTHEAST330 || direction == Direction.SOUTHEAST290)
        {
            return Direction.SOUTHEAST310;
        }
        if ( direction == Direction.SOUTH)
        {
            return Direction.SOUTH;       
        }
        else if (direction == Direction.SOUTHWEST230 || direction == Direction.SOUTHWEST210 || direction == Direction.SOUTHWEST230 || direction == Direction.SOUTHWEST250) 
        {
            return Direction.SOUTHWEST230;
        }
        else if ( direction == Direction.EAST)
        {
            return Direction.EAST;           
        }
        else if(direction == Direction.NORTHWEST130 || direction == Direction.NORTHWEST150 || direction == Direction.NORTHWEST110)
        {
            return Direction.NORTHWEST130;
        }
        else if (direction == Direction.WEST)
        {
            return Direction.WEST;          
        }
        else if (direction == Direction.NORTHEAST50 || direction == Direction.NORTHEAST70 || direction == Direction.NORTHEAST30)
        {
            return Direction.NORTHEAST50;
        }
        else if ( direction == Direction.NORTH)
        {
            return Direction.NORTH;
        }

        else return Direction.NULL;
    }

   

    public void LookAt(Vector3 position)
    {
        Vector3 difference = position - this.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
}
