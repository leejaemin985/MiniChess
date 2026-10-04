# 구현 가정 목록

「캐릭터 및 스킬 구현 명세 v0.1」 1절 지시(임시값·설계안 적용 시 가정 명시)에 따라, 코어 코드에 들어간 **확정되지 않은 값과 정책**을 기록한다.
값이나 정책이 확정되면 해당 항목을 갱신하거나 지운다.

## 1. 테스트 프리셋 임시값 (`Presets/PrototypeTestPreset.cs`)

플레이 테스트용이며 밸런스 값이 아니다. 명세상 모두 TBD.

### 캐릭터 수치

| ID | HP | 공격 | 사거리 | 근거 |
|---|---|---|---|---|
| SCYTHE | 10 | 3 | 1 | 전사와 암살자의 중간 스펙(4.1) |
| MORTAR | 9 | 2 | 3 | 장거리 포격수 방향(5.1) |
| GARDENER | 16 | 2 | 1 | 높은 체력 수호자(6.1), 기존 샘플 guardian 값 |
| WARRIOR | 12 | 3 | 1 | 준수한 체력·높은 공격(7.1), 기존 샘플 warrior 값 |
| ARCHER | 8 | 2 | 3 | 낮은 체력·긴 사거리(8.1), 기존 샘플 archer 값 |
| CHEMIST | 9 | 2 | 2 | 제어형 기본값 |
| FLAME | 9 | 3 | 2 | 딜러 방향(10.1) |
| SEAMSTRESS | 9 | 2 | 2 | 제어형 기본값 |
| CHAIN_GUARD | 15 | 2 | 1 | 수호형, GARDENER 보다 약간 낮게 |

### 경기 구성

- QuickBattle 양 팀 구성: WARRIOR, ARCHER, GARDENER, SCYTHE (기존 샘플의 역할 분포와 비슷하게).
- 테스트 맵 시작 위치: 기획서에 없어 아래 두 줄(P1), 위 두 줄(P2)로 지정.

규칙 수치(AP 6/4/4, 이월, 이동 1, 기본 공격 2, 4 vs 4, 점령 2회)는 명세의 [프로토타입] 값을 그대로 사용했다.

## 2. 명세 TBD 에 대한 임시 정책

| 항목 | 현재 정책 | 위치 | 명세 |
|---|---|---|---|
| 같은 턴 단계 안의 처리 순서 | 상태효과: 유닛 Id 오름차순 → 부여 순. 칸 효과: (y, x) 오름차순 → 레이어 순. 상태효과를 칸 효과보다 먼저 처리 | `StatusSystem.RunStep`, `CellEffectSystem.RunStep`, `TurnSystem.RunHooks` | 3.3 "같은 시점의 여러 피해와 사망 처리 순서" |
| 한 상태의 같은 단계 처리 | 발동(Trigger) 후 감소(Decrement) | `StatusSystem.RunStep` | 13.4 |
| 소환물만 남은 경우 | `SummonEliminationPolicy.Undecided`: 실제로 발생하면 설정 오류(GameConfigException)로 중단 | `MatchRuleData`, `PlayerState.IsEliminated` | 3.1, 14 |
| 사망한 유닛의 상태효과 | 모두 제거(TargetDied) | `DeathSystem` | 명시 없음 |
| 칸 효과를 벽 칸에 설치 | 거부 | `CellEffectSystem.CanPlace` | 명시 없음 |
| 피해 가로채기(호위/보호막) 순서 | Order 값 오름차순. 현재 등록된 규칙 없음 | `IDamageInterceptor` | 12.2, 13.3 |

## 3. 확정 규칙으로 고정한 것 (참고)

- 이동은 상하좌우 직선만. 공격/스킬 사거리는 체비셰프 거리. (사용자 확정)
- 외부 이동(다른 유닛이 일으킨 넉백/당기기/워프)은 행동 잠금의 예외이며, 사용한 전투 행동을 회복시키지 않는다. 자발적 여부는 "이동하는 유닛 = 이동을 일으킨 유닛"으로 판정한다.
- 장판은 타일당 1개(덮어쓰기), 덫은 별도 레이어로 공존, 점령 칸에는 덫 설치 불가·장판 허용.
