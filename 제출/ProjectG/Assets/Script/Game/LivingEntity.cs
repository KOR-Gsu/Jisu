using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    private Color skinColor;
    private Renderer entityRenderer;

    [SerializeField] private string damagedTextPrefabPath;

    public bool dead { get; protected set; }
    public bool isMarking { get; protected set; }
    public Color damagedTextColor { get; set; }
    public Transform hudPos;

    [HideInInspector] public float maxHP;
    private float _currentHP;
    public float currentHP 
    {
        get{ return _currentHP; }
        set
        {
            if (value > maxHP)
                _currentHP = maxHP;
            else if (value < 0)
                dead = true;
            else
                _currentHP = value;
        } 
    }
    
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
        ShowDamaged(damage, damagedTextColor);

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
        GameObject hudText = ResourceManager.instance.Instantiate(damagedTextPrefabPath, UIManager.instance.myCanvas.transform);
        hudText.GetComponent<DamageText>().targetTransform = hudPos;
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.GetComponent<DamageText>().textColor = color;
    }
}