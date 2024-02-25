using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer;
    
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (PlayerIn())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");
            } 
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerIn();
        }
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
    
    private bool PlayerIn()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3( boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
            , 0,Vector2.left, 0, playerLayer);

        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3( boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
