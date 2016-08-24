using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioOutside : MonoBehaviour {

    public float OverTime = 0.5f;

    private AudioMixerSnapshot snapshot;

    // Use this for initialization
    void Start()
    {
        snapshot = ReverbSnapshots.rs.GetSnapshotByZoneType(5);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        snapshot.TransitionTo(OverTime);
    }
}
