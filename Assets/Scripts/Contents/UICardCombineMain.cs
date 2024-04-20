using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScrollItemInfo = UIScrollView.ScrollItemInfo;

public class UICardCombineMain : MonoBehaviour
{
    #region UIFields

    [SerializeField] GameObject[] _selectedSlots;

    [SerializeField] GameObject _objSelectedEmpty;

    [SerializeField] GameObject _materialScroll;

    #endregion

    #region Fields



    #endregion

    #region Constructors

    private void Start()
    {
        InitializeSelectedSlots();
        SetMaterialScroll();
    }

    #endregion

    #region Methods

    /// <summary>
    /// 선택된 재료카드 슬롯(상단 5개) 초기화 함수
    /// </summary>
    private void InitializeSelectedSlots()
    {
        foreach (var objSlot in _selectedSlots)
        {
            if(objSlot)
            {
                objSlot.SetActive(false);
            }
        }

        if(_objSelectedEmpty)
        {
            _objSelectedEmpty.SetActive(true);
        }
    }

    /// <summary>
    /// 재료카드 스크롤(하단) 초기화 함수
    /// </summary>
    public void SetMaterialScroll()
    {
        if (_materialScroll == null)
        {
            Debug.LogError("materialScroll is null.");
            return;
        }

        StartCoroutine(SetMaterialScroll(VUtil.GenerateRandomCards()));
    }

    IEnumerator SetMaterialScroll(List<ScrollItemInfo> infoList)
    {
        yield return new WaitForEndOfFrame();

        if(_materialScroll)
        {
            _materialScroll.GetComponent<UIScrollView>()._initalizeScroll.Invoke(infoList);
        }
    }

    #endregion
}
