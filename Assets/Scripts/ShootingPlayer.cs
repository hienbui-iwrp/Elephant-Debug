using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayer : MonoBehaviour
{
    // Store all kinds of bullet 
    /*
     * Note before start the game:
     * Check if the prefabs of 4 bullets is setup in "Player" or not
     * If not, click to the "Player", find this script (ShootingPlayer.cs)
     * Drag and drop each bullet prefab into suitable fields in the following order :
     * 1. bouncy bullet (prefab)
     * 2. piercing bullet (prefab)
     * 3. bouncy bullet power (prefab)
     * 4. piercing bullet power (prefab)
     */
    enum BULLET
    {
        NORMAL_BOUNCING = 0,
        NORMAL_PIERCING = 1,
        POWER_BOUNCING = 2,
        POWER_PIERCING = 3
    }
    // power energy
    public int power;
    // power timing
    private float powerTime;
    // Check if power is activated or not
    private bool isPower;
    // This is assigned a bullet outside
    public GameObject normal_bouncing;
    public GameObject normal_piercing;
    public GameObject power_bouncing;
    public GameObject power_piercing;
    // The position that the bullet is fired from
    public Transform firePosition;
    // The position that the bouncy bullet is fired
    public Transform firePositionLeft;
    public Transform firePositionRight;
    // List of bullet to shoot
    private List<GameObject> ListBullet;
    // Get mouse position
    Vector2 mousePos;
    private void Start()
    {
        // Add all bullets into ListBullet
        ListBullet = new List<GameObject>(4)
        {
            normal_bouncing,
            normal_piercing,
            power_bouncing,
            power_piercing
        };
        // Set power energy to 0
        power = 0;
        isPower = false;
    }
    // Update is called once per frame
    void Update()
    {
        // Check if the power is full
        if (power == Constant.MAX_POWER_ENERGY)
        {
            isPower = true;
        }
        if (isPower)
        {
            // Count the time special bullet is shot
            powerTime += Time.deltaTime;
            // Shoot special bullet
            PowerShoot();
            // Check if the time is out or not
            if (powerTime >= Constant.POWER_DURATION)
            {
                // Set all variables to origin
                power = 0;
                isPower = false;
                powerTime = 0;
            }
        }
        else
        {
            NormalShoot();
        }
    }

    // Used to shoot normal bullets
    void NormalShoot()
    {
        // Get Left-click
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (distance(mousePos.x, mousePos.y, firePosition.position.x, firePosition.position.y) > 0.5f && distance(mousePos.x, mousePos.y, transform.position.x, transform.position.y) > 0.5f)
            {
                ShootNormalBullet((int)BULLET.NORMAL_BOUNCING);
            }
        }
        // Get Right-Click
        else if (Input.GetMouseButtonDown(1))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (distance(mousePos.x, mousePos.y, firePosition.position.x, firePosition.position.y) > 0.5f && distance(mousePos.x, mousePos.y, transform.position.x, transform.position.y) > 0.5f)
            {
                ShootNormalBullet((int)BULLET.NORMAL_PIERCING);
            }
        }
    }

    // Used to shoot special bullets
    void PowerShoot()
    {
        // Get Left-Click
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (distance(mousePos.x, mousePos.y, firePosition.position.x, firePosition.position.y) > 0.5f && distance(mousePos.x, mousePos.y, transform.position.x, transform.position.y) > 0.5f)
            {
                ShootPowerBouncy((int)BULLET.POWER_BOUNCING);
            }
        }
        // Get Right-Click
        else if (Input.GetMouseButtonDown(1))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (distance(mousePos.x, mousePos.y, firePosition.position.x, firePosition.position.y) > 0.5f && distance(mousePos.x, mousePos.y, transform.position.x, transform.position.y) > 0.5f)
            {
                ShootPowerPiercing((int)BULLET.POWER_PIERCING);
            }
        }
    }

    // Only used to shoot normal bullet based on the idxBullet -> enum BULLET
    void ShootNormalBullet(int idxBullet)
    {
        // Access the bullet by idxBullet in ListBullet
        // And create it
        GameObject bullet = Instantiate(ListBullet[idxBullet], firePosition.position, firePosition.rotation);
        // Get the bullet's rigidbody2D 
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // Get the direction of the Mouse
        Vector2 dir = mousePos - rb.position;
        // Normalize the direction 
        dir.Normalize();
        // Fire the bullet
        rb.AddForce(dir * Constant.BULLET_SPEED, ForceMode2D.Impulse);
        // Shoot 1 bullet, power increase by 1
        //power++;
        // Destroy the bullet after 4 seconds
        //  Destroy(bullet, 4.0f);
    }

    void ShootPowerPiercing(int idxBullet)
    {
        // Create 5 bullets to be fired in 5 directions
        GameObject bullet1 = Instantiate(ListBullet[idxBullet], firePosition.position, firePosition.rotation);
        GameObject bullet2 = Instantiate(ListBullet[idxBullet], firePosition.position, firePosition.rotation);
        GameObject bullet3 = Instantiate(ListBullet[idxBullet], firePosition.position, firePosition.rotation);
        /*GameObject bullet4 = Instantiate(ListBullet[idxBullet], firePosition.position, firePosition.rotation);
        GameObject bullet5 = Instantiate(ListBullet[idxBullet], firePosition.position, firePosition.rotation);*/

        // Create 5 rigidbodies corresponding to 5 bullets
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
        Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
        /*Rigidbody2D rb4 = bullet4.GetComponent<Rigidbody2D>();
        Rigidbody2D rb5 = bullet5.GetComponent<Rigidbody2D>();*/

        // Initialize first direction ( straight )
        Vector2 dir1 = mousePos - rb1.position;

        // Initialize second direction ( left 1 )
        Vector2 dir2;
        dir2 = Quaternion.AngleAxis(transform.eulerAngles.x + 15, transform.position) * dir1;

        // Initialize third direction ( right 1 )
        Vector2 dir3;
        dir3 = Quaternion.AngleAxis(transform.eulerAngles.x - 15, transform.position) * dir1;

        // Initialize second direction ( left 2 )
        /*Vector2 dir4;
        dir4 = Quaternion.AngleAxis(transform.eulerAngles.x + 20, transform.position) * dir1;

        // Initialize third direction ( right 2 )
        Vector2 dir5;
        dir5 = Quaternion.AngleAxis(transform.eulerAngles.x - 20, transform.position) * dir1;*/

        // Normalize all direction to synchronize them
        dir1.Normalize();
        dir2.Normalize();
        dir3.Normalize();
      /*  dir4.Normalize();
        dir5.Normalize();*/

        // Fire the bullet
        rb1.AddForce(dir1 * Constant.BULLET_SPEED, ForceMode2D.Impulse);
        rb2.AddForce(dir2 * Constant.BULLET_SPEED, ForceMode2D.Impulse);
        rb3.AddForce(dir3 * Constant.BULLET_SPEED, ForceMode2D.Impulse);
        /*rb4.AddForce(dir4 * Constant.BULLET_SPEED, ForceMode2D.Impulse);
        rb5.AddForce(dir5 * Constant.BULLET_SPEED, ForceMode2D.Impulse);*/

        // Destroy after 4 seconds
        /*Destroy(bullet1, 4.0f);
        Destroy(bullet2, 4.0f);
        Destroy(bullet3, 4.0f);
        Destroy(bullet4, 4.0f);
        Destroy(bullet5, 4.0f);*/
    }

    void ShootPowerBouncy(int idxBullet)
    {
        // Create 5 bullets to be fired in 5 directions
        GameObject bullet1 = Instantiate(ListBullet[idxBullet], firePositionLeft.position, firePositionLeft.rotation);
        GameObject bullet2 = Instantiate(ListBullet[idxBullet], firePositionRight.position, firePositionRight.rotation);

        // Create 5 rigidbodies corresponding to 5 bullets
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();

        Vector2 dir = mousePos - rb1.position;
        dir.Normalize();
        // Initialize second direction ( left 1 )
        Vector2 dir1;
        dir1 = Quaternion.AngleAxis(transform.eulerAngles.x, transform.position) * dir;

        // Initialize third direction ( right 1 )
        Vector2 dir2;
        dir2 = Quaternion.AngleAxis(transform.eulerAngles.x, transform.position) * dir;

        // Normalize all direction to synchronize them
        dir1.Normalize();
        dir2.Normalize();

        // Fire the bullet
        rb1.AddForce(dir1 * Constant.BULLET_SPEED, ForceMode2D.Impulse);
        rb2.AddForce(dir2 * Constant.BULLET_SPEED, ForceMode2D.Impulse);

        // Destroy after 4 seconds
        /*Destroy(bullet1, 4.0f);
        Destroy(bullet2, 4.0f);*/
    }

    float distance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

}
