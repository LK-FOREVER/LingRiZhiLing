using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanel : PanelBase
{
    public Button buyButton;

    public Button cancelButton;


    private void Start()
    {
        buyButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            DataManager.userData.isPayForGame = 1;
            SaveDataController.SaveData();
            EventManager.Send(new EventConst.BuyInGame());
        });
        
        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            EventManager.Send(new EventConst.CancelBuyInGame());
        });
    }
}
