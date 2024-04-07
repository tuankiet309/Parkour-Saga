using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private int ammountOfCoins;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;
    void Start()
    {
        ammountOfCoins = Random.Range(minCoin, maxCoin);
        int additionalOffset = ammountOfCoins / 2;
        for(int i=0;i<ammountOfCoins; i++)
        {
            Vector3 offset = new Vector2(i-additionalOffset, 0);
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity,transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
