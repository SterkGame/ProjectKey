using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public int destroy;
    public int damage;
    public float distance;
    public LayerMask whatIsSolid;

    public int rotateUp;
    public int rotateDown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Invoke("DestroyTime", destroy);
        transform.Rotate(transform.rotation.x, transform.rotation.y, Random.Range(rotateDown, rotateUp));
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<EnemyEntity>().TakeDamage(damage);
            }
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.GetComponent<Player>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    void DestroyTime()
    {
        Destroy(gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D hitInfo)
    //{
    //    Player player = hitInfo.GetComponent<Player>();
    //    if (player != null)
    //    {
    //        player.TakeDamage(damage);
    //    }
    //    Destroy(gameObject);
    //}

}
