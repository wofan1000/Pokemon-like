using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingSpike : MonoBehaviour
{
    public int speed;

    public Transform tar1, tar2;

   public  Vector2 tarpos;

    private void Start()
    {
        tarpos = tar2.position;
    }
  
    private void Update()
    {
        if (Vector2.Distance(transform.position, tar1.position) < .1f) tarpos = tar2.position;

        if (Vector2.Distance(transform.position, tar2.position) < .1f) tarpos = tar1.position;

        transform.position = Vector2.MoveTowards(transform.position, tarpos, speed * Time.deltaTime);
    }
}
