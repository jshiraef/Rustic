using UnityEngine;
using System.Collections;

public class AnnabelleController : Entity
{

    

    private GameObject AlertIcon;

    public TextAsset dialogue;
    private Dialogue dialogueUI;


    private bool _withinTalkingRange;

    // Use this for initialization
    void Start()
    {

        dialogueUI = GameObject.Find("Dialogue Text").GetComponent<Dialogue>();
        body = GetComponent<Rigidbody2D>();
        AlertIcon = GameObject.Find("AlertIcon");

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "player")
        {
            dialogueUI.LoadDialogueAsset(dialogue);
        }

        dialogueUI.setIsDialoguePlaying(false);

        _withinTalkingRange = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "player")
        {
            dialogueUI.UnloadDialogueAsset();
        }

        _withinTalkingRange = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (_withinTalkingRange && !dialogueUI.getIsDialoguePlaying())
        {
            AlertIcon.SetActive(true);
        }
        else
        {
            AlertIcon.SetActive(false);
        }

        if (!_withinTalkingRange)
        {
            dialogueUI.transform.parent.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        }

    }
}
