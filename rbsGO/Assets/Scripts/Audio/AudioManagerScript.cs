using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    [SerializeField] 
    private AudioSource musicSource; 
    
    [Header("AUDIO CLIPS")]
    [SerializeField] 
    private AudioClip music; 



    void Start()
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
