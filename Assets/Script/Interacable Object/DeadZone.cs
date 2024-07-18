using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            StartCoroutine(SlowDying());

        }
    }
    IEnumerator SlowDying()
    {       
        AudioManager.Instance.PlaySFX(4);
        yield return new WaitForSeconds(time);
        GameManager.Instance.GameEnded();
    }
    private void Update()
    {
        transform.position = new Vector2(player.transform.position.x,transform.position.y);
    }
}
