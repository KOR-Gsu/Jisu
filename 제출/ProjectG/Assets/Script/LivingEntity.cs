using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float maxHP = 100;
    private float _currentHP;
    public float currentHP 
    {
        get{ return _currentHP; }
        set
        {
            if (value > maxHP)
                _currentHP = maxHP;
            else if (value < 0)
                _currentHP = 0;
            else
                _currentHP = value;

            UIManager.instance.UpdateGaugeRate((int)UIManager.GAUGE.GAUGE_HP, _currentHP / maxHP);
        } 
    }

    public bool dead { get; protected set; }
    public bool isMarking { get; protected set; }
    public event Action onDeath;
    public GameObject healthBarPrefab;
    public GameObject hudDamageTextPrefab;
    public Transform hudPos;

    private Color skinColor;
    private Renderer entityRenderer;

    protected virtual void OnEnable()
    {
        dead = false;
        isMarking = false;
        currentHP = maxHP;

        entityRenderer = GetComponentInChildren<Renderer>();
        skinColor = entityRenderer.material.color;
    }

    public virtual bool OnDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0 && !dead)
        {
            Die();

            return true;
        }
        return false;
    }

    public virtual void RestoreHP(float newHP)
    {
        if (!dead)
            currentHP += newHP;
    }

    public virtual void Die()
    {
        if (onDeath != null)
            onDeath();

        UnMarking();
        dead = true;
    }

    public virtual void Marking(Color color)
    {
        isMarking = true;
        entityRenderer.material.color = color;
    }

    public virtual void UnMarking()
    {
        isMarking = false;
        entityRenderer.material.color = skinColor;
    }
}