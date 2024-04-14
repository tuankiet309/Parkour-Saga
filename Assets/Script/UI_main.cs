using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_main : MonoBehaviour
{
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        uiMenu.SetActive(true);
    }
    public void StartGame() => GameManager.Instance.UnlockPlayer();
}
