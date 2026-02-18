using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTeamName
{
    TeamA = 0,
    TeamB,
    TeamC,
    TeamD,
    TeamE,
    TeamF,
    TeamG,
    TeamH,
    TeamI,
    TeamJ,
}

public class PlayerData
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
        set { _grade = value; }
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
        set { _type = value; }
    }

    private string _fullName;
    public string _FullName
    {
        get { return _fullName; }
        set { _fullName = value; }
    }

    private int _season;
    public int _Season
    {
        get { return _season; }
        set { _season = value; }
    }

    #endregion

    #region Constructors



    #endregion

    #region Methods



    #endregion
}
