using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private FadeCanvas fadeCanvas;

    [Header("Transition Settings")]
    [Tooltip("Durée du fade avant le chargement d'une nouvelle scène")]
    public float fadeOutDuration = 1.0f;

    [Tooltip("Durée du fade après le chargement d'une nouvelle scène")]
    public float fadeInDuration = 1.0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // Si on revient à la scène d'intro, on garde le GameManager de la scène
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Destroy(instance.gameObject); // détruire l'ancien
                instance = this; // et le remplacer
            }
            else
            {
                Destroy(gameObject); // sinon détruire le nouveau
                return;
            }
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fadeCanvas = FindFirstObjectByType<FadeCanvas>();
        if (fadeCanvas != null)
            fadeCanvas.QuickFadeOut(); // Assure que l'écran commence visible puis s'efface
    }

    /// <summary>
    /// Charge la scène suivante dans la build settings (à associer à un bouton UI)
    /// </summary>
    public void LoadNextScene()
    {
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(LoadSceneWithFade(nextSceneIndex));
    }

    /// <summary>
    /// Recharge la scène actuelle (utile pour un bouton "Recommencer")
    /// </summary>
    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadSceneWithFade(SceneManager.GetActiveScene().buildIndex));
    }

    /// <summary>
    /// Charge une scène spécifique via son index (peut être appelée par un bouton)
    /// </summary>
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Indice de scène invalide !");
            return;
        }

        StartCoroutine(LoadSceneWithFade(sceneIndex));
    }

    /// <summary>
    /// Coroutine générique pour gérer le fade et le chargement
    /// </summary>
    private IEnumerator LoadSceneWithFade(int sceneIndex)
    {
        if (fadeCanvas == null)
            fadeCanvas = FindFirstObjectByType<FadeCanvas>();

        if (fadeCanvas != null)
        {
            fadeCanvas.StartFadeIn(); // Fade vers noir
            yield return new WaitForSeconds(fadeOutDuration);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
            yield return null;

        // Attendre un frame pour être sûr que la scène est bien chargée
        yield return null;

        fadeCanvas = FindFirstObjectByType<FadeCanvas>();
        if (fadeCanvas != null)
        {
            fadeCanvas.StartFadeOut(); // Réapparition douce
            yield return new WaitForSeconds(fadeInDuration);
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
                Application.Quit();
    }
}
