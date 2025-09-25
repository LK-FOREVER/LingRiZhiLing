using System;
using UnityEngine;

public class DataManager :MonoBehaviour
{
   static bool mIsDestroying;               //判断销毁的静态变量
   static DataManager mInstance;
   public static DataManager Instance
   {
      get
      {
         if (mInstance == null)
         {
            if (mIsDestroying)
            {
               Debug.LogWarning("[DataManager] Instance '" + typeof(DataManager) +
                                "' already destroyed. Returning null.");
               return null;     //如果已销毁则跳出，防止嵌套调用
            }
            mInstance = new GameObject("[DataManager]").AddComponent<DataManager>();
            DontDestroyOnLoad(mInstance.gameObject);  //创建实例并设置为DontDestroyOnLoad
         }
         return mInstance;
      }
   }
   
   void OnDestroy()
   {
      mIsDestroying = true;  //销毁时将标记变量设置为true，防止对已销毁单例进行重复创建
   }
   
   //选择的关卡
   public int selectedLevel;
   
   //游戏最大关卡
   public int maxLevel = 50;
   
   //玩家游戏数据
   public static UserData userData = new UserData();

   public class UserData
   {
      //玩家当前到达的关卡数
      public int currentLevel = 0;
      //是否已经重置解锁游戏(1为解锁)
      public int isPayForGame = 0;
      //是否进行了新手引导(1为完成)
      public int isGuideComplete = 0;
      //背景音乐音量
      public float BGMVolume = 1;
      //音效音量
      public float SoundVolume = 1;
      
      public string Account ;
      public int Age;
      public string TimeStamp;
   }
   
   public static class DataName
   {
      public static string currentLevel = "currentLevel";
      public static string isPayForGame = "isPayForGame";
      public static string isGuideComplete = "isGuideComplete";
      public static string BGMVolume = "BGMVolume";
      public static string SoundVolume = "SoundVolume";
   }
}
