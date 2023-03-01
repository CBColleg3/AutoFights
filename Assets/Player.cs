using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float health;
    public float attackMultiplier;
    public float speedMultiplier;
    public float attackSpeedMultiplier;
    public float damage;
    public float hitstun;
    public float knockbackX;
    public float knockbackY;
    public float hitstunMultiplier;
    public int hitlag;
    //public bool hurt;
    [HideInInspector]
    public PlatformerCharacter2D platformchar2D;
    public bool startCou;
    bool player1;
    float damageMultiplier = 0.6f;
    bool bypassBlock = false;
    bool loseGame = false;
    bool wonGame = false;
   // float damageScaling;

    private void Start()
    {
        platformchar2D = GetComponent<PlatformerCharacter2D>();
    }

    private void FixedUpdate()
    {
        //Find CanvasBoard
        platformchar2D.moveSpeedMultiplier = speedMultiplier;
        platformchar2D.attackSpeedMultiplier = this.attackSpeedMultiplier;
        if(platformchar2D.canvasBoard == null) platformchar2D.canvasBoard = transform.Find("Canvas").gameObject;
        if (!platformchar2D.m_Grounded && !platformchar2D.isHurt)
        {
            if (player1) GameObject.FindGameObjectWithTag("Player1Body").layer = LayerMask.NameToLayer("Player1"); else GameObject.FindGameObjectWithTag("Player2Body").layer = LayerMask.NameToLayer("Player2");
        } else if (!platformchar2D.isHurt)
        {
            if (player1) GameObject.FindGameObjectWithTag("Player1Body").layer = LayerMask.NameToLayer("P1BodyBlock"); else GameObject.FindGameObjectWithTag("Player2Body").layer = LayerMask.NameToLayer("P2BodyBlock");

        }

        if(platformchar2D.opponent.GetComponent<PlatformerCharacter2D>().deadAnim && health > 0 && !wonGame)
        {
            StartCoroutine(WinDelay());
        }

    }

    public void DamagePlayer(float damage)
    {
       // damageScaling = Random.Range(0.8f, 1.2f);
        health -= damage * damageMultiplier;
        if (health < 0)
        {
            health = 0;
            platformchar2D.dead = true;
            platformchar2D.freeze = true;
            platformchar2D.deadAnim = true;
            platformchar2D.atkType = 0;
            platformchar2D.block = false;
            platformchar2D.isCrouching = false;
           // platformchar2D.isHurt = true;
            StartCoroutine(DieAnimEnd());

        }
    }

    public void PlayerHurt(Player opponent)
    {
        // hurt = true;
        if (player1) GameObject.FindGameObjectWithTag("Player1Body").layer = LayerMask.NameToLayer("Player1"); else GameObject.FindGameObjectWithTag("Player2Body").layer = LayerMask.NameToLayer("Player2");


        if (startCou) StopCoroutine(PlayerHurtEnd(0));
        if (platformchar2D.atkType > 0) hitstunMultiplier = 2f;
        if (platformchar2D.atkType > 0) damageMultiplier = 1.1f;
        if (platformchar2D.atkType > 0) print("counter!");
        platformchar2D.atkType = 0;
        HighLowCheck(opponent);
        if (!platformchar2D.block || bypassBlock)DamagePlayer(opponent.damage);
        int side = (opponent.platformchar2D.m_FacingRight) ? 1 : -1;
        if (!platformchar2D.block || bypassBlock) platformchar2D.Knockback(side, opponent.knockbackX, opponent.knockbackY); else platformchar2D.Knockback(side, opponent.knockbackX / 5, 0);
        startCou = true;
        platformchar2D.isHurt = true;
        float hitstun = (!platformchar2D.block || bypassBlock)? opponent.hitstun * hitstunMultiplier: (opponent.hitstun / 3) * hitstunMultiplier;
        bypassBlock = false;
        if(!platformchar2D.dead)StartCoroutine(PlayerHurtEnd(hitstun));
    }

    void HighLowCheck(Player opponent)
    {
        PlatformerCharacter2D oPlatChar = opponent.GetComponent<PlatformerCharacter2D>();
        if (platformchar2D.block && platformchar2D.isCrouching && oPlatChar.atkGuardType=="H")
        {
            platformchar2D.block = false;
            platformchar2D.isCrouching = false;
            bypassBlock = true;
        } else if(platformchar2D.block && !platformchar2D.isCrouching && oPlatChar.atkGuardType=="L")
        {
            platformchar2D.block = false;
            platformchar2D.isCrouching = false;
            bypassBlock = true;
        }
    }

    public IEnumerator PlayerHurtEnd(float hitstun)
    {
        yield return new WaitForSeconds(hitstun);
        startCou = false;
        platformchar2D.isHurt = false;
        if (player1) GameObject.FindGameObjectWithTag("Player1Body").layer = LayerMask.NameToLayer("P1BodyBlock"); else GameObject.FindGameObjectWithTag("Player2Body").layer = LayerMask.NameToLayer("P2BodyBlock");

    }

    public void Throw()
    {
        StartCoroutine(ThrowEnd());
    }

    public void Thrown(int side, int damage)
    {
       // platformchar2D.Move(0, false, false);
        platformchar2D.FreezeMovement();
        platformchar2D.SetPosition(2 * side, 2.8f);
        StartCoroutine(ThrownEnd(side, damage));
    }

    IEnumerator ThrowEnd()
    {
        yield return new WaitForSeconds(0.5f);
        platformchar2D.throwAnim = false;
    }

    IEnumerator ThrownEnd(int side, int damage)
    {
        yield return new WaitForSeconds(0.5f);
        DamagePlayer(damage);
        platformchar2D.SetPosition(4 * -side, 0);
        platformchar2D.UnFreezeMovement();
    }

    IEnumerator DieAnimEnd()
    {
        yield return new WaitForSeconds(1f);
        platformchar2D.deadAnim = false;
    }

    IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(3f);
        platformchar2D.winAnim = true;
        platformchar2D.freeze = true;
        wonGame = true;
        StartCoroutine(WinAnimEnd());
    }

    IEnumerator WinAnimEnd()
    {
        yield return new WaitForSeconds(0.1f);
        platformchar2D.winAnim = false;
        StartCoroutine(GameEnd());
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("CharacterSelect");

    }

    public void SetPlayer1(bool p1)
    {
        player1 = p1;
    }


}
