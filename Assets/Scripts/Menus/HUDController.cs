﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private GameObject fullHealthPie, halfHealthPie, quarterPie, threeQuarterPie;
    private GameObject stamina;
    private GameObject health;
    private GameObject itemCompass;
    private GameObject selectShine;
    private PlayerControl player;
    private int playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<PlayerControl>();
        playerHealth = player.getCurrentHealth();

        fullHealthPie = transform.Find("Strawberry Pie whole").gameObject;
        halfHealthPie = transform.Find("Strawberry Pie 1_2").gameObject;
        quarterPie = transform.Find("Strawberry Pie 1_4").gameObject;
        threeQuarterPie = transform.Find("Strawberry Pie 3_4").gameObject;

        stamina = transform.Find("staminaVial").gameObject.transform.Find("staminaMeter").gameObject;
        health = transform.Find("health").gameObject;
        itemCompass = transform.FindChild("itemCompass").gameObject;
        selectShine = itemCompass.transform.GetChild(0).gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.getCurrentHealth() > 0 && health.transform.childCount <= 0)
        {
            for (int i = 0; i < player.getCurrentHealth()/4; i++)
            {               
                GameObject newPie = Instantiate(fullHealthPie, health.transform);
                newPie.transform.position = new Vector3(fullHealthPie.transform.position.x + (i * 2), fullHealthPie.transform.position.y, 0);
                newPie.SetActive(true);              
            }

            if (player.getCurrentHealth() % 4 == 3)
            {
                GameObject newPie = Instantiate(threeQuarterPie, health.transform);
                newPie.transform.position = new Vector3(fullHealthPie.transform.position.x + ((player.getCurrentHealth()/4) + 1 * 2), fullHealthPie.transform.position.y, 0);
                newPie.SetActive(true);
            }
            else if (player.getCurrentHealth() % 4 == 2)
            {
                GameObject newPie = Instantiate(halfHealthPie, health.transform);
                newPie.transform.position = new Vector3(fullHealthPie.transform.position.x + ((player.getCurrentHealth() / 4) + 1 * 2), fullHealthPie.transform.position.y, 0);
                newPie.SetActive(true);
            }
            else if (player.getCurrentHealth() % 4 == 1)
            {
                GameObject newPie = Instantiate(quarterPie, health.transform);
                newPie.transform.position = new Vector3(fullHealthPie.transform.position.x + ((player.getCurrentHealth() / 4) + 1 * 2), fullHealthPie.transform.position.y, 0);
                newPie.SetActive(true);
            }
            playerHealth = player.getCurrentHealth();
        }

        if (player.getCurrentHealth() != playerHealth)
        {
            foreach(Transform child in health.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            Debug.Log("you're health is low! Do you have any potions?");

            if(health.transform.childCount <= 0)
            {
                playerHealth = player.getCurrentHealth();
            }
        }

        if(Input.GetAxis("PS4_DPadHorizontal") != 0 || Input.GetAxis("PS4_DPadVertical") != 0)
        {
            selectShine.SetActive(true);
        }


        if(Input.GetAxis("PS4_DPadHorizontal") > 0)
        {
            selectShine.transform.localPosition = new Vector3(2.23f, -.4f, 0);
            selectShine.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetAxis("PS4_DPadHorizontal") < 0)
        {
            selectShine.transform.localPosition = new Vector3(-2.54f, -.65f, 0);
            selectShine.transform.localRotation = Quaternion.Euler(0, 0, -180f);
        }
        else if (Input.GetAxis("PS4_DPadVertical") > 0)
        {
            selectShine.transform.localPosition = new Vector3(-.3f, 2.19f, 0);
            selectShine.transform.localRotation = Quaternion.Euler(0, 0, 90f);
        }
        else if (Input.GetAxis("PS4_DPadVertical") < 0)
        {
            selectShine.transform.localPosition = new Vector3(0f, -3.12f, 0);
            selectShine.transform.localRotation = Quaternion.Euler(0, 0, -90f);
        }
        else
        {
            selectShine.SetActive(false);
        }

        //Debug.Log("the playerHealth is " + playerHealth);
        //Debug.Log("the player's current stamina is " + player.getCurrentStamina());
        //Debug.Log("the player's max stamina is " + player.getMaxStamina());
        //Debug.Log("the stamina sprite size is " + stamina.GetComponent<SpriteRenderer>().size);


        if (player.getCurrentStamina() >= player.getMaxStamina())
        {
            stamina.GetComponent<SpriteRenderer>().size = new Vector2(player.getCurrentStamina(), 2);
        }
        else if(player.getCurrentStamina() < player.getMaxStamina())
        {
            stamina.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Lerp(stamina.GetComponent<SpriteRenderer>().size.x, player.getCurrentStamina(), Time.deltaTime * 2), 2);
        }

    }

    private void generatePlayerHealth()
    {
       
    }
}
