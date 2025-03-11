using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class Guns : MonoBehaviour
{
    public GameObject bullet;
    public Transform BulletTransform;
    public GunType gunType;
    public float StartTimeFire;
    public float offset;
    public int currentAmmo = 20;
    public int maxCurrentAmmo = 20;
    public int allAmmo = 150;
    public int fullAmmo = 240;
    private float reloadTime = 3f;



    private float rotateZ;
    private float TimeFire;
    private bool isReloading = false;

    public enum GunType { Player, Enemy }
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private Image reloadingText;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Player player;
    [SerializeField] private PauseMenu pauseMenu;
    public Animator anim;
    void Start()
    {
        TimeFire = StartTimeFire;
        isReloading = false;

        reloadingText.gameObject.SetActive(false);
        
    }


    void Update()
    {
        if (gunType == GunType.Enemy)
        {
            enemyAI = GetComponentInParent<EnemyAI>();  // Шукаємо в батьківському об'єкті
        }
        player = FindObjectOfType<Player>();

        if (gunType == GunType.Player && Player.Instance.IsAlive() && pauseMenu?.pauseGame == false)
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


        if (((Input.GetKey(KeyCode.Mouse0) && gunType == GunType.Player && currentAmmo > 0 && Player.Instance.IsAlive()) || 
            (gunType == GunType.Enemy && enemyAI != null && enemyAI._currentState == EnemyAI.State.Attacking && currentAmmo > 0)) 
            && !isReloading)
        {
            if (TimeFire <= 0)
            {
                Instantiate(bullet, BulletTransform.position, BulletTransform.rotation);
                TimeFire = StartTimeFire;
                anim.SetBool("idle", false);
                anim.SetBool("fire", true);
                currentAmmo -= 1;

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

        if (gunType == GunType.Player)
        {
            ammoCount.text = currentAmmo + "/" + allAmmo;
        }

        if (((Input.GetKeyDown(KeyCode.R) && gunType == GunType.Player) || currentAmmo == 0) && !isReloading)
        {
            if (currentAmmo < maxCurrentAmmo)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AmmoClipAuto>() && gunType == GunType.Player)
        {
            allAmmo += 20;
            if (allAmmo > fullAmmo)
            {
                allAmmo = fullAmmo;
            }
            Destroy(collision.gameObject);
        }
    }


    private IEnumerator Reload()
    {
        reloadingText.gameObject.SetActive(true);
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);
        int reason = maxCurrentAmmo - currentAmmo;
        if (allAmmo >= reason)
        {
            allAmmo = allAmmo - reason;
            currentAmmo = maxCurrentAmmo;
        }
        else
        {
            currentAmmo = currentAmmo + allAmmo;
            allAmmo = 0;
        }

        isReloading = false;
        reloadingText.gameObject.SetActive(false);
    }      
}
