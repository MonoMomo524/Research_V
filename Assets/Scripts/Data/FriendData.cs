using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Unity.VisualScripting;

/// <summary>
/// 콘텐츠별 순위 항목 (총 6개 콘텐츠)
/// </summary>
public enum eContentType
{
    Content1 = 0,
    Content2,
    Content3,
    Content4,
    Content5,
    Content6,
}

/// <summary>
/// 콘텐츠별 순위 정보
/// </summary>
[Serializable]
public struct ContentRankInfo
{
    public eContentType _ContentType;  // 콘텐츠 종류
    public int _Rank;         // 해당 콘텐츠 순위 (0이면 미집계 또는 없음)
}

/// <summary>
/// 친구 관계 상태
/// </summary>
public enum eFriendState
{
    NONE = 0, // 친구 아님, 삭제된 친구
    FRIEND,       // 친구
    REQUEST,      // 내가 친구에게 요청을 보냄
    RECEIVED,     // 친구가 내게 요청을 보냄(내가 받음)
}

/// <summary>
/// 유저 기본 정보 + 친구 상태 통합 관리
/// </summary>
[Serializable]
public class FriendInfo
{
    public int _UserID;         // 유저 고유 ID
    public string _UserName;       // 유저 닉네임
    public eTeamName _FavoriteTeam;   // 선호 구단
    public float _OVR;            // 오버롤
    public long _LastAccessTime; // 최근 접속 시간 (Unix timestamp, 0이면 접속 중)

    public eFriendState _FriendState;       // 친구 관계 상태

    // FRIEND 상태일 때만 유효
    public List<ContentRankInfo> _ContentRanks = new List<ContentRankInfo>();

    public bool IsOnline => _LastAccessTime == 0;

    public TimeSpan TimeSinceLastAccess
    {
        get
        {
            if (IsOnline)
                return TimeSpan.Zero;

            return DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(_LastAccessTime);
        }
    }

    /// <summary>
    /// 서버로부터 받은 친구 정보 JSON을 파싱하여 FriendInfo 객체에 데이터를 채우는 함수
    /// </summary>
    /// <param name="json"></param>
    public void ReadFriendInfoJson(JSONNode json)
    { 
        if (json.HasKey("user_info"))
        {
            var userInfoJson = json["user_info"];
            _UserID = userInfoJson["user_id"].AsInt;
            _UserName = userInfoJson["user_name"].Value;
            _FavoriteTeam = (eTeamName)Enum.Parse(typeof(eTeamName), userInfoJson["favorite_team"].Value);
            _OVR = userInfoJson["ovr"].AsFloat;
            _LastAccessTime = userInfoJson["last_access_time"].AsLong;
        }
    }
}

/// <summary>
/// 친구 시스템 전체 데이터
/// </summary>
[Serializable]
public class FriendSystemData
{
    private int _maxFriendCount;               // 최대 친구 수
    private List<FriendInfo> _friendList = new List<FriendInfo>();        // 친구 목록
    private string _myFriendCode = string.Empty;      // 나의 친구 추가 코드

    /// <summary>
    /// 현재 친구 수
    /// </summary>
    public int CurrentFriendCount => _friendList.Count;

    /// <summary>
    /// 친구 목록이 가득 찼는지 여부
    /// </summary>
    public bool IsFriendListFull => CurrentFriendCount >= _maxFriendCount;

    FriendSystemData()
    {
        _maxFriendCount = 100; // 기본 최대 친구 수 설정 (기획데이터 사용 전 임시 값)
    }

    public void ReadFriendInfoJson(JSONNode json)
    {
        if (json.HasKey("max_friend_count"))
        {
            _maxFriendCount = json["max_friend_count"].AsInt;
        }
        if (json.HasKey("my_friend_code"))
        {
            _myFriendCode = json["my_friend_code"].Value;
        }
        if (json.HasKey("friend_list"))
        {
            var friendListJson = json["friend_list"].AsArray;
            _friendList.Clear();
            foreach (var friendJson in friendListJson)
            {
                FriendInfo friendInfo = new FriendInfo();
                friendInfo.ReadFriendInfoJson(friendJson);
                _friendList.Add(friendInfo);
            }
        }
    }
}