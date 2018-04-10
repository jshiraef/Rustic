using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XInputDotNetPure;


public class ButtonNextLevel : MonoBehaviour
{

    private PlayerIndex playerIndex;

    public int sceneIndex;

    private bool rumble = false;
    private float rumbleCoolDown;

    public string whichLevel = "Hub Town";

    void Update()
    {
        if (rumble && rumbleCoolDown <= 0)
        {
            XInputDotNetPure.GamePad.SetVibration(playerIndex, 0, 0);
        }

        if (rumbleCoolDown > 0)
        {
            rumbleCoolDown -= Time.deltaTime;
        }
    }
    //public void NextLevelButton(int index)
    //{
    //    Application.LoadLevel(index);
    //}

    //public void NextLevelButton(string levelName)
    //{
    //    Application.LoadLevel(levelName);

    //    Debug.Log("this happened");
    //}

    //public void LoadLevelScene(string level)
    //{
    //    SceneManager.LoadScene(level);

    //}

    public void LoadLevelScene(int index)
    {
        SceneManager.LoadScene(index);

        rumble = true;
        XInputDotNetPure.GamePad.SetVibration(playerIndex, 1f, 0f);
        rumbleCoolDown = .3f;
    }

    void OnCollisionEnter2d(Collision2D coll)
    {
        SceneManager.LoadScene(whichLevel);

        Debug.Log("this definitely happened");

    }
}