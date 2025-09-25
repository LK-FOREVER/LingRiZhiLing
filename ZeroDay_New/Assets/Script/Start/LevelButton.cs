using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    //关卡文本
    public TextMeshProUGUI levelText;
    
    public Button thisButton;

    public int thisLevel;

    public Sprite[] buttonSprites;
    

    //设置关卡按钮数据
    public void SetButtonData(int level)
    {
        levelText.text = level.ToString();
        thisLevel = level;
        thisButton.onClick.AddListener(() =>
        {
            DataManager.Instance.selectedLevel = thisLevel;
            EventManager.Send(new EventConst.StartGame());
        });
    }

    //设置按钮激活状态
    public void SetButtonActive(bool active)
    {
        thisButton.interactable = active;
        if (thisLevel<DataManager.userData.currentLevel-1)
        {
            thisButton.image.sprite =  buttonSprites[0];
        }
        else if (thisLevel == DataManager.userData.currentLevel-1)
        {
            thisButton.image.sprite =  buttonSprites[1];
        }
        else
        {
            thisButton.image.sprite =  buttonSprites[2];
        }
    }
}
