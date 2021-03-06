﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
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
    }

    public void linkUser(GameObject go) {
        if (go != null) Player = go.transform;
        else Player = null;
    }

    public void zoom(Vector2 position, float size) {
        StartCoroutine(zoomSlow(position, size));
    }

    public void setPosition(Vector2 v) {
        this.transform.position = new Vector3(v.x, v.y, -10);
    }

    public void setSize(float s) {
        MyCamera.orthographicSize = s;
    }

    private void LateUpdate() {
        if (isEffect) return;
        if(Player != null) {
            this.transform.position = new Vector3(Player.position.x, Player.position.y, -10);
        }
    }

    public void shakeCamera() {
        Debug.Log("shake Start");
        StartCoroutine(shake());
    }

    private IEnumerator zoomSlow(Vector2 position, float size) {
        isEffect = true;
        float x_p = (position.x - this.transform.position.x) * 0.1f;
        float y_p = (position.y - this.transform.position.y) * 0.1f;
        float size_p = (size - MyCamera.orthographicSize) * 0.1f;
        for (int i = 0; i < 10; i++) {
            this.transform.position = new Vector3(transform.position.x + x_p, transform.position.y + y_p, -10);
            MyCamera.orthographicSize += size_p;
            yield return new WaitForSeconds(0.05f);
        }
        setSize(size);
        setPosition(position);
        isEffect = false;
        yield break;
    }

    private IEnumerator shake() {
        Vector2 v = transform.position;
        isEffect = true;
        for (int i = 0; i < 10; i++) {
            this.transform.position = new Vector3(
                v.x + (Random.value - 0.5f)/i, v.y + (Random.value - 0.5f)/i, -10);
            yield return new WaitForSeconds(0.05f);
        }
        isEffect = false;

        yield break;
    }
}
