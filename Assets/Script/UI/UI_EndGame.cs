using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distance;
    [SerializeField] protected TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI score;
    void Start()
    {
        GameManager manager = GameManager.Instance;
        Time.timeScale = 0;
        if (manager.distance <= 0)
            return;
        distance.text = "Distacne: " + manager.distance.ToString("#,#") + "m";
        if (manager.coins <= 0)
            return;
        coins.text = "Coins: " + manager.coins.ToString("#,#") ;
        score.text =  manager.score.ToString("#,#");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
