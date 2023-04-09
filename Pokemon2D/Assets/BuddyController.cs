using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BuddyController : MonoBehaviour, ISwitchable
{
    private Charecter charecter;

    [SerializeField] private PlayerController player;
  

    public bool isActive { get; set; }

    

    public void Follow(Vector3 movePosition)
    {
        Vector2 moveVector = movePosition - this.transform.position;
        moveVector = moveVector.Generalize();

        if (!charecter.IsMoving)
        {
            StartCoroutine(this.charecter.Move(moveVector, null, true));
        }
    }

    private void Start()
    {
        charecter = GetComponent<Charecter>();
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


        charecter.HandleUpdate();
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
        isActive= false;
    }

    public void IsTogether()
    {
        isActive = true;
    }

    public Charecter Charecter => charecter;

    Transform ISwitchable.thecurrentChar => this.transform;
}






