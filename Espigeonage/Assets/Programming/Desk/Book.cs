using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Draggable))]
public class Book : MonoBehaviour
{
    [Header("Components")]
    private Grabbable grabbable;

    [Header("Book Models")]
    [SerializeField] private GameObject bookClose;
    [SerializeField] private GameObject bookOpen;

    [Header("Pages")]
    [SerializeField] private GameObject leftPage;
    [SerializeField] private GameObject rightPage;
    [SerializeField] private GameObject cover;

    private TextMeshProUGUI leftText;
    private TextMeshProUGUI rightText;
    private RawImage leftImage;
    private RawImage rightImage;

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

        leftImage = leftPage.GetComponent<RawImage>();
        rightImage = rightPage.GetComponent<RawImage>();

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
        if(pagePairIndex == 0) SoundManager.Instance.PlaySFX(SoundManager.SFXType.CLOSE_BOOK);
        else SoundManager.Instance.PlaySFX(SoundManager.SFXType.FLIP_PAGE);

        //Open/Close Book
        bookClose.SetActive(pagePairIndex == 0);
        bookOpen.SetActive(pagePairIndex > 0);

        //Activate Pages
        leftPage.SetActive(pagePairIndex != 0);
        rightPage.SetActive(pagePairIndex != 0 && pagePairIndex * 2 < pageData.Count);
        cover.SetActive(pagePairIndex == 0);

        //Update Pages
        leftText.text = pageData.ElementAtOrDefault((pagePairIndex * 2) - 1).text;
        rightText.text = pageData.ElementAtOrDefault(pagePairIndex * 2).text;

        leftImage.texture = pageData.ElementAtOrDefault((pagePairIndex * 2) - 1).image;
        rightImage.texture = pageData.ElementAtOrDefault(pagePairIndex * 2).image;

        if (pagePairIndex == 0)
        {
            cover.GetComponentInChildren<TextMeshProUGUI>().text =
                pageData.ElementAtOrDefault(pagePairIndex * 2).text;
            cover.GetComponent<RawImage>().texture = 
                pageData.ElementAtOrDefault(pagePairIndex * 2).image;
        }
    }
}
