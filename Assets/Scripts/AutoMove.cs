﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AutoMove : MonoBehaviour {

    public Transform target;
    public float walkingStepDistance;
    public float runningStepDistance;
    public float walkingSpeed;
    public float runningSpeed;
    public AudioClip[] footstepSounds;

    float moveDistance;
    NavMeshAgent agent;
    AudioSource audioSource;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponentInChildren<AudioSource>();
        moveDistance = 0f;
	}
	
	void FixedUpdate () {
        NavMeshHit navhit;
        RaycastHit rayHit;
        agent.SetDestination(target.position+new Vector3(0f, 0.2f, 0f));
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
            if(!Physics.Raycast(GetComponent<Transform>().position, target.position - GetComponent<Transform>().position, out rayHit)) return;
            else if(rayHit.collider.name != "Player"){ // 주인공을 목격하지 못하면 걷는다.
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
                return;
            }
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
}
