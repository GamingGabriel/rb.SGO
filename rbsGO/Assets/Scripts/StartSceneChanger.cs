using UnityEngine;
using UnityEngine.SceneManagement;
public class StartSceneChanger : MonoBehaviour
{
    [SerializeField]
    string levelName;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }
}
