using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ScrollItemInfo = UIScrollView.ScrollItemInfo;

public class UICardCombineMain : MonoBehaviour
{
    #region UIFields

    [SerializeField] List<GameObject> _selectedSlots = new List<GameObject>();
    [SerializeField] GameObject _objSelectedEmpty;
    [SerializeField] GameObject _materialScroll;

    #endregion

    #region Fields

    [SerializeField] EventSystem _eventSystem;
    private float dragTreshold = 0.5f;
    readonly float inch = 2.54f;

    private List<ScrollItemInfo> _selectedMaterials = new List<ScrollItemInfo>();
    private List<PlayerData> _selectedPlayers = new List<PlayerData>();

    private const int _maxCount = 5;

    #endregion

    #region Constructors

    private void Start()
    {
        InitializeSelectedSlots();
        SetMaterialScroll();

        // ScrollRect 내 버튼이 눌리지 않아 드래그 감도를 DPI에 맞게 조절
        _eventSystem.pixelDragThreshold = (int)(dragTreshold * Screen.dpi / inch);
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
                //Messaging.Broadcast<ISetSlot>(objSlot, t =>
                //{
                //    t.SetOnDeselectSlot()
                //});
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

        // 선수 보관함을 임시로 생성
        StartCoroutine(SetMaterialScroll(VUtil.GenerateRandomCards()));
    }

    IEnumerator SetMaterialScroll(List<ScrollItemInfo> infoList)
    {
        yield return new WaitForEndOfFrame();

        if(_materialScroll)
        {
            var scroll = _materialScroll.GetComponent<UIScrollView>();
            scroll?._initalizeScroll.Invoke(infoList);
            yield return new WaitUntil(() => scroll.CheckInitialize());
            var itemList = scroll.GetItems();

            foreach (var objItem in itemList)
            {
                Messaging.Execute<ISetSlot>(objItem, (t) =>
                {
                    t.SetOnClick(() =>
                    {
                        ClickItem(objItem);
                    });
                }, true);
            }
        }
    }

    private void ClickItem(GameObject obj)
    {
        // 이미 있으면 선택하지 않음
        if (_selectedSlots.Contains(obj))
        {
            var index = _selectedSlots.IndexOf(obj);
            _selectedSlots.Remove(obj);

            Messaging.Execute<ISetSlot>(obj, tt =>
            {
                tt.SetOnDeselectSlot(null);
            }, true);

            _selectedSlots.Remove(obj);
        }
        // 재료카드를 소모하기 위해 선택한 경우
        else if (_selectedMaterials.Count < _maxCount)
        {
            var index = _selectedMaterials.Count;
            
        }
        else
        {
            return;
        }

        RefreshSelectedSlots();
    }

    public void RefreshSelectedSlots()
    {
        for (int i = 0; i < _maxCount; i++)
        {
            if (_selectedMaterials.Count <= i)
            {
                _selectedSlots[i].SetActive(false);
                continue;
            }

            _selectedSlots[i].SetActive(true);
            Messaging.Execute<ISetSlot>(_selectedSlots[i], t =>
            {
                // t.SetImage()
            }, true);
        }
    }

    #endregion

    #region InterfaceImplements



    #endregion
}
