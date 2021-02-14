using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    public float health { get; protected set; }
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
        isMarking = true;
        entityRenderer.material.color = color;
    }

    public virtual void UnMarking()
    {
        isMarking = false;
        entityRenderer.material.color = skinColor;
    }
}
