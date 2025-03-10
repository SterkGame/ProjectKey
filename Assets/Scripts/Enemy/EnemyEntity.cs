using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    public int enemyHealth;
    public int enemyDamageAmount;
    public GameObject bloodPrefab;
    public GameObject guns;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;


    private CapsuleCollider2D capsuleCollider2D;
    private int _currentHealth;
    private EnemyAI _enemyAI;
    [SerializeField] private Image reloadingText;

    private void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _currentHealth = enemyHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            //player.TakeDamage(transform, enemyDamageAmount);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        Instantiate(bloodPrefab, transform.position, Quaternion.identity);

        DetectDeath();
    }


    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            reloadingText.gameObject.SetActive(false);
            if (capsuleCollider2D != null)
            {
                capsuleCollider2D.enabled = false;
            }
            guns.SetActive(false);
            _enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
            //Destroy(gameObject);
        }
    }


}
