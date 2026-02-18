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

    private void OnDestroy()
    {
        if(_content)
        {
            for (int i = _content.childCount; i>0; i--)
            {
                var child = _content.GetChild(i - 1);
                Destroy(child);
            }
        }
    }

    #endregion

    #region Methods

    private void InitScroll(List<ScrollItemInfo> infoList)
    {
        if (infoList == null) return;

        _itemCount = infoList.Count;
    }

    public bool CheckInitialize()
    {
        return _itemCount > 0;
    }

    private void SetScroll(List<ScrollItemInfo> infoList)
    {
        if (!_itemPrefab)
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

        for (int i = 0; i < _itemCount; i++)
        {
            GameObject objItem = Instantiate(_itemPrefab, _content);
            if (objItem is null)
            {
                Debug.LogError("Cannot instantiate item.");
                continue;
            }

            if (infoList[i]._itemType == ScrollItemInfo.eItemType.ITEM ||
                infoList[i]._itemType == ScrollItemInfo.eItemType.PLAYER)
            {
                var uiSlot = objItem.GetComponent<UISlot>();
                uiSlot.SetText(infoList[i]);
                uiSlot.SetImage(infoList[i]);

                Messaging.Execute<ISetSlot>(objItem, (t) =>
                {
                    t.SetText(infoList[i]);
                    t.SetImage(infoList[i]);
                }, true);
            }
        }
        _itemPrefab.SetActive(true);
    }

    public List<GameObject> GetItems()
    {
        if (_content == null || _content.childCount == 0) return null;

        List<GameObject> returnList = new List<GameObject>();
        for (int i = 0; i < _content.childCount; i++)
        {
            var child = _content.GetChild(i).gameObject;
            if(child) returnList.Add(child);
        }

        returnList.Remove(_itemPrefab);
        return returnList;
    }

    #endregion
}
