using UnityEngine;

public class NavigationButtonManager : MonoBehaviour
{

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToIntroScene()
    {
        if (Application.isPlaying) 
        { 
            gameManager.LoadSceneByIndex(0);
        }

    }
}
