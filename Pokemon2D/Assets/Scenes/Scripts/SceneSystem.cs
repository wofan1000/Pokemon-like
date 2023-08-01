using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSystem
{
    public static LevelManager currentLevelManager { get; private set; }

    static List<SceneDetails> currentsceneDetails = new List<SceneDetails>();

   [SerializeField]  //SceneSystem currentScenesLoaded

    public static IEnumerator EnterLevel(LevelManager newLevel)
    {
        if(currentLevelManager == newLevel)
        {
            yield break;
        }
        if(currentLevelManager == null)
        {
            // player loads in
           
        }
        currentLevelManager = newLevel;
        // player wild encounters change type

        // unload scene

        for (int i = currentsceneDetails.Count - 1; i >= 0; i--)
        {
            if (currentsceneDetails[i] == newLevel.levelRefs || newLevel.levelRefs.Connectedscenes.Contains(currentsceneDetails[i]) == true)
            {
                continue;
            }
            //AllActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            yield return SceneManager.UnloadSceneAsync(currentsceneDetails[i].name);
            currentsceneDetails.RemoveAt(i);
        }

        //Load
        foreach (SceneDetails newScene in newLevel.levelRefs.Connectedscenes)
        {
            if (currentsceneDetails.Contains(newScene) == true)
            {
                continue;
            }
            AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(newScene.name, LoadSceneMode.Additive);
            currentsceneDetails.Add(newScene);

            sceneToLoad.completed += (AsyncOperation op) =>
            {
                //savableEntities = GetSavableEntitiesInScene();
                //SavingSystem.i.RestoreEntityStates(savableEntities);
            };
        }


    }

   

}
