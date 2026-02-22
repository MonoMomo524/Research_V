# Ranny_V

모바일 야구 게임 콘텐츠 개발 전 개인적으로 구조를 사전 설계하고 검증한 Unity C# 프로젝트입니다.  
실무 투입 전 아키텍처를 미리 구상하고, 이후 실제 게임 개발 시 참고하여 적용하였습니다.

---

## 포함 콘텐츠

### 1. 조합 시스템 (미완성)
선수카드를 소모하여 새로운 카드를 획득하는 조합 콘텐츠의 프로토타입입니다.

- 시뮬레이션 검증을 위해 UI를 직접 구성하여 동작 확인
- 조합할 선수를 선택하는 단계까지 구현 후 중단 (실무 개발이 먼저 완료됨)

### 2. 친구 시스템 (설계 완료)

---

# 친구 시스템 (Friend System)

게임 내 소셜 기능을 위한 친구 추가/관리 시스템의 구조 설계 프로젝트

## 프로젝트 개요

단일 책임 원칙에 기반한 친구 시스템 설계로, 데이터-로직-UI 간 명확한 역할 분리를 통해 유지보수성과 확장성을 확보했습니다.

- **Model**: 친구 데이터 및 상태 관리 (`FriendData`)
- **View**: UI 렌더링 및 사용자 인터랙션 처리 (`UIPopupFriendListItem`)
- **Controller**: Model-View 간 흐름 제어 및 비즈니스 로직 (`UIPopupFriendList`)

---

## 🛠 사용 환경

- **Engine**: Unity
- **Language**: C#
- **Pattern**: MVC

---

## 핵심 설계 원칙

### 1. 단일 진실 원천 (Single Source of Truth)
- 모든 친구 데이터는 `FriendData.AllFriends` 하나에만 존재
- 상태 변경은 `UpdateFriendState()` 단일 진입점을 통해서만 처리
- View는 항상 원본을 읽어 표시하므로 stale 데이터 발생 없음

### 2. 원본 보호 (Immutability)
- `Get___View()` 메서드는 `FindAll`로 새 리스트 반환
- 호출측에서 정렬/검색 수행해도 `AllFriends` 원본 오염 없음

### 3. 이벤트 위임 (Event Delegation)
- `UIPopupFriendListItem`은 버튼 클릭 사실만 상위로 전달
- 서버 통신·데이터 변경 등 모든 판단은 `UIPopupFriendList`가 담당

---

## 시스템 구조

### 클래스 역할 분리

| 클래스 | 역할 | 책임 범위 |
|--------|------|-----------|
| `FriendData` | 데이터 저장 및 상태 변경 | 친구 정보 저장, 상태값 관리만 담당 |
| `UIPopupFriendList` | 서버 통신 및 뷰 조율 | 모든 의사결정 및 데이터 흐름 제어 |
| `UIPopupFriendListItem` | UI 표시 | 전달받은 데이터를 화면에 렌더링만 |

### 데이터 흐름

```
서버 응답
    ↓
UIPopupFriendList
    ↓ (FriendData.AllFriends 갱신)
FriendData
    ↓ (탭 전환 시 Get___View() 호출)
UIPopupFriendList
    ↓ (SetData + 콜백 전달)
UIPopupFriendListItem × N
    ↓ (버튼 클릭)
OnActionRequested
    ↓ (이벤트 위임)
UIPopupFriendList
    ↓ (Handler 호출)
서버 요청 → 응답 후 UpdateFriendState()
```

---

## 데이터 구조

### eFriendState (친구 관계 상태)

```csharp
public enum eFriendState
{
    NONE     = 0,  // 친구 아님, 삭제된 친구
    FRIEND,        // 친구
    REQUEST,       // 내가 친구에게 요청을 보냄
    RECEIVED,      // 친구가 내게 요청을 보냄
}
```

### FriendInfo (친구 정보)

```csharp
public class FriendInfo
{
    public string    UserID;         // 유저 고유 ID
    public string    UserName;       // 유저 닉네임
    public float     OVR;            // 능력치 지표
    public long      LastAccessTime; // 최근 접속 시간 (0 = 접속 중)
    public eFriendState State;       // 친구 관계 상태
    
    // 추가 정보 (게임 콘텐츠에 따라 확장 가능)
    // - 선호 팀, 랭킹 정보 등
}
```

---

## 주요 기능

### 1. 친구 목록
- 친구 이름, 능력치, 접속 상태 표시
- 추가 정보 표시 (게임 콘텐츠에 따라 확장 가능)
- 현재 친구 수 / 최대 친구 수 표시
- 친구 삭제 기능

### 2. 보낸 요청 목록
- 내가 보낸 친구 요청 목록
- 요청 취소 기능

### 3. 받은 요청 목록
- 받은 요청 목록
- 친구 추가 코드 표시
- 요청 수락/거절 기능

### 상태 전환 규칙

| 액션 | 상태 전환 | eFriendAction |
|------|-----------|---------------|
| 요청 수락 | `RECEIVED` → `FRIEND` | `ACCEPT` |
| 요청 거절 | `RECEIVED` → `NONE` | `DECLINE` |
| 친구 삭제 | `FRIEND` → `NONE` | `DELETE` |
| 요청 취소 | `REQUEST` → `NONE` | `CANCEL` |

---

## 설계 포인트

### 왜 단일 리스트 + 상태값 구조를 선택했는가?

초기에는 `FriendList`, `SentRequestList`, `ReceivedRequestList` 세 개의 리스트로 분리하는 방안도 검토했으나 다음과 같은 이유로 단일 리스트 방식을 채택했습니다:

- **상태 전환의 단순화**: 요청 수락 시 리스트 간 이동이 아닌 상태값 변경만으로 처리 가능
- **데이터 일관성**: 동일 유저가 여러 리스트에 중복 존재하는 문제 원천 차단
- **확장성**: 새로운 상태(예: 차단, 임시 친구 등) 추가 시 enum 확장만으로 대응 가능

### 원본 보호를 위한 FindAll 사용

`Get___View()` 메서드가 `IEnumerable`이 아닌 `List`를 반환하고 `FindAll`을 사용한 이유:

- 호출측에서 정렬/필터링을 수행해도 `AllFriends` 원본이 변경되지 않음
- 메서드 형태로 제공하여 "매번 새로 계산된다"는 의도를 명확히 전달
- 캐싱 없이도 탭 전환 비용이 무시할 수준 (친구 수백 명 규모)

---

## 향후 확장 가능성

현재 구조는 다음과 같은 기능 확장을 고려하여 설계되었습니다:

- 친구 검색/정렬 기능 (이름, 능력치, 접속 시간 등)
- 친구 추천 시스템
- 친구와의 대전 기록 연동
- 추가 상태 관리 (차단, 즐겨찾기 등)

---

## 기술적 고려사항

- **Stale 데이터 방지**: 캐시 없이 단일 원본만 참조하는 구조로, 데이터가 오염될 여지를 차단
- **중복 요청 방지**: 서버 요청 중 버튼 비활성화 처리
- **경계값 처리**: 빈 목록, 최대 친구 수 도달 등 엣지 케이스 고려

---

## 비고

본 저장소의 코드는 회사 업무와 별개로 개인 시간에 작성한 사전 설계 코드이며,  
회사 내부 코드 및 대외비 정보를 포함하지 않습니다.
