using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsCanvas : MonoBehaviour
{
    public Button QuitButton;
    void Start()
    {
        QuitButton.onClick.AddListener(QuitGame);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
    
}
