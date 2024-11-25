using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(LoadMenu), 2.5f);
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
