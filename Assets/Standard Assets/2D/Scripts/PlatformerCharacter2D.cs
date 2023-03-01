using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 5f;                    // The fastest the player can travel in the x axis.
        [SerializeField] public float m_AirMultiplier = 1f;
        [SerializeField] private float m_JumpForce = 200f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        public bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        public bool m_FacingRight = true;  // For determining which way the player is currently facing.
        public bool hasJumped;
        public int atkType = 0;
        public bool isHurt = false;
        public bool walkForward = false;
        public bool walkBack = false;
        public bool block = false;
        public bool grabAnim = false;
        public bool throwAnim = false;
        public GameObject opponent;
        public bool freeze = false;
        public bool thrownAnim = false;
        public bool isCrouching = false;
        public bool overhead = false;
        public bool stopSpecialCancel;
        public GameObject canvasBoard; // variable
        public Transform leftCorner;
        public Transform rightCorner;
        public string atkGuardType;
        public bool dead = false;
        public bool deadAnim = false;
        public bool winAnim = false;
        [HideInInspector]
        public float moveSpeedMultiplier = 1f;
        [HideInInspector]
        public float attackSpeedMultiplier = 1f;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Anim.speed *= attackSpeedMultiplier;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            if (freeze) m_Rigidbody2D.velocity = Vector2.zero;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }


            if ((opponent.transform.position.x < transform.position.x && m_FacingRight || opponent.transform.position.x > transform.position.x && !m_FacingRight) && m_Grounded && atkType == 0 && !freeze && !throwAnim && !grabAnim && !block)
            {
                //print("flip has happened!");
                Flip();
            }

            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            m_Anim.SetInteger("Attack", atkType);
            m_Anim.SetBool("Hurt", isHurt);
            m_Anim.SetBool("Block", block);
            m_Anim.SetBool("Grab", grabAnim);
            m_Anim.SetBool("Throw", throwAnim);
            m_Anim.SetBool("Special Cancel", stopSpecialCancel);
            m_Anim.SetBool("Dead", deadAnim);
            m_Anim.SetBool("Win", winAnim);

            if(stopSpecialCancel)
            {
                isCrouching = false;
                isHurt = false;
                block = false;
                grabAnim = false;
            }


        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch")) //  && m_Anim.GetBool("Crouch")
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    isCrouching = true;
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
           m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move*moveSpeedMultiplier);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
               m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
                //print(move * m_MaxSpeed);

                // If the input is moving the player right and the player is facing left...
                /*
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                */
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground")) // && m_Anim.GetBool("Ground")
            {
                // Add a vertical force to the player.
                hasJumped = true;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
     
            }
        }


        public void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            canvasBoard.transform.localScale = theScale; // add this after your player transform.
        }

        public void Knockback(int side, float hknockbackAMT, float vknockbackAMT)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.AddForce(new Vector2(side * hknockbackAMT, vknockbackAMT));
        }

        public void FreezeMovement()
        {
            freeze = true;
           // Move(0, false, false);
        }

        public void SetPosition(float xOffset, float yOffset)
        {
            transform.position = new Vector2(opponent.transform.position.x + xOffset, opponent.transform.position.y + yOffset);
        }

        public void UnFreezeMovement()
        {
            freeze = false;
        }


    }
}
