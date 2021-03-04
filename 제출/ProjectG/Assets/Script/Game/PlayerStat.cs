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

    [HideInInspector] public WeaponItemData equipWeapon;
    [HideInInspector] public ArmorItemData equipArmor;
    [HideInInspector] public Item[] invenItem = new Item[24];

    private Animator playerAnimator;
    private PlayerInput playerInput;

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

            UIManager.instance.UpdateGaugeRate(Define.Gauge.MP, _currentMP / maxMP);
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
                while (_currentEXP > maxEXP)
                {
                    _currentEXP -= maxEXP;
                    UpLevel();
                }
            }
            else if (value < 0)
                _currentEXP = 0;
            else
                _currentEXP = value;

            UIManager.instance.UpdateGaugeRate(Define.Gauge.EXP, _currentEXP / maxEXP);
        } 
    }

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        damagedTextColor = Color.red;

        InputManager.instance.keyAction -= OnItemKeyPress;
        InputManager.instance.keyAction += OnItemKeyPress;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public void Initializing(PlayerData data)
    {
        data.dataDictionary.TryGetValue("Level", out level);
        data.dataDictionary.TryGetValue("maxHP", out maxHP);
        data.dataDictionary.TryGetValue("maxMP", out maxMP);
        data.dataDictionary.TryGetValue("maxEXP", out maxEXP);
        data.dataDictionary.TryGetValue("currentHP", out float _currentHP);
        data.dataDictionary.TryGetValue("currentMP", out float _currentMP);
        data.dataDictionary.TryGetValue("currentEXP", out float _currentEXP);
        currentHP = _currentHP;
        currentMP = _currentMP;
        currentEXP = _currentEXP;

        data.dataDictionary.TryGetValue("attackDamage", out attackDamage);
        data.dataDictionary.TryGetValue("defense", out defense);
        data.dataDictionary.TryGetValue("attackRange", out attackRange);
        data.dataDictionary.TryGetValue("moveSpeed", out moveSpeed);
        data.dataDictionary.TryGetValue("rotateSpeed", out rotateSpeed);
        data.dataDictionary.TryGetValue("intvlAttackTime", out intvlAttackTime);
        data.dataDictionary.TryGetValue("gold", out gold);

        UIManager.instance.UpdateLevelText(level);
        UIManager.instance.UpdateGaugeRate(Define.Gauge.HP, currentHP / maxHP);
        UIManager.instance.UpdateGaugeRate(Define.Gauge.MP, currentMP / maxMP);
        UIManager.instance.UpdateGaugeRate(Define.Gauge.EXP, currentEXP / maxEXP);
    }

    public void GetExp(int newExp, int newGold)
    {
        currentEXP += newExp;
        gold += newGold;
    }

    private void UpLevel()
    {
        level++;

        maxHP += 10f;
        maxMP += 10f;

        currentHP = maxHP;
        currentMP = maxMP;

        UIManager.instance.UpdateLevelText(level);
        UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, currentHP / maxHP);
    }

    void Update()
    {
        
    }

    private void OnItemKeyPress()
    {
        if (playerInput.item1)
        {
            if (UIManager.instance.UseQuickSlot(Define.QuckSlot.Item_1))
                RestoreHP(20f);
        }
        if (playerInput.item2)
        {
            if (UIManager.instance.UseQuickSlot(Define.QuckSlot.Item_2))
                RestoreMP(20f);
        }
        if (playerInput.test1)
        {
            gold += 100;

            for (int i = 0; i < invenItem.Length; i++)
            {
                if (invenItem[i] == null)
                {
                    invenItem[i] = ItemManager.instance.FindItem(Define.ItemSort.Armor, Random.Range(0, 5));
                    invenItem[i].SetImage();
                    break;
                }
            }
        }
        if (playerInput.test2)
        {
            gold -= 100;

            for (int i = invenItem.Length - 1; i >= 0; i--)
            {
                if (invenItem[i] != null)
                {
                    invenItem[i] = null;
                    break;
                }
            }
        }
    }

    public override void RestoreHP(float newHP)
    {
        base.RestoreHP(newHP);
        UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, currentHP / maxHP);
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

        UIManager.instance.UpdateGaugeRate((int)Define.Gauge.HP, currentHP / maxHP);
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");
    }
}
