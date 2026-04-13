using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndSceneManager : MonoBehaviour
{
    [SerializeField]
    private AudioManagerScript manager;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.StopMusic();
            SceneManager.LoadScene("EndScreen");
        }
    }
}
