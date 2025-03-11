using System;
using UnityEngine;
using UnityEngine.UI;
using static Guns;

[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    public int enemyHealth;
    public int enemyDamageAmount;
    public GameObject bloodPrefab;
    public GameObject guns;
    public GameObject ammoClip;
    public GameObject medicalClip;
    public LevelTask levelTask;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;


    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;

    private int _currentHealth;
    private EnemyAI _enemyAI;
    [SerializeField] private Image reloadingText;

    private void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
        levelTask = FindObjectOfType<LevelTask>();
    }

    private void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
            rb.velocity = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
            capsuleCollider2D.isTrigger = true;
            
            guns.SetActive(false);
            _enemyAI.SetDeathState();
            levelTask.EnemyKilled();

            OnDeath?.Invoke(this, EventArgs.Empty);

            int randomChance = UnityEngine.Random.Range(0, 100);
            GameObject itemToSpawn = randomChance < 50 ? ammoClip : medicalClip; // Випадковість предмету
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);

            //Destroy(gameObject);
        }
    }


}
