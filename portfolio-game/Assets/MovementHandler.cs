using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementHandler : MonoBehaviour
{
    
    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sprite;
    [SerializeField] private float runSpeed;

    private Vector2 moveDirection;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveDirection.magnitude > 0){
            animator.SetInteger("AnimState", 1);
        }
        else {
            animator.SetInteger("AnimState", 0);
        }
    }

    public void Move(InputAction.CallbackContext context){
        moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0);
        if (moveDirection.x > 0){
            sprite.flipX = false;
        }
        else {
            sprite.flipX = true;
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + moveDirection * runSpeed * Time.deltaTime);        
    }
}
