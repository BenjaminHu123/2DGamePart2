using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour {

    public LayerMask enemyMask;
    public float speed = 1;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth;
    int rotat;
    private Animator myAnimator;
    // Use this for initialization
    void Start () {
        myTrans = this.transform;
        myBody = GetComponent<Rigidbody2D>();
        myWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        myAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate () {
        transform.rotation = Quaternion.Euler(0, rotat, 0);
        Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth;
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

        if (!isGrounded)
        {
            Vector3 currRot = myTrans.eulerAngles;
            rotat += 180;
            currRot.y = rotat;
            myTrans.eulerAngles = currRot;
            //multiply *= -1;
        }

        Vector2 myVel = myBody.velocity;
        myVel.x = -1 * myTrans.right.x * speed;
        myBody.velocity = myVel;

	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(BoxCollider2D) && 
            collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
