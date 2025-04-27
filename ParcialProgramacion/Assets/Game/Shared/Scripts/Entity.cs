using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Clase base para entidades del juego (jugador, enemigos).
/// Maneja componentes principales, detecciones, velocidad, daño y efectos de knockback.
/// </summary>
public class Entity : MonoBehaviour
{
    #region Componentes

    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public SpriteRenderer Sr { get; private set; }
    public CapsuleCollider2D Collider2D { get; private set; }

    #endregion

    #region Parámetros

    [Header("Knockback Settings")] [SerializeField]
    protected Vector2 knockbackDirection;

    [SerializeField] protected float knockbackDuration;
    protected bool IsKnocked;

    [Header("Collision Settings")] [SerializeField]
    protected Transform groundCheck;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Attack Settings")] public Transform attackCheck;
    public float attackCheckRadius;

    protected bool FacingRight = true;
    public int FacingDir { get; private set; } = 1;
    public Action OnFlipped;

    [Header("Agro Settings")] public float agroDistance = 2;

    #endregion

    #region Unity Methods
    
    protected virtual void Awake()
    {
        // Se puede sobrescribir en clases hijas
    }

    protected virtual void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponentInChildren<SpriteRenderer>();
        Collider2D = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {
        // Se puede sobrescribir en clases hijas
    }

    protected virtual void OnDrawGizmos()
    {
        // Dibuja rayos de detección de suelo y pared en el editor
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Métodos Públicos

    /// <summary>
    /// Ejecuta la lógica de muerte de la entidad (virtual, para sobreescribir).
    /// </summary>
    public virtual void Die()
    {
    }

    /// <summary>
    /// Aplica un efecto de ralentización a la entidad (virtual, para sobreescribir).
    /// </summary>
    public virtual void SlowEntityBy(float slowPercentage, float slowDuration)
    {
    }

    /// <summary>
    /// Lanza la animación de impacto por daño (knockback).
    /// </summary>
    public virtual void DamageImpact() => StartCoroutine(HitKnockback());

    /// <summary>
    /// Establece la velocidad de la entidad en X e Y.
    /// </summary>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (IsKnocked)
            return;

        Rb.velocity = new Vector2(xVelocity, yVelocity);

        FlipController(xVelocity);
    }

    /// <summary>
    /// Detiene el movimiento de la entidad.
    /// </summary>
    public void SetZeroVelocity()
    {
        if (IsKnocked)
            return;

        Rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Gira la entidad hacia el lado opuesto.
    /// </summary>
    public virtual void Flip()
    {
        FacingDir *= -1;
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);

        OnFlipped?.Invoke();
    }

    /// <summary>
    /// Controla automáticamente el giro de la entidad según su movimiento.
    /// </summary>
    public virtual void FlipController(float x)
    {
        switch (x)
        {
            case > 0 when !FacingRight:
            case < 0 when FacingRight:
                Flip();
                break;
        }
    }

    /// <summary>
    /// Detecta si la entidad está en el suelo.
    /// </summary>
    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    /// <summary>
    /// Detecta si la entidad choca contra una pared.
    /// </summary>
    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    #endregion

    #region Métodos Protegidos

    /// <summary>
    /// Restablece la velocidad de animación al valor por defecto.
    /// </summary>
    protected virtual void ReturnDefaultSpeed() => Anim.speed = 1;

    /// <summary>
    /// Coroutine que aplica un retroceso (knockback) al recibir daño.
    /// </summary>
    protected virtual IEnumerator HitKnockback()
    {
        IsKnocked = true;

        Rb.velocity = new Vector2(knockbackDirection.x * -FacingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        IsKnocked = false;
    }

    #endregion
}