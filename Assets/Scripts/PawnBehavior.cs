using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnBehavior : MonoBehaviour, IDamageable
{
    public GameObject bulletPrefab;
    public Transform spawnBullet;
    public float currentHealth = 100f;
    public int currentAmmo = 30;

    public void Shoot()
    {
        // Atira se tiver municao
        if (currentAmmo > 0)
        {
            Instantiate(bulletPrefab, spawnBullet.position, spawnBullet.rotation);
            currentAmmo--;
        }
    }

    public void TakeDamage(float damage)   // IDamageable
    {
        Debug.Log($"Levou {damage} de dano");
        currentHealth -= damage;        
        if (currentHealth <= 0) { OnDeath(); }
    }

    private void OnDeath()
    {
        Debug.Log("Morreu");
        Destroy(this.gameObject);
    }

    public void Heal(float health)     // IDamageable
    {
        Debug.Log($"Recebeu {health} de vida");
        currentHealth += health;
    }

    public void AddAmmo(int ammo)      // IDamageable
    {
        Debug.Log($"Adicionou {ammo} de municao");
        currentAmmo += ammo;
    }
}
