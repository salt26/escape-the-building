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
}
