using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_main : MonoBehaviour
{
    private bool gamePaused = false;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    private void Start()
    {
        SwitchMenuTo(mainMenu);
        Time.timeScale = 1;
        lastScoreText.text = "Last Score: " +PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highscoreText.text = "BestScore: " +PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        uiMenu.SetActive(true);
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }
    public void StartGame() => GameManager.Instance.UnlockPlayer();
    public void PauseGame()
    {
        if(gamePaused)
        {
            Time.timeScale = 1.0f;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }
}
