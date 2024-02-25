using UnityEngine;

public class FireBlaze : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float activationDelay = 2f;
    [SerializeField] private float activeTime = 3f;

    private bool setDamage;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ActivateHeat();
        }
        if (other.gameObject.CompareTag("Player") && anim.GetBool("fire") == true)
        {
            ApplyDamage(other);
        }
    }

    private void ActivateHeat()
    {
        anim.SetBool("heat", true);
        setDamage = true;

        Invoke("ActivateFire", activationDelay);
    }

    private void ActivateFire()
    {
        setDamage = false;
        anim.SetBool("fire", true);
        anim.SetBool("heat", false);
        Invoke("DesactivateFire", activeTime);
    }
    
    private void DesactivateFire()
    {
        setDamage = false;
        anim.SetBool("fire", false);
        anim.SetBool("heat", false);
    }

    private void ApplyDamage(Collider2D other)
    {
        if (setDamage)
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}