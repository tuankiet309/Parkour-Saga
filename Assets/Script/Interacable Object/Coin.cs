using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()!=null)
        {
            GameManager.Instance.coins++;
            Destroy(gameObject);
            AudioManager.Instance.PlaySFX(0);
        }    
    }
}
