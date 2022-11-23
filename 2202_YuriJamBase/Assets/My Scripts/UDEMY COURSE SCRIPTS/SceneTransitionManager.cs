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
    
    // update once per frame
    void Update() 
    {
        Scene scene = SceneManager.GetActiveScene();

        // if currently at Comic scene check if user want to skip to the character selection.
        if(scene.name == "2D_Comic")
        {
            if (Input.anyKey)
            {
                LoadSceneCharacterSelect();
            }
        }
        
    }
    public void ToNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index == SceneManager.sceneCountInBuildSettings)
        {
            index = 0;
        }
        StartCoroutine(FadeInAndLoadScene(Color.black, 1.0f, index));
    }

    public void ReloadScene()
    {
        Debug.Log("reload scene");
        
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(FadeInAndLoadScene(Color.black, 1.0f, index));
    }

    public static void LoadSceneManu()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadSceneComic(){
        SceneManager.LoadScene(1);
    }

    // Change scene to character select
    public static void LoadSceneCharacterSelect()
    {
        SceneManager.LoadScene(2);
    }

    public static void LoadSceneFight(){
        SceneManager.LoadScene(3);
    }

    public static void LoadSceneVictory(){
        SceneManager.LoadScene(4);
    }

    // public void ControlsMenu()
    // {
    //     SceneManager.LoadScene(4);
    // }

    // public void OptionsMenu()
    // {
    //     SceneManager.LoadScene(5);
    // }

    public void ExitGame()
    {
        Application.Quit();
    }
    

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
        StartCoroutine(FadeOut(1.0f));
    }
}
