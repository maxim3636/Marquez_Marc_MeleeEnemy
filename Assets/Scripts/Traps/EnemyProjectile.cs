using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private float damage;
    private float lifeTime;

    private Animator anim;

    private bool hit;
    private BoxCollider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifeTime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if(hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(-movementSpeed,0,0);

        lifeTime += Time.deltaTime;
        if (lifeTime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hit = true;
        coll.enabled = false;
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }

        if (anim != null)
        {
            anim.SetTrigger("explode");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void deactivate()
    {
        gameObject.SetActive(false);
    }
}
