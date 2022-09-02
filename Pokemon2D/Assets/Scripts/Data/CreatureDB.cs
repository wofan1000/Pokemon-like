using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDB 
{
    static Dictionary<string, CreatureBase> creatures;

    public static void init()
    {
        creatures = new Dictionary<string, CreatureBase>();

        var creatureArrey = Resources.LoadAll<CreatureBase>("");
            foreach (var creature in creatureArrey)
            {
            if (creatures.ContainsKey(creature.Name))
            {
                continue;
            }    


                creatures[creature.Name] = creature;
            }
    }

    public static CreatureBase GetCreaturebyName(string name)
    {
        if (!creatures.ContainsKey(name))
        {
            return null;
        } 
            return creatures[name];
    }
}
