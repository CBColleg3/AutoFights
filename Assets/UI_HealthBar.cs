using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    Image image;
    public bool P1Health;
    GameObject player;
    bool findPlayerStarted;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindPlayer());
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(findPlayerStarted)image.fillAmount = player.GetComponent<Player>().health / 100;
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        if (P1Health) player = GameObject.Find("Player1"); else player = GameObject.Find("Player2");
        findPlayerStarted = true;
    }
}
