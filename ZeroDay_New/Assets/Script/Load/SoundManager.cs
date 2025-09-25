using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public enum SoundIndex
   {
      MainPageBgm = 0,
      GameBgm,
      Click,
      ArriveEnd,
      GameSuccess,
      GameFailed
   }


   public AudioClip[] Music;
   public AudioSource BGMAudioSource;
   public AudioSource SoundAudioSource;
   
   private void Awake()
   {
      DontDestroyOnLoad(this.gameObject);
      EventManager.Register<EventConst.PlayBGM>(PlayBGM);
      EventManager.Register<EventConst.PlaySound>(PlaySound);
      EventManager.Register<EventConst.ChangeSoundVolume>(ChangeSoundVolume);
      EventManager.Register<EventConst.ChangeMusicVolume>(ChangeMusicVolume);
   }

   private void PlayBGM(EventConst.PlayBGM obj)
   {
      BGMAudioSource.Stop();
      BGMAudioSource.clip = Music[(int)obj.SoundIndex];
      BGMAudioSource.loop = true;
      BGMAudioSource.volume = DataManager.userData.BGMVolume;
      BGMAudioSource.Play();
   }
   
   private void PlaySound(EventConst.PlaySound obj)
   {
      SoundAudioSource.Stop();
      SoundAudioSource.clip = Music[(int)obj.SoundIndex];
      SoundAudioSource.loop = false;
      SoundAudioSource.volume = DataManager.userData.SoundVolume;
      SoundAudioSource.Play();
   }
   
   private void ChangeSoundVolume(EventConst.ChangeSoundVolume obj)
   {
      SoundAudioSource.volume = DataManager.userData.SoundVolume;
   }
   
   private void ChangeMusicVolume(EventConst.ChangeMusicVolume obj)
   {
      BGMAudioSource.volume = DataManager.userData.BGMVolume;
   }

}
