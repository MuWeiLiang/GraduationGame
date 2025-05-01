using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpriteRendererController : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField] private bool includeInactive = true; // 是否包含未激活的渲染器
    [SerializeField] private List<SpriteRenderer> excludedRenderers; // 要排除的特定渲染器

    [Header("Cloak Settings")]
    [SerializeField] private Color cloakColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private float cloakFadeDuration = 0.5f; // 渐隐/渐显时间

    private readonly List<SpriteRenderer> _allRenderers = new List<SpriteRenderer>();
    private readonly List<Color> _originalColors = new List<Color>();
    private readonly List<Material> _originalMaterials = new List<Material>();

    private Coroutine _cloakCoroutine;
    private bool _isCloaked;

    private void Awake()
    {
        InitializeRenderers();
    }

    // 初始化所有渲染器
    private void InitializeRenderers()
    {
        // 获取所有子物体中的SpriteRenderer
        GetComponentsInChildren<SpriteRenderer>(includeInactive, _allRenderers);

        // 移除被排除的渲染器
        foreach (var excluded in excludedRenderers)
        {
            _allRenderers.Remove(excluded);
        }

        // 存储原始状态
        _originalColors.Clear();
        _originalMaterials.Clear();

        foreach (var renderer in _allRenderers)
        {
            _originalColors.Add(renderer.color);
            _originalMaterials.Add(renderer.material);
        }
    }

    // 激活隐身效果
    public void ActivateCloak(float duration)
    {
        if (_isCloaked) return;

        if (_cloakCoroutine != null)
        {
            StopCoroutine(_cloakCoroutine);
        }

        _cloakCoroutine = StartCoroutine(CloakRoutine(duration));
    }

    // 取消隐身
    public void DeactivateCloak()
    {
        if (!_isCloaked) return;

        if (_cloakCoroutine != null)
        {
            StopCoroutine(_cloakCoroutine);
        }

        _cloakCoroutine = StartCoroutine(UncloakRoutine());
    }

    // 隐身协程
    private IEnumerator CloakRoutine(float duration)
    {
        _isCloaked = true;

        // 渐隐效果
        float timer = 0f;
        while (timer < cloakFadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / cloakFadeDuration;

            foreach (var renderer in _allRenderers)
            {
                renderer.color = Color.Lerp(_originalColors[_allRenderers.IndexOf(renderer)], cloakColor, progress);
            }
            yield return null;
        }

        // 保持隐身状态
        yield return new WaitForSeconds(duration - cloakFadeDuration * 2);

        // 自动取消隐身
        _cloakCoroutine = StartCoroutine(UncloakRoutine());
    }

    // 取消隐身协程
    private IEnumerator UncloakRoutine()
    {
        // 渐显效果
        float timer = 0f;
        while (timer < cloakFadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / cloakFadeDuration;

            for (int i = 0; i < _allRenderers.Count; i++)
            {
                _allRenderers[i].color = Color.Lerp(cloakColor, _originalColors[i], progress);
            }
            yield return null;
        }

        // 完全恢复
        for (int i = 0; i < _allRenderers.Count; i++)
        {
            _allRenderers[i].color = _originalColors[i];
            _allRenderers[i].material = _originalMaterials[i];
        }

        _isCloaked = false;
        _cloakCoroutine = null;
    }

    // 编辑器方法：收集所有渲染器
    [ContextMenu("Collect All Renderers")]
    private void CollectAllRenderersInEditor()
    {
        _allRenderers.Clear();
        GetComponentsInChildren<SpriteRenderer>(true, _allRenderers);
        Debug.Log($"已收集 {_allRenderers.Count} 个 SpriteRenderer");
    }

    // 重置所有渲染器状态
    [ContextMenu("Reset All Renderers")]
    private void ResetAllRenderers()
    {
        for (int i = 0; i < _allRenderers.Count; i++)
        {
            if (_allRenderers[i] != null)
            {
                _allRenderers[i].color = _originalColors[i];
                _allRenderers[i].material = _originalMaterials[i];
            }
        }
    }
}
