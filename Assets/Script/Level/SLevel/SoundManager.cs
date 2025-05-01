using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioPlayerSource;
    private AudioSource audioLoopSource;
    private AudioSource audioPropSource;
    private AudioSource audioSkillSource;
    public static Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();
    private string resourcePathPrefix = "Sounds/Player/";
    // Start is called before the first frame update
    void Awake()
    {
        audioPlayerSource = gameObject.GetComponent<AudioSource>();
        if (audioPlayerSource == null)
        {
            audioPlayerSource = gameObject.AddComponent<AudioSource>();
        }
        audioLoopSource = gameObject.AddComponent<AudioSource>();
        audioLoopSource.loop = true; // ����Ϊѭ������
        audioPropSource = gameObject.AddComponent<AudioSource>();
        audioSkillSource = gameObject.AddComponent<AudioSource>();
        InitializeAudioClips();
    }
    private void InitializeAudioClips()
    {
        // ����������Ҫ���ص���Ƶ����
        string[] audioNames = new string[]
        {
            "jump",
            "fly",
            "run",
            "attack1",
            "hurt",
            "die",
            "climb",
            "blink",
            "pickup",
            "pickup2",
            "inter",
            "SkillEarth1","SkillEarth2","SkillEarth3",
            "SkillThunder1","SkillThunder2","SkillThunder3",
            "SkillFire1","SkillFire2","SkillFire3",
            "SkillWater1","SkillWater2","SkillWater3",
            "SkillWind1","SkillWind3"
            // ��Ӹ�����Ƶ����
        };

        foreach (var name in audioNames)
        {
            LoadAudioClip(name);
        }
    }
    private void LoadAudioClip(string name)
    {
        string resourcePath = resourcePathPrefix + name;
        if (name.StartsWith("pickup") || name == "inter") resourcePath = "Sounds/Prop/" + name;
        if (name.StartsWith("Skill")) resourcePath = "Sounds/Skill/" + name.Substring(5);

        // ���� AudioClip
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);

        if (clip != null)
        {
            // �� AudioClip ��ӵ��ֵ���
            AudioClips[name] = clip;
            // Debug.Log($"�Ѽ�����Ƶ: {name}");
        }
        else
        {
            Debug.LogWarning($"δ�ҵ���Ƶ��Դ: {resourcePath}");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayerSound(string soundName)
    {
        // Debug.Log($"Playing sound: {soundName}");
        if (soundName == "stop")
        {
            if (audioLoopSource.clip == null)
            {
                return;
            }
            if (!audioLoopSource.isPlaying)
            {
                return;
            }
            audioLoopSource.Stop();
            return;
        }
        AudioClip clip = AudioClips[soundName];
        if (clip != null)
        {
            if (soundName == "jump") { audioPlayerSource.time = 0.3f; }
            if (soundName == "fly" || soundName == "run")
            {
                if (audioLoopSource.clip == null)
                {
                    audioLoopSource.clip = clip;
                }
                else
                {
                    if (audioLoopSource.clip.name != clip.name)
                    {
                        audioLoopSource.Stop();
                        audioLoopSource.clip = clip;
                    }
                }
                audioLoopSource.Play();
            }
            else
            {
                audioPlayerSource.clip = clip;
                audioPlayerSource.Play();
            }
        }
        else
        {
            Debug.LogError("Sound not found");
        }
    }

    public void PickupSound(string soundName)
    {
        Debug.Log("play sound with name : " + soundName);
        AudioClip clip = AudioClips[soundName];
        if (clip != null)
        {
            audioPropSource.clip = clip;
            audioPropSource.Play();
        }
        else
        {
            Debug.LogError("Sound not found");
        }
    }

    public void SkillSound(string soundName)
    {
        soundName = "Skill" + soundName;
        Debug.Log("play sound with name : " + soundName);
        if (soundName == "SkillWind2") return;
        AudioClip clip = AudioClips[soundName];
        if (clip != null)
        {
            audioSkillSource.clip = clip;
            if (soundName == "SkillThunder2") { audioSkillSource.time = 1f; }
            audioSkillSource.Play();
        }
        else
        {
            Debug.LogError("Sound not found");
        }
    }
}
