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

    public KeyCode DialogueInput = KeyCode.Space;

    private bool _isStringBeingRevealed = false;
    private bool _isDialoguePlaying = false;
    private bool _isEndofDialogue = false;

    public GameObject ContinueText;
    public GameObject StopText;

    private GameObject dialoguePanel;

    // Use this for initialization
    void Start()
    {
        _textComponent = GetComponent<Text>();
        _textComponent.text = "";

        dialoguePanel = GameObject.Find("Dialogue Panel");
        dialoguePanel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("player");
        GameObject baker = GameObject.Find("baker");
        
        if (Mathf.Abs(player.transform.position.x - baker.transform.position.x) < TRIGGER_DISTANCE
        && Mathf.Abs(player.transform.position.y - baker.transform.position.y) < TRIGGER_DISTANCE
        && Input.GetKeyDown(DialogueInput))
        {
            dialoguePanel.SetActive(true);
            Debug.Log("this should be happening");

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
                }
            }

            yield return 0;

        }

        while (true)
        {
            if (Input.GetKeyDown(DialogueInput))
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
        int currentCharacterIndex = 0;

        HideStuff();

        _textComponent.text = "";

        while (currentCharacterIndex < stringLength)
        {
            _textComponent.text += stringToDisplay[currentCharacterIndex];
            currentCharacterIndex++;

            if (currentCharacterIndex < stringLength)
            {
                if (Input.GetKey(DialogueInput))
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
                break;
            }
        }

        ShowIcon();

        while (true)
        {
            if (Input.GetKeyDown(DialogueInput))
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
