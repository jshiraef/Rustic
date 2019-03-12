using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntRootWhipAttack : MonoBehaviour
{


    private CircleCollider2D spinAttackHitPoint1;
    //private CircleCollider2D spinAttackHitPoint2;
    //private GameObject spinAttack2;
    private GruntRoot GruntRoot;

    private TrailRenderer whip1;
    //private TrailRenderer spin2;

    public float whipStartAngle;
    public float whipStartAngleOpposite;
    public float whipReach;

    // Use this for initialization
    void Start()
    {

        //spinAttack2 = transform.parent.Find("spinAttack2").gameObject;
        spinAttackHitPoint1 = this.GetComponent<CircleCollider2D>();
        //spinAttackHitPoint2 = spinAttack2.GetComponent<CircleCollider2D>();
        whip1 = this.GetComponent<TrailRenderer>();
        //spin2 = spinAttack2.GetComponent<TrailRenderer>();
        GruntRoot = transform.parent.GetComponent<GruntRoot>();

        whipStartAngle = 50f;
        whipStartAngleOpposite = whipStartAngle + 180;
        whipReach = 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (GruntRoot.getWhipAttack())
        {
            whipStartAngle += Mathf.Round(Time.deltaTime * 500);
            whipStartAngleOpposite += Mathf.Round(Time.deltaTime * 500);
            spinAttackHitPoint1.enabled = true;
            //spinAttackHitPoint2.enabled = true;
            whip1.enabled = true;
            //spin2.enabled = true;
        }
        else
        {
            spinAttackHitPoint1.enabled = false;
            //spinAttackHitPoint2.enabled = false;
            whip1.enabled = false;
            //spin2.enabled = false;
        }


        Vector3 whipAttackPointPosition = new Vector3(Mathf.Cos(whipStartAngle * Mathf.Deg2Rad) * whipReach, Mathf.Sin(whipStartAngle * Mathf.Deg2Rad) * whipReach, 0);
        //Vector3 spinAttackPointPosition2 = new Vector3(Mathf.Cos(whipStartAngleOpposite * Mathf.Deg2Rad) * spinReach, Mathf.Sin(whipStartAngleOpposite * Mathf.Deg2Rad) * spinReach, 0);
        this.transform.localPosition = whipAttackPointPosition;
        //spinAttack2.transform.localPosition = spinAttackPointPosition2;


    }
}
