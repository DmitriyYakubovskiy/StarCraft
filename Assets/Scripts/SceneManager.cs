using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void NextScene()
    {
        ChangeScene((UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex) + 1);
    }

    public static void ChangeScene(int numberScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(numberScene);
    }

    public static void Restart()
    {
        ChangeScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}