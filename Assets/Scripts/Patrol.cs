using UnityEngine;
using System.Collections;

/// <summary>
/// 추적자(캡슐)의 순찰 목적지를 계산하는 클래스
/// </summary>
public class Patrol : MonoBehaviour {

    public int patrolMode;              // 1이면 복도만(남자1), 2이면 복도와 잠기지 않은 방 전부를(남자2), 3이면 완전 랜덤으로(여자) 순찰함
    public float hearing = 10f;         // [능력치] 주인공의 발소리를 듣는 능력. 주인공이 뛸 때를 기준으로 주인공을 감지하는 최대 거리.
    public GameObject playerLocation;

    private float seeCount;
    private int moveState;              // 움직이는 상태가 0이면 순찰 중, 1이면 주인공을 목격하여 뛰어감, 2이면 주인공 발소리를 듣고 걸어감,
                                        // 3이면 주인공 발소리를 듣고 뛰어감, 4이면 10초간 아무 명령도 받지 않고 랜덤 지점으로 걸어갔다가 상태가 0으로 바뀜
    private Location tempLocation;      // 최근에 지난 지점(목적 지점으로 설정되지 않았더라도) -> Location.cs에서 설정해 줌
    private Location destLocation;      // 목적 지점
    private Location passedLocation;    // 이전에 마지막으로 설정되었던 목적 지점
    private int tempZoneID;             // 이 추적자가 현재 머물러 있는 AudioZone의 ID
    Vector3 target;
    Transform chaser;

	void Start () {
        tempLocation = null;
        destLocation = null;
        passedLocation = null;
        chaser = GetComponent<Transform>();
        seeCount = 0f;
        moveState = 0;
        tempZoneID = 0;
	}

    void FixedUpdate()
    {
        target = GetComponent<AutoMove>().GetTargetPosition();
        /*
        if (Input.GetKey(KeyCode.F1))
        {
            if (destLocation != null) Debug.Log("dest = " + destLocation.GetLocationID());
            if (passedLocation != null) Debug.Log("pass = " + passedLocation.GetLocationID());
            if (tempLocation != null) Debug.Log("temp = " + tempLocation.GetLocationID());
        }
        */
        RaycastHit rayHit;
        if (!Physics.Raycast(chaser.position, target - chaser.position, out rayHit))
        {
            // 이런 경우는 없다고 가정해도 좋다
            Debug.LogError("Raycast Failed");
            moveState = 0;
        }
        // 탈진하면
        else if (GetComponent<AutoMove>().isExhausted)
        {
            if (moveState != 0)
            {
                seeCount = 0f;
                moveState = 0;
                //Debug.Log("Chaser is exhausted");
            }
   
            // 경로 재설정
            destLocation = null;
        }
        // 주인공을 목격하면
        else if (rayHit.collider.name == "Player")
        {
            // 버그로 인해 딱 1프레임 동안만 주인공을 보고 돌진하는 경우를 방지
            if (moveState != 1 && seeCount < 0.2f)
            {
                seeCount += Time.fixedDeltaTime;
            }
            else if (moveState != 1 && seeCount >= 0.2f)
            {
                seeCount = 0f;
                moveState = 1;
                //Debug.Log("moveState = " + moveState);
            }

            if (moveState == 1)
            {
                // 기존 목적 지점을 버리고 주인공을 우선적으로 쫓아감
                playerLocation.GetComponent<Transform>().position = target + new Vector3(0f, 0.2f, 0f);
                destLocation = playerLocation.GetComponent<Location>();
            }
        }
        // 주인공을 목격하지 못했고 주인공의 발소리를 들었다면 (또는 주인공이 계단에 있고 거리가 가까우면)
        else if (Move.move.isMoving && (isSameFloor(target, chaser.position) &&
            ((isNearEnough(target, chaser.position, hearing) && Move.move.isRunning) ||
            (isNearEnough(target, chaser.position, hearing * 0.7f) && !Move.move.isRunning))) ||    // (음량은 거리에 반비례)
            ((Move.move.GetTempZoneID() / 100) % 10 == 0 &&
            ((Vector3.Distance(target, chaser.position) < hearing && Move.move.isRunning) ||
            ((Vector3.Distance(target, chaser.position) < hearing * 0.7f) && !Move.move.isRunning))))
        {
            if (moveState == 0)
            {
                seeCount = 0f;
                moveState = 2;      // 순찰 중 -> 듣고 걸어감 
                //Debug.Log("moveState = " + moveState);
            }
            else if (moveState == 1)
            {
                seeCount = 0f;
                moveState = 3; // 목격하고 뛰어감 -> 듣고 뛰어감
                //Debug.Log("moveState = " + moveState);
            }

            // 기존 목적 지점을 버리고 주인공을 우선적으로 쫓아감
            playerLocation.GetComponent<Transform>().position = target + new Vector3(0f, 0.2f, 0f);
            destLocation = playerLocation.GetComponent<Location>();
        }
        // 목적 지점에 도달했는데 (또는 목적 지점이 없는데)
        else if (tempLocation == destLocation || destLocation == null) {
            Location nextLocation;
            if (moveState == 0)                                                 // 주인공을 쫓는 중이 아니었다면
            {
                seeCount = 0f;
                nextLocation = NextDestLocation(passedLocation, destLocation);  // 다음 목적지 재설정
                passedLocation = destLocation;
            }
            else                                                                // 주인공을 쫓다가 놓쳤다면
            {
                seeCount = 0f;
                moveState = 0;
                //Debug.Log("moveState = " + moveState);
                nextLocation = NextDestLocation(passedLocation, null);          // 마지막으로 지난 지점을 기준으로 순찰 경로에 합류
            }
            destLocation = nextLocation;
        }
        else
        {
            seeCount = 0f;
        }

        // 주인공을 쫓는 중이고 새로운 고정 지점을 지나면
        if (moveState != 0 && tempLocation != destLocation && tempLocation != passedLocation)
        {
            // 그 지점을 마지막으로 지난 지점으로 설정
            passedLocation = tempLocation;
        }
        
        /* 한 자리에 멈춰서 4초(?) 이상 움직이지 않는 경우 주인공 무시하고 무조건 경로를 재탐색하도록 하기 */
        //stopCount += 3f * Time.fixedDeltaTime;
        //Debug.Log(stopCount);

        // 추적자가 주인공에게 접근했을 때 경고로서 안내 텍스트를 띄워준다. (잡히거나 탈출하면 띄워주지 않음.)
        if (isSameFloor(target, chaser.position) && (isNearEnough(target, chaser.position, 5f) && !isNearEnough(target, chaser.position, 4.8f)) &&
            !GetComponent<AutoMove>().isRunning && !GetComponent<AutoMove>().isExhausted && !Escape.escape.GetHasEscaped() && !Move.move.isCaptured)
            NoticeText.ntxt.NoticeChaserApproachByWalking();
        else if (isSameFloor(target, chaser.position) && (isNearEnough(target, chaser.position, 7.1f) && !isNearEnough(target, chaser.position, 6.9f)) &&
            GetComponent<AutoMove>().isRunning && !Escape.escape.GetHasEscaped() && !Move.move.isCaptured)
            NoticeText.ntxt.NoticeChaserApproachByRunning();

        // 주인공을 목격한 경우(주인공과 추적자 사이에 장애물이 없는 경우) 발소리에 Lowpass를 적용하지 않는다.
        if (moveState == 1 && !GetComponentInChildren<Lowpass>().isSeeing)
        {
            GetComponentInChildren<Lowpass>().isSeeing = true;
        }
        // 주인공과 추적자 사이에 장애물이 있으면 발소리에 Lowpass를 적용한다.
        else if (moveState != 1 && GetComponentInChildren<Lowpass>().isSeeing)
        {
            GetComponentInChildren<Lowpass>().isSeeing = false;
        }

        //if (rayHit.collider != null) Debug.Log(rayHit.collider.name + " " + moveState);
    }

    // 같은 층에서 A, B 사이의 거리가 distance보다 가까우면 true 반환
    bool isNearEnough(Vector3 A, Vector3 B, float distance)
    {
        if (Mathf.Pow(A.x - B.x, 2f) + Mathf.Pow(A.z - B.z, 2f) < Mathf.Pow(distance, 2f)) return true;
        else return false;
    }

    // A, B가 같은 층이면 true 반환
    bool isSameFloor(Vector3 A, Vector3 B)
    {
        if (Mathf.Abs(A.y - B.y) <= 1.5f) return true;
        else return false;
    }

    // patrolMode가 1 또는 2일 때 다음 목적 지점을 계산하여 반환함
    Location NextDestLocation(Location passed, Location arrived)
    {
        int index, nextIndex;
        if (patrolMode == 0) Debug.LogError("Chaser's patrolMode is 0");
        if (patrolMode == 1) // 복도만 돌아다님
        {
            if (arrived == null) // 도착 지점이 없는 경우
            {
                if (passed != null && passed.GetLocationType() == 2) // 마지막으로 지난 복도 지점이 있다면
                {
                    // 마지막으로 지난 지점과 인접한 랜덤 복도 지점을 반환
                    nextIndex = Random.Range(0, passed.GetAdjacentHallLocations().Count);
                    return passed.GetAdjacentHallLocations()[nextIndex];
                }
                else // 마지막으로 지난 지점도 없다면 (또는 마지막으로 지난 지점이 복도 지점이 아니라면)
                {
                    // 모든 복도 지점 중 랜덤 지점을 반환
                    nextIndex = Random.Range(0, BuildingManager.buildingManager.GetHallLocations().Count);
                    return BuildingManager.buildingManager.GetHallLocations()[nextIndex];
                }
            }
            else if (passed != null && (index = arrived.GetAdjacentHallLocations().IndexOf(passed)) != -1) // 인접한 복도 지점에서 온 경우
            {
                // 막다른 길이면 다시 돌아감
                if (arrived.GetAdjacentHallLocations().Count == 1)
                {
                    nextIndex = 0;
                }
                // 막다른 길이 아니면 마지막으로 지난 지점을 제외하고 도착 지점과 인접한 랜덤 복도 지점을 반환
                else
                {
                    nextIndex = Random.Range(1, arrived.GetAdjacentHallLocations().Count);
                    if (nextIndex == index) nextIndex = 0;
                }
                return arrived.GetAdjacentHallLocations()[nextIndex];
            }
            else // 도착 지점에 오기 전 마지막으로 지난 지점이 현재 도착한 지점과 인접하지 않은 경우 또는 마지막으로 지난 지점이 없는 경우
            {
                // 도착 지점과 인접한 랜덤 복도 지점을 반환
                nextIndex = Random.Range(0, arrived.GetAdjacentHallLocations().Count);
                return arrived.GetAdjacentHallLocations()[nextIndex];
            }
        }
        else if (patrolMode == 2) // 복도와 방 모두 돌아다님
        {
            if (arrived == null) // 도착 지점이 없는 경우
            {
                if (passed != null) // 마지막으로 지난 지점이 있다면
                {
                    // 마지막으로 지난 지점과 인접한 랜덤 지점을 반환
                    nextIndex = Random.Range(0, passed.GetAdjacentAllLocations().Count);
                    return passed.GetAdjacentAllLocations()[nextIndex];
                }
                else // 마지막으로 지난 지점도 없다면
                {
                    // 모든 지점 중 랜덤 지점을 반환
                    nextIndex = Random.Range(0, BuildingManager.buildingManager.GetAllLocations().Count);
                    return BuildingManager.buildingManager.GetAllLocations()[nextIndex];
                }
            }
            else if (passed != null && (index = arrived.GetAdjacentAllLocations().IndexOf(passed)) != -1) // 인접한 지점에서 온 경우
            {
                // 막다른 길이면 다시 돌아감
                if (arrived.GetAdjacentAllLocations().Count == 1)
                {
                    nextIndex = 0;
                }
                // 막다른 길이 아니면 마지막으로 지난 지점을 제외하고 도착 지점과 인접한 랜덤 지점을 반환
                else
                {
                    nextIndex = Random.Range(1, arrived.GetAdjacentAllLocations().Count);
                    if (nextIndex == index) nextIndex = 0;
                }
                return arrived.GetAdjacentAllLocations()[nextIndex];
            }
            else // 도착 지점에 오기 전 마지막으로 지난 지점이 현재 도착한 지점과 인접하지 않은 경우 또는 마지막으로 지난 지점이 없는 경우
            {
                // 도착 지점과 인접한 랜덤 지점을 반환
                nextIndex = Random.Range(0, arrived.GetAdjacentAllLocations().Count);
                return arrived.GetAdjacentAllLocations()[nextIndex];
            }
        }
        else
        {
            return NextDestLocation();
        }
    }

    // patrolMode가 3일 때 다음 목적 지점을 계산하여 반환함
    public Location NextDestLocation()
    {
        int nextIndex = Random.Range(0, BuildingManager.buildingManager.GetAllLocations().Count);
        return BuildingManager.buildingManager.GetAllLocations()[nextIndex];
    }

    public int GetMoveState()
    {
        if (moveState < 0 || moveState > 4)
        {
            Debug.LogError("moveState is invalid");
            return 0;
        }
        return moveState;
    }

    public Vector3 GetDestPosition()
    {
        if (destLocation == null)
        {
            //Debug.Log("destLocation is null");
            return Vector3.zero;
        }
        return destLocation.GetPosition();
    }

    public Location GetTempLocation()
    {
        return tempLocation;
    }

    public int GetTempZoneID()
    {
        return tempZoneID;
    }

    public void SetTempLocation(Location temp)
    {
        tempLocation = temp;
    }

    public void SetTempZoneID(int ID)
    {
        tempZoneID = ID;
    }
}
