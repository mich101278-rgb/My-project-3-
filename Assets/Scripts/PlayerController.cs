using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public event Action Died;
        public GameObject deathPrefab;
        public float movingSpeed;
        public float jumpForce;
        private Vector2 moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool isDied = false;

        private bool _jumpRequested;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;

        private void Die()
        {
            if (isDied)
            {
                return;
            }
            isDied = true;
            GameObject deathPlayer = (GameObject)Instantiate(deathPrefab, transform.position, transform.rotation);
            deathPlayer.transform.localScale = transform.localScale;
            gameObject.SetActive (false);
            Died?.Invoke();
        }
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

            float speed = Mathf.Abs(moveInput.x);

            animator.SetFloat("Speed", speed);

            if (_jumpRequested && IsGrounded())
            {
                rigidbody.AddForce(
            Vector2.up * jumpForce,
            ForceMode2D.Impulse
        );
                animator.SetTrigger("Jump");

            }

            _jumpRequested = false;


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
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log($"Player collided with: {other.gameObject.name}, tag: {other.gameObject.tag}");
                Die();
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
