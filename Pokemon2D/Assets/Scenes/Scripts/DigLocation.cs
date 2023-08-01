using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DigLocation : MonoBehaviour
{

    public Transform digSpawnPoint;

    [SerializeField] DesID destinationPortal;

    CompanionController companion;

    Fader fader;
    // Start is called before the first frame update
    void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Teleport()
    {
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);


        var portal = FindObjectsOfType<DigLocation>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        companion.Charecter.SetPositionAndSnapToTile(portal.digSpawnPoint.position);

        yield return fader.FadeOut(0.5f);

        GameController.Instance.PauseGame(false);
    }
}
