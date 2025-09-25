using System.Collections.Generic;
using UnityEngine;

public static class HistoryController
{
    public static List<MoveItemHistoryNode> historyNodes = new List<MoveItemHistoryNode>();

    public static void GetStep(int step)
    {
        foreach (MoveItemHistoryNode node in historyNodes)
        {
            if (node.nodeStep == -1)
            {
                node.nodeStep = step;
            }
        }
    }
    
    public static void InitHistory()
    {
        historyNodes.Clear();
    }
}

//一个MoveItem的历史节点类，每次移动之前需要记录MoveItem的所有参数
public class MoveItemHistoryNode
{
    //当前所记录的对象名称
    public string itemName;
    //移动的点位计数
    public int moveCount = 0;
    //线上门的状态
    public bool doorOpen;
    //是否到达终点
    [HideInInspector]
    public bool arriveEnd = false;
    //记录已停止
    public bool cacheStop = false;
    //记录当前Mask已经到达的遮罩长度
    public float cacheStopOffset;
    //是否将要达到终点（已经向最后一个点位移动）
    public bool alreadyArriveEnd = false;
    //当前所有Line遮罩的大小
    public List<Vector3> masksScale = new List<Vector3>();
    //当前node的具体轮次
    public int nodeStep = -1;
    //当前物体所在位置
    public Vector3 moveObjPos;
    //门是否关闭的缓存数据
    public bool isDoorActive;
    //stop是否关闭的缓存数据
    public bool isStopActive;
}
