using UnityEngine;
using System.Collections;

public class animationDelay : MonoBehaviour {
	Animator anim;
    GameObject waterRipple;
    private int animationTimer;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
		//anim.StartPlayback ();
		//StartCoroutine ("waitForSeconds");
		//anim.StopPlayback ();
        

	}
	
	// Update is called once per frame
	void Update () {

        if(animationTimer > 200)
        {
            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, .05f, Time.deltaTime));
        }
        else
        {
            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, 1f, Time.deltaTime));
        }

        if(getAnimatorNormalizedTime() > .9)
        {
            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 0f);
        }




        if (animationTimer > 0)
        {
            animationTimer -= Mathf.RoundToInt(Time.deltaTime * 100);
        }

        if(animationTimer <= 0)
        {
            animationTimer = Random.Range(400, 600);
        }



        //Debug.Log("the animation timer is " + animationTimer);
        //Debug.Log("the animation playtime is " + getAnimatorNormalizedTime());
    }

	IEnumerator waitForSeconds(){
        anim.StopPlayback();
		yield return new WaitForSeconds (Random.Range (0, 10));
        anim.StartPlayback();
		print ("Boom! We just waited man!");
	}

    public bool animationHasPlayedOnce()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0);
    }

    public bool animatorIsPlaying(string stateName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public float getAnimatorNormalizedTime()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime - (int)anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
