using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneChanger : MonoBehaviour
{
    [SerializeField]
    string levelName;
    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }
}
