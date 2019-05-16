using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSquiggle : MonoBehaviour
{
    GameObject Player;
    PlayerControl player;
    protected SpriteRenderer sprite;

    private int squiggleCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("player");

        this.transform.position = new Vector3(Player.transform.position.x + .06f, Player.transform.position.y - 1.6f, 0);

        sprite = GetComponent<SpriteRenderer>();

        player = Player.GetComponent<PlayerControl>();

        squiggleCoolDown = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.getIsIdle())
        {
            FadeOut();
        }

        Debug.Log("the player animator time is " + player.getAnimatorNormalizedTime());


        if (sprite.color.a <= .11f)
        {
            Destroy(this.gameObject);

            Debug.Log("this happened!");
        }


        if (player.getIsRunning() && squiggleCoolDown <= 0)
        {
            GameObject newSquiggle = Instantiate(this.gameObject);
            newSquiggle.GetComponent<GrassSquiggle>().FadeIn();

            squiggleCoolDown = 200;
        }


        if (squiggleCoolDown > 0)
        {
            squiggleCoolDown -= Mathf.RoundToInt(Time.deltaTime * 100);
        }
    }

    void FadeOut()
    {
        Color tmpColor = sprite.color;
        sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, .1f, Time.deltaTime));

        
    }

    void FadeIn()
    {
        Color tmpColor = sprite.color;
        sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(tmpColor.a, 1f, Time.deltaTime * 2));
    }
}
