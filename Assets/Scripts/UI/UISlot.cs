using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UISlot : MonoBehaviour
{
    #region UIFields

    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _img;
    [SerializeField] Button _button;

    #endregion

    #region Fields



    #endregion

    #region Constructors

    private void Start()
    {
        if(_button)
        {
            _button.onClick.AddListener(() =>
            {
                Debug.Log($"Clicked {_text.text}");
            });
        }
    }

    #endregion

    #region Methods

    public void SetText(UIScrollView.ScrollItemInfo item)
    {
        if (_text == null)
        {
            Debug.LogError("TextMeshPro is null.");
            return;
        }

        if(item._itemType == UIScrollView.ScrollItemInfo.eItemType.ITEM)
        {
            _text.SetText($"ItemID: {item._itemID}");
        }
        else if (item._itemType == UIScrollView.ScrollItemInfo.eItemType.PLAYER)
        {
            var playerData = item._playerData;
            StringBuilder sb = new StringBuilder();

            // Grade
            for(int star = 0; star<(int)playerData._Grade; star++)
            {
                sb.Append("#");
            }
            sb.Append("\n");

            // Name
            sb.Append($"{playerData._FullName}");
            if(playerData._Type != PlayerData.eType.IMPACT)
            {
                sb.Append($"\'{playerData._Season.ToString().Substring(2, 2)}");
            }
            sb.Append("\n");

            // Type
            sb.Append($"{playerData._Type.ToString()}\n");

            _text.SetText(sb);
            sb.Clear();
            sb = null;
        }
        else
            _text.SetText(string.Empty);
    }

    public void SetImage(UIScrollView.ScrollItemInfo item)
    {
        if (_img == null)
        {
            Debug.LogError("Image is null.");
            return;
        }

        if (item._itemType == UIScrollView.ScrollItemInfo.eItemType.ITEM)
            Debug.Log($"Set sprite to item_{item._itemID}");
        else if (item._itemType == UIScrollView.ScrollItemInfo.eItemType.PLAYER)
            Debug.Log($"Set sprite to {item._playerData._FullName}_{item._playerData._Season}_{item._playerData._Type.ToString()}.");
    }

    #endregion
}
