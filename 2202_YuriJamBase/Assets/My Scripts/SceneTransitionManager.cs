using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    // j'ai besoin d'une class qui va me permettre de navuger entre les scenes, elle contient tous les scripts de transitions et 
    // Start is called before the first frame update

    [SerializeField]
    private Image Curtain; // image that will fade to black or white


    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void ToNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index == SceneManager.sceneCountInBuildSettings)
        {
            index = 0;
        }
        StartCoroutine(FadeInAndLoadScene(Color.white, 0.5f, index));
    }

    public void ReloadScene()
    {
        Debug.Log("reload scene");
        
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(FadeInAndLoadScene(Color.white, 0.5f, index));
    }


    /*
    public void ToComic()
    {
        StartCoroutine(FadeInAndLoadScene(Color.white, 2, 1));
    }

    public void ControlsMenu()
    {
        SceneManager.LoadScene(4);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene(5);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    */

    private IEnumerator FadeInAndLoadScene(Color color, float duration, int toScene)
    {
        
        Color initialColor = color;
        initialColor.a = 0;
        Curtain.color = initialColor;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Curtain.color = Color.Lerp(initialColor, color, elapsedTime / duration);
            yield return null;
        }
        Debug.Log("Load scene " + toScene);
        SceneManager.LoadScene(toScene);
    }
    private IEnumerator FadeOut(float duration)
    {

        Color initialColor = Curtain.color;
        Color targetColor = initialColor;
        targetColor.a = 0;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Curtain.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            yield return null;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        StartCoroutine(FadeOut(0.5f));
    }
}
