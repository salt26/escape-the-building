using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioZone : MonoBehaviour {

    public int zoneID;
    public int zoneType;    // 1이면 작은 방, 2이면 큰 방, 3이면 복도, 4이면 계단
    public float OverTime = 1f;

    private AudioMixerSnapshot snapshot;

	// Use this for initialization
	void Start () {
        snapshot = ReverbSnapshots.rs.GetSnapshotByZoneType(zoneType);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Move>().SetTempZoneID(zoneID);
            snapshot.TransitionTo(OverTime);
        }
        else if (other.CompareTag("Chaser"))
        {
            other.GetComponent<Patrol>().SetTempZoneID(zoneID);
        }
    }
}
