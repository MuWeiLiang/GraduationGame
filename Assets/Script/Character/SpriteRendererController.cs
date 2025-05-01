using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpriteRendererController : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField] private bool includeInactive = true; // �Ƿ����δ�������Ⱦ��
    [SerializeField] private List<SpriteRenderer> excludedRenderers; // Ҫ�ų����ض���Ⱦ��

    [Header("Cloak Settings")]
    [SerializeField] private Color cloakColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private float cloakFadeDuration = 0.5f; // ����/����ʱ��

    private readonly List<SpriteRenderer> _allRenderers = new List<SpriteRenderer>();
    private readonly List<Color> _originalColors = new List<Color>();
    private readonly List<Material> _originalMaterials = new List<Material>();

    private Coroutine _cloakCoroutine;
    private bool _isCloaked;

    private void Awake()
    {
        InitializeRenderers();
    }

    // ��ʼ��������Ⱦ��
    private void InitializeRenderers()
    {
        // ��ȡ�����������е�SpriteRenderer
        GetComponentsInChildren<SpriteRenderer>(includeInactive, _allRenderers);

        // �Ƴ����ų�����Ⱦ��
        foreach (var excluded in excludedRenderers)
        {
            _allRenderers.Remove(excluded);
        }

        // �洢ԭʼ״̬
        _originalColors.Clear();
        _originalMaterials.Clear();

        foreach (var renderer in _allRenderers)
        {
            _originalColors.Add(renderer.color);
            _originalMaterials.Add(renderer.material);
        }
    }

    // ��������Ч��
    public void ActivateCloak(float duration)
    {
        if (_isCloaked) return;

        if (_cloakCoroutine != null)
        {
            StopCoroutine(_cloakCoroutine);
        }

        _cloakCoroutine = StartCoroutine(CloakRoutine(duration));
    }

    // ȡ������
    public void DeactivateCloak()
    {
        if (!_isCloaked) return;

        if (_cloakCoroutine != null)
        {
            StopCoroutine(_cloakCoroutine);
        }

        _cloakCoroutine = StartCoroutine(UncloakRoutine());
    }

    // ����Э��
    private IEnumerator CloakRoutine(float duration)
    {
        _isCloaked = true;

        // ����Ч��
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

        // ��������״̬
        yield return new WaitForSeconds(duration - cloakFadeDuration * 2);

        // �Զ�ȡ������
        _cloakCoroutine = StartCoroutine(UncloakRoutine());
    }

    // ȡ������Э��
    private IEnumerator UncloakRoutine()
    {
        // ����Ч��
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

        // ��ȫ�ָ�
        for (int i = 0; i < _allRenderers.Count; i++)
        {
            _allRenderers[i].color = _originalColors[i];
            _allRenderers[i].material = _originalMaterials[i];
        }

        _isCloaked = false;
        _cloakCoroutine = null;
    }

    // �༭���������ռ�������Ⱦ��
    [ContextMenu("Collect All Renderers")]
    private void CollectAllRenderersInEditor()
    {
        _allRenderers.Clear();
        GetComponentsInChildren<SpriteRenderer>(true, _allRenderers);
        Debug.Log($"���ռ� {_allRenderers.Count} �� SpriteRenderer");
    }

    // ����������Ⱦ��״̬
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
