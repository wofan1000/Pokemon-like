using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<CreatureEncounterRecord> creaturesInArea;

    [HideInInspector]
    [SerializeField] int totalChance = 0;

    private void OnValidate()
    {
         totalChance = 0;
        foreach (var record in creaturesInArea)
        {
            record.lowerChance = totalChance;
            record.upperChance = totalChance + record.chance;

            totalChance = totalChance + record.chance;
        }
    }


    private void Start()
    {
        int totalChance = 0;
        foreach (var record in creaturesInArea)
        {
            record.lowerChance = totalChance;
            record.upperChance = totalChance + record.chance;

            totalChance = totalChance + record.chance;
        }
    }
    public Creature GetRandomCreature()
    {
        int randomValue = Random.Range(1, 101);
        var creatureRecord = creaturesInArea.First(p => randomValue >= p.lowerChance && randomValue <= p.upperChance);

        var levelRange = creatureRecord.levelRange;
        int level = levelRange.y == 0 ? levelRange.x : Random.Range(levelRange.x, levelRange.y + 1);

        var wildCreature = new Creature(creatureRecord.creature, level);
       
        wildCreature.Init();
        return wildCreature;

       
    }
}


[System.Serializable]
public class CreatureEncounterRecord
{
    public CreatureBase creature;
    public Vector2Int levelRange;
    public int chance;

    public int lowerChance { get; set; }
    public int upperChance { get; set; }
}
