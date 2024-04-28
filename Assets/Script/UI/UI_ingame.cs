using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_ingame : MonoBehaviour
{
    public Player player;
    [SerializeField] private TextMeshProUGUI distance;
    [SerializeField] private TextMeshProUGUI coins;

    [SerializeField] private Image heartFull;
    [SerializeField] private Image heartEmpty;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateInfo", 0, 0.15f);
    }

    // Update is called once per frame
    void UpdateInfo()
    {
        distance.text = GameManager.Instance.distance.ToString() +"m";
        coins.text = GameManager.Instance.coins.ToString();
        heartFull.enabled = player.extraLife;
    }
}
