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
    private bool alive = true;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if(Curtain.color.a == 1){
            Debug.Log("reset curtain color.");
            Color tempColor = Color.white;
            tempColor.a = 0;
            Curtain.color = tempColor;
        }
        Debug.Log("Curtain " + Curtain.color.ToString());
        Debug.Log("alpha: curtain " + Curtain.color.a);
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
        if(!alive){
            Debug.Log("somehow can't find self.");
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

    public void LoadSceneComic(){
        SceneManager.LoadScene(1);
    }

    // Change scene to character select
    public void LoadSceneCharacterSelect()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadSceneFight()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSceneVictory()
    {
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
        // Curtain.color = initialColor;
        Debug.Log("initial color: " + initialColor.ToString() + " to curtain" + Curtain.color.ToString());
        Debug.Log("alpha: init " + initialColor.a.ToString() + " curtain " + Curtain.color.a);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Curtain.color = Color.Lerp(initialColor, color, elapsedTime / duration);
            yield return null;
        }

        Debug.Log("initial color: " + initialColor.ToString() + " to curtain" + Curtain.color.ToString());
        Debug.Log("alpha: init " + initialColor.a.ToString() + " curtain " + Curtain.color.a.ToString());
        Debug.Log("Load scene " + toScene);
        SceneManager.LoadScene(toScene);
        Debug.Log("Goodbye");
    }
    private IEnumerator FadeOut(float duration)
    {
        Debug.Log("Fading out...");
        Color initialColor = Curtain.color;
        Color targetColor = Color.black;
        targetColor.a = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Curtain.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        Debug.Log("finished fading");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        StartCoroutine(FadeOut(duration: 1.0f));
    }
}
