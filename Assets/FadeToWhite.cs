using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToWhite : MonoBehaviour
{

    public Image whiteFade;

    // Start is called before the first frame update
    void Start()
    {

        whiteFade.canvasRenderer.SetAlpha(0.0f);

        fadeIn();

    }

    // Update is called once per frame
    void fadeIn()
    {

        whiteFade.CrossFadeAlpha(1, 2, false);

    }
}
