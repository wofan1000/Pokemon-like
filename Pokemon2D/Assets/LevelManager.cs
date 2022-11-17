using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] SceneDetails levelRef;

    [SerializeField] List<CreatureEncounterRecord> creaturesInArea;
    [SerializeField] List<CreatureEncounterRecord> creaturesInWater;

    int totalChance = 0;

     int totalChanceWater = 0;


    public SceneDetails levelRefs{ get { return levelRef; } }

    private void Awake()
    {
        levelRef.SetLevelManager(this);
        CalculateChancePercentage();
    }

    private void OnValidate()
    {
        CalculateChancePercentage();
    }




    void CalculateChancePercentage()
    {
        totalChance = -1;
        totalChanceWater = -1;

        if (creaturesInArea.Count > 0)
        {
            totalChance = 0;
            foreach (var record in creaturesInArea)
            {
                record.lowerChance = totalChance;
                record.upperChance = totalChance + record.chance;

                totalChance = totalChance + record.chance;
            }

            if (creaturesInWater.Count > 0)
            {
                totalChanceWater = 0;
                foreach (var record in creaturesInWater)
                {
                    record.lowerChance = totalChanceWater;
                    record.upperChance = totalChanceWater + record.chance;

                    totalChanceWater = totalChanceWater + record.chance;
                }
            }
        }
    }
    public Creature GetRandomCreature(BattleTrigger trigger)
    {
        var creatureList = (trigger == BattleTrigger.Land) ? creaturesInArea : creaturesInWater;

        int randomValue = Random.Range(1, 101);
        var creatureRecord = creatureList.First(p => randomValue >= p.lowerChance && randomValue <= p.upperChance);

        var levelRange = creatureRecord.levelRange;
        int level = levelRange.y == 0 ? levelRange.x : Random.Range(levelRange.x, levelRange.y + 1);

        var wildCreature = new Creature(creatureRecord.creature, level);

        wildCreature.Init();
        return wildCreature;


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Entered {gameObject.name}");


           StartCoroutine(SceneSystem.EnterLevel(this));


        }
        Debug.Log("test");
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
