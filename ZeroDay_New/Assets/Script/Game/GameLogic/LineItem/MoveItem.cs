using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 共同移动类型
/// </summary>
public enum TogetherType
{
    None = -1,
    Together1 = 0,
    Together2 = 1,
    Together3 = 2,
}

public class MoveItem : MonoBehaviour
{
    //移动的线
    public LineRenderer moveLine;
    //移动起点
    private Vector3 startPoint;
    //移动时长
    private float duration = 0.5f;
    //移动的点位计数
    private int moveCount = 0;
    //是否正在移动
    [HideInInspector]
    public bool isMoving = false;
    //移动的物体
    public GameObject thisMoveObj;
    //类型图片
    // public SpriteRenderer TypeObj;
    //线上的门
    public GameObject lineDoor;
    //移动轨迹消失（通过遮罩实现）
    public GameObject masks;
    //是否有stop设计
    public bool originStopState;
    //记录哪一个点位是停止点位
    public int stopPoint;
    //停止标识
    public GameObject stopObj;
    //共同移动类型
    public TogetherType thisTogetherType;
    //线上门的状态
    [HideInInspector]
    public bool doorOpen;
    //线上门初始状态
    public bool originDoorOpen = false;
    //线上开始是否有门
    public bool originHaveDoor = false;
    //是否到达终点
    public bool arriveEnd = false;
    //类型图片池
    public Sprite[] typeSprites;
    //记录已停止
    private bool cacheStop = false;
    //记录当前Mask已经到达的遮罩长度
    private float cacheStopOffset;
    //是否将要达到终点（已经向最后一个点位移动）
    private bool alreadyArriveEnd = false;
    //终点
    public GameObject EndPoint;
    //线
    public LineRenderer Line;
    //线
    public Sprite maskResource;
    //遮罩空间
    public int maskOrder;
    private void Start()
    {
        //指定起始点
        startPoint = moveLine.GetPosition(0);
        originDoorOpen = doorOpen;
        originHaveDoor = lineDoor.activeSelf;
        originStopState = stopObj.activeSelf;
        InitMoveItem();
    }

    /// <summary>
    /// 初始化移动物体
    /// </summary>
    public void InitMoveItem()
    {
        //归位
        thisMoveObj.transform.localPosition = startPoint;
        //设置Type类型
        if (thisTogetherType != TogetherType.None)
        {
            int typeNumber = (int)thisTogetherType;
            thisMoveObj.GetComponent<SpriteRenderer>().sprite = typeSprites[typeNumber];
        }
        //保留起始门配置信息
        doorOpen = originDoorOpen;
        //将运动轨迹重置
        for (int i = 0; i < masks.transform.childCount; i++)
        {
            masks.transform.GetChild(i).transform.localScale = new Vector3(1, 0, 1);
            SpriteMask thisMask = masks.transform.GetChild(i).GetComponent<SpriteMask>();
            thisMask.sprite = maskResource;
            thisMask.alphaCutoff = 1f;
            thisMask.isCustomRangeActive = true;
            thisMask.frontSortingLayerID = SortingLayer.NameToID("Line");
            thisMask.backSortingLayerID = SortingLayer.NameToID("Line");
            thisMask.frontSortingOrder = maskOrder+1;
            thisMask.backSortingOrder = maskOrder;
        }
        Line.sortingLayerName = "Line";
        Line.sortingOrder = maskOrder+1;
        EndPoint.GetComponent<SpriteRenderer>().sortingLayerName = "NormalImage";
        thisMoveObj.GetComponent<SpriteRenderer>().sortingLayerName = "NormalImage";
        arriveEnd = false;
        isMoving = false;
        cacheStop = false;
        cacheStopOffset = 0;
        moveCount = 0;
        alreadyArriveEnd = false;
        thisMoveObj.GetComponent<MoveObj>().arriveEnd = false;
        lineDoor.SetActive(originHaveDoor);
        if (originHaveDoor)
        {
            lineDoor.transform.GetChild(0).gameObject.SetActive(!originDoorOpen);
            lineDoor.transform.GetChild(1).gameObject.SetActive(originDoorOpen);
        }
        if (originStopState)
        {
            stopObj.SetActive(true);
        }
        ChangeDoorState();
    }
    
    //移动方法
    public void Move()
    {
        if (alreadyArriveEnd || DataNode.anyPanelOpen)
        {
            return;
        }
        moveCount++;
        int moveTargetIndex = moveCount;
        Vector3[] path = new Vector3[moveLine.positionCount];
        moveLine.GetPositions(path);
        thisMoveObj.transform.DOLocalMove(path[moveTargetIndex], duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            if (moveCount == stopPoint && originStopState)
            {
                stopObj.SetActive(false);
            }
            if (!arriveEnd && !(originStopState && moveCount == stopPoint))
            {
                Move();
            }
            //判断是否达到终点
            if (moveTargetIndex == path.Length - 1)
            {
                arriveEnd = true;
                thisMoveObj.GetComponent<MoveObj>().arriveEnd = true;
                EventManager.Send(new EventConst.PlaySound(){ SoundIndex = SoundManager.SoundIndex.ArriveEnd});
            }
            EventManager.Send(new EventConst.CheckGameSuccess());
            
        });
        
        //运动轨迹（遮罩）大小差值计算，然后将遮罩大小随MoveObj一起缩放
        float xOffset = path[moveTargetIndex].x - path[moveTargetIndex - 1].x;
        float yOffset = path[moveTargetIndex].y - path[moveTargetIndex - 1].y;
        float offset = 0;
        //如果有一方为0，则证明是纯横向/纵向运动，线长为差值
        if (xOffset == 0 || yOffset == 0)
        {
            offset = xOffset == 0? yOffset : xOffset;
        }
        //如果都不是0,则应该用两个差值，勾股定理计算第三边长度为线长
        else
        {
            //c = 根号下 a*a + b*b
            offset = (float)Math.Sqrt(xOffset * xOffset + yOffset * yOffset);
        }
        
        int useMaskIndex = moveCount - 1;
        //如果被stop之后，第一个Mask没有走完，第二步还是需要走第一个Mask
        if (cacheStop)
        {
            useMaskIndex = moveCount - 2;
            offset += cacheStopOffset;
            cacheStopOffset = 0;
        }
        masks.transform.GetChild(useMaskIndex).transform.DOScaleY(Math.Abs(offset),duration).SetEase(Ease.Linear);
        
        isMoving = true;
        
        //在第一次移动（假拦截后），stop消失
        if (moveCount == stopPoint && originStopState)
        {
            cacheStopOffset = offset;
            cacheStop = true;
        }
        
        //判断是否将要达到终点
        if (moveTargetIndex == path.Length - 1)
        {
            alreadyArriveEnd = true;
        }
    }
    
    public void OnMove()
    {
        //如果不在移动且没有到达终点，向下一个点位移动
        if (!isMoving && !arriveEnd)
        {
            Move();
        }
    }
    
    /// <summary>
    /// 改变门的状态
    /// </summary>
    public void ChangeDoorState()
    {
        lineDoor.transform.GetChild(0).gameObject.SetActive(!doorOpen);
        lineDoor.transform.GetChild(1).gameObject.SetActive(doorOpen);
    }

    /// <summary>
    /// 创建一个历史节点
    /// </summary>
    public void CreateHistoryNode()
    {
        MoveItemHistoryNode node = new MoveItemHistoryNode();
        node.itemName = name;
        node.moveCount = moveCount;
        node.doorOpen = doorOpen;
        node.arriveEnd = arriveEnd;
        node.cacheStop = cacheStop;
        node.cacheStopOffset = cacheStopOffset;
        node.alreadyArriveEnd = alreadyArriveEnd;
        node.isDoorActive = lineDoor.activeSelf;
        node.isStopActive = stopObj.activeSelf;
        node.moveObjPos = thisMoveObj.transform.localPosition;
        for (int i = 0; i < masks.transform.childCount; i++)
        {
            node.masksScale.Add(masks.transform.GetChild(i).localScale);
        }
        HistoryController.historyNodes.Add(node);
    }

    /// <summary>
    /// 回溯状态
    /// </summary>
    public void BackStep(MoveItemHistoryNode backNode)
    {
        isMoving = false;
        moveCount = backNode.moveCount;
        doorOpen =  backNode.doorOpen;
        arriveEnd = backNode.arriveEnd;
        thisMoveObj.GetComponent<MoveObj>().arriveEnd = arriveEnd;
        cacheStop = backNode.cacheStop;
        cacheStopOffset = backNode.cacheStopOffset;
        alreadyArriveEnd = backNode.alreadyArriveEnd;
        //还原运动轨迹
        for (int i = 0; i < masks.transform.childCount; i++)
        {
            masks.transform.GetChild(i).transform.localScale = backNode.masksScale[i];
        }
        //还原物体位置
        thisMoveObj.transform.localPosition = backNode.moveObjPos;
        lineDoor.SetActive(backNode.isDoorActive);
        stopObj.SetActive(backNode.isStopActive);
    }
}
