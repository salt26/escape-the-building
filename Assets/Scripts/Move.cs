﻿#define NEW_VERSION
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif

public class Move : MonoBehaviour {

    public static Move move;

    public float walkingStepDistance;
    public float runningStepDistance;
    public float walkingSpeed;                  // [능력치] 걸을 때의 속도(m/초)
    public float runningSpeed;                  // [능력치] 뛸 때의 속도(m/초)
    public float turningSpeed;
    public float maxStamina = 30f;              // [능력치] 최대 체력. 달리기를 지속할 수 있는 시간(초)
    public AudioClip[] footstepSounds;
    public Image sliderFill;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isCaptured;
    [HideInInspector] public bool isExhausted;
    [HideInInspector] public CharacterController character;

    float y;
    float stamina;                              // 현재 체력
    Vector3 movement;
    Rigidbody player;
    Camera head;
    Slider staminaSlider;
    CollisionFlags collisionFlags;
    AudioSource audioSource;
    Animator gameOverAnim;
    MouseLook mouseLook = new MouseLook();

    void Awake()
    {
        move = this;
        player = GetComponent<Rigidbody>();
        head = GameObject.Find("Head").GetComponent<Camera>();
        staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
        character = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        gameOverAnim = GameObject.Find("GameOverPanel").GetComponent<Animator>();
        isCaptured = false;
        isExhausted = false;
        y = player.position.y;
        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
        mouseLook.Init(GetComponent<Transform>(), head.transform);
#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "1.Terrain and audio" || SceneManager.GetActiveScene().name == "2.Navigation") moveSpeed = 25f;
#endif
    }

    void Update()
    {
#if NEW_VERSION
        if ((SceneManager.GetActiveScene().name == "1.Terrain and audio" || SceneManager.GetActiveScene().name == "2.Navigation" ||
            SceneManager.GetActiveScene().name == "3.Modeling" || SceneManager.GetActiveScene().name == "4.Asset")) return;
#endif
        // 몸통 및 머리 회전
        mouseLook.LookRotation(GetComponent<Transform>(), head.transform);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "1.Terrain and audio" || SceneManager.GetActiveScene().name == "2.Navigation" ||
            SceneManager.GetActiveScene().name == "3.Modeling" || SceneManager.GetActiveScene().name == "4.Asset")
        {
            // 추적자에게 잡히면 이동하거나 회전할 수 없음.
            if (isCaptured)
            {
                h = 0f;
                v = 0f;
            }

            // 움직이는지 확인
            if (v != 0f && !isMoving) isMoving = true;
            else if (v == 0f && isMoving) isMoving = false;

            // 달리는지 확인
            if ((isMoving && v > 0f && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) && !isRunning) isRunning = true;
            else if (!(isMoving && v > 0f && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) && isRunning) isRunning = false;

            // 이동속도 변환
            if (isRunning) moveSpeed = runningSpeed;
            else moveSpeed = walkingSpeed;

            // 몸통 회전
            if (h != 0f && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) Turning(h);

            // 이동
            Moving(v);
        }
        else
        {
#endif
            if (stamina <= 0f) StartCoroutine("Exhaustion");

            // 추적자에게 잡히거나 탈진하면 이동할 수 없음.
            if (isCaptured || isExhausted)
            {
                h = 0f;
                v = 0f;
            }

            // 움직이는지 확인
            if ((v != 0f || h != 0f) && !isMoving) isMoving = true;
            else if (((v == 0f && h == 0f) && isMoving) || isExhausted) isMoving = false;

            // 달리는지 확인
            if ((isMoving && v > 0f && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) &&
                !isRunning && stamina / maxStamina > 0.1f) isRunning = true;
            else if ((!(isMoving && v > 0f && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) &&
                isRunning) || isExhausted) isRunning = false;

            // 이동속도 변환 및 체력 관리
            if (isRunning)          // 달릴 때
            {
                moveSpeed = runningSpeed;
                stamina -= Time.fixedDeltaTime;
            }
            else if (isMoving)      // 걸을 때
            {
                moveSpeed = walkingSpeed;
                if (stamina < maxStamina) stamina += Time.fixedDeltaTime * 0.4f;
                else stamina = maxStamina;
            }
            else if (isExhausted)   // 탈진했을 때
            {
                moveSpeed = 0f;
                if (stamina < maxStamina) stamina += Time.fixedDeltaTime * 0.25f;
                else stamina = maxStamina;
            }
            else if (!isCaptured)   // 정상적으로 멈춰 있을 때
            {
                moveSpeed = 0f;
                if (stamina < maxStamina) stamina += Time.fixedDeltaTime * 0.8f;
                else stamina = maxStamina;
            }
            staminaSlider.value = stamina;
            if (stamina / maxStamina >= 0.4f)
            {
                sliderFill.color = new Color((stamina / maxStamina) * (-13f / 15f) + (101f / 75f), 1f, 0.48f, 0.75f);
            }
            else if(stamina / maxStamina >= 0.1f)
            {
                sliderFill.color = new Color(1f, (stamina / maxStamina) * (26f / 15f) + (23f / 75f), 0.48f, 0.75f);
            }
            else
            {
                sliderFill.color = new Color(1f, (stamina / maxStamina) * 2f + 0.28f, (stamina / maxStamina) * 2f + 0.28f, 0.75f);
            }

            // 이동
            Moving(v, h);
#if NEW_VERSION
        }
#endif
    }

    void Turning(float h)
    {
        GetComponent<Transform>().Rotate(new Vector3(0f, turningSpeed * h, 0f));
    }

    void Moving(float v, float h = 0f)
    {
#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "1.Terrain and audio" || SceneManager.GetActiveScene().name == "2.Navigation")
        {
            // X축, Z축 방향의 속도
            movement = GetComponent<Transform>().forward * v * moveSpeed;
            player.velocity = movement;

            // Y축 아래 방향으로 중력 적용
            // 반지름이 0.5인 캡슐 콜라이더 기준으로 45도 이상의 경사는 올라갈 수 없음
            RaycastHit hit;
            Ray landingRay = new Ray(transform.position, Vector3.down);
            if (!Physics.Raycast(landingRay, out hit, y + (Mathf.Sqrt(2) - 1f) * GetComponent<CapsuleCollider>().radius))
            {
                player.AddForce(new Vector3(0f, -1000f, 0f), ForceMode.Acceleration);
            }

            // 맵 내에서 이동
            player.MovePosition(new Vector3(Mathf.Clamp(player.position.x, 1f, 499f), Mathf.Max(player.position.y, y), Mathf.Clamp(player.position.z, 1f, 499f)));
        }
        else
        {
#endif
            // X축, Z축 방향의 속도
            if (!isRunning || (isRunning && v > 0f))    // 걷거나 달릴 때
            {
                movement = GetComponent<Transform>().forward * v + GetComponent<Transform>().right * h;
                if (movement.magnitude > 1f) movement = movement.normalized;
                movement *= moveSpeed;                  // 속도는 앞에서 변환했음
            }
            if (v < 0f) movement /= 2f;                 // 뒷걸음질 칠 때

            // Y축 아래 방향으로 중력 작용
            if (character.isGrounded) movement += Physics.gravity * Time.fixedDeltaTime;
            else movement += Physics.gravity * 100f * Time.fixedDeltaTime;

            // 이동, 충돌 감지
            collisionFlags = character.Move(movement*Time.fixedDeltaTime);
#if NEW_VERSION
        }
#endif        
    }

    IEnumerator Exhaustion()
    {
        isExhausted = true;
        stamina = 0f;
        gameOverAnim.SetTrigger("exhaustion");
        NoticeText.ntxt.NoticePlayerExhausted();
        yield return new WaitForSeconds(4f);
        gameOverAnim.SetTrigger("recoverFromExhaustion");
        isExhausted = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(character.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    public void PlayFootStepAudio()
    {
        if (!character.isGrounded)
        {
            return;
        }
        if (isRunning) audioSource.volume = 0.5f;
        else audioSource.volume = 0.35f;

        // 0번째 소리를 제외한 나머지 중에서 랜덤하게 하나를 선택하여 재생
        int n = Random.Range(1, footstepSounds.Length);
        audioSource.clip = footstepSounds[n];
        audioSource.PlayOneShot(audioSource.clip);

        // 방금 재생한 소리를 0번째로 옮겨서 이전과 다른 소리가 재생되게 함
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = audioSource.clip;
    }

    public MouseLook GetMouseLook()
    {
        return mouseLook;
    }

    public float GetStamina()
    {
        return stamina;
    }
}
