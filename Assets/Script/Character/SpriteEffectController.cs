using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpriteEffectController : MonoBehaviour
{
    [System.Serializable]
    public class EffectPreset
    {
        public string effectName;
        public Color effectColor;
        public Material effectMaterial; // �������ʲο�
        public float fadeDuration = 0.5f;
        public bool overrideCurrentEffect = true;
        public string colorPropertyName = "_Color"; // ���Զ�����ɫ������
    }

    [Header("Renderer Settings")]
    [SerializeField] private bool includeInactive = true;
    [SerializeField] private List<SpriteRenderer> excludedRenderers;

    [Header("Effect Presets")]
    [SerializeField] private List<EffectPreset> effectPresets;

    private readonly List<SpriteRenderer> _allRenderers = new List<SpriteRenderer>();
    private readonly List<MaterialPropertyBlock> _originalPropertyBlocks = new List<MaterialPropertyBlock>();
    private readonly List<Material> _originalMaterials = new List<Material>();
    private readonly Dictionary<string, EffectPreset> _effectDictionary = new Dictionary<string, EffectPreset>();

    private Coroutine _currentEffectCoroutine;
    private string _currentEffectName;

    private void Awake()
    {
        InitializeRenderers();
        InitializeEffectPresets();
        InitializeEffectDictionary();
    }

    private void InitializeRenderers()
    {
        GetComponentsInChildren(includeInactive, _allRenderers);

        foreach (var excluded in excludedRenderers)
        {
            _allRenderers.Remove(excluded);
        }

        _originalPropertyBlocks.Clear();
        _originalMaterials.Clear(); // ��ղ����б�

        foreach (var renderer in _allRenderers)
        {
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            _originalPropertyBlocks.Add(block);
            _originalMaterials.Add(renderer.sharedMaterial); // �洢ԭʼ����
        }
    }

    private void InitializeEffectPresets()
    {
        effectPresets ??= new List<EffectPreset>();

        // ���Ĭ��Ч��Ԥ��
        AddDefaultEffectIfNotExists("Cloak", new Color(1, 1, 1, 0.5f));
        AddDefaultEffectIfNotExists("Stone", new Color(0.5f, 0.5f, 0.5f, 1f));
        AddDefaultEffectIfNotExists("Freeze", new Color(0.5f, 0.8f, 1f, 0.8f));
    }

    private void AddDefaultEffectIfNotExists(string name, Color color)
    {
        if (!EffectPresetExists(name))
        {
            effectPresets.Add(new EffectPreset
            {
                effectName = name,
                effectColor = color,
                effectMaterial = null,
                fadeDuration = 0.5f,
                overrideCurrentEffect = true
            });
        }
    }

    private bool EffectPresetExists(string effectName)
    {
        foreach (var preset in effectPresets)
        {
            if (preset.effectName == effectName)
            {
                return true;
            }
        }
        return false;
    }

    private void InitializeEffectDictionary()
    {
        _effectDictionary.Clear();
        foreach (var preset in effectPresets)
        {
            if (!_effectDictionary.ContainsKey(preset.effectName))
            {
                _effectDictionary.Add(preset.effectName, preset);
            }
            else
            {
                Debug.LogWarning($"Duplicate effect name found: {preset.effectName}");
            }
        }
    }

    public void ApplyEffect(string effectName, float duration)
    {
        if (!_effectDictionary.TryGetValue(effectName, out var effect))
        {
            Debug.LogWarning($"Effect {effectName} not found in presets");
            return;
        }

        // ����Ƿ������ǵ�ǰЧ��
        if (_currentEffectName == effectName && !effect.overrideCurrentEffect)
            return;

        // ֹͣ��ǰ�������е�Ч��
        if (_currentEffectCoroutine != null)
        {
            StopCoroutine(_currentEffectCoroutine);
            RemoveEffectImmediate(_currentEffectName);
        }

        _currentEffectName = effectName;
        _currentEffectCoroutine = StartCoroutine(EffectRoutine(effect, duration));
    }

    public void RemoveEffect(string effectName)
    {
        if (_currentEffectName != effectName) return;

        if (_currentEffectCoroutine != null)
        {
            StopCoroutine(_currentEffectCoroutine);
        }

        if (_effectDictionary.TryGetValue(effectName, out var effect))
        {
            _currentEffectCoroutine = StartCoroutine(ExitEffectRoutine(effect));
        }
    }

    private IEnumerator EffectRoutine(EffectPreset effect, float duration)
    {
        yield return EnterEffect(effect);
        yield return new WaitForSeconds(duration - effect.fadeDuration * 2);
        _currentEffectCoroutine = StartCoroutine(ExitEffectRoutine(effect));
    }

    private IEnumerator EnterEffect(EffectPreset effect)
    {
        float timer = 0f;
        while (timer < effect.fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / effect.fadeDuration;

            for (int i = 0; i < _allRenderers.Count; i++)
            {
                var renderer = _allRenderers[i];
                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);

                // ��ֵ��ɫ
                var originalColor = _originalPropertyBlocks[i].GetColor(effect.colorPropertyName);
                var targetColor = effect.effectColor;
                block.SetColor(effect.colorPropertyName, Color.Lerp(originalColor, targetColor, progress));

                // Ӧ�ò��ʱ仯
                if (effect.effectMaterial != null)
                {
                    renderer.sharedMaterial = effect.effectMaterial;
                }

                renderer.SetPropertyBlock(block);
            }
            yield return null;
        }
    }

    private IEnumerator ExitEffectRoutine(EffectPreset effect)
    {
        float timer = 0f;
        while (timer < effect.fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / effect.fadeDuration;

            for (int i = 0; i < _allRenderers.Count; i++)
            {
                var renderer = _allRenderers[i];
                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);

                // ��Ч����ɫ�����ԭʼ��ɫ
                var originalColor = _originalPropertyBlocks[i].GetColor(effect.colorPropertyName);
                var currentEffectColor = effect.effectColor;
                block.SetColor(effect.colorPropertyName, Color.Lerp(currentEffectColor, originalColor, progress));
                renderer.SetPropertyBlock(block);
            }
            yield return null;
        }

        // ��ȫ�ָ�ԭʼ״̬
        for (int i = 0; i < _allRenderers.Count; i++)
        {
            _allRenderers[i].sharedMaterial = _originalMaterials[i];
            _allRenderers[i].SetPropertyBlock(_originalPropertyBlocks[i]);
        }

        _currentEffectName = null;
        _currentEffectCoroutine = null;
    }

    private void RemoveEffectImmediate(string effectName)
    {
        if (!_effectDictionary.ContainsKey(effectName)) return;

        for (int i = 0; i < _allRenderers.Count; i++)
        {
            _allRenderers[i].SetPropertyBlock(_originalPropertyBlocks[i]);
        }
    }

    [ContextMenu("Collect All Renderers")]
    private void CollectAllRenderersInEditor()
    {
        _allRenderers.Clear();
        GetComponentsInChildren(includeInactive, _allRenderers);
        Debug.Log($"���ռ� {_allRenderers.Count} �� SpriteRenderer");
    }

    [ContextMenu("Reset All Renderers")]
    private void ResetAllRenderers()
    {
        for (int i = 0; i < _allRenderers.Count; i++)
        {
            if (_allRenderers[i] != null)
            {
                _allRenderers[i].SetPropertyBlock(_originalPropertyBlocks[i]);
            }
        }
    }
}