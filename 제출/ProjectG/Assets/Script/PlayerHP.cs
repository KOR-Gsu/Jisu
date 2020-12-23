using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : LivingEntity
{
    public int level { get; protected set; }
    public int startingMP = 100;
    public int MP { get; protected set; }
    public int maxExp = 100;
    public int exp { get; protected set; }

    public Text levelText;
    public Slider healthSlider;
    public Text healthText;
    public Slider magicSlider;
    public Text magicText;
    public Slider expSlider;

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
        MP = startingMP;
        exp = 30;

        levelText.text = level.ToString();

        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        magicSlider.maxValue = startingMP;
        magicSlider.value = MP;

        expSlider.maxValue = maxExp;
        expSlider.value = exp;

        healthText = healthSlider.GetComponentInChildren<Text>();
        healthText.text = "100%";
        magicText = magicSlider.GetComponentInChildren<Text>();
        magicText.text = "100%";

        playerMove.enabled = true;
    }

    public void GetExp(int newExp)
    {
        exp += newExp;

        if(exp >= maxExp)
        {
            exp -= maxExp;
            level++;
            startingHealth += 10;
            startingMP += 10;

            health = startingHealth;
            MP = startingMP;

            levelText.text = level.ToString();
            healthSlider.maxValue = startingHealth;
            healthSlider.value = health;
            magicSlider.maxValue = startingMP;
            magicSlider.value = MP;
        }

        expSlider.value = exp;
    }

    void Update()
    {
        float curHp = health / startingHealth;
        healthSlider.value = health;
        healthText.text = ((int)(curHp * 100)).ToString() + "%";

        float curMp = MP / startingMP;
        magicSlider.value = MP;
        magicText.text = ((int)(curMp * 100)).ToString() + "%";
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);        
    }

    public override bool OnDamage(float damage)
    {
        bool die = false;

        if (base.OnDamage(damage))
            die = true;

        playerAnimator.SetTrigger("Damaged");

        return die;
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMove.enabled = false;
    }
}
