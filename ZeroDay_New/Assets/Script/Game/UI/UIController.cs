using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //游戏结束
    public GameObject gameOverPanel;

    //重置按钮
    public Button restartButton;
    
    //游戏成功
    public GameObject gameSuccessPanel;

    //下一关按钮
    public Button nextLevelButton;
    
    //撤回按钮
    public Button backStepButton;
    
    //暂停界面
    public GameObject pausePanel;
    
    //暂停按钮
    public Button pauseButton;
    
    //暂停界面，继续游戏按钮
    public Button pause_ContinueButton;
    
    //暂停界面，重新开始按钮
    public Button pause_RestartButton;
    
    //暂停界面，返回菜单按钮
    public Button pause_BackButton;
    
    //支付界面
    public GameObject payPanel;
    
    //设置按钮
    public Button settingButton;
    
    //支付界面
    public GameObject settingPanel;
    
    public TextMeshProUGUI levelText;
    
    private void Start()
    {
        RegisterEvents();
        ButtonUseful(true);
        InitUI();
    }

    private void InitUI()
    {
        RefreshLevelText();
        gameSuccessPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        UnRegisterEvents();
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    void RegisterEvents()
    {
        pauseButton.onClick.AddListener(() =>
        {
            EventConst.GamePause e =  new EventConst.GamePause();
            e.pause = true;
            EventManager.Send(e);
            pausePanel.SetActive(true);
        });
        pause_ContinueButton.onClick.AddListener(() =>
        {
            pausePanel.SetActive(false);
            EventConst.GamePause e =  new EventConst.GamePause();
            e.pause = false;
            EventManager.Send(e);
        });
        pause_BackButton.onClick.AddListener(BackToMenu);
        pause_RestartButton.onClick.AddListener(RestartGame);
        restartButton.onClick.AddListener(RestartGame);
        nextLevelButton.onClick.AddListener(NextLevel);
        settingButton.onClick.AddListener(() =>
        {
            settingPanel.SetActive(true);
        });
        backStepButton.onClick.AddListener(() =>
        {
            DOTween.KillAll();
            EventManager.Send(new EventConst.BackStep());
        });
        EventManager.Register<EventConst.GameOver>(ShowGameOver);
        EventManager.Register<EventConst.GameSuccess>(ShowGameSuccess);
        EventManager.Register<EventConst.BuyInGame>(AfterBuy);
        EventManager.Register<EventConst.CancelBuyInGame>(BackToMenu);
    }
    
    /// <summary>
    /// 注销事件
    /// </summary>
    void UnRegisterEvents()
    {
        EventManager.UnRegister<EventConst.GameOver>(ShowGameOver);
        EventManager.UnRegister<EventConst.GameSuccess>(ShowGameSuccess);
        EventManager.UnRegister<EventConst.BuyInGame>(AfterBuy);
        EventManager.UnRegister<EventConst.CancelBuyInGame>(BackToMenu);
    }

    //回调：返回菜单
    private void BackToMenu(EventConst.CancelBuyInGame e)
    {
        SceneManager.LoadSceneAsync(1);
    }

    //监听：返回菜单
    private void BackToMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    /// <summary>
    /// 显示游戏结束
    /// </summary>
    private void ShowGameOver(EventConst.GameOver obj)
    {
        gameOverPanel.SetActive(true);
        EventManager.Send(new EventConst.PlaySound(){ SoundIndex = SoundManager.SoundIndex.GameFailed});
        ButtonUseful(false);
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    private void RestartGame()
    {
        EventManager.Send(new EventConst.RestartGame());
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        ButtonUseful(true);
    }
    
    /// <summary>
    /// 游戏成功
    /// </summary>
    private void ShowGameSuccess(EventConst.GameSuccess obj)
    {
        gameSuccessPanel.SetActive(true);
        EventManager.Send(new EventConst.PlaySound(){ SoundIndex = SoundManager.SoundIndex.GameSuccess});
        ButtonUseful(false);
    }
    
        
    /// <summary>
    /// 切换关卡
    /// </summary>
    private void NextLevel()
    {
        DataManager.Instance.selectedLevel++;
        if (DataManager.Instance.selectedLevel>50)
        {
            DataManager.Instance.selectedLevel = 50;
        }
        if (DataManager.Instance.selectedLevel == 10 &&  PlayerPrefs.GetInt(DataManager.DataName.isPayForGame) != 1)
        {
            gameSuccessPanel.SetActive(false);
            payPanel.SetActive(true);
        }
        else
        {
            EventManager.Send(new EventConst.LoadNextLevel());
            gameSuccessPanel.SetActive(false);
            RefreshLevelText();
            ButtonUseful(true);
        }
        SaveDataController.SaveData();
    }

    //购买之后继续下一关
    void AfterBuy(EventConst.BuyInGame e)
    {
        EventManager.Send(new EventConst.LoadNextLevel());
        gameSuccessPanel.SetActive(false);
        RefreshLevelText();
        ButtonUseful(true);
    }

    private void ButtonUseful(bool useful)
    {
        pauseButton.interactable = useful;
        backStepButton.interactable = useful;
        settingButton.interactable = useful;
    }

    void RefreshLevelText()
    {
        levelText.text = $"第{DataManager.Instance.selectedLevel}关";
    }

}
