using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
struct PageData
{
    public Texture2D image;
    [TextArea(3, 10)] public string text;
}

public class Document : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject page;
    private TextMeshProUGUI pageText;
    private RawImage pageImage;

    [Header("Pages")]
    [SerializeField] private int pageIndex = 0;
    [SerializeField] private List<PageData> pageData;

    private void Awake()
    {
        pageText = page.GetComponentInChildren<TextMeshProUGUI>();
        pageImage = page.GetComponent<RawImage>();

        UpdateVisuals();
    }

    public void PreviousPage()
    {
        if (pageIndex <= 0) return;
        pageIndex--;
        UpdateVisuals();
    }

    public void NextPage()
    {
        if (pageIndex >= pageData.Count - 1) return;
        pageIndex++;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        pageText.text = pageData.ElementAtOrDefault(pageIndex).text;
        pageImage.texture = pageData.ElementAtOrDefault(pageIndex).image;
    }
}
