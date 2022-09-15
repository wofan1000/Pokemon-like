using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] ChoiceBox choiceBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
   

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;

   

    public bool IsShowing { get; private set; }


    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void HandleUpdate()
    {
      
    }

    public IEnumerator ShowDialogText(string text, bool waitForInput = true, List<string> choices = null,
        Action<int> onchoiceSelected = null)
    {
        OnShowDialogue?.Invoke();
        IsShowing = true;

        dialogueBox.SetActive(true);

       yield return TypeDialogue(text);
        if(waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (choices != null && choices.Count > 1)
        {
            yield return choiceBox.ShowChoices(choices, onchoiceSelected);
        }

        dialogueBox.SetActive(false);
        IsShowing = false;
    }
    public IEnumerator ShowDialogue(Dialogue dialogue, List<string> choices = null, 
        Action<int> onchoiceSelected=null)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialogue?.Invoke();


        IsShowing = true;
       
        dialogueBox.SetActive(true);

        foreach(var line in dialogue.Lines)
        {
           yield return TypeDialogue(line);
           yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if(choices != null && choices.Count > 1)
        {
           yield return choiceBox.ShowChoices(choices,onchoiceSelected);
        }

            dialogueBox.SetActive(false);
            IsShowing=false;
            OnCloseDialogue?.Invoke();
    }

    public void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        IsShowing = false;
        OnCloseDialogue?.Invoke();
    }

    public IEnumerator TypeDialogue( string line)
    {
        dialogueText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f/ lettersPerSecond);
        }
        
    }
}
