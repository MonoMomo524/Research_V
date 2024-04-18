using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        InitializeSelectedSlots();
        SetMaterialScroll();
    }

    #endregion

    #region Methods

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

    public void SetMaterialScroll()
    {
        if (_materialScroll == null)
        {
            Debug.LogError("materialScroll is null.");
            return;
        }

        SendMessage("SetScroll", SendMessageOptions.DontRequireReceiver);
    }

    #endregion
}
