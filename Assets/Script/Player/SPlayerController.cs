using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SPlayerController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerSoundEvent : UnityEvent<string> { }
    public PlayerSoundEvent onPlayer;
    public PlayerSoundEvent onPlayerSkill;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteEffectController spriteEffectController;
    private DamagePopupSystem damagePopupSystem;

    private PlayerBase playerBase;
    private PlayerMove playerMove;
    private PlayerFight playerFight;
    private PlayerStatus playerStatus;
    private PlayerUI playerUI;
    private PlayerSkill playerSkill;

    private int LevelMode = 0; // 0: normal, 1: boss, 2: other

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
            LevelMode = 0;
        else if (sceneName.StartsWith("SLevel"))
            LevelMode = 1;
        else
            LevelMode = 2;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteEffectController = GetComponent<SpriteEffectController>();
        damagePopupSystem = FindObjectOfType<DamagePopupSystem>();

        InitializeEvent();

        playerBase = new PlayerBase();
        playerBase.Initialize(rb, anim, onPlayer);
        playerMove = new PlayerMove();
        playerMove.Initialize(rb, anim, transform, onPlayer, playerBase);
        playerFight = new PlayerFight();
        playerFight.Initialize(rb, anim, transform, playerBase, onPlayer);
        playerStatus = new PlayerStatus();
        playerStatus.Initialize(this, spriteEffectController);
        playerUI = new PlayerUI();
        playerUI.Initialize(playerBase);
        if (LevelMode == 1)
        {
            playerSkill = new PlayerSkill();
            playerSkill.Initialize(playerBase, playerMove, playerStatus, this, onPlayer);
        }

    }

    private void InitializeEvent()
    {
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager != null)
        {
            onPlayer.AddListener(soundManager.PlayerSound);
            onPlayerSkill.AddListener(soundManager.SkillSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerBase.Tick();
        if (playerBase.alive)
        {
            if (!playerStatus.isControlled)
            {
                playerMove.Tick();
                playerFight.Tick();
                if (LevelMode == 1)
                    playerSkill.Tick1();
            }
            if (LevelMode == 1)
                playerSkill.Tick2();
        }
        else
        {
            if (playerBase.CanResurrect() && !playerBase.IsInResurrecting())
            {
                playerBase.UpdateResurrectNum(-1);
                Invoke("Resurrect", 1f);
            }
            if (playerBase.isGameOver)
            {
                FindObjectOfType<FinishLevel>().Finishlevel(false);
            }
        }
        playerUI.Tick();
    }

    private void Resurrect()
    {
        playerBase.Resurrect();
        playerMove.RestorePhysics();
        playerMove.Restart();
        playerStatus.isImmuneToDamage = true;
        playerBase.ExecuteDie = false;
        ActivateCloak(1f);
        var spawnPosition = transform.position;
        spawnPosition.y += 1.5f; // Adjust spawn position if needed
        ActiveFX(spawnPosition);
        FindObjectOfType<SLevelManager>().UpdateLevelHealth(playerBase.GetResurrectNum());
        Invoke("EndImmune", 1f);
        //playerMove.UpdateFly();
        //playerStatus.UpdateCloak();
        //playerUI.UpdateUI();
    }

    void EndImmune()
    {
        playerStatus.isImmuneToDamage = false;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerMove.HandleTriggerEnter2D(other); // 手动调用碰撞逻辑
    }
    void OnTriggerExit2D(Collider2D other)
    {
        playerMove.HandleTriggerExit2D(other); // 手动调用碰撞逻辑
    }

    public void GetHurt(int dir = 0, int damageNum = 20)    
    {
        if (!playerBase.alive) return;
        if (playerStatus.isImmuneToDamage) return;

        playerFight.GetHurt(dir, damageNum); 
    }
    public bool GetIsFlying() => playerMove.GetIsFlying();

    public void StartOrStopFly()
    {
        playerMove.UpdateFly();
    }

    public void StartFly(float duration)
    {
        playerMove.UpdateFly();
        Invoke("UpdateFlyAfterDelay", duration);
    }
    private void UpdateFlyAfterDelay()
    {
        playerMove.UpdateFly(); // 实际执行目标方法
    }

    public void ActivateCloak(float duration) => playerStatus.ActivateCloak(duration);

    public bool IsAlive() => playerBase.alive;

    public bool IsCloaked() => playerStatus.isCloaked;

    public bool IsManaEnough(int manaCost) => playerBase.IsManaEnough(manaCost);
    public void CostMana(int manaCost) => playerBase.UpdateMana(-manaCost);

    public void GetHeal(int healAmount)
    {
        playerBase.GetHeal(healAmount);
        if (damagePopupSystem != null) {
            Vector3 position = transform.position;
            position.y += 2f;
            damagePopupSystem.ShowHeal(healAmount,position);
        }
    }
    public void GetMana(int manaAmount)
    {
        playerBase.GetMana(manaAmount);
        if (damagePopupSystem != null)
        {
            Vector3 position = transform.position;
            position.y += 2f;
            damagePopupSystem.ShowMana(manaAmount, position);
        }
    }
    public int GetResurrectNum()
    {
        if (playerBase != null) return playerBase.GetResurrectNum();
        return LevelBaseData.Instance.LevelMode == 0 ? 1 : 2;
        //return 3;
    }

    public float GetEvaStatus()
    {
        float Num = playerBase.GetResurrectNum();
        float favor1 = Num * playerBase.maxHealth + playerBase.health;
        float favor2 = 4f * playerBase.maxHealth;
        return favor1 / favor2 * 100f;
    }
    void ActiveFX(Vector3 spawnPosition)
    {
        GameObject fx2Prefab = Resources.Load<GameObject>("Prefab/FX/fx00");
        if (fx2Prefab != null)
        {
            GameObject fx2Instance = Instantiate(fx2Prefab, spawnPosition, Quaternion.identity);
            Destroy(fx2Instance, 0.667f); // 3秒后自动销毁
        }
        else
        {
            Debug.LogError("FX2 prefab is not assigned!");
        }
    }

    public int GetDamage() => playerFight.GetDamage();
    public void SetVampire(bool flag)
    {
        GetComponentInChildren<AttackCheck>().SetVampire(flag);
    }
    public void MulDefense(float Num)
    {
        playerFight.MulDefense(Num);
    }

    public void InitSkillData(ElementType elementType)
    {
        string wandPath = "Skeletal/bone_1/bone_2/bone_3/bone_5/bone_6/bone_20/";
        wandPath += elementType.ToString() + "Wand";
        switch (elementType) {
            case ElementType.Fire:
                playerBase.maxHealth += 50;
                playerBase.health = playerBase.maxHealth;
                break;
            case ElementType.Water:
                playerBase.maxMana += 50;
                playerBase.mana = playerBase.maxMana;
                break;
            case ElementType.Thunder:
                playerFight.AddDamage(10);
                break;
            case ElementType.Earth:
                playerFight.AddDefense(10);
                break;
            case ElementType.Wind:
                playerMove.movePower += 2f;
                break;
        }
        GameObject wand = FindWand(wandPath);
        if(wand != null)
        {
            wand.SetActive(true);
            wandPath = "Skeletal/15 Staff";
            wand = FindWand(wandPath);
            wand.SetActive(false);
        }
            
    }
    private GameObject FindWand(string wandPath)
    {
        Transform current = transform; // 从Player的Transform开始

        string[] pathSegments = wandPath.Split('/');

        foreach (string segment in pathSegments)
        {
            current = current.Find(segment);
            if (current == null)
            {
                Debug.LogError($"路径错误: 找不到节点 {segment}");
                return null;
            }
        }
        return current.gameObject;
    }
}
