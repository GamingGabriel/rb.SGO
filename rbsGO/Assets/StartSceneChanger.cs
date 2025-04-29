using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneChanger : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Robot Body Factory");
    }
}
