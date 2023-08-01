using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileEraser : MonoBehaviour
{
    
    public Tilemap map;
    
    // Update is called once per frame
    void Update()
    {

        RemoveTile();
    }

    public void RemoveTile()
    {
       
        var tilePos = map.WorldToCell(transform.position);
        map.SetTile(tilePos, null);
    }

   
}
