using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Movement : NetworkBehaviour
{

    [Header("OUT_Component")]
    GameObject Sword;


    [Header("IN_Component")]
    Rigidbody2D rb;
    CircleCollider2D circleCollider2D;
    Animator animator;


    [Header("IN_Variable")]
    [SerializeField] float _speed = 4f;
    float _speeddefault = 0f;
    Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();

        Sword = GameObject.FindGameObjectWithTag("Sword");
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            _Movement();

            // Set up Animation for Player 
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);


            // Set sword appear when Plaer move Up
            if (movement.y > 0.01f)
            {
                Sword.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
            else
            {
                Sword.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }

        }
        // Attack Function
        Attacking();
    }

    void _Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + movement * _speed * Time.fixedDeltaTime);

        // Idle on Verticle Dimension
        if (movement.y > 0)
        {
            animator.SetTrigger("isBehind");
            animator.ResetTrigger("isForward");
        }
        if (movement.y < 0)
        {
            animator.SetTrigger("isForward");
            animator.ResetTrigger("isBehind");
        }

        // Idle on Horizontal Demonsion
        if (movement.x > 0)
        {
            animator.SetTrigger("isRight");
            animator.ResetTrigger("isLeft");
        }
        if (movement.x < 0)
        {
            animator.SetTrigger("isLeft");
            animator.ResetTrigger("isRight");
        }
    }

    void Attacking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttacking");
            // when atk Player can't walk
            _speeddefault = _speed;
            _speed = 0;
        }


    }


    void StopAtkEventFuncion()
    {
        animator.ResetTrigger("isAttacking");

        _speed = _speeddefault;                                                     // Return value of _speed for Player
    }
}
