using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    public float attackTime;
    public float startTime;

    private Animator anim;
    private PolygonCollider2D collider2D;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        collider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    void Attack() 
    {
        if (Input.GetButtonDown("Attack"))
        {
            Debug.Log("is Attcked");
            collider2D.enabled = true;
            anim.SetTrigger("Attack");
            StartCoroutine(StartAttack());
        }

    }
    IEnumerator StartAttack() 
    {
        yield return new WaitForSeconds(startTime);
        collider2D.enabled = true;
        StartCoroutine(disableHitBox());
    }
    IEnumerator disableHitBox() 
    {
        yield return new WaitForSeconds(attackTime);
        collider2D.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            other.GetComponent<Enemy>().TakeDamage(damage);

        }
    }
}
