﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [Header("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip enemyDieSound;
    [SerializeField] AudioClip enemyFireSound;
    [Range(0,1)] [SerializeField] float deathSoundVolume = 0.7f;


    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);        
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

        laser.transform.rotation = Quaternion.Euler(180, 0, 0);

        // negative speed to make it shoot down
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

        // PLay sound and store so it won't be destroyed
        AudioSource.PlayClipAtPoint(enemyFireSound, Camera.main.transform.position);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // protect vs null
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) { Die(); }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue); //add points for killing
        // PLay sound and store so it won't be destroyed
        AudioSource.PlayClipAtPoint(enemyDieSound, Camera.main.transform.position, deathSoundVolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);

        Destroy(gameObject);
        Destroy(explosion, durationOfExplosion);
    }
}
