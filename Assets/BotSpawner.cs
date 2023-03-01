using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public GameObject[] characters;
    public int charToSpawnIndex;
    public string charToSpawn;
    Player player;
    public bool p1;
    public bool useImportedScript;

    // Start is called before the first frame update
    void Start()
    {
        //if (p1) gameObject.layer = 10; else gameObject.layer = 11;
        foreach (GameObject character in characters) {
            if(character.name == charToSpawn)
            {
                SpawnBot(charToSpawnIndex,p1);
                break;
            }
            charToSpawnIndex++;
        }
       // InvokeRepeating("FindGameObjects", 0.1f, 0.1f);
    }

    void SpawnBot(int index, bool p1)
    {
        GameObject bot = Instantiate(characters[index], transform);
        player = bot.GetComponent<Player>();
        CommandListActions commands = bot.GetComponent<CommandListActions>();
        commands.useImportedScript = this.useImportedScript;
        if (p1) bot.gameObject.layer = 10; else bot.gameObject.layer = 11;
        foreach (Transform child in bot.transform)
        {
            if (p1 && child.name != "Hitbox" && child.name != "Body") child.gameObject.layer = 10; else if (!p1 && child.name != "Hitbox" && child.name != "Body") child.gameObject.layer = 11;
            if (child.name == "Hitbox" && p1) child.gameObject.layer = 9; else if (child.name == "Hitbox" && !p1) child.gameObject.layer = 12;
            if (p1 && child.name == "Body") child.gameObject.tag = "Player1Body"; else if (!p1 && child.name == "Body") child.gameObject.tag = "Player2Body";
            if (child.name == "Body" && p1) child.gameObject.layer = 13; else if (child.name == "Body" && !p1) child.gameObject.layer = 14;
        }

        if (p1) bot.name = "Player1"; else bot.name = "Player2";
        FindOpponent(p1);
       // if(p1) bot.transform.Find("Body").tag = "Player1Body"; else bot.transform.Find("Body").tag = "Player2Body";
       // player.platformchar2D.canvasBoard = bot.transform.Find("Canvas").gameObject;
        //player.platformchar2D.leftCorner = GameObject.Find("WallL").transform;
       // player.platformchar2D.rightCorner = GameObject.Find("WallR").transform;
        if (p1) player.SetPlayer1(true); else player.SetPlayer1(false);
        if (p1) commands.SetPlayer1(true); else commands.SetPlayer1(false);

    }

    private void FixedUpdate()
    {
       // print("this code is running");
        if (player.platformchar2D.opponent == null) FindOpponent(p1);
        if (player.platformchar2D.leftCorner == null) player.platformchar2D.leftCorner = GameObject.Find("WallL").transform;
        if (player.platformchar2D.rightCorner == null) player.platformchar2D.rightCorner = GameObject.Find("WallR").transform;
        player.SetPlayer1(p1);
        player.GetComponent<CommandListActions>().SetPlayer1(p1);
    }

    void FindOpponent(bool p1)
    {
        if (p1) player.platformchar2D.opponent = GameObject.Find("Player2"); else player.platformchar2D.opponent = GameObject.Find("Player1");
    }
}
