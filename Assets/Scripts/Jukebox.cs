using UnityEngine;

public class Jukebox : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip menuClip;
    [SerializeField] AudioClip gameClip;
    [SerializeField] bool inGame = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on Jukebox. Please add an AudioSource component.");
        }

        if (inGame)
        {
            PlayGameMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (menuClip != null)
        {
            audioSource.clip = menuClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Menu music clip not assigned to Jukebox.");
        }
    }

    public void PlayGameMusic()
    {
        if (gameClip != null)
        {
            audioSource.clip = gameClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Game music clip not assigned to Jukebox.");
        }
    }

}
