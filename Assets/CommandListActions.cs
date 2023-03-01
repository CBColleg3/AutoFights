using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;
using System.Text.RegularExpressions;

public class CommandListActions : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] commandList;
    public string curCMD;
    public int index = 0;
    public float seconds;
    public float CMDseconds;
    PlatformerCharacter2D platformChar2D;
    Player player;
    HitBoxAttack hitbox;
    public bool performOnce;
    public bool conditionalSatisfied;
    public bool shootOnce;


    [SerializeField]bool player1;
    public bool useImportedScript;
    [SerializeField] GameObject playerScript;
    public TextMeshProUGUI commandText;
    public GameObject P1projectile;
    public GameObject P2projectile;

    // Start is called before the first frame update
    void Start()
    {
        platformChar2D = GetComponent<PlatformerCharacter2D>();
        //hitbox = GetComponent<HitBoxAttack>();
        player = GetComponent<Player>();
        if(useImportedScript)GetScript();
        curCMD = commandList[0];
        seconds = 0;
        CMDseconds = 10;
        InvokeRepeating("CountSeconds", 1.0f, 1.0f);
        //InvokeRepeating("StopSpecialCancel", 0.1f, 0.1f);
        //InvokeRepeating("ReadCommandList", CMDseconds, CMDseconds);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CountSeconds()
    {
        seconds++;
        //if (seconds >= CMDseconds) seconds = 0;
    }

    IEnumerator StopSpecialCancel()
    {
        yield return new WaitForSeconds(0.1f);
        platformChar2D.stopSpecialCancel = false;
    }

    IEnumerator ShootProjectile(float timer)
    {
        yield return new WaitForSeconds(timer);
        if(!platformChar2D.isHurt && !shootOnce)
        {
            shootOnce = true;
            Vector2 fireballPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
            if (player1) print("firing p1Projectile");
            GameObject fireball = (player1)? Instantiate(P1projectile, fireballPos, Quaternion.identity): Instantiate(P2projectile, fireballPos, Quaternion.identity);
            Fireball fireballScript = fireball.GetComponent<Fireball>();
            fireballScript.projectileOwner = this.gameObject.GetComponent<Player>();
            player.damage = 2;
            player.hitstun = 0.8f;
            player.knockbackX = 155f;
            player.knockbackY = 0f;
            platformChar2D.atkGuardType = "M";
            fireballScript.side = (platformChar2D.m_FacingRight) ? 1 : fireballScript.side = -1;
            fireballScript.lifetime = 10f;
            Destroy(fireball, fireballScript.lifetime);
        }

    }

    void FixedUpdate()
    {
       // seconds = 0;
        if(!conditionalSatisfied && !platformChar2D.freeze) curCMD = commandList[index];
        curCMD = curCMD.ToLower().Trim();
      //  print(curCMD.Split('/')[1].Substring(0,1));
        CMDseconds = Int32.Parse(curCMD.Split('/')[1].Substring(0,1));
        if(useImportedScript)commandText.SetText(curCMD);
        //if (useImportedScript) commandText.gameObject.

        PerformAction();

        if(platformChar2D.freeze && platformChar2D.thrownAnim)
        {
            curCMD = "thrown/99";
        }

        if (index < commandList.Length - 1 && seconds >= CMDseconds && platformChar2D.m_Grounded && !platformChar2D.isHurt && !platformChar2D.throwAnim && !platformChar2D.freeze) //what if they do an air attack? lol just have an else if bool for air attacks then
        {
            seconds = 0;
            platformChar2D.atkType = 0;
            index++;
            performOnce = false;
            conditionalSatisfied = false;
            platformChar2D.walkForward = false;
            platformChar2D.walkBack = false;
            platformChar2D.block = false;
            platformChar2D.grabAnim = false;
            platformChar2D.isCrouching = false;
            platformChar2D.overhead = false;
            platformChar2D.stopSpecialCancel = false;
            shootOnce = false;

        }
        else if (index < commandList.Length - 1 && seconds >= CMDseconds && !platformChar2D.isHurt && !platformChar2D.throwAnim && !platformChar2D.freeze && (commandList[index + 1].StartsWith("jump") || commandList[index + 1].StartsWith("air")))
        {
            seconds = 0;
            platformChar2D.atkType = 0;
            index++;
            performOnce = false;
            conditionalSatisfied = false;
            platformChar2D.walkForward = false;
            platformChar2D.walkBack = false;
            platformChar2D.block = false;
            platformChar2D.grabAnim = false;
            platformChar2D.isCrouching = false;
            platformChar2D.overhead = false;
            platformChar2D.stopSpecialCancel = false;
            shootOnce = false;
        }
        else if (index == commandList.Length - 1 && seconds >= CMDseconds && platformChar2D.m_Grounded && !platformChar2D.isHurt && !platformChar2D.throwAnim && !platformChar2D.freeze)
        {
            index = 0;
            performOnce = false;
            seconds = 0;
            platformChar2D.atkType = 0;
            conditionalSatisfied = false;
            platformChar2D.walkForward = false;
            platformChar2D.walkBack = false;
            platformChar2D.block = false;
            platformChar2D.grabAnim = false;
            platformChar2D.isCrouching = false;
            platformChar2D.overhead = false;
            platformChar2D.stopSpecialCancel = false;
            shootOnce = false;
        }


        //Forward Moving specials
        if(platformChar2D.atkType == 15)
        {
            int side2 = (platformChar2D.m_FacingRight) ? 1 * 2 : -1 * 2;
            platformChar2D.Move(side2, true, false);
        }

        /*
        BoxCollider2D[] bcs = this.gameObject.GetComponentsInChildren<BoxCollider2D>();
        if (!this.gameObject.GetComponent<PlatformerCharacter2D>().isHurt)
        {
            foreach (BoxCollider2D bc in bcs)
            {
                bc.isTrigger = false;
            }
        }
        */
    }

    void PerformAction()
    {
        if(platformChar2D.m_Grounded && !platformChar2D.isHurt)
        {
            if (curCMD.StartsWith("flip") && !performOnce)
            {
                performOnce = true;
                platformChar2D.Flip();
            }
            if (curCMD.StartsWith("walk"))
            {
                int side = 0;
                platformChar2D.walkForward = (curCMD.StartsWith("walkforward")) ? true : false;
                platformChar2D.walkBack = (curCMD.StartsWith("walkback")) ? true : false;
                if (curCMD.StartsWith("walkforward") && platformChar2D.m_FacingRight || curCMD.StartsWith("walkback") && !platformChar2D.m_FacingRight) {
                    side = 1;
                } else if (curCMD.StartsWith("walkforward") && !platformChar2D.m_FacingRight || curCMD.StartsWith("walkback") && platformChar2D.m_FacingRight)
                {
                    side = -1;
                }
               // print(side + "side");
              //  print(curCMD.StartsWith("walkforward") + " " + platformChar2D.m_FacingRight);
                platformChar2D.Move(side, false, false);
            }
            if(curCMD.StartsWith("crouch"))
            {
                if(curCMD.StartsWith("crouchblock")) platformChar2D.block = true;
                if (curCMD.StartsWith("crouchblock")) platformChar2D.isCrouching= true;
                platformChar2D.Move(0, true, false);
            }
            if (curCMD.StartsWith("wait"))
            {
                platformChar2D.Move(0, false, false);
            }
            if (curCMD.StartsWith("jump"))
            {
                int side = 0;
                if (curCMD.StartsWith("jumpforward") && platformChar2D.m_FacingRight || curCMD.StartsWith("jumpback") && !platformChar2D.m_FacingRight)
                {
                    side = 1;
                }
                else if (curCMD.StartsWith("jumpforward") && !platformChar2D.m_FacingRight || curCMD.StartsWith("jumpback") && platformChar2D.m_FacingRight)
                {
                    side = -1;
                }

                if (seconds < CMDseconds) platformChar2D.Move(side * platformChar2D.m_AirMultiplier, false, true);
            }
            if(curCMD.StartsWith("block"))
            {
                platformChar2D.block = true;
                platformChar2D.Move(0, false, false);
            }
            if (curCMD.StartsWith("grab"))
            {
                platformChar2D.grabAnim = true;
                platformChar2D.Move(0, false, false);
            }
            PerformConditional();
        }
        bool airAttackBypass = (curCMD.StartsWith("jump") || curCMD.StartsWith("air"));
        if ((platformChar2D.m_Grounded || airAttackBypass) && !platformChar2D.isHurt && !performOnce) PerformAttack();
    }

    void PerformConditional()
    {
        if (curCMD.StartsWith("if"))
        {
            int offset = (curCMD.StartsWith("ifelse")) ? 2 : 1;
            PlatformerCharacter2D oPlatChar2D = platformChar2D.opponent.GetComponent<PlatformerCharacter2D>();
            int oAtkType = oPlatChar2D.atkType;
            bool oWalkForward = oPlatChar2D.walkForward;
            bool oWalkBack = oPlatChar2D.walkBack;
            bool oHit = oPlatChar2D.isHurt;
            bool oGrounded = oPlatChar2D.m_Grounded;
            bool oCrouching = oPlatChar2D.isCrouching;
            bool oBlock = oPlatChar2D.block;
            float healthLeft = player.health;
            float oHealthLeft = platformChar2D.opponent.GetComponent<Player>().health;

            if (curCMD.Contains("oatk") && oAtkType > 0)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("owalkb") && oWalkBack)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("owalkf") && oWalkForward)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("ohit") && oHit)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("oblock") && oBlock)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("ogrounded") && oGrounded)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("oairborne") && !oGrounded)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("ocrouching") && oCrouching)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }
            if (curCMD.Contains("!ocrouching") && !oCrouching)
            {
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset];
                //print(curCMD.Substring(1));
            }

            if(curCMD.Contains("distgt"))
            {
                print("distance being checked");
                float dist = Mathf.Abs(transform.position.x - platformChar2D.opponent.transform.position.x);
                int distValue = Int32.Parse(Regex.Match(curCMD, @"distgt\d\d?").ToString().Substring(6));
                print("dist: " + dist + "distValue" + distValue);
                if (dist >= distValue)
                {
                    print(distValue);
                    // print(Int32.Parse(curCMD.Substring(8, 9)));
                    conditionalSatisfied = true;
                    print("distanceGT detected!");
                    string[] splitArr = curCMD.Split(',');
                    curCMD = splitArr[splitArr.Length - offset];
                }
            }

            if (curCMD.Contains("distlt"))
            {
                float dist = Mathf.Abs(transform.position.x - platformChar2D.opponent.transform.position.x);
                int distValue = Int32.Parse(Regex.Match(curCMD, @"distlt\d\d?").ToString().Substring(6));
                if (dist <= distValue)
                {
                    print(distValue);
                    // print(Int32.Parse(curCMD.Substring(8, 9)));
                    conditionalSatisfied = true;
                    print("distanceLT detected!");
                    string[] splitArr = curCMD.Split(',');
                    curCMD = splitArr[splitArr.Length - offset];
                }
            }

            if (curCMD.Contains("distlc"))
            {
                float dist = Mathf.Abs(transform.position.x - platformChar2D.leftCorner.position.x);
                int distValue = Int32.Parse(Regex.Match(curCMD, @"distlc\d\d?").ToString().Substring(6));
                if (dist <= distValue)
                {
                    print(distValue);
                    // print(Int32.Parse(curCMD.Substring(8, 9)));
                    conditionalSatisfied = true;
                    print("distanceLT detected!");
                    string[] splitArr = curCMD.Split(',');
                    curCMD = splitArr[splitArr.Length - offset];
                }
            }

            if (curCMD.Contains("distrc"))
            {
                float dist = Mathf.Abs(transform.position.x - platformChar2D.rightCorner.position.x);
                int distValue = Int32.Parse(Regex.Match(curCMD, @"distrc\d\d?").ToString().Substring(6));
                if (dist <= distValue)
                {
                    print(distValue);
                    // print(Int32.Parse(curCMD.Substring(8, 9)));
                    conditionalSatisfied = true;
                    print("distanceLT detected!");
                    string[] splitArr = curCMD.Split(',');
                    curCMD = splitArr[splitArr.Length - offset];
                }
            }

            if (curCMD.Contains("healthleft"))
            {
                int healthValue = Int32.Parse(Regex.Match(curCMD, @"healthleft\d\d?").ToString().Substring(11));
                if (healthLeft <= healthValue)
                {
                   // print(distValue);
                    // print(Int32.Parse(curCMD.Substring(8, 9)));
                    conditionalSatisfied = true;
                    print("distanceLT detected!");
                    string[] splitArr = curCMD.Split(',');
                    curCMD = splitArr[splitArr.Length - offset];
                }
            }

            if (curCMD.Contains("ohealthleft"))
            {
                int healthValue = Int32.Parse(Regex.Match(curCMD, @"ohealthleft\d\d?").ToString().Substring(12));
                if (oHealthLeft <= healthValue)
                {
                    // print(distValue);
                    // print(Int32.Parse(curCMD.Substring(8, 9)));
                    conditionalSatisfied = true;
                    print("distanceLT detected!");
                    string[] splitArr = curCMD.Split(',');
                    curCMD = splitArr[splitArr.Length - offset];
                }
            }


            //else statements lol
            if (!conditionalSatisfied && curCMD.StartsWith("ifelse"))
            {
               // print("else statement happened");
                conditionalSatisfied = true;
                string[] splitArr = curCMD.Split(',');
                curCMD = splitArr[splitArr.Length - offset + 1];
            }
        }
    }

    void PerformAttack()
    {

        string command = curCMD.Split('/')[0];
        switch (command)
        {
            case "jab":
                platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 1;
                player.damage = 1;
                player.hitstun = 0.5f;
                player.knockbackX = 50;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "M";
                break;
            case "overhead":
                platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 2;
                player.damage = 7;
                player.hitstun = 1f;
                player.knockbackX = 80;
                player.knockbackY = 0;
                platformChar2D.overhead = true;
                platformChar2D.atkGuardType = "H";
                break;
            case "kick":
                platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 3;
                player.damage = 4;
                player.hitstun = 0.8f;
                player.knockbackX = 70;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "M";
                break;
            case "highkick":
                platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 4;
                hitbox = GetComponent<HitBoxAttack>();
                player.damage = 6f;
                player.hitstun = 1f;
                player.knockbackX = 20;
                player.knockbackY = 500;
                platformChar2D.atkGuardType = "M";
                break;
            case "crouchjab":
                platformChar2D.Move(0, true, false);
                platformChar2D.atkType = 5;
                player.damage = 3;
                player.hitstun = 0.5f;
                player.knockbackX = 50;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "L";
                break;
            case "crouchaa":
                platformChar2D.Move(0, true, false);
                platformChar2D.atkType = 6;
                player.damage = 8;
                player.hitstun = 3f;
                player.knockbackX = 0;
                player.knockbackY = 40;
                platformChar2D.atkGuardType = "L";
                break;
            case "crouchsweep":
                platformChar2D.Move(0, true, false);
                platformChar2D.atkType = 7;
                player.damage = 4;
                player.hitstun = 0.5f;
                player.knockbackX = 0;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "L";
                break;
            case "crouchslide":
                int side = (platformChar2D.m_FacingRight)? 1: -1;
                platformChar2D.Move(side, true, false);
                platformChar2D.atkType = 8;
                player.damage = 2;
                player.hitstun = 0.3f;
                player.knockbackX = 30;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "L";
                break;
            case "airjab":
                //platformChar2D.Move(0, false, true);
                platformChar2D.atkType = 9;
                player.damage = 2;
                player.hitstun = 0.5f;
                player.knockbackX = 50;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "H";
                break;
            case "jumpjab":
                //platformChar2D.Move(0, false, true);
                platformChar2D.atkType = 9;
                player.damage = 2;
                player.hitstun = 0.5f;
                player.knockbackX = 50;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "H";
                break;
            case "airoverhead":
                // platformChar2D.Move(0, false, true);
                platformChar2D.atkType = 10;
                player.damage = 4;
                player.hitstun = 1f;
                player.knockbackX = 80;
                player.knockbackY = -30;
                platformChar2D.overhead = true;
                platformChar2D.atkGuardType = "H";
                break;
            case "jumpoverhead":
               // platformChar2D.Move(0, false, true);
                platformChar2D.atkType = 10;
                player.damage = 4;
                player.hitstun = 1f;
                player.knockbackX = 80;
                player.knockbackY = -30;
                platformChar2D.overhead = true;
                platformChar2D.atkGuardType = "H";
                break;
            case "airkick":
              //  platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 11;
                player.damage = 4;
                player.hitstun = 0.8f;
                player.knockbackX = 40;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "H";
                break;
            case "jumpkick":
                //  platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 11;
                player.damage = 4;
                player.hitstun = 0.8f;
                player.knockbackX = 40;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "H";
                break;
            case "airdropkick":
                // platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 12;
                hitbox = GetComponent<HitBoxAttack>();
                player.damage = 8f;
                player.hitstun = 1f;
                player.knockbackX = 20;
                player.knockbackY = -20;
                platformChar2D.atkGuardType = "H";
                break;
            case "jumpdropkick":
               // platformChar2D.Move(0, false, false);
                platformChar2D.atkType = 12;
                hitbox = GetComponent<HitBoxAttack>();
                player.damage = 8f;
                player.hitstun = 1f;
                player.knockbackX = 20;
                player.knockbackY = -20;
                platformChar2D.atkGuardType = "H";
                break;
            case "fireball":
                platformChar2D.atkType = 13;
                platformChar2D.stopSpecialCancel = true;
                performOnce = true;
                StartCoroutine(ShootProjectile(0.8f));
                StartCoroutine(StopSpecialCancel());
                break;
            case "dragonpunch":
                platformChar2D.Move(0, false, true);
                platformChar2D.atkType = 14;
                performOnce = true;
                platformChar2D.stopSpecialCancel = true;
                player.damage = 2f;
                player.hitstun = 0.8f;
                player.knockbackX = 140;
                player.knockbackY = 50;
                platformChar2D.atkGuardType = "M";
                StartCoroutine(StopSpecialCancel());
                break;
            case "spinkick":
                platformChar2D.atkType = 15;
                platformChar2D.stopSpecialCancel = true;
                performOnce = true;
                player.damage = 1f;
                player.hitstun = 0.8f;
                player.knockbackX = 5;
                player.knockbackY = 0;
                platformChar2D.atkGuardType = "M";
                StartCoroutine(StopSpecialCancel());
                break;
        }
    }


    public void GetScript()
    {
        if (player1) playerScript = GameObject.Find("FileManagerP1"); else playerScript = GameObject.Find("FileManagerP2");

        
        if (playerScript == null) Debug.Log("Uh Oh! FileManager Not Found, Script will not be imported!");
        if(useImportedScript) commandList = playerScript.GetComponent<FileManager>().importText;
    }

    public void SetPlayer1(bool p1)
    {
        player1 = p1;
    }

}
