using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipComic : MonoBehaviour
{
    //
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void GoToCharacterSelect()
    {
        SceneManager.LoadScene(2);
    }
}
