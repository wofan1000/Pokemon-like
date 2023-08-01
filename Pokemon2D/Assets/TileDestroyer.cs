using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    public Tilemap tilemap;

    public Vector3Int position;
    // Start is called before the first frame update
    void Start()
    {
        tilemap= GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = GetComponent<PlayerController>().transform.position;
        RemoveTile();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RemoveTile();
        }
    }

    public void RemoveTile()
    {
        tilemap.SetTile(position, null);
    }
}
