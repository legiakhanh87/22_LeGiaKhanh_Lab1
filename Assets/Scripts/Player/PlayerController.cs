using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    float p_targetDirection;
    float p_direction;

    Vector3 p_targetScale = new Vector3();
    Vector3 p_scale = new Vector3();

    public Vector2 p_targetVel = new Vector2();
    public Vector3 p_vel = new Vector3();

    Rigidbody2D rb;

    bool movesDuringTheFrame = false;
    bool movesChange = true;

    bool canDash = true;
    bool isInDash = false;
    int dashCooldownCurrentFrame;
    int dashCooldownTimeFrames;

    [Header("Particles")]
    public PlayerEffectsManager MovementParticles;
    public ParticleSystem DashParticles;
    public ParticleSystem CircleParticles;
    public ParticleSystem HitParticles;

    [Header("GameObjects")]
    public GameObject VisualObj;

    [Header("Player Settings")]
    public float Speed = 10;
    public float dashCooldownTime = 0.34f;
    public float dashDuration = 0.18f;
    public float DamageCooldown = 1f;
    public int MaxLives = 3;
    public int Lives = 3;

    int framesDuringCooldownDamage;
    int currentDmgCooldownFrame = 0;
    Color defaultColor;

    [Header("Runtime")]
    public bool IsInDamageCooldown;
    int effectFrame;
    bool isMagenta = false;

    public static int numberOfRespawns;

    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Cache SpriteRenderer (JSAB style)
        sr = VisualObj.GetComponent<SpriteRenderer>();

        // Lấy màu gốc đúng cách cho URP 2D
        defaultColor = sr.color;

        dashCooldownTimeFrames = Mathf.FloorToInt(dashCooldownTime * 50);
        framesDuringCooldownDamage = Mathf.FloorToInt(DamageCooldown * 50);
    }


    void KeyboardMovement()
    {
        movesDuringTheFrame = false;
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            movement += Vector2.up;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            movement += Vector2.right;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            movement += Vector2.down;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            movement += Vector2.left;

        if (movement != Vector2.zero)
        {
            movesDuringTheFrame = true;
            p_targetDirection = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        // Health radial (shader)
        VisualObj.GetComponent<SpriteRenderer>().material
            .SetFloat("_Fillpercentage",
                (1f / MaxLives) * (MaxLives - Lives));

        KeyboardMovement();

        p_direction = Mathf.Lerp(p_direction, p_targetDirection, 35 * Time.deltaTime);

        if (movesDuringTheFrame != movesChange)
        {
            if (movesDuringTheFrame)
            {
                MovementParticles.Play();
                p_targetScale = new Vector3(0.85f, 1.15f, 1);
                VisualObj.transform.DOKill();
            }
            else
            {
                MovementParticles.Stop();
                p_targetScale = Vector3.one;
                p_scale = p_targetScale;
                p_targetDirection = -90;
                VisualObj.transform
                    .DOScale(1, 1.1f)
                    .SetEase(Ease.OutElastic, 5f, 0.2f);
            }

            movesChange = movesDuringTheFrame;
        }

        if (movesDuringTheFrame)
        {
            p_scale = Vector3.Lerp(p_scale, p_targetScale, 10 * Time.deltaTime);
            VisualObj.transform.localScale = p_scale;
        }

        if (movesDuringTheFrame)
        {
            float radians = p_targetDirection * Mathf.Deg2Rad;
            p_targetVel = new Vector2(
                -Speed * Mathf.Sin(radians),
                 Speed * Mathf.Cos(radians));
        }
        else
        {
            p_targetVel = Vector2.zero;
        }

        rb.linearVelocity = p_targetVel;
        transform.rotation = Quaternion.Euler(0, 0, p_direction);

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            DashParticles.Play();
            CircleParticles.Play();
            DashStuff();
        }
    }

    void FixedUpdate()
    {
        if (!canDash)
        {
            dashCooldownCurrentFrame--;
            if (dashCooldownCurrentFrame <= 0)
                canDash = true;
        }

        if (IsInDamageCooldown)
        {
            currentDmgCooldownFrame--;
            effectFrame--;

            if (effectFrame <= 0)
            {
                VisualObj.GetComponent<SpriteRenderer>().material
                    .SetColor("_Backgroundfillcolor",
                        isMagenta ? defaultColor : Color.magenta);

                effectFrame = 2;
                isMagenta = !isMagenta;
            }

            if (currentDmgCooldownFrame <= 0)
            {
                VisualObj.GetComponent<SpriteRenderer>().material
                    .SetColor("_Backgroundfillcolor", defaultColor);

                isMagenta = false;
                IsInDamageCooldown = false;
            }
        }
    }

    void DashStuff()
    {
        float originalSpeed = Speed;
        float dashSpeed = originalSpeed * 4.2f;

        Vector3 prevScale = p_targetScale;
        p_targetScale = new Vector3(0.5f, 1.23f, 1);

        dashCooldownCurrentFrame = dashCooldownTimeFrames;
        canDash = false;
        isInDash = true;

        DOTween.To(() => dashSpeed, x =>
        {
            dashSpeed = x;
            Speed = originalSpeed + dashSpeed;
        }, 0f, dashDuration)
        .OnComplete(() =>
        {
            isInDash = false;
            p_targetScale = prevScale;
            Speed = originalSpeed;
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryTakeDamage(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryTakeDamage(collision);
    }

    private void TryTakeDamage(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("LevelHitbox"))
            return;

        if (IsInDamageCooldown || isInDash)
            return;

        HitParticles.Play();

        currentDmgCooldownFrame = framesDuringCooldownDamage;
        IsInDamageCooldown = true;

        Lives--;

        if (Lives <= 0)
        {
            numberOfRespawns++;
            Debug.Log("Player Dead");
            gameObject.SetActive(false);
            FindFirstObjectByType<LevelLogic>().PlayerDied();

        }
    }



}
