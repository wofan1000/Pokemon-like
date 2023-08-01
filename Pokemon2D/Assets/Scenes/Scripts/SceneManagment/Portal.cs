using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DesID destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    Fader fader;
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Charecter.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    private void Start()
    {
      fader =  FindObjectOfType<Fader>();
    }
    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

         GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var portal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        //player.Charecter.SetPosToTile(portal.spawnPoint.position);

        yield return fader.FadeOut(0.5f);

        GameController.Instance.PauseGame(false);

        Destroy(gameObject);
    }

    public void OnCompanionTriggered(CompanionController companion)
    {
        throw new System.NotImplementedException();
    }

    public Transform SpawnPoint => spawnPoint;

    public bool triggerRepeatedly => false;
}

public enum DesID
{
    a,
    b,
    c,
    d,
    e
    
}
   