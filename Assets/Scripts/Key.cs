using UnityEngine;
using System.Collections;

/// <summary>
/// 줍기 전 열쇠의 클래스입니다.
/// </summary>
public class Key : MonoBehaviour {

    /* 
     * 열쇠의 고유번호는 4자리의 정수로,
     * 첫 번째 자리는 건물 동수(1~5),
     * 두 번째 자리는 층수(1~),
     * 세 번째와 네 번째 자리는 호수(01~)입니다.
     * 
     * 호수는 위치의 x값이 가장 큰
     * (x값이 가장 큰 곳이 여러 곳 있으면 그 중 건물 중심보다 z값이 큰 곳 중 z값이 가장 작은)
     * 곳부터 반시계 방향으로 1씩 증가하면서 정해집니다.
     * 
     * 열쇠의 고유번호는 열쇠가 건물에 놓일 때 놓인 지점의 고유번호와 같게 설정됩니다.
     */

	[SerializeField] bool typeIsEntrance; // 열쇠가 출입문 열쇠이면 true, 방 문 열쇠이면 false
    [SerializeField] int targetDoorID; // 이 열쇠로 열 수 있는 문의 고유번호

    int keyID; // 열쇠의 고유번호
    /* 
     * 지점이 구현된 후 [SerializeField] 빼고 SetKeyID() 사용하기
     */

    

    Transform player;
    Transform thisKey;
    KeyInventory inventory;


	void Start(){
        player = GameObject.Find("Player").GetComponent<Transform>();
        inventory = GameObject.Find("Player").GetComponent<KeyInventory>();
        thisKey = GetComponent<Transform>();
	}

    void FixedUpdate()
    {
        GetComponent<Transform>().Rotate(new Vector3(0f, 1f, 0f));
        if (isNearEnough(player.position, thisKey.position, 1f) && isSameFloor(player.position, thisKey.position))
        {
            Debug.Log("Pick key up"); // 열쇠를 주웠을 때 주는 피드백 구현하기
            PickUp();
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

	// 처음에 열쇠를 지점에 배치할 때 그 지점의 고유번호와 일치하게 설정하기
	public void SetKeyID(int ID){
		keyID = ID;
	}

    public int GetKeyID()
    {
        return keyID;
    }

    public int GetTargetDoorID()
    {
        return targetDoorID;
    }

	/// <summary>
	/// 열쇠의 종류가 출입문 열쇠이면 true 반환, 방 문 열쇠이면 false 반환
	/// </summary>
	public bool GetKeyType(){
		return typeIsEntrance;
	}

	// 열쇠를 주우면 열쇠 보관함에 이 열쇠의 데이터를 저장하고 이 객체는 사라짐
	public void PickUp(){
        inventory.keyInventory.Add(new KeyInInventory(keyID, typeIsEntrance, targetDoorID));
        Destroy(gameObject);
	}
}

/// <summary>
/// 줍고 나서 보관함에 들어있는 열쇠의 클래스입니다.
/// </summary>
public class KeyInInventory
{
    bool typeIsEntrance; // 열쇠가 출입문 열쇠이면 true, 방 문 열쇠이면 false
    int keyID; // 열쇠의 고유번호
    int targetDoorID; // 이 열쇠로 열 수 있는 문의 고유번호
    bool isUsed; // 열쇠 사용 여부

    // 열쇠 생성자(보관함에 넣을 때 사용)
    public KeyInInventory(int ID, bool type, int target)
    {
        keyID = ID;
        typeIsEntrance = type;
        targetDoorID = target;
        isUsed = false;
    }

    public int GetKeyID()
    {
        return keyID;
    }

    public int GetTargetDoorID()
    {
        return targetDoorID;
    }

    /// <summary>
    /// 열쇠의 종류가 출입문 열쇠이면 true 반환, 방 문 열쇠이면 false 반환
    /// </summary>
    public bool GetKeyType()
    {
        return typeIsEntrance;
    }

    public bool GetIsUsed()
    {
        return isUsed;
    }

    public void UseKey()
    {
        isUsed = true;
    }
}