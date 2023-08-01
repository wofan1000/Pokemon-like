using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCannon : MonoBehaviour
{
    public Transform fireDes, firePoint;

    public AutoProjectile lavaRock;

    [SerializeField] public bool IsInstanciated;

    [SerializeField] public PlayerController player, buddy;





    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        buddy = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
      StartCoroutine(SpawnLavaRock());
           
    }

    IEnumerator SpawnLavaRock()
    {
              if (!IsInstanciated)
            {
                IsInstanciated = true;
           AutoProjectile newLavaRockProjectile = Instantiate(lavaRock, firePoint.transform.position, Quaternion.identity);

            
            newLavaRockProjectile.tarPos = fireDes.position;

            yield return new WaitForSeconds(.5f);

                IsInstanciated = false;
            }
        
    }
}
  

