using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Idamagable
{
    [SerializeField]protected float totalHealth;
    [SerializeField]protected float damageHealth;
    [SerializeField]protected Slider healthSlider;
    [SerializeField] private GameObject enemyDieParticle;
    
    protected float currentHealth;

    void Start()
    {
        currentHealth = totalHealth;
        healthSlider.value = 1;
    }

    void Update()
    {
        
    }

    public void TakeDamage()
    {
        currentHealth -= totalHealth/damageHealth;
        healthSlider.value = currentHealth / totalHealth;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Instantiate(enemyDieParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
