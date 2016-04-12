using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 열쇠 보관함의 클래스입니다. 주운 후의 열쇠의 List를 가지고 있습니다.
/// </summary>
public class KeyInventory : MonoBehaviour {

	public List<KeyInInventory> keyInventory = new List<KeyInInventory>();

    public KeyInInventory FindByID(int ID)
    {
        foreach (KeyInInventory key in keyInventory)
        {
            if (ID == key.GetKeyID())
            {
                return key;
            }
        }
        return null;
    }

    public KeyInInventory FindByTargetDoorID(int target, bool findThatIsNotUsed = true) {
        foreach (KeyInInventory key in keyInventory)
        {
            //if (findThatIsNotUsed && key.GetIsUsed()) continue;
            if (target == key.GetTargetDoorID())
            {
                return key;
            }
        }
        return null;
    }

    /// <summary>
    /// 획득한 열쇠들 중 사용한 적이 있는 열쇠의 수를 반환합니다.
    /// </summary>
    public int NumberOfUsedKey()
    {
        int num = 0;
        foreach (KeyInInventory key in keyInventory)
        {
            if (key.GetIsUsed()) num++;
        }
        return num;
    }

    /// <summary>
    /// 획득한 열쇠들 중, 인자가 true일 때 출입문 열쇠, 인자가 false일 때 방 문 열쇠이고 사용한 적이 있는 열쇠의 수를 반환합니다.
    /// </summary>
    public int NumberOfUsedKey(bool isEntranceType)
    {
        int num = 0;
        foreach (KeyInInventory key in keyInventory)
        {
            if (key.GetIsUsed() && (key.GetKeyType() == isEntranceType)) num++;
        }
        return num;
    }

    /// <summary>
    /// 획득한 열쇠들 중 아직 사용하지 않은 열쇠의 수를 반환합니다.
    /// </summary>
    public int NumberOfNotUsedKey()
    {
        int num = 0;
        foreach (KeyInInventory key in keyInventory)
        {
            if (!key.GetIsUsed()) num++;
        }
        return num;
    }

    /// <summary>
    /// 획득한 열쇠들 중, 인자가 true일 때 출입문 열쇠, 인자가 false일 때 방 문 열쇠이고 아직 사용하지 않은 열쇠의 수를 반환합니다.
    /// </summary>
    public int NumberOfNotUsedKey(bool isEntranceType)
    {
        int num = 0;
        foreach (KeyInInventory key in keyInventory)
        {
            if (!key.GetIsUsed() && (key.GetKeyType() == isEntranceType)) num++;
        }
        return num;
    }
}
