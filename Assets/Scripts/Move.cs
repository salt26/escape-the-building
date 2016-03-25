using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {

    public static Move move;

    public float walkingStepDistance;
    public float runningStepDistance;
    public float walkingSpeed;
    public float runningSpeed;
    public float turningSpeed;
    public AudioClip[] footstepSounds;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isCaptured;
    [HideInInspector] public CharacterController character;

    float y;
    Vector3 movement;
    Rigidbody player;
    CollisionFlags collisionFlags;
    AudioSource audioSource;

    void Awake()
    {
        move = this;
        player = GetComponent<Rigidbody>();
        character = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        isCaptured = false;
        y = player.position.y;
        if (SceneManager.GetActiveScene().name == "1.Terrain and audio" || SceneManager.GetActiveScene().name == "2.Navigation") moveSpeed = 25f;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

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
        if(h != 0f && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) Turning(h);

        // 이동
        Moving(v);

    }

    void Turning(float h)
    {
        GetComponent<Transform>().Rotate(new Vector3(0f, turningSpeed * h, 0f));
    }

    void Moving(float v)
    {
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
            // X축, Z축 방향의 속도
            if (!isRunning)                 // 걸을 때
            {
                movement = GetComponent<Transform>().forward * v * moveSpeed;
            }
            else if (isRunning && v > 0f)   // 달릴 때
            {
                movement = GetComponent<Transform>().forward * v * runningSpeed;
            }
            if (v < 0f) movement /= 2f;     // 뒷걸음질 칠 때

            // Y축 아래 방향으로 중력 작용
            if(character.isGrounded) movement += Physics.gravity * Time.fixedDeltaTime;
            else movement += Physics.gravity * 100f * Time.fixedDeltaTime;

            // 이동, 충돌 감지
            collisionFlags = character.Move(movement*Time.fixedDeltaTime);
        }
        
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
        if (isRunning) audioSource.volume = 0.8f;
        else audioSource.volume = 0.56f;

        // 0번째 소리를 제외한 나머지 중에서 랜덤하게 하나를 선택하여 재생
        int n = Random.Range(1, footstepSounds.Length);
        audioSource.clip = footstepSounds[n];
        audioSource.PlayOneShot(audioSource.clip);

        // 방금 재생한 소리를 0번째로 옮겨서 이전과 다른 소리가 재생되게 함
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = audioSource.clip;
    }
}
