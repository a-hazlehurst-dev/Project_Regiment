using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMenu : MonoBehaviour {

    public void OnMenuSelect(string text)
    {
        if(text == "Exit")
        {
            Application.Quit();
        }
        else if (text == "Continue")
        {
            GameManager.Instance.ContinueGame();
        }

        else if (text == "New Game")
        {
            GameManager.Instance.StartNewGame();
        }
    }
}
