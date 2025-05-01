using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoss : MonoBehaviour
{
    private int maxHealth = 200;
    public int Health = 200;
    private int maxMana = 100;
    public int Mana = 100;
    public int AttackDamage = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsManaEnough(int manaCost)
    {
        return Mana >= manaCost;
    }
    public void UpdateMana(int manaCost)
    {
        Mana += manaCost;
    }
}
