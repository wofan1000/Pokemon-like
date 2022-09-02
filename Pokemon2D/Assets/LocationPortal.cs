using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    
    [SerializeField] DesID destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    Fader fader;
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Charecter.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(Teleport());
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }
    IEnumerator Teleport()
    {
        GameController.instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

       
        var portal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Charecter.SetPosToTile(portal.spawnPoint.position);

        yield return fader.FadeOut(0.5f);

        GameController.instance.PauseGame(false);
    }

    public Transform SpawnPoint => spawnPoint;
}
