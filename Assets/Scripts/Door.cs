﻿using UnityEngine;
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

	[SerializeField] bool typeIsEntrance; // 문이 출입문이면 true, 방 문이면 false
	[SerializeField] int doorID; // 문의 고유번호
	[SerializeField] bool isLocked; // 잠김 여부

	Transform player;
    Transform capsule;
    Transform thisDoor;
    NavMeshObstacle obstacle;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        capsule = GameObject.Find("Capsule").GetComponent<Transform>();
        thisDoor = GetComponent<Transform>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

	void FixedUpdate () {
		if (isLocked) {
			Close (1000f);
			if(!obstacle.enabled) obstacle.enabled = true; // 잠긴 문은 추적자가 장애물로 인식
		}
        if (Input.GetKey(KeyCode.Space) && isSameFloor(player.position,thisDoor.position) && isNearEnough(player.position,thisDoor.position,5f) && !isLocked)
        {
            Open(10f);
        }
        if (isSameFloor(capsule.position, thisDoor.position) && isNearEnough(capsule.position, thisDoor.position, 5f) && !isLocked)
        {
            Open(10f);
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
		return typeIsEntrance;
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
	}
}
