using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour {
    [SerializeField]
    private Image ImgStick, ImgBackground;
    Vector2 posOrigin;
    float stickRadius = 0, runRadius = 0;

    public float Horizontal = 0, Vertical = 0;
    public bool Run = false;

    public void OnDrag() {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            ImgBackground.rectTransform, Input.mousePosition,
            this.GetComponent<Canvas>().worldCamera, out pos);

        Vector2 dir = pos.normalized;

        float touchAreaRadius = Vector3.Distance(posOrigin, pos);
        if (touchAreaRadius > stickRadius) {
            ImgStick.rectTransform.localPosition = posOrigin + (dir * stickRadius);
        } else { ImgStick.rectTransform.localPosition = pos; }

        if (touchAreaRadius > runRadius) {
            Run = true;
        } else {
            Run = false;
        }
        Horizontal = dir.x;
        Vertical = dir.y;
    }
    public void OnEndDrag() {
        ImgStick.rectTransform.localPosition = posOrigin;
        Horizontal = Vertical= 0;
        Run = false;
    }
    
    private void Start() {
        posOrigin = ImgStick.rectTransform.localPosition;

        stickRadius = ImgBackground.rectTransform.sizeDelta.x * 0.45f;
        runRadius = stickRadius * 0.8f;
    }
    private void Update() {
        if (GameManager.getInstance().isScenePlay || ScriptManager.getInstance().isPlaying) {
            transform.GetChild(0).gameObject.SetActive(false);
        } else {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
