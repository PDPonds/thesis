using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    public event Action onDamage;
    public event Action onAttack;

    public int maxHp;
    public int hp { get; set; }

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    public CircleCollider2D attackCol;
    public float attackRange;
    public LayerMask playerMask;

    public float attackDelay;

    float currentAttackDelay;
    bool canAttack;

    public float takedamageKnockback;

    private void OnEnable()
    {
        onDamage += PlayHurtAnim;
        onAttack += PlayAttackAnim;
    }

    private void OnDisable()
    {
        onDamage -= PlayHurtAnim;
        onAttack -= PlayAttackAnim;
    }

    private void Awake()
    {
        hp = maxHp;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentAttackDelay = attackDelay;
        attackCol.radius = attackRange;
        attackCol.enabled = false;

    }

    private void Update()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);

        Collider2D[] around = Physics2D.OverlapCircleAll(origin, attackRange, playerMask);
        if (around.Length > 0)
        {
            if (canAttack)
            {
                onAttack?.Invoke();
                attackCol.enabled = true;
                canAttack = false;
            }

            if (!canAttack)
            {
                currentAttackDelay -= Time.deltaTime;
                if (currentAttackDelay < 0)
                {
                    canAttack = true;
                    currentAttackDelay = attackDelay;
                }
            }

        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                attackCol.enabled = false;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Gizmos.DrawWireSphere(origin, attackRange);
    }

    public void TakeDamage()
    {
        hp--;
        onDamage?.Invoke();

        rb.AddForce(Vector2.right * takedamageKnockback, ForceMode2D.Impulse);

        attackCol.enabled = false;
        canAttack = false;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void PlayHurtAnim()
    {
        anim.Play("Hurt");
    }

    void PlayAttackAnim()
    {
        anim.Play("Attack");
    }

}
