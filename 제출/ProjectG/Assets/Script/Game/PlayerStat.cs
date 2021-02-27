using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : LivingEntity
{
    [HideInInspector] public float level;
    [HideInInspector] public float maxMP;
    [HideInInspector] public float maxEXP;
    [HideInInspector] public float attackDamage;
    [HideInInspector] public float defense;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float intvlAttackTime;
    [HideInInspector] public float gold;

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

            UIManager.instance.UpdateGaugeRate(UIManager.GAUGE.MP, _currentMP / maxMP);
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

            UIManager.instance.UpdateGaugeRate(UIManager.GAUGE.EXP, _currentEXP / maxEXP);
        } 
    }

    private Animator playerAnimator;
    private PlayerMove playerMove;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        playerInput = GetComponent<PlayerInput>();

        damagedTextColor = Color.red;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.HP, currentHP / maxHP);

        playerMove.enabled = true;
    }

    public override void Initializing(PlayerData data)
    {
        base.Initializing(data);
        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.HP, currentHP / maxHP);

        data.dataDictionary.TryGetValue("Level", out level);
        UIManager.instance.UpdateLevelText(level);

        data.dataDictionary.TryGetValue("maxMP", out maxMP);
        data.dataDictionary.TryGetValue("maxEXP", out maxEXP);
        data.dataDictionary.TryGetValue("currentMP", out float tmpCurrentMP);
        data.dataDictionary.TryGetValue("currentEXP", out float tmpCurrentEXP);
        currentMP = tmpCurrentMP;
        currentEXP = tmpCurrentEXP;

        data.dataDictionary.TryGetValue("attackDamage", out attackDamage);
        data.dataDictionary.TryGetValue("defense", out defense);
        data.dataDictionary.TryGetValue("attackRange", out attackRange);
        data.dataDictionary.TryGetValue("moveSpeed", out moveSpeed);
        data.dataDictionary.TryGetValue("rotateSpeed", out rotateSpeed);
        data.dataDictionary.TryGetValue("intvlAttackTime", out intvlAttackTime);
        data.dataDictionary.TryGetValue("gold", out gold);
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

        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.HP, currentHP / maxHP);
    }

    void Update()
    {
        if (playerInput.item1)
        {
            if(UIManager.instance.UseQuickSlot(UIManager.QUICKSLOT.Item_1))
                RestoreHP(20f);
        }
        if (playerInput.item2)
        {
            if(UIManager.instance.UseQuickSlot(UIManager.QUICKSLOT.Item_2))
                RestoreMP(20f);
        }
    }

    public override void RestoreHP(float newHP)
    {
        base.RestoreHP(newHP);

        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.HP, currentHP / maxHP);
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

        base.OnDamage(finalDamage);

        UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.HP, currentHP / maxHP);
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMove.enabled = false;
    }
}
