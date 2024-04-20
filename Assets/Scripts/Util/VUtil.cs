using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScrollItemInfo = UIScrollView.ScrollItemInfo;

public static class VUtil
{
    #region UIFields



    #endregion

    #region Fields



    #endregion

    #region Constructors



    #endregion

    #region Methods

    public static List<ScrollItemInfo> GenerateRandomCards()
    {
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

            var info = new ScrollItemInfo()
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

        return infoList;
    }

    #endregion
}
