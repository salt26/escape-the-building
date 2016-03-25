using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour {

    private GameObject player;
    private Animator gameOverAnim;

	void Start () {
        player = GameObject.Find("Player");
        gameOverAnim = GameObject.Find("GameOverPanel").GetComponent<Animator>();
	}
	
	void OnTriggerEnter (Collider other) {
        if (other.name != "Player") return;
            
        player.GetComponent<Move>().isCaptured = true;
        GetComponent<NavMeshAgent>().autoBraking = true;
        StateText.stxt.PleaseRestart();
        gameOverAnim.SetTrigger("gameOver");
	}
}
