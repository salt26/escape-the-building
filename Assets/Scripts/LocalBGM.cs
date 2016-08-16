using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class LocalBGM : MonoBehaviour {

    public int zoneID;                          // 이 BGM의 AudioSource가 위치한 AudioZone의 ID
    public float maxDistance;                   // 이 BGM이 들리기 시작하는 최대 거리
    public AudioMixerSnapshot[] BGMSnapshots;   // 0번째 원소는 Normal(MainBGM만 재생), 1번째 원소는 Lowpass(LocalBGM을 벽 뒤에서 재생),
                                                // 2번째 원소는 Local(LocalBGM을 바로 앞에서 재생)
    Transform player;

	void Start () {
        player = GameObject.Find("Player").GetComponent<Transform>();
	}
	
	void FixedUpdate () {
        // 주인공이 같은 AudioZone에 있을 때
        if (Move.move.GetTempZoneID() == zoneID)
        {
            // LocalBGM을 크게 재생(MainBGM은 꺼짐)
            BGMSnapshots[2].TransitionTo(1.5f);
        }
        // 주인공과의 거리가 maxDistance보다 가까울 때
        else if (Vector3.Distance(player.position, GetComponent<Transform>().position) <= maxDistance)
        {
            // LocalBGM에 Lowpass를 적용하여 재생(MainBGM은 꺼짐)
            BGMSnapshots[1].TransitionTo(3f);
        }
        // 주인공과 멀리 떨어져 있을 때
        else
        {
            // MainBGM을 크게 재생(LocalBGM은 꺼짐)
            BGMSnapshots[0].TransitionTo(4f);
        }
	
	}
}
