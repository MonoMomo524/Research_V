using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    #region UIFields



    #endregion

    #region Fields

    public enum eGrade
    {
        STAR_1 = 1,
        STAR_2,
        STAR_3,
        STAR_4,
        STAR_5,
    }
    private eGrade _grade;
    public eGrade _Grade
    {
        get { return _grade; }
    }

    public enum eType
    {
        SEASON      = 0,
        NORMAL      = 1,
        IMPACT      = 2,
        SIGNATURE   = 3,
        GOLDENGLOVE = 4,
    }
    private eType _type;
    public eType _Type
    {
        get { return _type; }
    }

    private string _fullName;
    public string _FullName
    {
        get { return _fullName; }
    }

    private int _season;
    public int _Season
    {
        get { return _season; }
    }

    #endregion

    #region Constructors



    #endregion

    #region Methods



    #endregion
}
