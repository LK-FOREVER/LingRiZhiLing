using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public class LoginParam
    {
        public string adult_level;
        public string is_holiday;
        public string user_id;
        public string nickname;
        public string timestamp;
    }
    
    //开始游戏按钮
    public Button startGameButton;

    public AdultTimer adultTimer;

    public List<string> SVIPAccount;//高级账号
    public List<string> VIPAccount;//中级账号

    private void Start()
    {
        SVIPAccount = new List<string>() { "t75870280", "z75870291","j75870297","t75870354","x75870361","d75870366" };
        VIPAccount = new List<string>() { "f75870256", "v75870265","w75870273","f75870324","q75870340","h75870347" };
        startGameButton.onClick.AddListener(OnBtnStart);
        Application.targetFrameRate = 280;
        EventManager.Send(new EventConst.PlayBGM(){ SoundIndex = SoundManager.SoundIndex.MainPageBgm});
    }
    
    public void OnBtnStart()
    {
#if UNITY_EDITOR
        string str =
            "{\"adult_level\":\"3\",\"is_holiday\":\"false\",\"user_id\":\"123456789\",\"nickname\":\"d75870366\",\"timestamp\":\"2024-11-19T09:15:00Z\"}";
        LoginCallBack(str);
#elif UNITY_ANDROID
        //Android平台调用SDK
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        unityActivity.Call("login");
#endif
    }

    public void LoginCallBack(string str)
    {
        LoginParam param = JsonConvert.DeserializeObject<LoginParam>(str);
        if (PlayerPrefs.HasKey(param.nickname))
        {
            //旧号
            DataManager.userData =
                FullSerializerAPI.Deserialize(typeof(DataManager.UserData), PlayerPrefs.GetString(param.nickname)) as DataManager.UserData;
        }
        //新号
        else
        {
            DataManager.userData = new DataManager.UserData();
        }

        DataManager.userData.Account = param.nickname;
        DataManager.userData.Age = Convert.ToInt32(param.adult_level);
        DataManager.userData.TimeStamp = param.timestamp;
        if (DataManager.userData.Age == 0 || DataManager.userData.Age == 1)
        {
            return;
        }
        else if (DataManager.userData.Age == 2)
        {
            adultTimer.StartTimer(DataManager.userData.TimeStamp);
        }
        InitPlayerData();
        LoadStartScene();
    }

    
    /// <summary>
    /// 初始化玩家数据
    /// </summary>
    private void InitPlayerData()
    {
        if (DataManager.userData.currentLevel == 0)
        {
            DataManager.userData.currentLevel = 1;
            DataManager.userData.BGMVolume = 1;
            DataManager.userData.SoundVolume = 1;
        }

        if (SVIPAccount.IndexOf(DataManager.userData.Account)!=-1)
        {
            DataManager.userData.isGuideComplete = 1;
            DataManager.userData.currentLevel = 50;
            DataManager.userData.isPayForGame = 1;
        }
        if(VIPAccount.IndexOf(DataManager.userData.Account)!=-1)
        {
            DataManager.userData.isGuideComplete = 1;
            DataManager.userData.currentLevel = 25;
            DataManager.userData.isPayForGame = 1;
        }
        
        SaveDataController.SaveData();
    }

    /// <summary>
    /// 加载关卡场景
    /// </summary>
    private void LoadStartScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
