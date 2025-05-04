using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkill1 : SkillBase
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float maxLifetime = 2.5f;
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
        if(_rb.velocity.y == 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -0.5f);
        }
        if (_hasExploded) return;

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            Explode(); // ʱ�䵽�Զ���ը
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasExploded) return;

        if (other.CompareTag("Boss") || other.CompareTag("monster"))
        {
            Explode(); // ��ײ����������ը
            AttackEnemy(other);
        }
    }

    private void Explode()
    {
        _hasExploded = true;
        _rb.velocity = Vector2.zero; // ֹͣ�ƶ�
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
            Debug.Log("û���ҵ��������");
        }
    }
    int ComputeDamage()
    {
        float damage = attackDamage;
        damage *= 1.5f;
        return (int)damage;
    }
}
