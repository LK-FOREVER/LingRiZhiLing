using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdultTimer : MonoBehaviour
{
  public GameObject TipsUI;
  
  private void Awake()
  {
      DontDestroyOnLoad(this.gameObject);
  }

  public void StartTimer(string str)
  {
      DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      DateTime utcDateTime = epoch.AddSeconds(int.Parse(str));
      DateTime localDateTime = utcDateTime.ToLocalTime();
      int delayTime = (21 - localDateTime.Hour) * 60 * 60 + (0 - localDateTime.Minute) * 60 + (0 - localDateTime.Second);
      StartCoroutine(Timer(delayTime));
  }

  IEnumerator Timer(int time)
  {
    yield return new WaitForSeconds(time);
    Instantiate(TipsUI);
  }
}
