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
    public Slider magicSlider;
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
                
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        float curHp = health / startingHealth;
        UpdateHPSlider(curHp);
    }

    public override bool OnDamage(float damage)
    {
        bool die = false;

        if (base.OnDamage(damage))
            die = true;

        float curHp = health / startingHealth;
        UpdateHPSlider(curHp);

        playerAnimator.SetTrigger("Damaged");

        return die;
    }

    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger("Die");

        playerMove.enabled = false;
    }

    public void UpdateHPSlider(float curHp)
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, curHp, Time.deltaTime * 10);
    }

    private void UpdateMPSlider(float rate)
    {
        magicSlider.value = Mathf.Lerp(magicSlider.value, rate, Time.deltaTime * 10);
    }

    private void UpdateExpSlider(float rate)
    {
        expSlider.value = Mathf.Lerp(expSlider.value, rate, Time.deltaTime * 10);
    }
}
