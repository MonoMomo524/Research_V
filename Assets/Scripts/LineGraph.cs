using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineGraph : MonoBehaviour
{
    #region Fields

    UILineConnector _lineConnector;

    private enum eQuadrant
    {
        NONE = 0,
        FIRST = 1,
        SECOND,
        THIRD,
        FOURTH
    }

    [SerializeField]
    eQuadrant _graphQuadrant;


    #endregion

    #region Constructor

    private void Awake()
    {
        if(!_lineConnector)
        {
            _lineConnector = new UILineConnector();
        }
    }

    private void OnDestroy()
    {
        if (_lineConnector) _lineConnector = null;
    }

    #endregion

    #region Methods

    #endregion
}
