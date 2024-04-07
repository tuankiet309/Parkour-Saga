using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer headerSr;
    private BoxCollider2D bd;

    private void Start()
    {
        bd = GetComponent<BoxCollider2D>();
        bd.size = new Vector2(bd.size.x-0.1f,bd.size.y);
        headerSr.transform.parent = transform.parent;
        headerSr.transform.localScale = new Vector2(sr.bounds.size.x, .2f);
        headerSr.transform.position = new Vector2(transform.position.x, sr.bounds.max.y+sr.bounds.max.y*0.03f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            if(GameManager.Instance.colorEntilePlatform)
            {
                headerSr.color = GameManager.Instance.platformColor;
                sr.color = GameManager.Instance.platformColor;
            }
            else
            headerSr.color = GameManager.Instance.platformColor;
        }
    }
}
