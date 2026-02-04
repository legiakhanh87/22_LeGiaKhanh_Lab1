using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class LayoutUpdater : MonoBehaviour
{
    HorizontalLayoutGroup group;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<HorizontalLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        group.SetLayoutHorizontal();
        //group.CalculateLayoutInputHorizontal();
    }
}
