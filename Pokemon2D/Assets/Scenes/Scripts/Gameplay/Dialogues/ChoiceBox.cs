using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] ChoiceText choicetextPrefab;
    bool choiceselected = false;

    List<ChoiceText> choicetexts;

    int currentChoice;

  public IEnumerator ShowChoices(List<string> choices, Action<int> onChoiceSelected)
    {
        choiceselected = false;
        currentChoice = 0;


        gameObject.SetActive(true);

        //delete Existing Choices
        foreach(Transform child in transform)
            Destroy(child.gameObject);

        choicetexts = new List<ChoiceText>();

        foreach(var choice in choices)
        {
          var choiceTextObj =  Instantiate(choicetextPrefab, transform);
            choiceTextObj.TextField.text = choice;
            choicetexts.Add(choiceTextObj);
        }
        yield return new WaitUntil(() => choiceselected == true);

        onChoiceSelected?.Invoke(currentChoice);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            ++currentChoice;
        else if (Input.GetKeyDown(KeyCode.W))
            --currentChoice;

        currentChoice = Mathf.Clamp(currentChoice, 0, choicetexts.Count - 1);

        for (int i = 0; i < choicetexts.Count; i++)
        {
            choicetexts[i].SetSelected(i == currentChoice);
        }

        if (Input.GetKeyDown(KeyCode.Z))
            choiceselected = true;
    }
}
