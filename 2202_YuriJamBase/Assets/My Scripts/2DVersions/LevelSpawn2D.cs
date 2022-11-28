using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawn2D : MonoBehaviour
{
    private GameObject Player1;
    private GameObject Player2;
    private GameObject Player1Character;
    private GameObject Player2Character;
    public Transform Player1Spawn;
    public Transform Player2Spawn;


    public AudioClip[] opponentThemes;
    // Start is called before the first frame update
    void Start()
    {
        if (SaveScript.P1Select == null || SaveScript.P1Select == "")
        {
            SaveScript.P1Select = "BeatrizP1";
            Debug.Log("No P1 Selected default to : P1 = " + SaveScript.P1Select);
        }
        if (SaveScript.P2Select == null || SaveScript.P2Select == "")
        {
            SaveScript.P2Select = "MioP2";
            Debug.Log("No P2 Selected default to : P2 = " + SaveScript.P2Select);
        }
        Player1 = GameObject.Find(SaveScript.P1Select);
        Player1.gameObject.GetComponent<SwitchOnP12D>().enabled = true;
        Player2 = GameObject.Find(SaveScript.P2Select);
        Player2.gameObject.GetComponent<SwitchOnP22D>().enabled = true;
        StartCoroutine(SpawnPlayers());
    }

    private void Update()
    {
    }

    IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(0.1f);
        Player1Character = SaveScript.Player1Load;
        Player2Character = SaveScript.Player2Load;
        Instantiate(Player1Character, Player1Spawn.position, Player1Spawn.rotation);
        Instantiate(Player2Character, Player2Spawn.position, Player2Spawn.rotation);
    }

    
}
