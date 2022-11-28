using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnP22D : MonoBehaviour
{
    
    public GameObject P2Character;
    public GameObject AICharacter;
    public string P2Name = "Player 2";

    // Start is called before the first frame update
    void Start()
    {
        if (SaveScript.Player1Mode == false)
        {
            SaveScript.Player2Load = P2Character;
        }
        if (SaveScript.Player1Mode == true)
        {
            SaveScript.Player2Load = AICharacter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // Debug.Log("Player 2 is: " + SaveScript.P2Select);
    }
}
