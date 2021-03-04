using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
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
        logData = DataManager.instance.currentLog;
#if UNITY_EDITOR
        logData = new LogData("test", "123");
#endif
        LoadPlayerData();
    }

    public void EndGame()
    {
        isGameOver = true;
    }

    public void SavePlayerData()
    {
        PlayerData savingPlayerData = CreateSavingPlayerData();
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

        PlayerStat playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();

        playerStat.Initializing(playerData);
    }

    private PlayerData CreateSavingPlayerData()
    {
        PlayerData playerData = new PlayerData();
        PlayerStat playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();

        playerData.log = logData;

        playerData.dataDictionary.Add("Level", playerStat.level);
        playerData.dataDictionary.Add("currentHP", playerStat.currentHP);
        playerData.dataDictionary.Add("currentMP", playerStat.currentMP);
        playerData.dataDictionary.Add("currentEXP", playerStat.currentEXP);
        playerData.dataDictionary.Add("maxHP", playerStat.maxHP);
        playerData.dataDictionary.Add("maxMP", playerStat.maxMP);
        playerData.dataDictionary.Add("maxEXP", playerStat.maxEXP);
        playerData.dataDictionary.Add("attackDamage", playerStat.attackDamage);
        playerData.dataDictionary.Add("defense", playerStat.defense);
        playerData.dataDictionary.Add("attackRange", playerStat.attackRange);
        playerData.dataDictionary.Add("moveSpeed", playerStat.moveSpeed);
        playerData.dataDictionary.Add("rotateSpeed", playerStat.rotateSpeed);
        playerData.dataDictionary.Add("intvlAttackTime", playerStat.intvlAttackTime);
        playerData.dataDictionary.Add("gold", playerStat.gold);

        return playerData;
    }

    private void CreateNewPlayerData(PlayerDataJson playerDataJson)
    {
        PlayerData newPlayerData = DataManager.instance.JsonToData<PlayerData>(DataManager.instance.playerDefaultDataFileName);

        newPlayerData.log = logData;
        playerDataJson.Add(newPlayerData);

        DataManager.instance.DataToJson<PlayerDataJson>(DataManager.instance.playerSavedDataFileName, playerDataJson);
    }
}