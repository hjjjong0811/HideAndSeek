using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    private enum effect {shake = 0};

    private Camera MyCamera;
    private Transform Player;
    private bool isEffect;

	// Use this for initialization
	private void Start () {
        MyCamera = GetComponent<Camera>();
        GameObject go = GameObject.Find("Player");
        if (go != null) {
            Player = go.transform;
            this.transform.position = new Vector3(Player.position.x, Player.position.y, -10);
        }
        isEffect = false;
        MyCamera.backgroundColor = Color.black;
        MyCamera.orthographicSize = 4;

        shakeCamera();
    }

    public void setPosition(Vector2 v) {
        this.transform.position = new Vector3(v.x, v.y, -10);
    }

    public void setSize(int s) {
        MyCamera.orthographicSize = s;
    }

    private void LateUpdate() {
        if (isEffect) return;
        if(Player != null) {
            this.transform.position = new Vector3(Player.position.x, Player.position.y, -10);
        }
    }

    public void shakeCamera() {
        StartCoroutine(myCoroutine(effect.shake));
    }

    private IEnumerator myCoroutine(effect what) {
        isEffect = true;
        if(what == effect.shake) {
            for (int i = 0; i < 10; i++) {
                this.transform.position = new Vector3(
                    Player.position.x + (Random.value - 0.5f)/i, Player.position.y + (Random.value - 0.5f)/i, -10);
                yield return new WaitForSeconds(0.05f);
            }
        }
        isEffect = false;

        yield break;
    }
}
