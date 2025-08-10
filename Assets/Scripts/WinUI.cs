using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [SerializeField] string gameplaySceneName = "GameScene"; // name of your gameplay scene

    public void PlayAgain()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}
