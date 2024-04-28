using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // A static reference to the GameManager instance
    public UI_main ui;
    public Player player;

    [Header("Color info")]
    public bool colorEntilePlatform = false;
    public Color platformColor;
    public Color playerColor = Color.white;
    [Header("Score info")]
    public float distance= 0;
    public int coins = 300;
    public float score = 0;
    void Awake()
    {
        /*if (Instance == null) // If there is no instance already
        {
            DontDestroyOnLoad(gameObject); // Keep the GameObject, this component is attached to, across different scenes
            Instance = this;
        }
        else if (Instance != this) // If there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        }*/
        Instance = this;
        //LoadColor();
        Time.timeScale = 1;


    }
    private void SaveColor(float r, float b, float g)
    {
        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorB", b);
        PlayerPrefs.SetFloat("ColorG", g);

    }
    private void LoadColor()
    {
        playerColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                PlayerPrefs.GetFloat("ColorG"),
                                PlayerPrefs.GetFloat("ColorB")
                                 );
        player.GetComponent<SpriteRenderer>().material.color = playerColor;
    }

    public void RestartScence() 
    {
        SaveInfo();
        SceneManager.LoadScene(0);
    } 
    public void UnlockPlayer() => player.playerUnlock = true;
    private void Update()
    {
        if(player.transform.position.x > distance)
        {
            distance = (int)player.transform.position.x;
        }
    }
    public void SaveInfo()
    {
        int savedCoins = PlayerPrefs.GetInt("Coins");
        score = coins * distance;
        PlayerPrefs.SetInt("Coins", coins + savedCoins);
        PlayerPrefs.SetFloat("LastScore", score);
        if (PlayerPrefs.GetFloat("HighScore") < score)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }
    public void GameEnded()
    {
        SaveInfo();
        ui.OpenEndGameUI();
    } 
}
