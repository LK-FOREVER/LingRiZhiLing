using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayButtonSound);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(PlayButtonSound);
    }

    private void PlayButtonSound()
    {
        EventManager.Send(new EventConst.PlaySound(){ SoundIndex = SoundManager.SoundIndex.Click});
    }
}
