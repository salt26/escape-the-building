using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyInventory : MonoBehaviour {

	[HideInInspector] public List<Key> keyInventory = new List<Key>();

    public Key FindByID(int ID)
    {
        for (int i = 0; i < keyInventory.Count; i++)
        {
            if (ID == keyInventory[i].GetKeyID())
            {
                return keyInventory[i];
            }
        }
        return null;
    }
    public Key FindByTargetDoorID(int target) {
        for (int i = 0; i < keyInventory.Count; i++)
        {
            if (target == keyInventory[i].GetTargetDoorID())
            {
                return keyInventory[i];
            }
        }
        return null;
    }
}
