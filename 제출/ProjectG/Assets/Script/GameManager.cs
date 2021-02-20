using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private LogData logData;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        FindObjectOfType<PlayerHP>().onDeath += EndGame;

        logData = DataManager.instance.currentLog;

        LoadPlayerData();
    }
    
    public void EndGame()
    {
        isGameOver = true;
    }

    public void SavePlayerData()
    {
        PlayerData savingPlayerData = new PlayerData();
        PlayerHP playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();

        savingPlayerData.log = logData;

        savingPlayerData.dataDictionary.Add("Level", playerHP.level);
        savingPlayerData.dataDictionary.Add("defense", playerHP.defense);
        savingPlayerData.dataDictionary.Add("currentHP", playerHP.currentHP);
        savingPlayerData.dataDictionary.Add("currentMP", playerHP.currentMP);
        savingPlayerData.dataDictionary.Add("currentEXP", playerHP.currentEXP);
        savingPlayerData.dataDictionary.Add("maxHP", playerHP.maxHP);
        savingPlayerData.dataDictionary.Add("maxMP", playerHP.maxMP);
        savingPlayerData.dataDictionary.Add("maxEXP", playerHP.maxEXP);
        savingPlayerData.dataDictionary.Add("moveSpeed", playerMove.moveSpeed);
        savingPlayerData.dataDictionary.Add("rotateSpeed", playerMove.rotateSpeed);
        savingPlayerData.dataDictionary.Add("attackDamage", playerMove.attackDamage);
        savingPlayerData.dataDictionary.Add("attackRange", playerMove.attackRange);
        savingPlayerData.dataDictionary.Add("intvlAttackTime", playerMove.intvlAttackTime);

        PlayerDataJson playerDataJson = DataManager.instance.JsonToData<PlayerDataJson>(DataManager.instance.playerSavedDataFileName);

        if (playerDataJson.playerDataDictionary.ContainsKey(logData.id))
        {
            foreach (var key in playerDataJson.playerDataDictionary.Keys.ToList())
            {
                if (key == logData.id)
                {
                    playerDataJson.playerDataDictionary[key] = savingPlayerData;
                    break;
                }
            }
        }
        else
            playerDataJson.Add(savingPlayerData);


        DataManager.instance.DataToJson<PlayerDataJson>(DataManager.instance.playerSavedDataFileName, playerDataJson);
    }

    public void LoadPlayerData()
    {
        PlayerDataJson playerDataJson = DataManager.instance.JsonToData<PlayerDataJson>(DataManager.instance.playerSavedDataFileName);

        if (!playerDataJson.playerDataDictionary.ContainsKey(logData.id))
            CreateNewPlayerData(playerDataJson);

        playerDataJson.playerDataDictionary.TryGetValue(logData.id, out PlayerData playerData);

        PlayerHP playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();

        playerHP.Initializing(playerData);
        playerMove.initializing(playerData);
    }

    private void CreateNewPlayerData(PlayerDataJson playerDataJson)
    {
        PlayerData newPlayerData = DataManager.instance.JsonToData<PlayerData>(DataManager.instance.playerDefaultDataFileName);

        newPlayerData.log = logData;
        playerDataJson.Add(newPlayerData);

        DataManager.instance.DataToJson<PlayerDataJson>(DataManager.instance.playerSavedDataFileName, playerDataJson);
    }
}