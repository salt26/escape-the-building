#define NEW_VERSION
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif

public class Head : MonoBehaviour {

    public CurveControlledBob motionBob = new CurveControlledBob();
    //public float stepInterval;

    public AudioClip[] breathSounds;

    Camera sight;
    float turningAngle; // 정면으로부터 고개(시야)가 회전한 각도
    //bool beforeStepOn; // 발이 땅에 닿기 전까지(머리가 내려오는 중에) true
    float gradient; // 머리 위치의 y값 변화량
    AudioSource audioSource;
	
	void Start () {
        sight = GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        motionBob.Setup(sight, 1f);
        motionBob.VerticalToHorizontalRatio = 2f;
        turningAngle = 0f;
        gradient = 1f;
	}
	
	void FixedUpdate () {
        // 추적자에게 잡히면 고개(시야)를 움직일 수 없음.
        if (Move.move.isCaptured) return;

#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "3.Modeling" || SceneManager.GetActiveScene().name == "4.Asset")
        {
            // 몸은 가만히 있고 고개(시야)만 돌리는 기능. Shift + 왼쪽 화살표 키 또는 Shift + 오른쪽 화살표 키를 눌러 돌릴 수 있음.
            // -90도 ~ 90도 사이에서 움직임
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.LeftArrow) && turningAngle > -90f) turningAngle -= 5f;
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.RightArrow) && turningAngle < 90f) turningAngle += 5f;
            turningAngle = Mathf.Clamp(turningAngle, -90f, 90f);

            // 고개(시야)를 돌린 후 키를 놓았을 때 다시 정면을 바라보도록 함.
            if (!((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.LeftArrow))
                && !((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.RightArrow)) && turningAngle != 0f)
            {
                if (Mathf.Abs(turningAngle) < 2f) turningAngle = 0f;
                else if (turningAngle < 0f) turningAngle += 3f;
                else if (turningAngle > 0f) turningAngle -= 3f;
            }

            sight.transform.localRotation = Quaternion.Euler(new Vector3(0f, turningAngle, 0f));
        }
#endif

        // v.0.4.1 이후로 고개 회전은 MouseLook.cs에서 담당함.

        // 이동 중이면 머리가 상하로 살짝 흔들림
        // 한 걸음 걸을 때마다 한 번 머리가 아래로 내려감(VerticalToHorizontalRatio가 2f일 때)
        // 발을 디디는 순간(머리가 가장 아래로 내려갔다 올라오는 순간) 발걸음 소리를 재생함.
        if (Move.move.isMoving && Move.move.character.isGrounded)
        {
            float oldY = sight.transform.localPosition.y;
            float oldGradient = gradient;
            if (Move.move.isRunning)
            {
                motionBob.VerticalBobRange = 0.04f;
                sight.transform.localPosition = motionBob.DoHeadBob(Move.move.runningSpeed / Move.move.runningStepDistance);
            }
            else
            {
                motionBob.VerticalBobRange = 0.02f;
                sight.transform.localPosition = motionBob.DoHeadBob(Move.move.walkingSpeed / Move.move.walkingStepDistance);
            }
            gradient = sight.transform.localPosition.y - oldY;
            if (oldGradient <= 0f && gradient > 0f) Move.move.PlayFootStepAudio();
        }

        // 체력이 낮으면 숨소리를 낸다.
        if (Move.move.isExhausted && !audioSource.isPlaying)
        {
            PlayBreathAudio(0.85f);
        }
        else if (Move.move.GetStamina() / Move.move.maxStamina < 0.25f && Move.move.isRunning && !audioSource.isPlaying)
        {
            PlayBreathAudio(0.6f);
        }
        else if (((Move.move.GetStamina() / Move.move.maxStamina < 0.5f && Move.move.isRunning) ||
            (Move.move.GetStamina() / Move.move.maxStamina < 0.25f && !Move.move.isRunning)) && !audioSource.isPlaying)
        {
            PlayBreathAudio(0.25f);
        }
    }

    public void PlayBreathAudio(float volume)
    {
        audioSource.volume = volume;

        // 0번째 소리를 제외한 나머지 중에서 랜덤하게 하나를 선택하여 재생
        int n = Random.Range(1, breathSounds.Length);
        audioSource.clip = breathSounds[n];
        audioSource.PlayOneShot(audioSource.clip);

        // 방금 재생한 소리를 0번째로 옮겨서 이전과 다른 소리가 재생되게 함
        breathSounds[n] = breathSounds[0];
        breathSounds[0] = audioSource.clip;
    }
}
