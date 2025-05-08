using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementUpdate : MonoBehaviour
{
    public List<GameObject> children = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
    }
    public void OnClick()
    {
        foreach (var child in children)
        {
            child.GetComponent<ElementSelect>().ColorUpdate();
        }
    }
}
