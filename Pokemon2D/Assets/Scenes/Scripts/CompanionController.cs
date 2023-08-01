using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Creature;

public class CompanionController : MonoBehaviour
{
    private Vector2 input;

    private Charecter charecter;

    public Party creatureparty { get; private set; }

    public static CompanionController i;

    private void Awake()
    {
        i = this;

        creatureparty = GetComponent<Party>();

        charecter = GetComponent<Charecter>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HandleUpdate()
    {
        if (!charecter.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
                
        }

        charecter.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(Interact());
    }

    IEnumerator Interact()
    {
        var facingDir = new Vector3(charecter.Animator.MoveX, charecter.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        Debug.Log($"{charecter.Animator.MoveX} {charecter.Animator.MoveY}");

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.I.InteractableLayer | GameLayers.I.Diglayer);
        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);
        if (collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }
    IPlayerTriggerable currentlyInTrigger;

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, charecter.OffsetY), 0.2f, GameLayers.I.TriggerableLayers);

        IPlayerTriggerable triggerable = null;
        foreach (var collider in colliders)
        {
            triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {

                if (triggerable == currentlyInTrigger && !triggerable.triggerRepeatedly)
                    break;


                triggerable.OnCompanionTriggered(this);
                currentlyInTrigger = triggerable;
                break;
            }
        }

        if (colliders.Count() == 0 || triggerable != currentlyInTrigger)
            currentlyInTrigger = null;
    }

    public object CaptureState()
    {
        var saveData = new PlayerSaveData()
        {
            pos = new float[] { transform.position.x, transform.position.y },
            creatures = creatureparty.Creatures.Select(p => p.GetSaveData()).ToList()
        };

        float[] pos = new float[] { transform.position.x, transform.position.y };
        return saveData;
    }

    public void RestoreState(object state)
    {
        var savedata = (PlayerSaveData)state;

        var pos = savedata.pos;
        transform.position = new Vector3(pos[0], pos[1]);

        creatureparty.Creatures = savedata.creatures.Select(s => new Creature(s)).ToList();
    }


    [Serializable]
    public class PlayerSaveData
    {
        public float[] pos;
        public List<CreatureSaveData> creatures;
    }

    public Charecter Charecter => charecter;

    public string Name { get; set; }
}
