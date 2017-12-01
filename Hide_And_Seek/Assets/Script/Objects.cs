using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objects : MonoBehaviour, IObject {
    public int _key_num;
    public bool _t_thing_f_portal = false;//오브젝트 구분용 (true : 씬이동용 포탈 / false : 스크립트용 물건)
    IObject _obj;

    [System.Serializable]
    public struct Detail {
        public int chapter;         //chapter부터
        public bool isActive;
        public Sprite sprite;       //해당 스프라이트로 변경
        public AudioClip sound_default;
        public Type type;

        public bool isActByCollision;
        public output outputByCollision;
        public bool isActByCall;
        public output outputByCall;
    }

    [System.Serializable]
    public struct Detail_useItem {
        public int chapter_canUse;     //chapter부터 사용가능
        public int materialItem_key;
        public output outputByUsingItem;
    }

    [System.Serializable]
    public struct output {
        public int script_key;
        public AudioClip sound;
        public int item_key;
        public bool isBlockPortal;
        public int script_key_isExistitem;
    }

    public enum Type {
        no = 0, script = 1, sound = 2, hide = 3, getItem = 4
    }
    
    public Detail[] InfoByChapter;      //챕터에따른 오브젝트정보
    public Detail_useItem[] usingItem;  //사용가능아이템정보

    private const int mode_detail = 0, mode_detail_useitem = 1;
    private const int invalidValue = 0;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start() {
        int index = findIndexByChapter(mode_detail);
        
        if (!InfoByChapter[index].isActive) {
            this.gameObject.SetActive(false);
            return;
        } else {
            this.gameObject.SetActive(true);
        }

        if (!_t_thing_f_portal) {
            _obj = Scene_Manager.getInstance()._get_portal(_key_num);
        } else {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            
            if(InfoByChapter[index].sprite != null) spriteRenderer.sprite = InfoByChapter[index].sprite;
            if(InfoByChapter[index].sound_default != null) {
                //Sound재생
            }
        }

        
    } //Start()

    public void action() {
        if (!_t_thing_f_portal) {
            int index = findIndexByChapter(mode_detail);
            Detail curInfo = InfoByChapter[index];
            if (!curInfo.isActByCall) return;

            if (curInfo.outputByCall.script_key != invalidValue)
                ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key });

            if (!curInfo.outputByCall.isBlockPortal) _obj.action();
            
            return;
        } else {
            int index = findIndexByChapter(mode_detail);
            Detail curInfo = InfoByChapter[index];
            if (!curInfo.isActByCall) return;
            
            if (curInfo.type == Type.hide) {
                //hide
            } else if (curInfo.type == Type.no) {
                return;
            } else if (curInfo.type == Type.getItem) {
                //Sound 있으면재생
                if (curInfo.outputByCall.sound != null)
                    //Sound
                    Debug.Log("SoundCall");
                //Item 획득가능하면 획득
                if (curInfo.outputByCall.item_key != invalidValue) {
                    if (!Inventory.getInstance().addItem(curInfo.outputByCall.item_key)) {
                        //이미존재
                        if (curInfo.outputByCall.script_key_isExistitem != invalidValue)
                            ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key_isExistitem });
                    } else {
                        //Script 획득
                        if (curInfo.outputByCall.script_key != invalidValue)
                            ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key });
                    }
                }
            } else {
                //Sound 있으면재생
                if (curInfo.outputByCall.sound != null)
                    //Sound
                    Debug.Log("SoundCall");

                //Script 있으면 재생
                if (curInfo.outputByCall.script_key != invalidValue)
                    ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key });
            }
            GameManager.getInstance().CheckMainChapter();
            afterAction();
        }
    } //action()

    public void useitem(int item_key) {
        Debug.Log("actionitemtest");
        for (int i = 0; i < usingItem.Length; i++) {
            if(usingItem[i].materialItem_key == item_key) {
                if(usingItem[i].chapter_canUse > GameManager.getInstance().GetMainChapter()) {
                    continue;
                } else {
                    output curOutput = usingItem[i].outputByUsingItem;
                    //Sound 있으면재생
                    if (curOutput.sound != null)
                        //Sound
                        Debug.Log("SoundCall");

                    //Item 획득가능하면 획득
                    if (curOutput.item_key != invalidValue) {
                        if (!Inventory.getInstance().addItem(curOutput.item_key)) {
                            //이미존재
                            if (curOutput.script_key_isExistitem != invalidValue)
                                ScriptManager.getInstance().showScript(true, new int[] { curOutput.script_key_isExistitem });
                        } else {
                            //Script 획득
                            if (curOutput.script_key != invalidValue)
                                ScriptManager.getInstance().showScript(true, new int[] { curOutput.script_key });
                        }
                    }
                    //Script 있으면 재생
                    else if (curOutput.script_key != invalidValue)
                        ScriptManager.getInstance().showScript(true, new int[] { curOutput.script_key });
                    
                    if (!_t_thing_f_portal && !curOutput.isBlockPortal) {
                        _obj.action();

                    } 

                    GameManager.getInstance().CheckMainChapter();
                    afterUsingItem(item_key);
                    break;
                }
            } //if
        }
    } //action
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!_t_thing_f_portal) {
            return;
        } else {
            int index = findIndexByChapter(mode_detail);
            Detail curInfo = InfoByChapter[index];
            if (curInfo.isActByCollision)  {
                //Sound 있으면재생
                if (curInfo.outputByCollision.sound != null)
                    //Sound
                    Debug.Log("SoundCall");
                //Script 있으면 재생
                if (curInfo.outputByCollision.script_key != invalidValue)
                    ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCollision.script_key });
                //Item 획득가능하면 획득
                if (curInfo.outputByCollision.item_key != invalidValue)
                    Inventory.getInstance().addItem(curInfo.outputByCollision.item_key);
            }

            GameManager.getInstance().CheckMainChapter();
        }
    }

    private int findIndexByChapter(int mode) {
        int chapter = GameManager.getInstance().GetMainChapter();
        int index = 0;
        
        if (_key_num == 13002 && GameManager.getInstance().BreakDisplay == 1) {
            return 2;
        }else if (_key_num == 13001 && GameManager.getInstance().BreakDisplay == 1) {
            return 1;
        }else if (_key_num == 110) {
            if (Inventory.getInstance().isExitItem(3) || Inventory.getInstance().isExitItem(5))
                return 2;
        }else if (_key_num == 83 && GameManager.getInstance().Wallpaper == 1) {
            return 1;
        }else if(_key_num == 1806 && (Inventory.getInstance().isExitItem(20) ||
            Inventory.getInstance().isExitItem(21) || Inventory.getInstance().isExitItem(22))) {
            return 1;
        }

        if (mode == mode_detail) {
            for (int i = 0; i < InfoByChapter.Length; i++) {
                index = i;
                if (i + 1 < InfoByChapter.Length) {
                    if(InfoByChapter[i+1].chapter > chapter) {
                        break;
                    }
                }
            } //for
        } else if(mode == mode_detail_useitem) {
            for (int i = 0; i < usingItem.Length; i++) {
                index = i;
                if (i + 1 < usingItem.Length) {
                    if (usingItem[i + 1].chapter_canUse > chapter) {
                        break;
                    }
                }
            } //for
        }
        if (_key_num == 1501 && (InfoByChapter[index].chapter == 5)) {
            if (GameManager.getInstance().CheckArray(GameManager.getInstance().FindJeongyeon, 8)) {
                return 1;
            }else if (GameManager.getInstance().Salt == 1) {
                return 2;
            }
        }

        return index;
    }

    private void afterAction() {

        if (_key_num == 110) this.gameObject.SetActive(false);
        else if (_key_num == 116 && GameManager.getInstance().GetMainChapter() == 12) {
            GameManager.getInstance().Wallpaper = 1;
            GameManager.getInstance().CheckMainChapter();
            SceneManager.LoadScene("2_Baby");
        }else if (_key_num == 83 && GameManager.getInstance().Wallpaper == 1) {
            this.gameObject.SetActive(false);
        }
    }

    private void afterUsingItem(int item_Key) {
        if(_key_num == 13002 && item_Key == 15) {
            GameManager.getInstance().BreakDisplay = 1;
            SceneManager.LoadScene("2_Dress");
        }
    }
}
