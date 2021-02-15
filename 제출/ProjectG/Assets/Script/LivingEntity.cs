using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float maxHP = 100f;
    private float _currentHP;
    public float currentHP 
    {
        get{ return _currentHP; }
        set
        {
            if (value > maxHP)
                _currentHP = maxHP;
            else if (value < 0)
                Die();
            else
                _currentHP = value;
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
    private Canvas damagedCanvas;

    protected virtual void OnEnable()
    {
        dead = false;
        isMarking = false;
        currentHP = maxHP;

        entityRenderer = GetComponentInChildren<Renderer>();
        skinColor = entityRenderer.material.color;
    }

    public virtual void OnDamage(float damage)
    {
        currentHP -= damage;
    }

    public virtual void RestoreHP(float newHP)
    {
        if (!dead)
            currentHP += newHP;
    }

    public virtual void Die()
    {
        _currentHP = 0;

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

    public void ShowDamaged(float damage, Color color)
    {
        damagedCanvas = UIManager.instance.myCanvas;
        GameObject hudText = Instantiate<GameObject>(hudDamageTextPrefab, damagedCanvas.transform);
        hudText.GetComponent<DamageText>().targetTransform = hudPos;
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.GetComponent<DamageText>().textColor = color;
    }
}