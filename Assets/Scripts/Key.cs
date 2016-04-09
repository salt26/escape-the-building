using UnityEngine;
using System.Collections;

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
	[SerializeField] GameObject targetDoor; // 이 열쇠로 열 수 있는 문 (인스펙터에서 설정, 이것에 따라 targetDoorID가 정해짐)

	int keyID; // 열쇠의 고유번호
    int targetDoorID; // 이 열쇠로 열 수 있는 문의 고유번호
    bool isInInventory; // 열쇠를 주운 후라서 보관함에 있으면 true, 줍기 전이라서 건물 어딘가에 있으면 false
	bool isUsed; // 열쇠 사용 여부
    KeyInventory inventory;

    // 열쇠 생성자(보관함에 넣을 때 사용)
    public Key(int ID, bool type, int target)
    {
        keyID = ID;
        typeIsEntrance = type;
        targetDoorID = target;
        isInInventory = true;
        isUsed = false;
    }

	void Start(){
        inventory = GameObject.Find("Player").GetComponent<KeyInventory>();
        isInInventory = false;
		isUsed = false;
        targetDoorID = targetDoor.GetComponent<Door>().GetDoorID();
	}

    // 주인공이 열쇠에 닿아 열쇠를 주웠을 때(줍기 버튼은 나중에 구현하기)
    void isTriggerEnter(Collider other)
    {
        if (isInInventory) return;
        if (other.name != "Player") return;
        PickUp();
    }
	// 처음에 열쇠를 지점에 배치할 때 그 지점의 고유번호와 일치하게 설정하기
	public void SetKeyID(int ID){
		keyID = ID;
	}

	public int GetKeyID(){
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
		// 열쇠 보관함 구현 후 작성할 것.
        if (isInInventory) return;
        inventory.keyInventory.Add(new Key(keyID, typeIsEntrance, targetDoorID));
        Destroy(this);
	}
}