using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElementSelect : MonoBehaviour
{
    private TextMeshProUGUI text;
    Dictionary<ElementType, Color> colorMapping = new Dictionary<ElementType, Color>(){
       {ElementType.Fire, Color.red},
       {ElementType.Water, Color.blue},
       {ElementType.Earth, Color.yellow},
       {ElementType.Thunder, new Color(0.54f, 0.17f, 0.89f, 1f)},
       {ElementType.Wind, Color.green}
   };

    private ClickSound ClickSound;

    // Start is called before the first frame update  
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        ColorUpdate();
        ClickSound = FindObjectOfType<ClickSound>();
    }

    // Update is called once per frame  
    void Update()
    {

    }

    bool IsEqualElement(ElementType element)
    {
        return gameObject.name == element.ToString();
    }

    public void ColorUpdate()
    {
        text.color = Color.grey;
        if (IsEqualElement(LevelBaseData.Instance.elementType))
        {
            text.color = colorMapping.GetValueOrDefault(LevelBaseData.Instance.elementType);
        }
    }

    public void SelectElement()
    {
        if(ClickSound == null)
        {
            ClickSound = FindObjectOfType<ClickSound>();
        }
        if (ClickSound != null)
        {
            ClickSound.PlayClickSound();
        }
        else
        {
            Debug.Log("ClickSound component not found in the scene.");
        }

        foreach (ElementType element in System.Enum.GetValues(typeof(ElementType)))
        {
            if(IsEqualElement(element))
            {
                LevelBaseData.Instance.elementType = element;
                break;
            }
        }
    }
}
