using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour
{
    private const float TRIGGER_DISTANCE = 5;

    private Text _textComponent;

    public string[] DialogueStrings;

    public float TimeBetweenCharacters = 0.05f;
    public float CharacterRate = 0.5f;

 //   public KeyCode DialogueInput = KeyCode.Space;

    private bool _isStringBeingRevealed = false;
    private bool _isDialoguePlaying = false;
    private bool _isEndofDialogue = false;

    public GameObject ContinueText;
    public GameObject StopText;

    private GameObject dialoguePanel;

    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.Find("player");
        GameObject baker = GameObject.Find("baker");

        _textComponent = GetComponent<Text>();
        _textComponent.text = "";

        HideStuff();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("PS4_X"))
        {
            if (!_isDialoguePlaying)
            {
                _isDialoguePlaying = true;
                StartCoroutine(StartDialogue());
            }
        }

    }

    private IEnumerator StartDialogue()
    {
        int dialogueLength = DialogueStrings.Length;

        //the current line number within the dialogue array
        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
                StartCoroutine(DisplayString(DialogueStrings[currentDialogueIndex++]));

                if (currentDialogueIndex >= dialogueLength)
                {
                    _isEndofDialogue = true;
                    Debug.Log("You have reached the end of the dialogue array");
                }
            }

            yield return 0;

        }

        while (true)
        {
            if (Input.GetButtonDown("PS4_X"))
            {
                break;
            }

            yield return 0;

        }

        HideStuff();
        _isEndofDialogue = false;
        _isDialoguePlaying = false;

    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        int stringLength = stringToDisplay.Length;

        // the exact letter of the current line of dialogue
        int currentCharacterIndex = 0;

        HideStuff();

        _textComponent.text = "";

        while (currentCharacterIndex < stringLength)
        {
            _textComponent.text += stringToDisplay[currentCharacterIndex];
            currentCharacterIndex++;

            if (currentCharacterIndex < stringLength)
            {
                if (Input.GetButton("PS4_X"))
                {
                    yield return new WaitForSeconds(TimeBetweenCharacters * CharacterRate);
                }
                else
                {
                    yield return new WaitForSeconds(TimeBetweenCharacters);
                }
            }

            else
            {
                if(_isEndofDialogue)
                {
                    Debug.Log("IT IS THE LAST LETTER OF THE LAST DIALOGUE ENTRY");
                }
                break;
            }
        }

        ShowIcon();

        while (true)
        {
            if (Input.GetButtonDown("PS4_X"))
            {
                break;
            }

            yield return 0;
        }

        HideStuff();

        _isStringBeingRevealed = false;
        _textComponent.text = "";
    }



    private void HideStuff()
    {
        ContinueText.SetActive(false);
        StopText.SetActive(false);
    }

    private void ShowIcon()
    {
        if (_isEndofDialogue)
        {
            StopText.SetActive(true);
            return;
        }

        ContinueText.SetActive(true);
    }

}
