using UnityEngine;
using System.Collections;

public class Escape : MonoBehaviour {

    // 게임 오버 또는 탈출 여부를 기록하는 클래스는 Manager.cs로 이전

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !Manager.manager.GetHasEscaped())
        {
            Manager.manager.SetHasEscaped();
        }
    }
}
