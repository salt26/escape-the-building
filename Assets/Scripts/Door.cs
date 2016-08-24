using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    /* 
     * 문의 고유번호는 4자리의 정수로,
     * 첫 번째 자리는 건물 동수(1~5),
     * 두 번째 자리는 층수(1~),
     * 세 번째와 네 번째 자리는 호수(01~)입니다.
     * 
     * 호수는 위치의 x값이 가장 큰
     * (x값이 가장 큰 곳이 여러 곳 있으면 그 중 건물 중심보다 z값이 큰 곳 중 z값이 가장 작은)
     * 곳부터 반시계 방향으로 1씩 증가하면서 정해집니다.
     * 
     * 문이 있는 방 안의 지점의 고유번호와 문의 고유번호는 일치합니다.
     */

	[SerializeField] bool isEntranceType; // 문이 출입문이면 true, 방 문이면 false
	[SerializeField] int doorID; // 문의 고유번호
	[SerializeField] bool isLocked; // 잠김 여부

    public AudioClip doorUnlock;
    public AudioClip doorCreaking;

	Transform player;
    Transform thisDoor;
    NavMeshObstacle obstacle;
    KeyInventory inventory;
    AudioSource audioSource;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        inventory = GameObject.Find("Player").GetComponent<KeyInventory>();
        thisDoor = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

	void FixedUpdate () {
		if (isLocked) {
			Close (1000f);
			if(!obstacle.enabled) obstacle.enabled = true; // 잠긴 문은 추적자가 장애물로 인식

            // 잠긴 문을 열려고 시도
            // (v.0.4.1부터 Space키 대신 마우스 좌클릭으로 문을 열게 됨)
            if (Input.GetMouseButton(0) && isSameFloor(player.position, thisDoor.position) && isNearEnough(player.position, thisDoor.position, 2f) &&
                !player.gameObject.GetComponent<Move>().isExhausted && !player.gameObject.GetComponent<Move>().isCaptured)
            {
                bool haveNoKey = false;

                if (inventory.NumberOfNotUsedKey(isEntranceType) == 0)
                {
                    if (!isEntranceType) NoticeText.ntxt.NoticeLockedDoor();
                    else NoticeText.ntxt.NoticeLockedEntranceDoor();
                    haveNoKey = true;
                }

                // 주인공이 문을 여는 동안 대기하는 시간 구현하기

                KeyInInventory key = inventory.FindByTargetDoorID(doorID);
                if (key == null)
                {
                    if (!haveNoKey)
                    {
                        //Debug.Log("Fail to open"); // 여는 데에 실패하면 주는 피드백 구현하기
                        if (!isEntranceType) NoticeText.ntxt.NoticeFailedToUnlock();
                        else NoticeText.ntxt.NoticeFailedToUnlockEntrance();
                    }
                    return;
                }
                Unlock();
                key.UseKey();

            }
		}
        // v.0.4.1부터 Space키 대신 마우스 좌클릭으로 문을 열게 되었고, 문을 열 수 있는 거리가 5m에서 2m로 감소함. 탈진한 상태에서는 열 수 없음
        if (Input.GetMouseButton(0) && isSameFloor(player.position, thisDoor.position) && isNearEnough(player.position, thisDoor.position, 2f) &&
            !isLocked && !player.GetComponent<Move>().isExhausted && !player.GetComponent<Move>().isCaptured)
        {
            Open(10f);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = doorCreaking;
                audioSource.Play();
            }
        }
        foreach (GameObject chaser in Manager.manager.chasers)
        {
            if (isSameFloor(chaser.GetComponent<Transform>().position, thisDoor.position) &&
                isNearEnough(chaser.GetComponent<Transform>().position, thisDoor.position, chaser.GetComponent<AutoMove>().doorDistance) &&
            !isLocked && !chaser.GetComponent<AutoMove>().isExhausted)
            {
                Open(10f);
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = doorCreaking;
                    audioSource.Play();
                }
            }
        }
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

    void Open(float forceAmount)
    {
        // 열릴 때 사운드 재생
        GetComponent<Rigidbody>().AddForce(GetComponent<Transform>().forward * forceAmount, ForceMode.Acceleration);
    }

	void Close(float forceAmount){
		GetComponent<Rigidbody> ().AddForce (-GetComponent<Transform> ().forward * forceAmount, ForceMode.Acceleration);
	}

	/// <summary>
	/// 문이 잠겼으면 true 반환
	/// </summary>
	public bool GetLock(){
		return isLocked;
	}

	/// <summary>
	/// 문의 종류가 출입문이면 true 반환, 방 문이면 false 반환
	/// </summary>
	public bool GetDoorType(){
		return isEntranceType;
	}

	/// <summary>
	/// 문의 고유번호 반환
	/// </summary>
	public int GetDoorID(){
		return doorID;
	}

    // 문의 잠금 해제
	public void Unlock(){
		isLocked = false;
		obstacle.enabled = false;
        //Debug.Log("Success to open"); // 여는 데에 성공하면 주는 피드백 구현하기
        NoticeText.ntxt.NoticeSuccessedToUnlock();
        audioSource.clip = doorUnlock;
        audioSource.Play();
	}
}
