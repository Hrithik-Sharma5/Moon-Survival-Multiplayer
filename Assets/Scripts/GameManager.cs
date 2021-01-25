using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<GameObject> allPlayers= new List<GameObject>();
    public Transform[] spawnPos;
    public GameObject enemy;
    public Transform enemyContainer;

    [SerializeField]private GameObject projectileContainer;
    [SerializeField]private GameObject shootParticleContainer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject shootParticlePrefab;
    [SerializeField] private GameObject enemyPrefab;

    List<GameObject> enemyList = new List<GameObject>(); 

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        for (int i = 0; i < 25; i++)
        {
            PhotonNetwork.Instantiate(shootParticlePrefab.name, Vector3.zero,Quaternion.identity);
            PhotonNetwork.Instantiate(projectilePrefab.name, Vector3.zero,Quaternion.identity);
            PhotonNetwork.Instantiate(enemy.name, Vector3.zero, Quaternion.identity);
        }

        foreach (var item in spawnPos)
        {
            enemyPrefab = GetEnemyPool();
            if (enemyPrefab != null)
            {
                enemyPrefab.transform.position = item.position;
                enemyPrefab.SetActive(true);
                //Instantiate(enemy, spawnPos[randomPoint].position, Quaternion.identity, enemyContainer);
            }
        }

        StartCoroutine(SpawnEnemy());


    }

    IEnumerator SpawnEnemy()
    {
        GameObject enemyPrefab;
        while (true)
        {
            yield return new WaitForSeconds(10);
            int randomPoint = Random.Range(0, spawnPos.Length);
            enemyPrefab = GetEnemyPool();
            if (enemyPrefab != null)
            {
                enemyPrefab.transform.position = spawnPos[randomPoint].position;
                enemyPrefab.SetActive(true);
                //Instantiate(enemy, spawnPos[randomPoint].position, Quaternion.identity, enemyContainer);
            }
        }
    }

    GameObject GetEnemyPool()
    {
        foreach (var item in enemyList)
        {
            if (item.activeSelf) return item;
        }
        return null;
    }

}
