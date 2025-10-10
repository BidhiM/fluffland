using UnityEngine;

public class SizeIncFabStone : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool madeDynamic = false;

    private void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static; 
        //start it static, we'll make it dynamic when player hits it
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "stone"
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!madeDynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                madeDynamic = true;
            }
            rb.linearVelocityX = 1f; //push it forward so that the player doesn't get stuck   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("stone"))
            rb.bodyType = RigidbodyType2D.Static;
            //stop the stone once it hits the collider wehre it needs to stop
    }
}