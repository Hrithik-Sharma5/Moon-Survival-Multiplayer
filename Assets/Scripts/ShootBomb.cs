using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBomb : MonoBehaviour
{
    public void Start()
    {
        Invoke("Deactivate", 10);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.transform.tag.Equals("Player"))
        {
            Idamagable hit = other.gameObject.GetComponent<Idamagable>();
            hit.TakeDamage();
        }
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        
    }
}
