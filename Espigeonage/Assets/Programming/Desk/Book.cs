using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Draggable))]
public class Book : MonoBehaviour
{
    [Serializable]
    struct PageData
    {
        [TextArea(3, 10)] public string content;
    }

    [Header("Components")]
    private Grabbable grabbable;

    [SerializeField] private GameObject bookClose;
    [SerializeField] private GameObject bookOpen;

    [SerializeField] private GameObject leftPage;
    [SerializeField] private GameObject rightPage;

    private TextMeshProUGUI leftText;
    private TextMeshProUGUI rightText;

    [Header("Book")]
    [SerializeField] private int pagePairIndex = 0;
    [SerializeField] private List<PageData> pageData;

    private void OnEnable()
    {
        grabbable.GrabbedStatus += CloseBook;
    }

    private void OnDisable()
    {
        grabbable.GrabbedStatus -= CloseBook;
    }

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        leftText = leftPage.GetComponentInChildren<TextMeshProUGUI>();
        rightText = rightPage.GetComponentInChildren<TextMeshProUGUI>();
        UpdateBookVisuals();
    }

    public void PreviousPage()
    {
        if (pagePairIndex <= 0) return;
        pagePairIndex--;
        UpdateBookVisuals();
    }

    public void NextPage()
    {
        if (pagePairIndex * 2 >= pageData.Count - 1) return;
        pagePairIndex++;
        UpdateBookVisuals();
    }

    public void CloseBook(bool _state)
    {
        if (!_state) return;
        pagePairIndex = 0;
        UpdateBookVisuals();
    }

    private void UpdateBookVisuals()
    {
        //Open/Close Book
        bookClose.SetActive(pagePairIndex == 0);
        bookOpen.SetActive(pagePairIndex > 0);

        //Update UI
        leftPage.SetActive(pagePairIndex != 0);
        rightPage.SetActive(pagePairIndex * 2 < pageData.Count);
        leftText.text = pageData.ElementAtOrDefault((pagePairIndex * 2) - 1).content;
        rightText.text = pageData.ElementAtOrDefault(pagePairIndex * 2).content;
    }
}
