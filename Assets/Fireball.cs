using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [HideInInspector]
    public int side = 1;
    public float moveSpeed = 6f;
    public float lifetime = 10f;
    //public float damage;
    Rigidbody2D rb;
    public Player projectileOwner;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FireballMove();
    }

    public void FireballMove()
    {
        rb.AddForce(new Vector2(side * moveSpeed * 40, rb.velocity.y));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponentInParent<Player>();

        if(_player != null)
        {
            if(_player.platformchar2D.atkType != 15)
            {
                print("player has been hit");
                _player.PlayerHurt(projectileOwner);
            }

        }
        Destroy(this.gameObject);
    }
}
