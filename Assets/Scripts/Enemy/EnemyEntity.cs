using System;
using UnityEngine;
using UnityEngine.Rendering;
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
    private SortingGroup sortingGroup;
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
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {

        var data = SaveSystem.LoadPlayer();
        switch (data.difficulty)
        {
            case GameDifficulty.������:
                enemyHealth = 5;
                break;
            case GameDifficulty.��������:
                enemyHealth = 10;
                break;
            case GameDifficulty.������:
                enemyHealth = 15;
                break;
        }


        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        _currentHealth = enemyHealth;
    }

    Vector3 lastPosition;
    void LateUpdate()
    {

        if (transform.position != lastPosition)
        {
            sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 2);
            lastPosition = transform.position;
        }
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
            GameObject itemToSpawn = randomChance < 50 ? ammoClip : medicalClip; // ����������� ��������
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
            sortingGroup.sortingOrder = Mathf.RoundToInt(-2000);

            //Destroy(gameObject);
        }
    }


}
