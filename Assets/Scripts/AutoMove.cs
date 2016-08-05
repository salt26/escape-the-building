#define NEW_VERSION
using UnityEngine;
using System.Collections;
#if NEW_VERSION
using UnityEngine.SceneManagement;
#endif

/// <summary>
/// 추적자(캡슐)의 움직임을 담당하는 클래스
/// 추적자의 목적지는 이 클래스에서 계산하지 않고,
/// 목적지를 향해 걷거나 뛰는 행동을 이 클래스에서 담당한다.
/// </summary>
public class AutoMove : MonoBehaviour {

    public float walkingStepDistance;
    public float runningStepDistance;
    public float walkingSpeed;
    public float runningSpeed;
    public AudioClip[] footstepSounds;

    float moveDistance;
    Transform target;
    Vector3 targetPosition;
    NavMeshAgent agent;
    AudioSource audioSource;

	void Start () {
        target = GameObject.Find("Player").GetComponent<Transform>();
        targetPosition = target.position + new Vector3(0f, 0.25f, 0f);
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponentInChildren<AudioSource>();
        moveDistance = 0f;
	}
	
	void FixedUpdate () {
        targetPosition = target.position + new Vector3(0f, 0.25f, 0f);
        NavMeshHit navhit;
        if (!Escape.escape.GetHasEscaped())
        {
#if NEW_VERSION
            if (SceneManager.GetActiveScene().name == "3.Modeling") {
                agent.SetDestination(target.position + new Vector3(0f, 0.2f, 0f));
            } else {
#endif
            agent.SetDestination(GetComponent<Patrol>().GetDestPosition()); // 목적지 설정
#if NEW_VERSION
            }
#endif
        }
#if NEW_VERSION
        if (SceneManager.GetActiveScene().name == "2.Navigation")
        {
            if (!agent.Raycast(target.position, out navhit))
            {
                agent.speed = 14f;
            }
            else agent.speed = 7f;
        }
        else if (SceneManager.GetActiveScene().name != "1.Terrain and audio")
        {
#endif
            
            if (GetComponent<Patrol>().GetMoveState() % 2 == 0)
            {
                agent.speed = walkingSpeed;
                if (agent.velocity.magnitude > 0f)
                {
                    moveDistance += walkingSpeed * Time.fixedDeltaTime;
                    if (moveDistance > walkingStepDistance)
                    {
                        moveDistance -= walkingStepDistance;
                        PlayFootStepAudio(false);
                    }
                }
            }
            // 주인공을 목격하거나 목격했던 상태에서 소리가 들리면 뛴다. /* 나중에 탈진 고려해서 체력 낮으면 뛰지 않고 걷는 기능 만들기 */
            else
            {
                agent.speed = runningSpeed;
                if (agent.velocity.magnitude > 0f)
                {
                    moveDistance += runningSpeed * Time.fixedDeltaTime;
                    if (moveDistance > runningStepDistance)
                    {
                        moveDistance -= runningStepDistance;
                        PlayFootStepAudio(true);
                    }
                }
            }

#if NEW_VERSION
        }
#endif
	}

    void PlayFootStepAudio(bool isRunning)
    {
        if (isRunning) audioSource.volume = 1f;
        else audioSource.volume = 0.7f;

        // 0번째 소리를 제외한 나머지 중에서 랜덤하게 하나를 선택하여 재생
        int n = Random.Range(1, footstepSounds.Length);
        audioSource.clip = footstepSounds[n];
        audioSource.PlayOneShot(audioSource.clip);

        // 방금 재생한 소리를 0번째로 옮겨서 이전과 다른 소리가 재생되게 함
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = audioSource.clip;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
}
