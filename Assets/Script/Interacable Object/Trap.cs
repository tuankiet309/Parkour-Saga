using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damage;
    [SerializeField] protected  float chancesToSpawn;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            Debug.Log("Die");
            collision.GetComponent<Player>().Damage();
        }    
    }
    protected virtual void Start()
    {
        bool canSpawn = chancesToSpawn >= Random.Range(0, 100);
        if(!canSpawn)
        {
            Destroy(gameObject);
        }
    }
}
