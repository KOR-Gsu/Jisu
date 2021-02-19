using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;


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

        LoadPlayerData();
    }
    
    public void EndGame()
    {
        isGameOver = true;
    }

    public void SavePlayerData()
    {



        PlayerData playerData = new PlayerData();
        PlayerHP playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();

        playerData.log = logData;

        playerData.dataDictionary.Add("Level", playerHP.level);
        playerData.dataDictionary.Add("defense", playerHP.defense);
        playerData.dataDictionary.Add("currentHP", playerHP.currentHP);
        playerData.dataDictionary.Add("currentMP", playerHP.currentMP);
        playerData.dataDictionary.Add("currentEXP", playerHP.currentEXP);
        playerData.dataDictionary.Add("maxHP", playerHP.maxHP);
        playerData.dataDictionary.Add("maxMP", playerHP.maxMP);
        playerData.dataDictionary.Add("maxEXP", playerHP.maxEXP);
        playerData.dataDictionary.Add("moveSpeed", playerMove.moveSpeed);
        playerData.dataDictionary.Add("rotateSpeed", playerMove.rotateSpeed);
        playerData.dataDictionary.Add("attackDamage", playerMove.attackDamage);
        playerData.dataDictionary.Add("attackRange", playerMove.attackRange);
        playerData.dataDictionary.Add("intvlAttackTime", playerMove.intvlAttackTime);

        PlayerDataJson playerDataJson = new PlayerDataJson();
        playerDataJson.Add(playerData);
        DataManager.instance.DataToJson(DataManager.instance.playerSavedDataFileName, playerDataJson);

        DataManager.instance.DataToJson(DataManager.instance.playerCurrentDataFileName, playerData);
    }

    public void LoadPlayerData()
    {
        PlayerData playerData = DataManager.instance.JsonToData<PlayerData>(DataManager.instance.playerCurrentDataFileName);

        PlayerHP playerHP = GameObject.Find("Player").GetComponent<PlayerHP>();
        PlayerMove playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();

        logData = playerData.log;

        playerHP.Initializing(playerData);
        playerMove.initializing(playerData);
    }
}