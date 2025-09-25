using UnityEngine;

public class EventConst 
{
   /// <summary>
   /// 移动事件
   /// </summary>
   public class MoveEvent
   {
      public MoveItem thisMoveItem;
      public TogetherType moveTogetherType;
   }
   
   /// <summary>
   /// 游戏失败
   /// </summary>
   public class GameOver
   { }

   /// <summary>
   /// 重置游戏
   /// </summary>
   public class RestartGame
   { }
   
   /// <summary>
   /// 检测关卡是否通关
   /// </summary>
   public class CheckGameSuccess
   { }

   /// <summary>
   /// 游戏通关
   /// </summary>
   public class GameSuccess
   { }
   
   /// <summary>
   /// 切换关卡
   /// </summary>
   public class LoadNextLevel
   { }
   
   /// <summary>
   /// 点击一个物体进行移动的回调，表示一轮
   /// </summary>
   public class OnceStep
   { }
   
   
   /// <summary>
   /// 撤回操作
   /// </summary>
   public class BackStep
   { }

   /// <summary>
   /// 游戏暂停
   /// </summary>
   public class GamePause
   {
      public bool pause;
   }
   
   /// <summary>
   /// 开始游戏
   /// </summary>
   public class StartGame
   { }
   
   /// <summary>
   /// 在关卡中购买游戏
   /// </summary>
   public class BuyInGame
   { }
   
   /// <summary>
   /// 在关卡中取消购买游戏
   /// </summary>
   public class CancelBuyInGame
   { }

   /// <summary>
   /// 播放音乐
   /// </summary>
   public class PlayBGM
   {
      public SoundManager.SoundIndex SoundIndex;
   }

   /// <summary>
   /// 播放音效
   /// </summary>
   public class PlaySound
   {
      public SoundManager.SoundIndex SoundIndex;
   }

   /// <summary>
   /// 更改音效音量
   /// </summary>
   public class ChangeSoundVolume
   {
   }
   
   /// <summary>
   /// 更改BGM音量
   /// </summary>
   public class ChangeMusicVolume
   {
   }
}


