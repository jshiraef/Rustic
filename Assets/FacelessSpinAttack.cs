using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacelessSpinAttack : MonoBehaviour {


    private CircleCollider2D spinAttackHitPoint1;
    private CircleCollider2D spinAttackHitPoint2;
    private GameObject spinAttack2;
    private FacelessController facelessControl;

    public float spinStartAngle;
    public float spinStartAngleOpposite;
    public float spinReach;

	// Use this for initialization
	void Start () {

        spinAttack2 = transform.parent.Find("spinAttack2").gameObject;
        spinAttackHitPoint1 = this.GetComponent<CircleCollider2D>();
        spinAttackHitPoint2 = spinAttack2.GetComponent<CircleCollider2D>();
        facelessControl = transform.parent.GetComponent<FacelessController>();

        spinStartAngle = 50f;
        spinStartAngleOpposite = spinStartAngle + 180;
        spinReach = 1.75f;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (facelessControl.getSpinAttack())
        {
            spinStartAngle += Mathf.Round(Time.deltaTime * 500);
            spinStartAngleOpposite += Mathf.Round(Time.deltaTime * 500);
            spinAttackHitPoint1.enabled = true;
            spinAttackHitPoint2.enabled = true;
        }
        else
        {
            spinAttackHitPoint1.enabled = false;
            spinAttackHitPoint2.enabled = false;
        }
        

        Vector3 spinAttackPointPosition = new Vector3(Mathf.Cos(spinStartAngle * Mathf.Deg2Rad) * spinReach, Mathf.Sin(spinStartAngle * Mathf.Deg2Rad) * spinReach, 0);
        Vector3 spinAttackPointPosition2 = new Vector3(Mathf.Cos(spinStartAngleOpposite * Mathf.Deg2Rad) * spinReach, Mathf.Sin(spinStartAngleOpposite * Mathf.Deg2Rad) * spinReach, 0);
        this.transform.localPosition = spinAttackPointPosition;
        spinAttack2.transform.localPosition = spinAttackPointPosition2;


	}
}
