using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //所有移动物体
    private MoveItem[] moveItems;

    //当前游戏轮次
    private int step = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegisterEvents();
        InitLevel();
    }
    
    private void OnDestroy()
    {
        UnRegisterEvents();
    }

    private void InitLevel()
    {
        moveItems = transform.Find("MoveItems").GetComponentsInChildren<MoveItem>();
        for (int i = 0; i < moveItems.Length; i++)
        {
            moveItems[i].maskOrder = i;
            moveItems[i].InitMoveItem();
        }
        step = 0;
        HistoryController.InitHistory();
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    void RegisterEvents()
    {
        EventManager.Register<EventConst.MoveEvent>(MoveCallBack);
        EventManager.Register<EventConst.RestartGame>(RestartGame);
        EventManager.Register<EventConst.CheckGameSuccess>(CheckGameSuccess);
        EventManager.Register<EventConst.OnceStep>(CompleteStep);
        EventManager.Register<EventConst.BackStep>(BackStep);
    }
    
    /// <summary>
    /// 注销事件
    /// </summary>
    void UnRegisterEvents()
    {
        EventManager.UnRegister<EventConst.MoveEvent>(MoveCallBack);
        EventManager.UnRegister<EventConst.RestartGame>(RestartGame);
        EventManager.UnRegister<EventConst.CheckGameSuccess>(CheckGameSuccess);
        EventManager.UnRegister<EventConst.OnceStep>(CompleteStep);
        EventManager.UnRegister<EventConst.BackStep>(BackStep);
    }
    
    /// <summary>
    /// 完成本次所有移动
    /// </summary>
    private void MoveCallBack(EventConst.MoveEvent moveEvent)
    {
        //有相同类型的一起移动
        if (moveEvent.moveTogetherType != TogetherType.None)
        {
            MoveTogether(moveEvent);
        }
    }

    private void CompleteStep(EventConst.OnceStep stepEvent)
    {
        //每有一个点击事件，记录次数增加
        step++;
        HistoryController.GetStep(step);
        ChangeAllDoorState();
    }
    
    /// <summary>
    /// 更新全部门的状态
    /// </summary>
    private void ChangeAllDoorState()
    {
        foreach (MoveItem item in moveItems)
        {
            if (item.lineDoor!=null)
            {
                item.doorOpen = !item.doorOpen;
                item.ChangeDoorState();
            }
        }
    }

    /// <summary>
    /// 相同类型一起移动
    /// </summary>
    private void MoveTogether(EventConst.MoveEvent moveEvent)
    {
        foreach (MoveItem item in moveItems)
        {
            //类型相同且不能移动本身移动发送事件的MoveObj
            if (item.thisTogetherType == moveEvent.moveTogetherType && item != moveEvent.thisMoveItem)
            {
                item.CreateHistoryNode();
                HistoryController.GetStep(step);
                item.Move();
            }
        }
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    private void RestartGame(EventConst.RestartGame obj)
    {
        DOTween.KillAll();
        HistoryController.historyNodes.Clear();
        step = 0;
        for (int i = 0; i < moveItems.Length; i++)
        {
            moveItems[i].InitMoveItem();
        }
    }
    
    /// <summary>
    /// 检测游戏是否通关
    /// </summary>
    private void CheckGameSuccess(EventConst.CheckGameSuccess e)
    {   
        foreach (var item in moveItems)
        {
            if (!item.arriveEnd)
            {
                return;
            }
        }
        //完成关卡
        HistoryController.historyNodes.Clear();
        //如果选择的关卡大于当前最大关卡，再增加关卡数
        if (DataManager.Instance.selectedLevel >= DataManager.userData.currentLevel)
        {
            int currentLevel = DataManager.userData.currentLevel +1 ;
            DataManager.userData.currentLevel = currentLevel;
            SaveDataController.SaveData();
        }
        EventManager.Send(new EventConst.GameSuccess());
    }
    
    /// <summary>
    /// 回溯方法
    /// </summary>
    private void BackStep(EventConst.BackStep e)
    {
        if (HistoryController.historyNodes.Count == 0)
        {
            return;
        }
        //将门的状态再变换一次，回到上一轮
        ChangeAllDoorState();
        foreach (var node in HistoryController.historyNodes.ToList())
        {
            foreach (var item in moveItems)
            {
                //如果记录的名字一样，而且是上一轮进行完的轮次
                if (item.name == node.itemName && node.nodeStep == step)
                {
                    item.BackStep(node);
                    HistoryController.historyNodes.Remove(node);
                }
            }
        }
        //将轮次减1
        step--;
    }


}
