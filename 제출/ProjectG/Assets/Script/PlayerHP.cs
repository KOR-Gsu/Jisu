using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : LivingEntity
{
    [HideInInspector] public float level;
    [HideInInspector] public float defense;
    [HideInInspector] public float maxMP;
    [HideInInspector] public float maxEXP;
    
    private float _currentMP;
    public float currentMP 
    {
        get { return _currentMP; }
        set
        {
            if (value > maxMP)
                _currentMP = maxMP;
            else if (value < 0)
                _currentMP = 0;
            else
                _currentMP = value;

            UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_MP, _currentMP / maxMP);
        } 
    }

    private float _currentEXP;
    public float currentEXP
    {
        get { return _currentEXP; }
        set
        {
            if (value >= maxEXP)
            {
                _currentEXP = value;
                _currentEXP -= maxEXP;
                UpLevel();
            }
            else if (value < 0)
                _currentEXP = 0;
            else
                _currentEXP = value;

            UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_EXP, _currentEXP / maxEXP);
        } 
    }

    private float lastItemTime1;
    private float lastItemTime2;

    private float intvlItemTime1;
    private float intvlItemTime2;

    private Animator playerAnimator;
    private PlayerMove playerMove;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        playerInput = GetComponent<PlayerInput>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_HP, currentHP / maxHP);

        intvlItemTime1 = 0.8f;
        intvlItemTime2 = 0.8f;

        playerMove.enabled = true;
    }

    public override void Initializing(PlayerData data)
    {
        base.Initializing(data);
        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_HP, currentHP / maxHP);

        data.dataDictionary.TryGetValue("Level", out level);
        UIManager.instance.UpdateLevelText(level);

        data.dataDictionary.TryGetValue("defense", out defense);
        data.dataDictionary.TryGetValue("maxMP", out maxMP);
        data.dataDictionary.TryGetValue("maxEXP", out maxEXP);

        float tmpCurrentMP, tmpCurrentEXP;
        data.dataDictionary.TryGetValue("currentMP", out tmpCurrentMP);
        data.dataDictionary.TryGetValue("currentEXP", out tmpCurrentEXP);
        currentMP = tmpCurrentMP;
        currentEXP = tmpCurrentEXP;
    }

    public void GetExp(int newExp)
    {
        currentEXP += newExp;
    }

    private void UpLevel()
    {
        level++;
        UIManager.instance.UpdateLevelText(level);

        maxHP += 10f;
        maxMP += 10f;

        currentHP = maxHP;
        currentMP = maxMP;

        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_HP, currentHP / maxHP);
    }

    void Update()
    {
        if (playerInput.item1 && Time.time >= lastItemTime1 + intvlItemTime1)
        {
            RestoreHP(15f);
            lastItemTime1 = Time.time;
        }
        if (playerInput.item2 && Time.time >= lastItemTime2 + intvlItemTime2)
        {
            RestoreMP(15f);
            lastItemTime2 = Time.time;
        }
    }

    public override void RestoreHP(float newHP)
    {
        base.RestoreHP(newHP);

        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_HP, currentHP / maxHP);
    }

    public void RestoreMP(float newMP)
    {
        if (!dead)
        {
            currentMP += newMP;
        }
    }

    public override void OnDamage(float damage)
    {
        float finalDamage = damage - defense;
        if (finalDamage <= 0)
            finalDamage = 0;

        ShowDamaged(finalDamage, Color.red);

        base.OnDamage(finalDamage);

        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_HP, currentHP / maxHP);
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMove.enabled = false;
    }
}
