using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : LivingEntity
{
    public int level { get; protected set; }

    public float maxMP = 100;
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

    public int maxEXP = 80;
    private int _currentEXP;
    public int currentEXP
    {
        get { return _currentEXP; }
        set
        {
            if (value > maxEXP)
                UpLevel();
            else if (value < 0)
                _currentEXP = 0;
            else
                _currentEXP = value;

            UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_EXP, _currentEXP / maxEXP);
        } 
    }

    private Canvas damageCanvas;
    private Animator playerAnimator;
    private PlayerMove playerMove;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        level = 1;
        currentMP = maxMP;
        currentEXP = 30;

        playerMove.enabled = true;
    }

    public void GetExp(int newExp)
    {
        currentEXP += newExp;
    }

    private void UpLevel()
    {
        level++;
        maxHP += 10;
        maxMP += 10;

        currentHP = maxHP;
        currentMP = maxMP;
    }

    void Update()
    {

    }

    public override void RestoreHP(float newHP)
    {
        base.RestoreHP(newHP);
    }

    public void RestoreMP(float newMP)
    {
        if (!dead)
        {
            currentMP += newMP;
        }
    }

    public override bool OnDamage(float damage)
    {
        damageCanvas = GameObject.Find("UI").GetComponent<Canvas>();
        GameObject hudText = Instantiate<GameObject>(hudDamageTextPrefab, damageCanvas.transform);
        hudText.GetComponent<DamageText>().targetTransform = hudPos;
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.GetComponent<DamageText>().textColor = Color.red;

        bool die = false;
        if (base.OnDamage(damage))
        {
            die = true;
        }

        return die;
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMove.enabled = false;
    }
}
