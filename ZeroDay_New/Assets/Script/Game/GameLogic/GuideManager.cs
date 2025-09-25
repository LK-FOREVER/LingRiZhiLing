using UnityEngine;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    public GameObject GuideObj;
    public GameObject QuestionObj;
    public Image GuideImage;
    public Sprite[] GuideSprites;

    private int GuideStep = 0;
    private int QuestionStep = 0;
    
    public GameObject NextButton;
    public GameObject PreviousButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DataManager.userData.isGuideComplete == 0)
        {
            GuideStep = 0;
            GuideObj.SetActive(true);
            GuideImage.sprite = GuideSprites[0];
            EventConst.GamePause e =  new EventConst.GamePause();
            e.pause = true;
            EventManager.Send(e);
        }
        else
        {
            GuideObj.SetActive(false);
        }
    }

    public void NextStep()
    {
        if (DataManager.userData.isGuideComplete == 0)
        {
            GuideStep++;
            if (GuideStep <= GuideSprites.Length-1)
            {
                GuideImage.sprite = GuideSprites[GuideStep];
            }
            else
            {
                DataManager.userData.isGuideComplete = 1;
                SaveDataController.SaveData();
                GuideObj.SetActive(false);
                EventManager.Send(new EventConst.RestartGame());
            }
        }
    }

    public void QuestionButtonClick()
    {
        QuestionStep = 0;
        EventConst.GamePause e =  new EventConst.GamePause();
        e.pause = true;
        EventManager.Send(e);
        GuideImage.sprite = GuideSprites[0];
        GuideObj.SetActive(true);
        QuestionObj.SetActive(true);
        NextButton.SetActive(true);
        PreviousButton.SetActive(false);
    }

    public void NextButtonClick()
    {
        QuestionStep++;
        GuideImage.sprite = GuideSprites[QuestionStep];
        if (QuestionStep == GuideSprites.Length - 1)
        {
            NextButton.SetActive(false);
        }
        PreviousButton.SetActive(true);
    }

    public void PreviousButtonClick()
    {
        QuestionStep--;
        GuideImage.sprite = GuideSprites[QuestionStep];
        if (QuestionStep == 0)
        {
            PreviousButton.SetActive(false);
        }
        NextButton.SetActive(true);
    }
                                                                                                                                                               
    public void CloseButtonClick()
    {
        GuideObj.SetActive(false);
        QuestionObj.SetActive(false);
        EventConst.GamePause e =  new EventConst.GamePause();
        e.pause = false;
        EventManager.Send(e);
    }
}
