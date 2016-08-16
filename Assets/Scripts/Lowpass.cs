using UnityEngine;
using System.Collections;

public class Lowpass : MonoBehaviour {

    public float transitionSpeed = 5000f;
    [HideInInspector] public bool isSeeing; // 추적자에게 주인공이 보이면 Lowpass를 적용하지 않는다.

    float destCutoff;                       // 목표 CutoffFrequency. 낮을수록 Lowpass가 많이 적용되어 소음처럼 들린다.
    //int debugCode;
    AudioLowPassFilter lowpass;
    Vector3 target;

	void Start () {
        lowpass = GetComponent<AudioLowPassFilter>();
        isSeeing = false;                   // Patrol.cs에서 값을 변경함
	}
	
	void FixedUpdate () {
        target = GetComponentInParent<AutoMove>().GetTargetPosition();
        Vector3 A = GetComponent<Transform>().position;
        Vector3 B = target;
        float d = Mathf.Min(Vector3.Distance(A, B), 25f);
        
        // 추적자와 주인공 사이에 벽이나 장애물이 존재하지 않는 경우
        if (isSeeing)
        {
            destCutoff = 20000f;
            //debugCode = 1;
        }
        // 같은 AudioZone에 있는 경우
        else if (Move.move.GetTempZoneID() == GetComponentInParent<Patrol>().GetTempZoneID())
        {
            
            // 20000 ~ 5000 사이에서 거리에 대한 이차함수로 감소함
            destCutoff = 24f * Mathf.Pow(d, 2f) - 1200f * d + 20000f;
            //debugCode = 2;
        }
        // 같은 건물 같은 층에 있거나, 추적자가 계단에 있고 주인공이 같은 건물에 있는 경우
        else if (Move.move.GetTempZoneID() / 100 == GetComponentInParent<Patrol>().GetTempZoneID() / 100
            || ((GetComponentInParent<Patrol>().GetTempZoneID() / 100) % 10 == 0 &&
            Move.move.GetTempZoneID() / 1000 == GetComponentInParent<Patrol>().GetTempZoneID() / 1000))
        {
            // 10000 ~ 2500 사이에서 거리에 대한 이차함수로 감소함
            destCutoff = 12f * Mathf.Pow(d, 2f) - 600f * d + 10000f;
            //debugCode = 3;
        }
        // 층 또는 건물이 다른 경우
        else
        {
            // 5000 ~ 500 사이에서 거리에 대한 이차함수로 감소함
            destCutoff = 5f * Mathf.Pow(d, 2f) - 305f * d + 5000f;
            //debugCode = 4;
        }
        destCutoff = Mathf.Clamp(destCutoff, 500f, 20000f);

        if (lowpass.cutoffFrequency >= destCutoff + transitionSpeed)
        {
            lowpass.cutoffFrequency -= transitionSpeed * Time.fixedDeltaTime;
        }
        else if (lowpass.cutoffFrequency >= destCutoff + transitionSpeed / 50f)
        {
            lowpass.cutoffFrequency -= transitionSpeed * Time.fixedDeltaTime / 10f;
        }
        else if (lowpass.cutoffFrequency <= destCutoff - transitionSpeed)
        {
            lowpass.cutoffFrequency += transitionSpeed * Time.fixedDeltaTime;
        }
        else if (lowpass.cutoffFrequency <= destCutoff - transitionSpeed / 50f)
        {
            lowpass.cutoffFrequency += transitionSpeed * Time.fixedDeltaTime / 10f;
        }

        //Debug.Log(destCutoff + " " + debugCode + " " + d);
	}
}
