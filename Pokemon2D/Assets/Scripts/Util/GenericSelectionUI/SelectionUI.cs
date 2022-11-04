using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.GenericSelection { 
public class SelectionUI<T> : MonoBehaviour where T : ISelectableItem 
{
    List<T> items;
    int selectedItem = 0;

    float selectionTimer = 0;

    const float selectionSpeed = 5;

    public event Action<int> OnSelected;

    public event Action OnBack;
      

        public virtual void HandleUpdate()
        {
            UpdateSelectionTimer();

            int prevSelection = selectedItem;
            HandleListSelection();

            selectedItem = Mathf.Clamp(selectedItem, 0, items.Count - 1);

            if (selectedItem != prevSelection)
                UpdateSelectionUI();

            if (Input.GetButtonDown("Action"))
                OnSelected?.Invoke(selectedItem);
            else if (Input.GetButtonDown("Back"))
                OnBack?.Invoke();



        }

        public virtual void HandleListSelection()
        {
            float v = Input.GetAxis("Vertical");

            if (selectionTimer == 0 && Mathf.Abs(v) > 0.2f)
            {
                 selectedItem += -(int)Mathf.Sign(v);

                selectionTimer = 0.2f;

                selectionTimer = 1 / selectionSpeed;
            }
        }

        public void SetItems(List<T> items)
        {
            this.items = items;
            UpdateSelectionUI();

        }   

        void UpdateSelectionUI()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnSelectionChanged(i == selectedItem);
            }
        }

        void UpdateSelectionTimer()
        {
            if (selectionTimer > 0)
                selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
        }
    }
}
