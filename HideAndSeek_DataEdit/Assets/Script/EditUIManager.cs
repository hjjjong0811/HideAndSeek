using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditUIManager : MonoBehaviour {
    public abstract void Init();
    public abstract void editPanelSetting();
    public abstract void editPanelSetting(object arg);
    public abstract void CreateClick();
    public abstract void displayItemList();

    public abstract void itemClick(int clickedIndex);
    public abstract void editPanelOKClick();
    public abstract void editPanelDeleteClick();
    public abstract void editPanelCloseClick();
}
