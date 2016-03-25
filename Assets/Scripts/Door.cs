using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

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
        if (Input.GetKey(KeyCode.Space) && isSameFloor(player.position,thisDoor.position) && isNearEnough(player.position,thisDoor.position,5f))
        {
            Open(10f);
        }
        if (isSameFloor(capsule.position, thisDoor.position) && isNearEnough(capsule.position, thisDoor.position, 5f))
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
}
