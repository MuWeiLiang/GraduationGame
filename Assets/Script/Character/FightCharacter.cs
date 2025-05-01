using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private MoveCharacter moveCharacter;
    // �������
    public int attackDamage = 30; // �����˺�
    public float attackRange = 0.5f; // ������Χ
    public LayerMask enemyLayer; // �������ڵĲ㼶
    private Transform attackPoint; // ��������
    // �������
    public bool isHurting = false;
    private int hitDirection = 0;
    private int hurtDamage = 20; // �����˺�
    private bool canFight = true; // �Ƿ�����
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveCharacter = GetComponent<MoveCharacter>();
        GetAttackPoint();
    }
    // Update is called once per frame
    void Update()
    {
        if(!canFight) return;
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) || isHurting)
        {
            Hurt();
        }
    }
    void GetAttackPoint()
    {
        attackPoint = transform.Find("Skeletal/bone_1/bone_2/bone_3/bone_5/bone_6/bone_20/attackPoint");
        if (attackPoint == null)
        {
            Debug.Log("AttackPoint not found! Make sure it's a child of the weapon bone.");
        }
    }
    void Attack()
    {
        anim.SetTrigger("attack");
        StartCoroutine(DetectEnemiesAfterDelay());
    }

    IEnumerator DetectEnemiesAfterDelay()
    {
        float attackDuration = anim.GetCurrentAnimatorStateInfo(0).length; // ��ȡ����ʱ��
        yield return new WaitForSeconds(attackDuration / 5); // �ȴ������������
        // yield return new WaitForSeconds(0.10f);

        if (attackPoint == null)
            yield break;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit One!");
            enemy.GetComponent<BaseEnemy>().TakeDamage(attackDamage);
        }
    }
    void Hurt()
    {
        if(moveCharacter.alive == false) return;
        anim.SetTrigger("hurt");
        moveCharacter.TakeDamage(hurtDamage);
        if (hitDirection != 0 && hitDirection == moveCharacter.direction)
        {
            moveCharacter.ReversalDir();
            hitDirection = 0;
        }
        if (moveCharacter.direction == 1)
            rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        isHurting = false;
    }

    public void GetHurt(int dir = 0, int damageNum = 20)
    {
        isHurting = true;
        if(dir != 0)
            hitDirection = dir;
        if(damageNum != 0)
            hurtDamage = damageNum;
    }
    public void SetCanFight(bool canFight)
    {
        this.canFight = canFight;
    }
}
