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
        Player1 = GameObject.Find(SaveScript.P1Select);
        Player1.gameObject.GetComponent<SwitchOnP12D>().enabled = true;
        Player2 = GameObject.Find(SaveScript.P2Select);
        Player2.gameObject.GetComponent<SwitchOnP22D>().enabled = true;
        StartCoroutine(SpawnPlayers());

    }

    private void Update()
    {
        if(SaveScript.Player1Health <=0.5f)
        {
            Debug.Log("Player 1 " + SaveScript.P1Select + " health less than half.");
        }
        if(SaveScript.Player1Health <= 0.0f)
        {
            Debug.Log("Player 1 " + SaveScript.P1Select + " no more health.");
        }
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
