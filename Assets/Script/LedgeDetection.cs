using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float radius;
    [SerializeField] private Player player;
    [SerializeField] private LayerMask layerMask;
    private bool canDetect =true ;

    private BoxCollider2D boxCollider2D =>GetComponent<BoxCollider2D>();

    // Update is called once per frame
    void Update()
    {
        if(canDetect)
        player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius,layerMask); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            canDetect = false ; 
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(boxCollider2D.bounds.center, boxCollider2D.size, 0);
        foreach(var collider2d in collider)
        {
            if(collider2d.gameObject.tag == "Platform")
            {
                return;
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            canDetect = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
