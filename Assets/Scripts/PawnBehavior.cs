using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnBehavior : MonoBehaviour, IDamageable
{
    public GameObject bulletPrefab;
    public Transform spawnBullet;
    public float health = 100f;
    public int ammo = 30;

    public void Shoot()
    {
        if (ammo > 0)
        {
            GameObject _bullet = Instantiate(bulletPrefab, spawnBullet.position, spawnBullet.rotation);     // Instancia a bala
            ammo--;
        }
    }

    public void TakeDamage(float _damage)   // IDamageable
    {
        Debug.Log($"Levou {_damage} de dano");
        health -= _damage;        
        if (health <= 0) { OnDeath(); }
    }

    private void OnDeath()
    {
        Debug.Log("Morreu");
        Destroy(this.gameObject);
    }

    public void Heal(float _health)     // IDamageable
    {
        Debug.Log($"Recebeu {_health} de vida");
        health += _health;
    }

    public void AddAmmo(int _ammo)      // IDamageable
    {
        Debug.Log($"Adicionou {_ammo} de municao");
        ammo += _ammo;
    }
}
