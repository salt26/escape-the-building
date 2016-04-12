using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour {

    private GameObject player;
    private Animator gameOverAnim;
    private bool isGameOver;

	void Start () {
        player = GameObject.Find("Player");
        gameOverAnim = GameObject.Find("GameOverPanel").GetComponent<Animator>();
        isGameOver = false;
	}
	
	void OnTriggerEnter (Collider other) {
        if (other.name != "Player") return;

        if (!isGameOver && !Escape.escape.GetHasEscaped())
        {
            player.GetComponent<Move>().isCaptured = true;
            GetComponent<NavMeshAgent>().autoBraking = true;
            StateText.stxt.PleaseRestart();
            NoticeText.ntxt.NoticeCaptured();
            gameOverAnim.SetTrigger("gameOver");
            isGameOver = true;
            Escape.escape.SetIsGameOver();
        }
	}
}
