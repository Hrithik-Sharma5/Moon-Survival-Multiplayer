using UnityEngine;
using System.Collections;

public class ThrowProjectile : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();    
    }

    public void ShootWeapon()
    {
        enemy.Shoot();
    }
}
