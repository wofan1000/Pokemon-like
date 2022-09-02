using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
   

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;

    Action onDialogueFinished;

    public bool ShowingDialogue { get; private set; }


    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    int currentline = 0;
    Dialogue dialogue;
    bool isTyping;

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogueBox.SetActive(true);
            ++currentline;
            if(currentline < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentline]));
            } else
            {
                currentline = 0;
                ShowingDialogue = false;
                dialogueBox.SetActive(false);
                onDialogueFinished?.Invoke();
                OnCloseDialogue?.Invoke();
            }
        }
    }
    public IEnumerator ShowDialogue(Dialogue dialogue, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialogue?.Invoke();


        ShowingDialogue = true;
        this.dialogue = dialogue;
        onDialogueFinished = onFinished;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }

    public IEnumerator TypeDialogue( string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f/ lettersPerSecond);
        }
        isTyping=false;
    }
}
