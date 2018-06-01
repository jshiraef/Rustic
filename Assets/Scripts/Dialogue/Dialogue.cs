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

    private List<string> DialogueStringsList = new List<string>();

    // Use this for initialization
    void Start()
    {

        _textComponent = GetComponent<Text>();
        _textComponent.text = "";

        HideStuff();

        transform.parent.transform.parent.GetComponent<CanvasGroup>().alpha = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //only temporary, this should be Input.GetButton("PS4_X");
        if (Input.GetKey(KeyCode.X) && DialogueStringsList.Count > 0)
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
        int dialogueLength = DialogueStringsList.Count;

        //the current line number within the dialogue array
        int currentDialogueIndex = 0;

        //make the dialogue box appear
        transform.parent.transform.parent.GetComponent<CanvasGroup>().alpha = 1;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
                StartCoroutine(DisplayString(DialogueStringsList[currentDialogueIndex++]));

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
            //only temporary, this should be Input.GetButton("PS4_X");
            if (Input.GetKey(KeyCode.X))
            {
                break;
            }

            yield return 0;

        }

        HideStuff();
        //_isEndofDialogue = false;
        _isDialoguePlaying = false;

    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        int stringLength = stringToDisplay.Length;

        // the exact letter of the current line of dialogue
        int currentCharacterIndex = stringToDisplay.IndexOf("]") + 1;

        LoadRawImage(stringToDisplay);

        HideStuff();

        _textComponent.text = "";

        while (currentCharacterIndex < stringLength)
        {
            _textComponent.text += stringToDisplay[currentCharacterIndex];
            currentCharacterIndex++;

            if (currentCharacterIndex < stringLength)
            {
                //only temporary, this should be Input.GetButton("PS4_X");
                if (Input.GetKey(KeyCode.X))
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
                Debug.Log("the end of dialogue bool is " + _isEndofDialogue);

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
            //only temporary, this should be Input.GetButton("PS4_X");
            if (Input.GetKey(KeyCode.X))
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

    public void LoadDialogueAsset(TextAsset asset)
    {
        DialogueStringsList.Clear();

        string[] lines = asset.text.Split('\n');
        foreach(string line in lines)
        {
            if(line.Length > 0)
            {
                DialogueStringsList.Add(line);
            }
        }
    }

    public void UnloadDialogueAsset()
    {
        // make the dialogue box disappear
        transform.parent.transform.parent.GetComponent<CanvasGroup>().alpha = 0;

        _isDialoguePlaying = false;
    }

    // This will pull up images (or other resources) for the character whenever their dialogue is being displayed
    public void LoadRawImage(string stringToDisplay)
    {
        if(stringToDisplay[0] == '[')
        {
            string[] commands = stringToDisplay.Substring(1, stringToDisplay.IndexOf("]") - 1).Split(',');
            if (GameObject.Find("RawImage") != null)
            {
                RawImage avatar = GameObject.Find("RawImage").GetComponent<RawImage>();

                // this next line loads an image (found in Assets/Resources) with the same name given within the text file
                avatar.texture = Resources.Load(commands[0]) as Texture;
            }
        }
    }

    public bool getIsDialoguePlaying()
    {
        return _isDialoguePlaying;
    }

    public void setIsDialoguePlaying(bool b)
    {
        _isDialoguePlaying = b;
    }
}
