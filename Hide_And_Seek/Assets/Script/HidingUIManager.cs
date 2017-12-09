using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingUIManager : MonoBehaviour {
    public void stop_breath_button_click()
    {
    }

    public void exitHiding() {
        GameObject.Find("Player").SendMessage("player_hide_exit");
    }
}
