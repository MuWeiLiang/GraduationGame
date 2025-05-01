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
        // ��Ӹ����ǩ�ͷ���ֵ
    };
    private int scoreValue; // ��ҵķ���ֵ
    private bool isCollected = false; // �Ƿ��ѱ��ռ�
    private void Awake()
    {
        // ���ݽ�ҵ� Tag ���ö�Ӧ�ķ���ֵ
        if (TagScoreMap.TryGetValue(gameObject.tag, out int value))
        {
            scoreValue = value;
        }
        else
        {
            scoreValue = 0; // Ĭ�Ϸ���ֵ�����Ը�����Ҫ����
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
                isCollected = true; // ���ý��Ϊ���ռ�
                FindObjectOfType<PropSoundBase>().Pickup();
                // Pickup();
                Destroy(gameObject); // ���ٽ�Ҷ���
            }
        }
    }
}
