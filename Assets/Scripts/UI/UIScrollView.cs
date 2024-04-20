using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScrollView : MonoBehaviour
{
    #region UIFields

    [SerializeField] GameObject _itemPrefab;
    [SerializeField] GameObject _emptyText;

    [SerializeField] RectTransform _content;

    #endregion

    #region Fields

    public delegate void InitializeScroll(List<ScrollItemInfo> infoList);
    public InitializeScroll _initalizeScroll;

    private int _itemCount;

    public class ScrollItemInfo
    {
        public enum eItemType
        {
            NONE = -1,
            PLAYER = 0,
            ITEM = 1,
            MAX
        }
        public eItemType _itemType;
        public int _itemID;
        public PlayerData _playerData;
    }

    #endregion

    #region Constructors

    private void Awake()
    {
        _initalizeScroll = new InitializeScroll(InitScroll);
        _initalizeScroll += SetScroll;
    }

    #endregion

    #region Methods

    private void InitScroll(List<ScrollItemInfo> infoList)
    {
        if (infoList == null) return;

        _itemCount = infoList.Count;
    }

    private void SetScroll(List<ScrollItemInfo> infoList)
    {
        if(!_itemPrefab)
        {
            Debug.LogError("itemPrefab is null.");
            return;
        }

        bool needSetItem = _itemCount > 0;
        if (_emptyText)
        {
            _emptyText.SetActive(!needSetItem);
        }

        _itemPrefab.SetActive(needSetItem);
        if (!needSetItem) return;

        for (int i = 0; i< _itemCount; i++)
        {
            GameObject objItem = Instantiate(_itemPrefab, _content);
            if (objItem is null)
            {
                Debug.LogError("Cannot instantiate item.");
                continue;
            }

            if(infoList[i]._itemType == ScrollItemInfo.eItemType.ITEM ||
                infoList[i]._itemType == ScrollItemInfo.eItemType.PLAYER)
            {
                var uiSlot = objItem.GetComponent<UISlot>();
                uiSlot.SetText(infoList[i]);
                uiSlot.SetImage(infoList[i]);
            }
        }
        _itemPrefab.SetActive(!needSetItem);
    }

    #endregion
}
