using UnityEngine;
using System.Collections;

public class TestAxes : MonoBehaviour {

	public GUIText controllerDebugGui;
	public bool checkL2;


	// Update is called once per frame
	void Update () 
	{

	}

	void OnGUI()
	{
		GUI.Box (new Rect (10, 10, 200, 900), 

		         "PS4 R1: " + Input.GetButton ("PS4_R1") + "\n"
		         + "PS4 L1: " + Input.GetButton ("PS4_L1") + "\n"
		         + "PS4 R2: " + Input.GetAxis ("PS4_R2") + "\n"
		         + "PS4 L2: " + Input.GetAxis ("PS4_L2") + "\n"
		         + "PS4 R3: " + Input.GetButton ("PS4_L3") + "\n"
		         + "PS4 L3: " + Input.GetButton ("PS4_R3") + "\n\n"


		         + "Left Analog Horizontal: " + Input.GetAxis ("Horizontal") + "\n"
		         + "Left Analog Vertical: " + Input.GetAxis ("Vertical") + "\n"
		         + "Right Analog Horizontal: " + Input.GetAxis ("PS4_RightAnalogHorizontal") + "\n"
		         + "Right Analog Vertical: " + Input.GetAxis ("PS4_RightAnalogVertical") + "\n\n"

		         + "PS4 X Button: " + Input.GetButton("PS4_X") + "\n"
		         + "PS4 O Button: " + Input.GetButton("PS4_O") + "\n"
		         + "PS4 Triangle Button: " + Input.GetButton ("PS4_Triangle") + "\n"
		         + "PS4 Sqr Button: " + Input.GetButton ("PS4_Square") + "\n\n"

		         + "PS4 Dpad Horizontal: " + Input.GetAxis ("PS4_DPadHorizontal") + "\n"
		         + "PS4 Dpad Vertical: " + Input.GetAxis ("PS4_DPadVertical") + "\n\n"

		         + "PS4 Share Button: " + Input.GetButton ("PS4_Share") + "\n"
		         + "PS4 Options Button: " + Input.GetButton ("PS4_Options") + "\n"
		         + "PS4 PSN Button: " + Input.GetButton ("PS4_PSN") + "\n"
		         + "PS4 Touch Button: " + Input.GetButton ("PS4_Touch") + "\n\n"

		         + "PS4 Virtual Analog: " + Input.GetButton ("VirtualLeftAnalog")

		         );
	}
}
