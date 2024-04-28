using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct ColorToSell
{
    public Color color;
    public int price;
}
public enum ColorType
{
    playerColor,
    platformColor,
}
public class UI_Shope : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI notifyText;
    [Space]

    [SerializeField] private ColorToSell[] platformColor;
    [SerializeField] private ColorToSell[] playerColor;
    [Space]
    [SerializeField] private GameObject platformToColor;
    [SerializeField] private Transform platformColorParent;
    [SerializeField] private Image platformDisplay;
    [Space]
    [SerializeField] private GameObject playerToColor;
    [SerializeField] private Transform playerColorParent;
    [SerializeField] private Image playerDisplay;
    private void Start()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
        for (int i = 0; i < platformColor.Length; i++)
        {
            Color color = platformColor[i].color;
            int price = platformColor[i].price;
            GameObject newButton = Instantiate(platformToColor, platformColorParent);
            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#");
            newButton.GetComponent<Button>().onClick.AddListener( () => PurchaseColor(color,price,ColorType.platformColor) );
        }
        for (int i = 0; i < playerColor.Length; i++)
        {
            Color color = playerColor[i].color;
            int price = playerColor[i].price;
            GameObject newButton = Instantiate(playerToColor, playerColorParent);
            newButton.transform.GetChild(0).GetComponent<Image>().color = color;
            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString("#,#");
            newButton.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(color, price, ColorType.playerColor));
        }
    }
    public void PurchaseColor(Color color, int price, ColorType colorType)
    {
        AudioManager.Instance.PlaySFX(3);
        if (EnoughMoney(price))
        {
            if (colorType == ColorType.platformColor)
            {
                GameManager.Instance.platformColor = color;
                platformDisplay.color = color;
            }
            else if(colorType == ColorType.playerColor)
            {
                GameManager.Instance.player.GetComponent<SpriteRenderer>().color = color;
                playerDisplay.GetComponent<Image>().color = color;
            }
            StartCoroutine(Notify("Purchase Successful !",1));
            coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
        }
        else
        {
            StartCoroutine(Notify("Not enough money !", 1));

        }
    }
    private bool EnoughMoney(int price)
    {

        int mycoins = PlayerPrefs.GetInt("Coins");
        if(mycoins > price)
        {
            int newAmountOfCoins = mycoins - price;
            PlayerPrefs.SetInt("Coins", newAmountOfCoins);
            transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coins").ToString("#,#");
            return true;
        }
        return false;
    }
    IEnumerator Notify(string text, float second)
    {
        notifyText.text = text;
        yield return new WaitForSeconds(second);
        notifyText.text = "CLICK TO BUY";
    }
}
