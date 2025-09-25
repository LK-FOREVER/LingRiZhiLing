using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] levels;

    public GameObject levelParent;

    public Image BG;
    
    public Sprite[] BGSprites;
    private void Start()
    {
        DestroyLevel();
        LoadLevel(new EventConst.LoadNextLevel());
        RegisterEvents();
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
        EventManager.Register<EventConst.LoadNextLevel>(LoadLevel);
    }
    
    /// <summary>
    /// 注销事件
    /// </summary>
    void UnRegisterEvents()
    {
        EventManager.UnRegister<EventConst.LoadNextLevel>(LoadLevel);
    }
    
    /// <summary>
    /// 加载下一关
    /// </summary>
    private void LoadLevel(EventConst.LoadNextLevel obj)
    {
        //生成下一关预制体
        DestroyLevel();
        if (DataManager.Instance.selectedLevel!=50)
        {
            BG.sprite = BGSprites[(DataManager.Instance.selectedLevel / 10)];
        }
        else
        {
            BG.sprite = BGSprites[4];
        }
        Instantiate(levels[DataManager.Instance.selectedLevel - 1], levelParent.transform, true);
        EventManager.Send(new EventConst.PlayBGM(){ SoundIndex = SoundManager.SoundIndex.GameBgm});
    }

    private void DestroyLevel()
    {
        for (int i = 0; i < levelParent.transform.childCount; i++)
        {
            Destroy(levelParent.transform.GetChild(i).gameObject);
        }
    }
}
