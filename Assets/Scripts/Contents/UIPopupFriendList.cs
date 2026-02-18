using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UIPopupFriendListItem;

public interface IPopupFriendList
{
    void OpenPopupFriendList();
}

public class UIPopupFriendList : MonoBehaviour, IPopupFriendList
{
    #region UIFields

    [SerializeField] private UIPopupFriendListItem _friendListItemPrefab;
    [SerializeField] private Transform _myFriendsContent;
    [SerializeField] private Transform _sentContent;
    [SerializeField] private Transform _receivedContent;

    #endregion

    #region Fields

    public enum eFriendListTab
    {
        MY_FRIENDS = 0,
        SENT_REQUESTS,
        RECEIVED_REQUESTS,
    }
    private eFriendListTab _currentTab = eFriendListTab.MY_FRIENDS;

    #endregion

    #region Constructors

    #endregion

    #region Methods

    private void SetPage()
    {
        SpawnItem();
    }

    private void SpawnItem()
    {
        // 현재 탭에 맞는 부모 오브젝트 선택
        var parent = _currentTab switch
        {
            eFriendListTab.MY_FRIENDS => _myFriendsContent,
            eFriendListTab.SENT_REQUESTS => _sentContent,
            eFriendListTab.RECEIVED_REQUESTS => _receivedContent,
            _ => null
        };
        if (parent == null) return;

        // 부모 오브젝트 하위의 아이템 개수와 데이터 개수가 일치하지 않으면 아이템을 새로 생성
        eFriendState stateType = _currentTab switch
        {
            eFriendListTab.MY_FRIENDS => eFriendState.FRIEND,
            eFriendListTab.SENT_REQUESTS => eFriendState.REQUEST,
            eFriendListTab.RECEIVED_REQUESTS => eFriendState.RECEIVED,
            _ => eFriendState.NONE
        };

        var friendInfoList = FriendSystemData.Instance.GetFriendListView(stateType);
        if (friendInfoList.Count > parent.childCount)
        {
            var diff = friendInfoList.Count - parent.childCount;
            for (int i = diff; i < friendInfoList.Count; i++)
            {
                var item = Instantiate(_friendListItemPrefab, parent);
            }
        }
        else if (friendInfoList.Count < parent.childCount)
        {
            for (int i = friendInfoList.Count; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }

        // 아이템에게 데이터와 콜백을 함께 전달
        for (int i = 0; i < parent.childCount; i++)
        {
            var item = parent.GetChild(i).GetComponent<UIPopupFriendListItem>();
            if (item == null)
            {
                Debug.LogError("FriendListItem prefab is missing UIPopupFriendListItem component.");
                continue;
            }

            item.SetData(friendInfoList[i], OnFriendActionRequested);
        }
    }

    // 모든 아이템의 버튼 이벤트가 여기로 집결
    private void OnFriendActionRequested(FriendInfo friendInfo, eFriendAction action)
    {
        switch (action)
        {
            case eFriendAction.ACCEPT: RequestAcceptFriend(friendInfo._UserID); break;
            case eFriendAction.DECLINE: RequestDeclineFriend(friendInfo._UserID); break;
            case eFriendAction.DELETE: RequestDeleteFriend(friendInfo._UserID); break;
            case eFriendAction.CANCEL: RequestCancelFriend(friendInfo._UserID); break;
        }
    }

    private void RequestAcceptFriend(int userID)
    {
        // AcceptFriendHandler 호출
        // 서버 응답 후 FriendData.UpdateFriendState(userID, eFriendState.FRIEND)
    }

    private void RequestDeclineFriend(int userID)
    {
        // DeclineFriendHandler 호출
        // 서버 응답 후 FriendData.RemoveFriend(userID)
    }

    private void RequestDeleteFriend(int userID)
    {
        // DeleteFriendHandler 호출
        // 서버 응답 후 FriendData.RemoveFriend(userID)
    }

    private void RequestCancelFriend(int userID)
    {
        // CancelFriendHandler 호출
        // 서버 응답 후 FriendData.RemoveFriend(userID)
    }

    #endregion

    #region interface implementations

    public void OpenPopupFriendList()
    {
        _currentTab = eFriendListTab.MY_FRIENDS;

        SetPage();
    }

    #endregion
}
