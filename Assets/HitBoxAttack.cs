using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

public class HitBoxAttack : MonoBehaviour
{

    public Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player _Opponentplayer = collision.GetComponentInParent<Player>();
       // print("collision with hitboc detected");
        if (_Opponentplayer != null)
        {
            if(!player.platformchar2D.grabAnim && !_Opponentplayer.platformchar2D.throwAnim)
            {
                _Opponentplayer.PlayerHurt(player);
               // _Opponentplayer.platformchar2D.isCrouching = false;
            } else if (!_Opponentplayer.platformchar2D.throwAnim && !player.platformchar2D.throwAnim && _Opponentplayer.platformchar2D.atkType == 0)
            {
                print("opponent has been grabbed, entering the throw!");
                player.platformchar2D.grabAnim = false;
                player.platformchar2D.throwAnim = true;
                player.Throw();
                int side = (_Opponentplayer.platformchar2D.m_FacingRight) ? 1 : -1;
                float damage = 10 * player.attackMultiplier;
                _Opponentplayer.Thrown(side, (int)damage);
            }

            //print("collision with other player detected!");
        }
    }
}
