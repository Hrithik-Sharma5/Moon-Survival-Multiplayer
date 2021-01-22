using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Idamagable
{
    [SerializeField]protected float totalHealth;
    [SerializeField]protected float damageHealth;
    [SerializeField]protected Slider healthSlider;
    
    protected float currentHealth;

    void Start()
    {
        currentHealth = totalHealth;
        healthSlider.value = 1;
        Debug.Log("sasasassssssssss");
    }

    void Update()
    {
        
    }

    public void TakeDamage()
    {
        currentHealth -= totalHealth/damageHealth;
        healthSlider.value = currentHealth / totalHealth;
    }
}
