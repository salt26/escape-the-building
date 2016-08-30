using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 추적자(캡슐)의 움직임을 담당하는 클래스
/// 추적자의 목적지는 이 클래스에서 계산하지 않고,
/// 목적지를 향해 걷거나 뛰는 행동을 이 클래스에서 담당한다.
/// </summary>
public class AutoMove : MonoBehaviour {

    public float walkingStepDistance;
    public float runningStepDistance;
    public float walkingSpeed;                  // [능력치] 걸을 때의 속도(m/초).
    public float runningSpeed;                  // [능력치] 달릴 때의 속도(m/초).
    public float maxStamina;                    // [능력치] 최대 체력. 연속으로 달릴 수 있는 최대 시간(초).
    public float footstepVolume = 1f;           // [능력치] 주인공에게 들리는 추적자의 발소리 크기. 클수록 주인공에게 유리.
    public float doorDistance = 2f;             // [능력치] 문을 열기 위해 필요한 최대 거리. 이보다 문과 근접해야 문이 열림.
    public bool restrainExhaustion = false;     // [능력치] false이면 끝까지 달리다가 탈진하고, true이면 탈진하기 직전에 달리기를 멈추고 걸어감.
                                                // [능력치]가 두 개 더 있는데, 하나는 Nav Mesh Agent의 Acceleration(가속도)이고, 다른 하나는 Patrol의 hearing(청력)이다.
    public AudioClip[] footstepSounds;
    public GameObject tutorialBox;              // 튜토리얼용. 보통은 꼭 없어도 됨.
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isExhausted;

    float moveDistance;
    float forwardAmount;
    float stamina;
    bool isTutorial;
    bool tutorialBox8;
    Transform target;
    Vector3 targetPosition;
    NavMeshAgent agent;
    AudioSource audioSource;
    Animator animator;

	void Start () {
        target = GameObject.Find("Player").GetComponent<Transform>();
        targetPosition = target.position + new Vector3(0f, 0.25f, 0f);
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponentInChildren<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        moveDistance = 0f;
        stamina = maxStamina;
        isRunning = false;
        isExhausted = false;
        tutorialBox8 = false;
        if (SceneManager.GetActiveScene().name == "TrainingRoom4") isTutorial = true;
        else isTutorial = false;
	}

    void FixedUpdate()
    {
        targetPosition = target.position + new Vector3(0f, 0.25f, 0f);
        if (!Manager.manager.GetHasEscaped() && !Move.move.isCaptured)
        {
            agent.SetDestination(GetComponent<Patrol>().GetDestPosition()); // 목적지 설정
        }
        else
        {
            agent.speed = 0f;
            animator.SetFloat("Forward", 0f, 0.1f, Time.fixedDeltaTime);
            return;
        }

        if (stamina <= 0f) StartCoroutine("Exhaustion");

        // 순찰 상태이거나 주인공을 목격하지 못하고 주인공의 발소리만 들은 경우 걷는다. 탈진해도 뛰는 상태가 해제된다.
        if ((GetComponent<Patrol>().GetMoveState() % 2 == 0 && isRunning) || isExhausted ||
            (restrainExhaustion && stamina / maxStamina < 0.05f)) isRunning = false;
        // 주인공을 목격하거나 목격했던 상태에서 소리가 들리면 뛴다. (물론 체력이 어느 정도 있을 때)
        else if (GetComponent<Patrol>().GetMoveState() % 2 == 1 && !isRunning &&
            stamina / maxStamina > 0.1f) isRunning = true;

        // 탈진한 경우
        if (isExhausted)
        {
            agent.speed = 0f;
            if (stamina < maxStamina) stamina += Time.fixedDeltaTime * 0.25f;
            else stamina = maxStamina;
        }
        // 뛰는 경우
        else if (isRunning)
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
            stamina -= Time.fixedDeltaTime;
        }
        // 걷는 경우
        else
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
            if (stamina < maxStamina) stamina += Time.fixedDeltaTime * 0.55f;
            else stamina = maxStamina;
        }

        // 3D 모델에 걷거나 뛰는 애니메이션 적용
        Vector3 move = agent.velocity;
        move.y = 0f;
        forwardAmount = move.magnitude / 3.6f;
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.fixedDeltaTime);
    }

    IEnumerator Exhaustion()
    {
        isExhausted = true;
        stamina = 0f;
        NoticeText.ntxt.NoticeChaserExhausted();
        if (isTutorial && !tutorialBox8)
        {
            tutorialBox.SetActive(true);
            Manager.manager.OpenMsgBox();
            tutorialBox8 = true;
        }
        yield return new WaitForSeconds(4f);
        isExhausted = false;
    }

    void PlayFootStepAudio(bool isRunning)
    {
        if (footstepVolume > 1f) footstepVolume = 1f;
        else if (footstepVolume < 0f) footstepVolume = 0f;

        if (isRunning) audioSource.volume = footstepVolume;
        else audioSource.volume = footstepVolume * 0.7f;

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
