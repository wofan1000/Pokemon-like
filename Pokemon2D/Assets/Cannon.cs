using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject lavaRock;

    [SerializeField] public bool IsInstanciated;

    [SerializeField] public PlayerController player, buddy;


    private void Start()
    {    
       player = FindObjectOfType<PlayerController>();
 
       buddy = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (!lavaRock.activeInHierarchy)
        {
            StartCoroutine(SpawnLavaRock());
        } 
    }

    IEnumerator SpawnLavaRock ()
    {
        if ((Vector2.Distance(player.transform.position, this.transform.position) < 5 || Vector2.Distance(buddy.transform.position, this.transform.position) < 5))
        {

            if (!IsInstanciated)
            {
                IsInstanciated = true;
                Instantiate(lavaRock, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
                IsInstanciated = false;
            }
        }
    }
}
