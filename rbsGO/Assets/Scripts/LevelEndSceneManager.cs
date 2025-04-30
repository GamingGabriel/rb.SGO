using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndSceneManager : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("EndScreen");
        }
    }
}
