using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    //关卡父物体
    public GameObject levelParent;
    //关卡预制体
    public LevelButton levelButtonPrefab;
    //支付窗口
    public GameObject payPanel;
    //设置按钮
    public Button settingButton;
    //设置窗口
    public GameObject settingPanel;

    public GameObject Guide;
    
    private void Start()
    {
        RegisterEvents();
        InitPlayerData();
        EventManager.Send(new EventConst.PlayBGM(){ SoundIndex = SoundManager.SoundIndex.MainPageBgm});
        InitLevelButtons();
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
        settingButton.onClick.AddListener(() =>
        {
            settingPanel.SetActive(true);
        });
        EventManager.Register<EventConst.StartGame>(EnterLevel);
    }
    
    /// <summary>
    /// 注销事件
    /// </summary>
    void UnRegisterEvents()
    {
        EventManager.UnRegister<EventConst.StartGame>(EnterLevel);
    }

    /// <summary>
    /// 初始化关卡按钮状态
    /// </summary>
    private void InitLevelButtons()
    {
        //本身有就直接更新状态
        if (levelParent.transform.childCount > 0)
        {
            LevelButton[] levelButtons = levelParent.transform.GetComponentsInChildren<LevelButton>();
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].SetButtonActive(i<=DataManager.userData.currentLevel-1);
            }
        }
        //没有就生成关卡按钮
        else
        {
            for (int i = 0; i < DataManager.Instance.maxLevel; i++)
            {
                LevelButton levelBtn = Instantiate(levelButtonPrefab,levelParent.transform);
                levelBtn.SetButtonActive(i<=DataManager.userData.currentLevel-1);
                levelBtn.SetButtonData(i + 1);
            }
        }
    }
    
    /// <summary>
    /// 进入关卡
    /// </summary>
    public void EnterLevel(EventConst.StartGame e)
    {
        //大于十关要付费解锁
        if (DataManager.Instance.selectedLevel >= 10 && DataManager.userData.isPayForGame == 0)
        {
            //弹出支付窗口
            payPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
    
    /// <summary>
    /// 初始化玩家引导
    /// </summary>
    private void InitPlayerData()
    {
        Guide.SetActive(DataManager.userData.isGuideComplete == 0);
    }
}
