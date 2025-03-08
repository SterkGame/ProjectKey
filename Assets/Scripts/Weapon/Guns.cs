using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Guns : MonoBehaviour
{
    public GameObject bullet;
    public Transform BulletTransform;
    public GunType gunType;
    public float StartTimeFire;
    public float offset;
    [SerializeField] private EnemyAI enemyAI;

    private float rotateZ;
    private float TimeFire;
    public enum GunType { Player, Enemy }
    [SerializeField] private Player player;
    public Animator anim;
    void Start()
    {
        TimeFire = StartTimeFire;
        ////////////////////////////////
        //enemyAI = FindObjectOfType<EnemyAI>();
        //player = FindObjectOfType<Player>();
        ////////////////////////////////
    }


    void Update()
    {
        if (gunType == GunType.Enemy)
        {
            enemyAI = GetComponentInParent<EnemyAI>();  // Шукаємо в батьківському об'єкті
        }
        player = FindObjectOfType<Player>();

        if (gunType == GunType.Player)
        {
            Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;

            Vector3 LocalScale = Vector3.one;
            LocalScale.x = 0.45f;
            LocalScale.z = 0.45f;

            if (rotateZ > 90 || rotateZ < -90)
            {
                LocalScale.y = -0.45f;

            }
            else
            {
                LocalScale.y = +0.45f;
            }

            transform.localScale = LocalScale;
        }
        else if (gunType == GunType.Enemy && enemyAI != null && enemyAI._currentState == EnemyAI.State.Attacking)
        {
            Vector3 diference = player.transform.position - transform.position;
            rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;

            Vector3 LocalScale = Vector3.one;
            LocalScale.x = 0.45f;
            LocalScale.z = 0.45f;

            if (rotateZ > 90 || rotateZ < -90)
            {
                LocalScale.y = -0.45f;

            }
            else
            {
                LocalScale.y = +0.45f;
            }

            transform.localScale = LocalScale;
        }


        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);


        if ((Input.GetKey(KeyCode.Mouse0) && gunType == GunType.Player) || 
            (gunType == GunType.Enemy && enemyAI != null && enemyAI._currentState == EnemyAI.State.Attacking))
        {
            if (TimeFire <= 0)
            {
                Instantiate(bullet, BulletTransform.position, BulletTransform.rotation);
                TimeFire = StartTimeFire;
                anim.SetBool("idle", false);
                anim.SetBool("fire", true);

            }else 
            {
                TimeFire -= Time.deltaTime;
                
            }

        }
        else
        {
            anim.SetBool("idle", true);
            anim.SetBool("fire", false);
        }
    }
}
