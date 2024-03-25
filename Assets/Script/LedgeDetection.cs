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
