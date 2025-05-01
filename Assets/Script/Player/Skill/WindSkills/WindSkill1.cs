using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkill1 : SkillBase
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float maxLifetime = 1.5f;
    float dir = 1f;

    private Rigidbody2D _rb;
    private Animator _anim;
    private float _lifeTimer;
    private bool _hasExploded;
    private bool useDir = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _rb.velocity = transform.right * moveSpeed;
        _lifeTimer = maxLifetime;
        _hasExploded = false;
    }

    public void Init(float dir)
    {
        this.dir = dir;
    }

    void Update()
    {
        if (!useDir)
        {
            _rb.velocity *= dir;
            useDir = true;
        }
        if (_hasExploded) return;

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            Explode(); // 时间到自动爆炸
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasExploded) return;

        if (other.CompareTag("Boss"))
        {
            Explode(); // 碰撞敌人立即爆炸
            AttackEnemy(other);
        }
    }

    private void Explode()
    {
        _hasExploded = true;
        _rb.velocity = Vector2.zero; // 停止移动
        _anim.SetTrigger("hit");
        Destroy(gameObject,0.35f);
    }

    void AttackEnemy(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(ComputeDamage());
            Debug.Log("Damage Enemy " + ComputeDamage());
        }
        else
        {
            Debug.Log("没有找到敌人组件");
        }
    }
    int ComputeDamage()
    {
        float damage = attackDamage;
        damage *= 1.5f;
        return (int)damage;
    }
}
