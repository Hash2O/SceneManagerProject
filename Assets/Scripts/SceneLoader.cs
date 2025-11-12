using UnityEngine;
using UnityEngine.SceneManagement;

// This MonoBehaviour could be placed as a component inside the first scene in the Build Profiles Scene List.
// When the Player starts it instantiates this MonoBehaviour, which in turn loads
// an additional scene.
public class SceneLoader : MonoBehaviour
{
    // This scene must be listed in the Scene List in the Build Profiles Window,
    // or available from a loaded AssetBundle.
    const string sceneToLoad = "Assets/Scenes/AnotherScene.unity";

    void Start()
    {
        var op = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        op.completed += (AsyncOperation obj) =>
        {
            Scene loadedScene = SceneManager.GetSceneByPath(sceneToLoad);
            Debug.Log($"{sceneToLoad} finished loading (build index: {loadedScene.buildIndex}).");
            Debug.Log($"It has {loadedScene.rootCount} root(s).");
            Debug.Log($"There are now {SceneManager.loadedSceneCount} Scenes open.");
        };
    }

    private void OnDestroy()
    {
        // When closing the Scene containing this MonoBehaviour we also remove the Scene we loaded
        SceneManager.UnloadSceneAsync(sceneToLoad);
    }
}
