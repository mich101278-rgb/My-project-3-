using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private Vector2 moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool _jumpRequested;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        void Start()
        {
           
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _jumpRequested = true;
            }
        }


        void FixedUpdate()
        {
            isGrounded = IsGrounded();
            
                rigidbody.linearVelocity = new Vector2(
                    moveInput.x * movingSpeed,
                    rigidbody.linearVelocity.y
                    );
            
            

            if (_jumpRequested && IsGrounded())
            {
                rigidbody.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );
            }

            _jumpRequested = false;
            if (!isGrounded)
            {
                animator.SetInteger("playerState", 2);
            }
            else if (Mathf.Abs(moveInput.x) > 0.01f)
            {
                animator.SetInteger("playerState", 1);
            }
            else
            {
                animator.SetInteger("playerState", 0);
            }

            if (!facingRight && moveInput.x > 0.01f)
            {
                Flip();
            }
            else if (facingRight && moveInput.x < -0.01f)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private bool IsGrounded()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
            return isGrounded;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }
    }
}
