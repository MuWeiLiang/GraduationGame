using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicStone : MonoBehaviour
{
    [Header("UI Settings")]
    private Slider progressBar;
    [SerializeField] private float collectionDuration = 5f;
    [SerializeField] private float movementThreshold = 0.1f;
    [SerializeField] private float inputCooldown = 0.3f;

    private SLevelManager levelManager;

    private enum CollectState { Ready, Collecting, Cooldown }
    private CollectState currentState = CollectState.Ready;

    private Vector3 playerInitialPosition;
    private float lastInputTime;
    private Coroutine activeCoroutine;
    private bool isPlayerInTrigger;
    private void Awake()
    {
        GameObject magicStonePrefab = Resources.Load<GameObject>("Prefab/Canvas/magicStone");
        if (magicStonePrefab == null)
        {
            Debug.LogError("预制体加载失败！请检查路径。");
            return;
        }

        // 2. 找到父物体（Canvas/MagicStoneSlider）
        GameObject parentSlider = GameObject.Find("Canvas/MagicStoneSlider");
        if (parentSlider == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("未找到Canvas！请确保场景中有Canvas。");
                return;
            }

            // 创建空物体并命名为MagicStoneSlider
            parentSlider = new GameObject("MagicStoneSlider");
            parentSlider.transform.SetParent(canvas.transform);
        }

        // 3. 实例化预制体，并设置为父物体的子物体
        GameObject magicStoneInstance = Instantiate(magicStonePrefab, parentSlider.transform);

        progressBar = magicStoneInstance.GetComponentInChildren<Slider>(true);

        if (progressBar == null)
        {
            Debug.LogError("未找到Slider组件！");
        }
    }
    private void Start()
    {
        if (levelManager == null)
        {
            levelManager = FindObjectOfType<SLevelManager>();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerValid(other))
        {
            isPlayerInTrigger = true;
            Debug.Log("玩家进入收集区域");
            levelManager.ActivePrompt("Press F");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isPlayerInTrigger || !IsPlayerValid(other)) return;

        // 使用GetKey代替GetKeyDown实现持续检测
        if (Input.GetKey(KeyCode.F) && currentState == CollectState.Ready)
        {
            FindObjectOfType<PropSoundBase>().Pickup(3);
            StartCollection(other.GetComponent<SPlayerController>(), other.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayerValid(other))
        {
            isPlayerInTrigger = false;
            Debug.Log("玩家离开收集区域");

            if (currentState == CollectState.Collecting)
            {
                CancelCollection();
            }
        }
    }

    private void StartCollection(SPlayerController player, Vector3 position)
    {
        if (player == null) return;

        currentState = CollectState.Collecting;
        playerInitialPosition = position;

        // 显示进度条
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0f;
        }

        // 确保只有一个协程在运行
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(CollectionProcess(player));
    }

    private IEnumerator CollectionProcess(SPlayerController player)
    {
        float timer = 0;

        while (timer < collectionDuration)
        {
            // 实时检测玩家移动和按键状态
            if (HasPlayerMoved(player.transform.position)/* || !Input.GetKey(KeyCode.F)*/)
            {
                levelManager.ActivePrompt("Player Move! Cancel Collection!");
                CancelCollection();
                yield break;
            }

            timer += Time.deltaTime;
            UpdateProgress(timer / collectionDuration);
            yield return null;
        }

        CompleteCollection(player);
    }

    private void CompleteCollection(SPlayerController player)
    {
        if(levelManager == null)
        {
            levelManager = FindObjectOfType<SLevelManager>();
        }
        if(levelManager == null)
        {
            Debug.LogError("LevelManager not found!");
            return;
        }
        if (!levelManager.IsGoalScoreReached())
        {
            Debug.Log("分数不足，无法收集");
            levelManager.ActivePrompt("Coins not enough!");
        }
        else
        {
            levelManager.AddMagicStone();
            Debug.Log("魔法石收集成功");
            levelManager.ActivePrompt("Collect success!");
            FindObjectOfType<PropSoundBase>().Pickup();
            Destroy(gameObject);
            Destroy(progressBar.gameObject); // 销毁进度条
            return; // 对象已销毁，直接返回
        }

        // 收集未完成时的清理
        currentState = CollectState.Cooldown;
        lastInputTime = Time.time;
        activeCoroutine = null;
        ResetProgress();

        StartCoroutine(ResetCooldown());
    }

    private void CancelCollection()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        currentState = CollectState.Cooldown;
        lastInputTime = Time.time;
        ResetProgress();

        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(inputCooldown);
        currentState = CollectState.Ready;
        Debug.Log("冷却结束，准备就绪");
    }

    private void UpdateProgress(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = progress;
        }
    }

    private void ResetProgress()
    {
        if (progressBar != null)
        {
            progressBar.value = 0f;
            progressBar.gameObject.SetActive(false);
        }
    }

    private bool IsPlayerValid(Collider2D other)
    {
        return other.CompareTag("Player") && !IsCollected();
    }

    private bool HasPlayerMoved(Vector3 currentPosition)
    {
        return Vector3.Distance(currentPosition, playerInitialPosition) > movementThreshold;
    }

    private bool IsCollected()
    {
        return this == null; // 如果对象已被销毁，则为已收集
    }

    public Slider GetSlider()
    {
        return progressBar;
    }
}