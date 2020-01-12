using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player")]
    // Game Settings (speed for player)
    [SerializeField] float moveSpeed = 10f;
    // Padding from Borders
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;


    [Header("Projectile")]
    // Load the Laser button
    [SerializeField] GameObject laserPrefab;
    // Define Vertical Speed
    [SerializeField] float projectileSpeed = 10f;
    // Define Duration of Fire
    [SerializeField] float projectileFiringPeriod = 0.1f;

    // Hold a Coroutine (handle)
    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

    }

    //This is a Coroutine
    IEnumerator FireContinuously()
    {
        // While spacebar is held (or Sub being called)
        while (true)
        {
            //Q.id = use current rotation (assign to var: laser)
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            //Remember laser needs to be a rigid body (hint: change body type to kinematic)
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            //Do (separately) and return control meanwhile
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        NewMethod(damageDealer);

    }

    private void NewMethod(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        if (health <= 0) { Destroy(gameObject); }
    }


    private void Move()
    {
        // refer to Edit>ProjSettings>Input (Using Keyboard arrows or alt)
        // deltaTime used to make speed/distance frame independent.
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        // define NewPosition (and clap limits)
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        // move sideways
        transform.position = new Vector2(newXPos, newYPos);
    }



    private void SetupMoveBoundaries()
    {
        // What camera
        Camera gameCamera = Camera.main;
        // Get The value of the X element for our Viewport from Camera
        // Refer to slides to understand Viewport pos (0,0:1,0:1,1:0,1)
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

}