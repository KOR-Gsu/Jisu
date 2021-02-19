using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

[System.Serializable]
public class LogData
{
    public string id;
    public string pw;

    public LogData(string _id, string _pw)
    {
        id = _id;
        pw = _pw;
    }
}
[System.Serializable]
public class PlayerData
{
    public LogData log;
    public Dictionary<string, float> dataDictionary = new Dictionary<string, float>();

    public void PrintData()
    {
        Debug.Log(string.Format("id : {0}", log.id));
        Debug.Log(string.Format("pw : {0}", log.pw));

        foreach (var v in dataDictionary)
            Debug.Log(string.Format("{0} : {1}", v.Key, v.Value));
    }
}
[System.Serializable]
public class LogDataJson
{
    public Dictionary<string, LogData> logDataList = new Dictionary<string, LogData>();

    public void Add(LogData data)
    {
        logDataList.Add(data.id, data);
    }

    public bool IsData(string id)
    {
        return logDataList.ContainsKey(id);
    }

    public LogData FindLogData(string id)
    {
        LogData logData;

        logDataList.TryGetValue(id, out logData);

        return logData;
    }
}
[System.Serializable]
public class PlayerDataJson
{
    public Dictionary<string, PlayerData> playerDataDictionary = new Dictionary<string, PlayerData>();

    public void Add(PlayerData data)
    {
        playerDataDictionary.Add(data.log.id, data);
    }

    public bool IsData(string id)
    {
        return playerDataDictionary.ContainsKey(id);
    }

    public PlayerData FindPlayerData(string id)
    {
        PlayerData playerData;

        playerDataDictionary.TryGetValue(id, out playerData);

        return playerData;
    }
}

public class DataManager : MonoBehaviour
{
    private static DataManager m_instance;
    public static DataManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<DataManager>();

            return m_instance;
        }
    }

    private string filePath = "/SaveData";
    private string fileExtension = ".json";

    public string logFileName = "Log_Data";
    public string defaultPlayerDataFileName = "Player_Default";
    public string playerSavedDataFileName = "Player_Saved_Data";
    public string playerCurrentDataFileName = "Player_Save";
    public string monsterDataFileName = "Monster_Data";
    public string itemDataFileName = "Item_Data";

    public void DataToJson<T>(string filename, T data)
    {
        string saveString = JsonConvert.SerializeObject(data);
        byte[] saveData = Encoding.UTF8.GetBytes(saveString);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.dataPath + filePath, filename + fileExtension), FileMode.Create);
        fileStream.Write(saveData, 0, saveData.Length);
        fileStream.Close();
    }

    public T JsonToData<T>(string filename)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.dataPath + filePath, filename + fileExtension), FileMode.Open);
        byte[] loadData = new byte[fileStream.Length];
        fileStream.Read(loadData, 0, loadData.Length);
        fileStream.Close();

        string loadString = Encoding.UTF8.GetString(loadData);
        return JsonConvert.DeserializeObject<T>(loadString);
    }
}