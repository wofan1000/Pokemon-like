using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class BeastierySystem : MonoBehaviour
{
    [SerializeField] Habitates testHabs;


    [SerializeField] List<Text> pageTitles = new List<Text>();
     [SerializeField] List<CreaterBeastieryEntry> currentBeastieryEntries = new List<CreaterBeastieryEntry>();
    [SerializeField]List<CreaterBeastieryEntry> completeEntries = new List<CreaterBeastieryEntry>();
    [SerializeField] List<HabitateInfo> habitateInformation = new List<HabitateInfo>();
    [SerializeField] List<CreatureButtonUI> Creaturebuttons = new List<CreatureButtonUI>();

    [SerializeField] List<BeastieryButton> beastieryButton = new List<BeastieryButton>();

    private enum ButtonType
    {
        NextButton,
        PreviousButton
    }

    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    private Vector3 rotation, startPos;
    private Quaternion startRotation;

    private bool isClicked;

    private DateTime startTime, endTime;
    // Start is called before the first frame update
    void Start()
    {

        startRotation = transform.rotation;
        startPos = transform.position;

        if(nextButton!= null)
        {
            nextButton.onClick.AddListener(() => TurnPage(ButtonType.NextButton));
        }

        if (prevButton != null)
        {
            nextButton.onClick.AddListener(() => TurnPage(ButtonType.PreviousButton));
        }

        //LoadHabitate(testHabs);

        for (int i = 0; i < beastieryButton.Count; i++)
        {
            int k = i;
            beastieryButton[i].GetButton.onClick.AddListener(() =>
            {
                LoadHabitate(beastieryButton[k].GetHabitates);
         });

        }
        EventSystem.current.SetSelectedGameObject(beastieryButton[0].GetButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(isClicked)
        {
            transform.Rotate(rotation * Time.deltaTime);

            endTime = DateTime.Now;

            if((endTime - startTime).TotalSeconds >= 1)
            {
                isClicked = false;
                transform.rotation= startRotation;
                transform.position= startPos;
            }
        }
    }

    private void TurnPage(ButtonType type)
    {
        isClicked= true;

        if(type == ButtonType.NextButton ) 
        {
            rotation = new Vector3(0, 180, 0);
        } else if (type == ButtonType.PreviousButton )
        {
            Vector3 newRotation = new Vector3 (startRotation.x, 180, startRotation.z);
            transform.rotation = Quaternion.Euler(newRotation);

            rotation = new Vector3(0, -180, 0);
        }
    }

    private void OpenBeastiery ()
    {
        
    }

    private void LoadHabitate(Habitates habitate)
    {
        Debug.Log($"testing {habitate}");
        currentBeastieryEntries.Clear();
        currentBeastieryEntries.AddRange(completeEntries.Where(x => x.GetCreatureBase.getHabitate == habitate));
        HabitateInfo tempInfo = habitateInformation.FirstOrDefault(x => x.GetHabitates == habitate);
        for (int i = 0; i < pageTitles.Count; i++)
        {
            pageTitles[i].text = tempInfo.GetTitle;
            pageTitles[i].color = tempInfo.GetColor;
        }
    }

}

[System.Serializable]
public class HabitateInfo
{
    [SerializeField] Habitates Thehabitate;
    [SerializeField] string habitatetitle;
    [SerializeField] Color habitateColor;
    public Habitates GetHabitates { get { return Thehabitate; } }
    public string GetTitle { get { return habitatetitle; } }
    public Color GetColor { get { return habitateColor; } }
}

[System.Serializable]
public class BeastieryButton
{
    [SerializeField] Button habitateButton;
    public Button GetButton { get { return habitateButton; } set { habitateButton = value; } }

    [SerializeField] Habitates Thehabitate;
    
    public Habitates GetHabitates { get { return Thehabitate; } }

}
