using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStop : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("movingPlat"))
        {
            collision.GetComponent<MovingPlatform>().velocity.x = 0;
        }
    }
}
