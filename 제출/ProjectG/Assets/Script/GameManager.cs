using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class PlayerData
{
    public Dictionary<string, string> logInfoDictionary = new Dictionary<string, string>();
    public Dictionary<string, float> dataDictionary = new Dictionary<string, float>();

    public void PrintData()
    {
        foreach(var v in logInfoDictionary)
            Debug.Log(string.Format("{0} : {1}", v.Key, v.Value));

        foreach (var v in dataDictionary)
            Debug.Log(string.Format("{0} : {1}", v.Key, v.Value));
    }
}


public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }
    public bool isGameOver { get; private set; }

    private string id = "default";
    private string pw = "123123";

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        FindObjectOfType<PlayerHP>().onDeath += EndGame;

        LoadPlayerInfoFromJson();
    }
    
    public void EndGame()
    {
        isGameOver = true;
    }

    public void SavePlayerInfoToJson()
    {
        PlayerData data = new PlayerData();
        PlayerHP playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();

        data.logInfoDictionary.Add("id", id);
        data.logInfoDictionary.Add("pw", pw);

        data.dataDictionary.Add("Level", playerHP.level);
        data.dataDictionary.Add("defense", playerHP.defense);
        data.dataDictionary.Add("currentHP", playerHP.currentHP);
        data.dataDictionary.Add("currentMP", playerHP.currentMP);
        data.dataDictionary.Add("currentEXP", playerHP.currentEXP);
        data.dataDictionary.Add("maxHP", playerHP.maxHP);
        data.dataDictionary.Add("maxMP", playerHP.maxMP);
        data.dataDictionary.Add("maxEXP", playerHP.maxEXP);
        data.dataDictionary.Add("moveSpeed", playerMove.moveSpeed);
        data.dataDictionary.Add("rotateSpeed", playerMove.rotateSpeed);
        data.dataDictionary.Add("attackDamage", playerMove.attackDamage);
        data.dataDictionary.Add("attackRange", playerMove.attackRange);
        data.dataDictionary.Add("intvlAttackTime", playerMove.intvlAttackTime);

        string saveString = JsonConvert.SerializeObject(data);
        byte[] saveData = Encoding.UTF8.GetBytes(saveString);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.dataPath, "Player_Save.json"), FileMode.Create);
        fileStream.Write(saveData, 0, saveData.Length);
        fileStream.Close();
    }

    public void LoadPlayerInfoFromJson()
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}", Application.dataPath, "Player_Save.json"), FileMode.Open);
        byte[] loadData = new byte[fileStream.Length];
        fileStream.Read(loadData, 0, loadData.Length);
        fileStream.Close();

        string loadString = Encoding.UTF8.GetString(loadData);
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(loadString);

        PlayerHP playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();

        data.logInfoDictionary.TryGetValue("id", out id);
        data.logInfoDictionary.TryGetValue("pw", out pw);

        playerHP.Initializing(data);
        playerMove.initializing(data);
    }
}
