using UnityEngine;

public class LaunchScene : MonoBehaviour
{
    [SerializeField] string _sceneName;

    public void LaunchGameScene() => UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
}
