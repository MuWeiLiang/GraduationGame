using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ElementType { Earth, Fire, Water, Wind, Thunder }
public abstract class SkillController : MonoBehaviour
{
    public ElementType ElementType;
    string skillName;
    protected Vector3 spawnPoint;
    protected int Dir;
    public int manaCost { get; private set; }
    public float currentCooldown { get; private set; }
    public float Cooldown { get; private set; }

    public int AttackDamage = 20; // ¹¥»÷ÉËº¦
    public SPlayerController playerController;

    public bool IsReady => currentCooldown <= 0;

    public abstract void Initialize();
    public void Init(ElementType elementType,string skillname, float cooldown, int manacost)
    {
        ElementType = elementType;
        skillName = skillname;
        Cooldown = cooldown;
        manaCost = manacost;
        currentCooldown = 0;
    }

    public virtual void Activate()
    {
        if (!IsReady) return;
        if (playerController == null) return;
        if (!playerController.IsManaEnough(manaCost)) return;
        playerController.CostMana(manaCost);
        currentCooldown = Cooldown;
        playerController.onPlayerSkill?.Invoke(skillName);
        ApplyEffect();
    }

    protected abstract void ApplyEffect();
    public void SetSpawnPotionAndDir(Vector3 position,int dir)
    {
        spawnPoint = position;
        Dir = dir;
    }

    public void UpdateCooldown()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
    }
}
