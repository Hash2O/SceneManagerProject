using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        if (audioInstance != null && audioInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

}
