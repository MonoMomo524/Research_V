using System;
using UnityEngine;

public class UIPopupFriendListItem : MonoBehaviour
{
    private FriendInfo _friendInfo;

    public enum eFriendAction
    {
        ACCEPT,  // 받은 요청 수락
        DECLINE, // 받은 요청 거절
        DELETE,  // 친구 삭제
        CANCEL,  // 보낸 요청 취소
    }

    // 버튼 클릭 시 누가 어떤 액션을 요청했는지를S 상위에 전달
    public Action<FriendInfo, eFriendAction> OnActionRequested;

    public void SetData(FriendInfo friendInfo, Action<FriendInfo, eFriendAction> onActionRequested)
    {
        _friendInfo = friendInfo;
        OnActionRequested = onActionRequested;

        // UI 할당
        // nameText.text = friendInfo.UserName;
        // emblemImage.sprite = GetEmblem(friendInfo.FavoriteTeam);
        // ...
    }

    // 각 버튼의 onClick에 연결
    public void OnClickAccept() => OnActionRequested?.Invoke(_friendInfo, eFriendAction.ACCEPT);
    public void OnClickDecline() => OnActionRequested?.Invoke(_friendInfo, eFriendAction.DECLINE);
    public void OnClickDelete() => OnActionRequested?.Invoke(_friendInfo, eFriendAction.DELETE);
    public void OnClickCancel() => OnActionRequested?.Invoke(_friendInfo, eFriendAction.CANCEL);
}