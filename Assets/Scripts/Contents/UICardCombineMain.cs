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

        char c = 'A';
        List<ScrollItemInfo> infoList = new List<ScrollItemInfo>();
        for (int i = 0; i < 25; i++)
        {
            var playerData = new PlayerData();
            if (playerData is not null)
            {
                playerData._FullName = ((char)((int)c + i)).ToString();
                playerData._Type = (PlayerData.eType)(int)Random.Range(0, 3.9f);
            }

            ScrollItemInfo info = new ScrollItemInfo()
            {
                _itemType = ScrollItemInfo.eItemType.PLAYER,
                _playerData = playerData
            };

            switch (info._playerData._Type)
            {
                case PlayerData.eType.NORMAL:
                    info._playerData._Grade = (PlayerData.eGrade)(int)Random.Range(1, 5.9f);
                    info._playerData._Season = (int)Random.Range(2021, 2024.9f);
                    break;
                case PlayerData.eType.SEASON:
                    info._playerData._Grade = (PlayerData.eGrade)(int)Random.Range(3, 5.9f);
                    info._playerData._Season = (int)Random.Range(1982, 2024.9f);
                    break;
                case PlayerData.eType.IMPACT:
                    info._playerData._Grade = PlayerData.eGrade.STAR_4;
                    info._playerData._Season = -1;
                    break;
                case PlayerData.eType.SIGNATURE:
                    info._playerData._Grade = PlayerData.eGrade.STAR_5;
                    info._playerData._Season = (int)Random.Range(1982, 2024.9f);
                    break;

                default:
                    break;
            }

            infoList.Add(info);
        }

        StartCoroutine(SetMaterialScroll(infoList));
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
