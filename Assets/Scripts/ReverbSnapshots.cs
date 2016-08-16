using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ReverbSnapshots : MonoBehaviour
{

    public static ReverbSnapshots rs;

    [SerializeField]
    private AudioMixerSnapshot[] Snapshots;

    void Awake()
    {
        rs = this;
    }

    // type이 1일 때 SmallRoom, 2일 때 LargeRoom, 3일 때 Hall, 4일 때 Stair
    public AudioMixerSnapshot GetSnapshotByZoneType(int type)
    {
        if (type - 1 < 0 || type - 1 >= Snapshots.Length) return null;
        return Snapshots[type - 1];
    }
}
