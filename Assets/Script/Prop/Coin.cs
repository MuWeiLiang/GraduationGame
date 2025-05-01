using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private static readonly Dictionary<string, int> TagScoreMap = new Dictionary<string, int>
    {
        { "GoldCoin", 5 },
        { "SilverCoin", 3 },
        { "BronzeCoin", 1 },
        { "GoldSnitch", 2 }
        // 添加更多标签和分数值
    };
    private int scoreValue; // 金币的分数值
    private bool isCollected = false; // 是否已被收集
    private void Awake()
    {
        // 根据金币的 Tag 设置对应的分数值
        if (TagScoreMap.TryGetValue(gameObject.tag, out int value))
        {
            scoreValue = value;
        }
        else
        {
            scoreValue = 0; // 默认分数值，可以根据需要调整
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            // SLevelData levelData = FindObjectOfType<SLevelData>();
            SLevelManager sLevelManager = FindObjectOfType<SLevelManager>();
            if (sLevelManager != null)
            {
                sLevelManager.AddScore(scoreValue);
                isCollected = true; // 设置金币为已收集
                FindObjectOfType<PropSoundBase>().Pickup();
                // Pickup();
                Destroy(gameObject); // 销毁金币对象
            }
        }
    }
}
