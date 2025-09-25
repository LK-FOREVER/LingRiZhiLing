using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelBase
{
   public Button closeButton;
   
   public Slider BGMSlider;
   
   public Slider SoundSlider;

   private void Start()
   {
      closeButton.onClick.AddListener(() =>
      {
         gameObject.SetActive(false);
      });
   }
   private void OnEnable()
   {
      BGMSlider.value = DataManager.userData.BGMVolume;
      SoundSlider.value = DataManager.userData.SoundVolume;
   }

   public void OnMusicSliderChangeValue()
   {
      DataManager.userData.BGMVolume = BGMSlider.value;
      SaveDataController.SaveData();
      EventManager.Send(new EventConst.ChangeMusicVolume(){});
   }

   public void OnSoundSliderChangeValue()
   {
      DataManager.userData.SoundVolume = SoundSlider.value;
      SaveDataController.SaveData();
      EventManager.Send(new EventConst.ChangeSoundVolume(){});
   }
   
}
