using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Creature> creaturesInArea;

    public Creature GetRandomCreature()
    {
        var creatursinArea= creaturesInArea[Random.Range(0, creaturesInArea.Count)];
        creatursinArea.Init();
        return creatursinArea;
    }
}
