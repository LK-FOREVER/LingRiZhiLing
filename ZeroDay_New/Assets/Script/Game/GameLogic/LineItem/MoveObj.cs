using System;
using DG.Tweening;
using UnityEngine;

public class MoveObj : MonoBehaviour
{

    public bool arriveEnd = false;
    private bool alreadyArriveEnd = false;
    public GameObject thisDoor;
    private void OnMouseDown()
    {
        if (DataNode.anyPanelOpen)
        {
            return;
        }
        EventManager.Send(new EventConst.PlaySound(){ SoundIndex = SoundManager.SoundIndex.Click});
        MoveItem parentMoveItem = transform.parent.GetComponent<MoveItem>(); 
        if (!parentMoveItem.isMoving && !arriveEnd)
        {
            //先创建一个历史节点保存当前数据
            parentMoveItem.CreateHistoryNode();
            parentMoveItem.OnMove();
            EventManager.Send(new EventConst.OnceStep());
            
            //发送移动事件
            EventConst.MoveEvent moveEvent = new EventConst.MoveEvent();
            moveEvent.thisMoveItem = parentMoveItem;
            moveEvent.moveTogetherType = parentMoveItem.thisTogetherType;
            EventManager.Send(moveEvent);
        }
    }
    
    //碰撞后游戏失败
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.parent.GetComponent<MoveItem>().isMoving)
        {
            if (other.gameObject.CompareTag("MoveObj") )
            {
                EventManager.Send(new EventConst.GameOver());
                DOTween.KillAll();
            }
            if (other.gameObject.CompareTag("lineDoor") && other.gameObject == thisDoor)
            {
                if (!transform.parent.GetComponent<MoveItem>().doorOpen)
                {
                    EventManager.Send(new EventConst.GameOver());
                    DOTween.KillAll();
                }
                else
                {
                    other.gameObject.SetActive(false);
                }
            }
            if (other.gameObject.CompareTag("End") && transform.parent.GetComponent<MoveItem>().EndPoint == other.gameObject)
            {
                alreadyArriveEnd = true;
            }
        }
    }
}
