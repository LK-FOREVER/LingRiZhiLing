using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDataController
{
    public static void SaveData()
    {
        string gameData = FullSerializerAPI.Serialize(typeof(string), DataManager.userData);
        PlayerPrefs.SetString(DataManager.userData.Account, gameData);
        PlayerPrefs.Save();
    }
}
