using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnP12D : MonoBehaviour
{
    public GameObject P1Character;
    public string P1Name = "Player 1";

    // Start is called before the first frame update
    void Start()
    {
        SaveScript.Player1Load = P1Character;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Player 1 is: " + SaveScript.P1Select);
    }

}
