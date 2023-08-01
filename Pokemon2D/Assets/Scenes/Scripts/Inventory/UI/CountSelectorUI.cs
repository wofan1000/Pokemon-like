using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSelectorUI : MonoBehaviour
{
    [SerializeField] Text countText;
    [SerializeField] Text priceText;

    bool selected;
    int currentCount;

    int maxcount;
    float priceperUnit;

    public IEnumerator ShowSelector(int maxCount, float pricePerUnit, Action<int> OnCountSelected)
    {
        this.maxcount = maxCount;
        this.priceperUnit = pricePerUnit;

        selected = false;
        currentCount = 1;

        gameObject.SetActive(true);
        SetValues();

        yield return new WaitUntil(() => selected == true);

        OnCountSelected?.Invoke(currentCount);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        int prevcount = currentCount;

        if (Input.GetKeyDown(KeyCode.S))
            ++currentCount;
        else if (Input.GetKeyDown(KeyCode.W))
            --currentCount;

        currentCount = Mathf.Clamp(currentCount, 1, maxcount);

        if(currentCount != prevcount)
            SetValues();

        if (Input.GetKeyDown(KeyCode.Z))
            selected = true;
    }

    void SetValues()
    {
        countText.text = "x" + currentCount;
        priceText.text = "$" + priceperUnit * currentCount;
    }
}
