using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNextScene : MonoBehaviour
{
    void OnEnable()
    {
        SceneTransitionManager sm = GameObject.Find("StayAcrossScenes").GetComponent<SceneTransitionManager>();
        sm.ToNextScene();
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        //SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
