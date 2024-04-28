using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_main : MonoBehaviour
{
    private bool gamePaused = false;
    private bool gameMuted;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endgameUI;

    [Space] 
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Volumn Info")]
    [SerializeField] private UI_VolumnSilder[] slider;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image ingameMuteIcon;
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

        AudioManager.Instance.PlaySFX(3);
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }
    public void SwitchSkyBox(int index)
    {
        GameManager.Instance.SetupSkybox(index);
    }
    public void MuteButton()
    {
        gameMuted = !gameMuted;
        if(gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, .5f);
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }
    public void StartGame() 
    {
        muteIcon = ingameMuteIcon;
        if (gameMuted)
            muteIcon.color = new Color(1, 1, 1, .5f);

        GameManager.Instance.UnlockPlayer(); 
    }
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
    public void OpenEndGameUI()
    {
        SwitchMenuTo(endgameUI);
    }
}
