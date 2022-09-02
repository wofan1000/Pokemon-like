using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    public bool IsLoaded { get; private set; }

    List<SavableEntity> savableEntities;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            LoadScene();
            GameController.instance.SetCurrentScene(this);

            foreach(var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            if(GameController.instance.prevScene != null)
            {
                var prevScene = GameController.instance.prevScene;
                var prevLoadedScene = GameController.instance.prevScene.connectedScenes;
                foreach(var scene in prevLoadedScene)
                {
                    if(!connectedScenes.Contains(scene) && scene != this)
                        scene.UnLoadScene();
                }
                if(!connectedScenes.Contains(prevScene))
                    prevScene.UnLoadScene();
            }
        }
    }
    public void LoadScene()
    {
        if (!IsLoaded)
        {
            var operation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            operation.completed += (AsyncOperation) =>
            {
                savableEntities = GetSavableEntitiesInScene();
                SavingSystem.i.RestoreEntityStates(savableEntities);
            };

        }
    }

    public void UnLoadScene()
    {
        if (!IsLoaded)
        {
            SavingSystem.i.CaptureEntityStates(savableEntities);

            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }
   
    List<SavableEntity>  GetSavableEntitiesInScene()
    {
        var currentScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntitys = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currentScene).ToList();
        return savableEntitys;
}
}
