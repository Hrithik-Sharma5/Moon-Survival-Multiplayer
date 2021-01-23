using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Idamagable
{
    [SerializeField]protected float totalHealth;
    [SerializeField]protected float damageHealth;
    [SerializeField]protected Slider healthSlider;
    [SerializeField]private GameObject enemyDieParticle;
    [SerializeField] private GameObject shootProjectile;
    [SerializeField] Transform shootingPoint;
    protected float currentHealth;
    private float enemyDistance;
    private GameObject closestEnemy;
    private NavMeshAgent navMesh;
    private Animator anim;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        anim=GetComponentInChildren<Animator>();
        currentHealth = totalHealth;
        healthSlider.value = 1;
        enemyDistance = Vector3.Distance(transform.position, GameManager.allPlayers[0].transform.position);
        closestEnemy = GameManager.allPlayers[0];
    }

    void Update()
    {
        FollowPlayer();
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

    void FollowPlayer()
    {
        foreach (GameObject enemy in GameManager.allPlayers)
        {
            float minDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (minDistance < enemyDistance)
            {
                enemyDistance = minDistance;
                closestEnemy = enemy;
            }
        }
        navMesh.SetDestination(closestEnemy.transform.position);

        if(Vector3.Distance(closestEnemy.transform.position, this.transform.position) < 12)
        {
            navMesh.speed = 0;
            Attack();
        }
        else
        {
            navMesh.speed = 4;
            anim.SetBool("Attack", false);
        }
    }

    void Attack()
    {
        transform.LookAt(closestEnemy.transform);
        anim.SetBool("Attack", true);
    }

    public void Shoot()
    {
        var projectile = Instantiate(shootProjectile, shootingPoint.position, Quaternion.identity);
        var direction = ((closestEnemy.transform.position+ (Vector3.up*2))- projectile.transform.position).normalized;

        projectile.GetComponent<Rigidbody>().AddForce(direction * 50 , ForceMode.Impulse);
        //projectile.GetComponent<ShootBomb>().StartSimulate(closestEnemy.transform);
        //throwProjectile.ProjectileSimulate(closestEnemy.transform, projectile.transform);
        //projectile.GetComponent<Rigidbody>().velocity = (closestEnemy.transform.position - this.transform.position).normalized*30;
        //projectile.GetComponent<Rigidbody>().AddForce((closestEnemy.transform.position - this.transform.position).normalized*50, ForceMode.Impulse);
    }

}
