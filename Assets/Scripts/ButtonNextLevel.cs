using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class ButtonNextLevel : MonoBehaviour
{
    public void NextLevelButton(int index)
    {
        Application.LoadLevel(index);
    }

    public void NextLevelButton(string levelName)
    {
        Application.LoadLevel(levelName);

        Debug.Log("this happened");
    }

    public void LoadLevelScene(string level)
    {
        EditorSceneManager.LoadScene(level);
    }

    public void LoadLevelScene(int index)
    {
        EditorSceneManager.LoadScene(index);
    }
}