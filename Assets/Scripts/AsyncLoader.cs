using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] Animator _blackScreen;
    [SerializeField] Animator _audio;
    [SerializeField] GameObject _menu;

    public void LoadLevel(string levelToLoad)
    {
        StartCoroutine(LoadAsync(levelToLoad));
    }

    /// <summary>
    /// Load the level asynchronously, fading in and out the black screen and hiding the menu.
    /// </summary>
    /// <param name="levelToLoad">The name of the level to load.</param>
    /// <returns>An IEnumerator to be used in a coroutine.</returns>
    IEnumerator LoadAsync(string levelToLoad)
    {
        // Play the fade in animation for the black screen
        _blackScreen.Play("BlackScreenFadeIn");
        _audio.Play("FadeOut");

        // Wait for 1 second to make sure the animation has finished
        yield return new WaitForSeconds(1);

        // Hide the menu
        _menu.SetActive(false);

        // Play the fade out animation for the black screen
        _blackScreen.Play("BlackScreenFadeOut");

        // Load the level asynchronously
        SceneManager.LoadSceneAsync(levelToLoad);
    }
}