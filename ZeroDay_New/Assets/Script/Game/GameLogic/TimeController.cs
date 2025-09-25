using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TimeController : MonoBehaviour
{
    //是否暂停了游戏
    public bool isPaused = false;

    //时间限制
    public int timeLimit = 90;
    
    //剩余时间
    private int currentTime = 0;
    
    //时间限制文本
    public TextMeshProUGUI timeLimitText;

    private IEnumerator timerIE;
    
    private void Start()
    {
        RegisterEvents();
        ResetTimeLimit();
        timerIE = StartTimer();
        if (DataManager.userData.isGuideComplete == 1)
        {
            StartCoroutine(timerIE);
        }
    }

    private void OnDestroy()
    {
        UnRegisterEvents();
        StopAllCoroutines();
    }
    
    /// <summary>
    /// 注册事件
    /// </summary>
    void RegisterEvents()
    {
        EventManager.Register<EventConst.GamePause>(SetPause);
        EventManager.Register<EventConst.RestartGame>(EventRestartResetTimer);
        EventManager.Register<EventConst.GameSuccess>(StopTimer);
        EventManager.Register<EventConst.LoadNextLevel>(EventNextLevelTimer);
    }
    
    /// <summary>
    /// 注销事件
    /// </summary>
    void UnRegisterEvents()
    {
        EventManager.UnRegister<EventConst.GamePause>(SetPause);
        EventManager.UnRegister<EventConst.RestartGame>(EventRestartResetTimer);
        EventManager.UnRegister<EventConst.GameSuccess>(StopTimer);
        EventManager.UnRegister<EventConst.LoadNextLevel>(EventNextLevelTimer);
    }

    /// <summary>
    /// 重置计时时间
    /// </summary>
    void ResetTimeLimit()
    {
        timeLimit = 90;
        currentTime = timeLimit;
    }

    void EventRestartResetTimer(EventConst.RestartGame e)
    {
        isPaused = false;
        ResetTimer();
    }
    
    void EventNextLevelTimer(EventConst.LoadNextLevel e)
    {
        ResetTimer();
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    void ResetTimer()
    {
        ResetTimeLimit();
        StopCoroutine(timerIE);
        timerIE = StartTimer();
        StartCoroutine(timerIE);
    }
    
    /// <summary>
    /// 停止计时器
    /// </summary>
    void StopTimer(EventConst.GameSuccess e)
    {
        StopCoroutine(timerIE);
    }

    //开启计时器
    IEnumerator StartTimer()
    {
        ResetTimeLimit();
        for (int i = 0; i < timeLimit; i++)
        {
            //如果没有暂停，就读秒
            if (!isPaused && !DataNode.anyPanelOpen)
            {
                timeLimitText.text = currentTime.ToString();
                yield return new WaitForSeconds(1);
                currentTime--;
            }
            //如果暂停了，每过一秒就给时间限制再加一秒
            else
            {
                yield return new WaitForSeconds(1);
                timeLimit++;
            }
        }
        //如果时间用完了，游戏失败
        DOTween.KillAll();
        EventManager.Send(new EventConst.GameOver());
    }
    
    private void SetPause(EventConst.GamePause e)
    {
        isPaused = e.pause;
    }
    
}
