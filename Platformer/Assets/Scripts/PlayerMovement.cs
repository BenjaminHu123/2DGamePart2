using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float movementSpeed;
    private bool facingRight;
    private Animator myAnimator;
    private bool attack;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;
    private bool jump;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private bool airControl;

    // Use this for initialization
    void Start() {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isGrounded = IsGrounded();
        HandleMovement(horizontal);
        Flip(horizontal);
        HandleAttacks();
        HandleLayers();
        ResetValues();
    }

    private void HandleMovement(float horizontal)
    {
        //Debug.Log(myRigidbody.velocity.y);
        if (IsGrounded() == false && myRigidbody.velocity.y < 0) {
            myAnimator.SetBool("land", true);
        }
        if ((isGrounded || airControl))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
            myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        }
        if (isGrounded && jump) {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }
    }

    private void HandleAttacks() {
        if (attack) {
            myAnimator.SetTrigger("attack");
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            attack = true;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            jump = true;
        }
    }

    private void Flip(float horizontal) {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
            facingRight = !facingRight;
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void ResetValues()
    {
        attack = false;
        jump = false;

    }

    private bool IsGrounded() {
        if (myRigidbody.velocity.y <= 0) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers() {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
            myAnimator.SetLayerWeight(1, 0);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.GetComponent<Animator>().Play("SnakeDie");
            collision.GetComponent<EnemyMove>().speed = 0;
            //collision.GetComponent<SpriteRenderer>().sortingOrder = 1;
            //if (collision.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SnakeDie"))
            collision.transform.gameObject.SetActive(false);
            //Destroy(collision);
        }
        if (collision.gameObject.CompareTag("winning"))
        {
            Debug.Log("ewr");
        }
    }

    private IEnumerator wait(float delay) {
        yield return new WaitForSeconds(delay);
    }
}
