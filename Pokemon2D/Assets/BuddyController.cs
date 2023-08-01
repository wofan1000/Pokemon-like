using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BuddyController : MonoBehaviour, ISwitchable
{
    private Charecter character;

    [SerializeField] private PlayerController player;
    
    public static BuddyController instance;

    public bool isActive { get; set; }

    public Transform thecurrentChar => throw new System.NotImplementedException();

    private void Awake()
    {
        instance= this;
    }

    public void Follow(Vector3 movePosition)
    {
        Vector2 moveVector = movePosition - this.transform.position;
        moveVector = moveVector.Generalize();

        if (!character.IsMoving)
        {
            StartCoroutine(this.character.Move(moveVector, null, true));
        }
    }

    private void Start()
    {
        character = GetComponent<Charecter>();
        this.transform.position = GameController.Instance.PlayerController.transform.position;
    }

    private void Update()
    {
        if (CharecterSwap.istogether == false) return;
        if (Vector3.Distance(transform.position, GameController.Instance.PlayerController.transform.position) > 3f)
        {

            transform.position = GameController.Instance.PlayerController.transform.position;
        }

        if (Vector3.Distance(transform.position, GameController.Instance.PlayerController.transform.position) > 15f)
        {

            CharecterSwap.istogether= true;
        }

        character.HandleUpdate();
    }
    public void OnSwitch(bool state)
    {
        player.playerActive = state;
        GetComponent<Party>().enabled = state;
        GetComponent<PlayerController>().enabled = state;
        GetComponent<BuddyController>().enabled = !state;
    }

   

    public void IsSeperated()
    {
        throw new System.NotImplementedException();
    }

    public void IsTogether()
    {
        throw new System.NotImplementedException();
    }

   
    }

   


 





