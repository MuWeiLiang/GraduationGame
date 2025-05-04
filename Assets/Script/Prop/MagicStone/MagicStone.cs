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
            Debug.LogError("Ԥ�������ʧ�ܣ�����·����");
            return;
        }

        // 2. �ҵ������壨Canvas/MagicStoneSlider��
        GameObject parentSlider = GameObject.Find("Canvas/MagicStoneSlider");
        if (parentSlider == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("δ�ҵ�Canvas����ȷ����������Canvas��");
                return;
            }

            // ���������岢����ΪMagicStoneSlider
            parentSlider = new GameObject("MagicStoneSlider");
            parentSlider.transform.SetParent(canvas.transform);
        }

        // 3. ʵ����Ԥ���壬������Ϊ�������������
        GameObject magicStoneInstance = Instantiate(magicStonePrefab, parentSlider.transform);

        progressBar = magicStoneInstance.GetComponentInChildren<Slider>(true);

        if (progressBar == null)
        {
            Debug.LogError("δ�ҵ�Slider�����");
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
            Debug.Log("��ҽ����ռ�����");
            levelManager.ActivePrompt("Press F");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isPlayerInTrigger || !IsPlayerValid(other)) return;

        // ʹ��GetKey����GetKeyDownʵ�ֳ������
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
            Debug.Log("����뿪�ռ�����");

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

        // ��ʾ������
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.value = 0f;
        }

        // ȷ��ֻ��һ��Э��������
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
            // ʵʱ�������ƶ��Ͱ���״̬
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
            Debug.Log("�������㣬�޷��ռ�");
            levelManager.ActivePrompt("Coins not enough!");
        }
        else
        {
            levelManager.AddMagicStone();
            Debug.Log("ħ��ʯ�ռ��ɹ�");
            levelManager.ActivePrompt("Collect success!");
            FindObjectOfType<PropSoundBase>().Pickup();
            Destroy(gameObject);
            Destroy(progressBar.gameObject); // ���ٽ�����
            return; // ���������٣�ֱ�ӷ���
        }

        // �ռ�δ���ʱ������
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
        Debug.Log("��ȴ������׼������");
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
        return this == null; // ��������ѱ����٣���Ϊ���ռ�
    }

    public Slider GetSlider()
    {
        return progressBar;
    }
}