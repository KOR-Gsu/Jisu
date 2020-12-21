using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;

    private bool isMarking;
    private Color skinColor;
    private Renderer entityRenderer;
    
    protected virtual void OnEnable()
    {
        dead = false;
        isMarking = false;
        health = startingHealth;

        entityRenderer = GetComponentInChildren<Renderer>();
        skinColor = entityRenderer.material.color;
    }

    public virtual bool OnDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();

            return true;
        }
        return false;
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (!dead)
            health += newHealth;
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
        entityRenderer.material.color = color;
    }

    public virtual void UnMarking()
    {
        entityRenderer.material.color = skinColor;
    }
}
