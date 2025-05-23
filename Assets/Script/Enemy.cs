using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int damage;
    public float flashtime;
    public GameObject bloodEffect;
    public GameObject dropCoin;


    private SpriteRenderer sr;
    private Color originalColor;
    private PlayerHealth playerHealth;
    public void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 0)
        {
            Instantiate(dropCoin, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        FlashColor(flashtime);
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        GameController.camShake.Shake();
    }
    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor",time);
    }
    void ResetColor()
    {
        sr.color = originalColor;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damage);
            }
    }
}
