using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";
    private const string TAKEHIT = "TakeHit";

    Vector2 movement;
    [SerializeField] private PauseMenu pauseMenu;

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() 
    {
        pauseMenu = GetComponent<PauseMenu>();
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        Player.Instance.OnTakeHits += Player_OnTakeHit;
    }

    private void Player_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKEHIT);
    }

    private void Player_OnPlayerDeath(object sendler, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
    }

    private void Instance_OnPlayerDeath(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Update() {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        if (Player.Instance.IsAlive())//&& pauseMenu?.pauseGame == false)
            AdjustPlayerFacingDirection();
    }


    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
