using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour {

    public GameObject msgBox;
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

        if (!isGameOver && !Manager.manager.GetHasEscaped())
        {
            player.GetComponent<Move>().isCaptured = true;
            player.GetComponent<Move>().GetMouseLook().SetCursorLock(false);
            GetComponent<NavMeshAgent>().autoBraking = true;
            NoticeText.ntxt.NoticeCaptured();
            gameOverAnim.SetTrigger("gameOver");
            isGameOver = true;
            Manager.manager.SetIsGameOver();
            if (msgBox != null)
            {
                StartCoroutine("OpenDeadMsgBox");
            }
        }
	}

    IEnumerator OpenDeadMsgBox()
    {
        yield return new WaitForSeconds(2f);
        msgBox.SetActive(true);
        Manager.manager.OpenMsgBox();
    }
}
