﻿using System.Collections;
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
    private bool isInputPassword;
    private bool isValidPassword;

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
            if(transform.childCount != 0)
                spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            
            if(InfoByChapter[index].sprite != null) spriteRenderer.sprite = InfoByChapter[index].sprite;
            if(InfoByChapter[index].sound_default != null) {
                SoundManager.getInstance().playEffectLoop(InfoByChapter[index].sound_default);
            }
        }

        isInputPassword = false;
        isValidPassword = false;
    } //Start()

    public void action() {
        if (!_t_thing_f_portal) {
            int index = findIndexByChapter(mode_detail);
            Detail curInfo = InfoByChapter[index];
            if (!curInfo.isActByCall) return;

            if (curInfo.outputByCall.sound != null)
                SoundManager.getInstance().playEffect(curInfo.outputByCall.sound);
            if (curInfo.outputByCall.script_key != invalidValue)
                ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key });

            if (!curInfo.outputByCall.isBlockPortal) _obj.action();
            
            return;
        } else {
            int index = findIndexByChapter(mode_detail);
            Detail curInfo = InfoByChapter[index];
            if (!curInfo.isActByCall) return;

            //Sound 있으면재생
            if (curInfo.outputByCall.sound != null)
            {
                //호빈추가
                if (GameObject.FindGameObjectWithTag("Player") != null && Enemy.get_enemy_working() && !Enemy.get_enemy_chasing())
                {
                    //Debug.Log("소리 -> 어그로 발생!!");//test
                    Enemy.go_straight(Player.get_player_spot());
                }
                else
                {
                    //Debug.Log("소리 -> 어그로 발생 (X)");//test
                }

                SoundManager.getInstance().playEffect(curInfo.outputByCall.sound);
            }
            if (curInfo.type == Type.hide) {
                //hide
                GameObject pl = GameObject.Find("Player");
                if (pl != null) {
                    pl.GetComponent<Player>().player_hide();
                }
                //Script 있으면 재생
                if (curInfo.outputByCall.script_key != invalidValue)
                    ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key });
            } else if (curInfo.type == Type.no) {
                return;
            } else if (curInfo.type == Type.getItem) {
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
                //Script 있으면 재생
                if (curInfo.outputByCall.script_key != invalidValue)
                    ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCall.script_key });
            }
            GameManager.getInstance().CheckMainChapter();
            afterAction();
        }
    } //action()

    public void useitem(int item_key) {
        for (int i = 0; i < usingItem.Length; i++) {
            if(usingItem[i].materialItem_key == item_key) {
                if(usingItem[i].chapter_canUse > GameManager.getInstance().GetMainChapter()) {
                    continue;
                } else {
                    output curOutput = usingItem[i].outputByUsingItem;
                    //Sound 있으면재생
                    if (curOutput.sound != null)
                        SoundManager.getInstance().playEffect(curOutput.sound);

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
                    else if (curOutput.script_key != invalidValue) {
                        ScriptManager.getInstance().showScript(true, new int[] { curOutput.script_key });
                    }
                    if (!_t_thing_f_portal && !curOutput.isBlockPortal) {
                        _obj.action();
                    }

                    afterUsingItem(item_key);
                    GameManager.getInstance().CheckMainChapter();
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
                {
                    //호빈추가
                    if (GameObject.FindGameObjectWithTag("Player") != null && Enemy.get_enemy_working() && !Enemy.get_enemy_chasing())
                    {
                        Debug.Log("소리 -> 어그로 발생!!");//test
                        Enemy.go_straight(Player.get_player_spot());
                    }
                    else
                    {
                        Debug.Log("소리 -> 어그로 발생 (X)");//test
                    }
                    SoundManager.getInstance().playEffect(curInfo.outputByCollision.sound);
                }
                //Script 있으면 재생
                if (curInfo.outputByCollision.script_key != invalidValue)
                    ScriptManager.getInstance().showScript(true, new int[] { curInfo.outputByCollision.script_key });
                //Item 획득가능하면 획득
                if (curInfo.outputByCollision.item_key != invalidValue)
                    Inventory.getInstance().addItem(curInfo.outputByCollision.item_key);
            }

            GameManager.getInstance().CheckMainChapter();
            afterCollision();
        }
    }

    private int findIndexByChapter(int mode) {
        int chapter = GameManager.getInstance().GetMainChapter();
        int index = 0;

        if (GameManager.getInstance().BreakDisplay == 1) {
            if (_key_num == 607) return 2;
            else if (_key_num == 608) return 1;
            else if (_key_num == 13002) return 2;
            else if (_key_num == 13001) return 1;
        }
        if (_key_num == 110) {
            if (Inventory.getInstance().isExitItem(3) || Inventory.getInstance().isExitItem(5))
                return 1;
        }else if (_key_num == 83 && GameManager.getInstance().Wallpaper == 1) {
            return 1;
        }else if(_key_num == 1806 && (Inventory.getInstance().isExitItem(20) ||
            Inventory.getInstance().isExitItem(21) || Inventory.getInstance().isExitItem(22))) {
            return 2;
        }else if(_key_num == 1903) {
            if (!isInputPassword) return 0;
            else if (isInputPassword && isValidPassword) return 2;
        }else if(_key_num == 702) {
            if (Inventory.getInstance().isExitItem(1) || Inventory.getInstance().isExitItem(5))
                return 2;
        } else if (_key_num == 1005 && GameManager.getInstance().OpenBabyBox == 1) {
            return 2;
        }else if (_key_num == 3001 && GameManager.getInstance().FindCharacter[0] == 1) {
            return 1;
        } else if (_key_num == 3002 && GameManager.getInstance().FindCharacter[2] == 1) {
            return 1;
        } else if (_key_num == 3004 && GameManager.getInstance().FindCharacter[1] == 1) {
            return 1;
        } else if (_key_num == 3003 && GameManager.getInstance().FindCharacter[3] == 1) {
            return 1;
        }else if(_key_num == 202 && Inventory.getInstance().isExitItem(10)) {
            return 2;
        } else if (!_t_thing_f_portal && _key_num == 7 && GameManager.getInstance().OpenPantry == 1) {
            return 1;
        } else if (!_t_thing_f_portal && _key_num == 18 && GameManager.getInstance().OpenGarret == 1) {
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
            if(GameManager.getInstance().MeetCharacter[0] != 1 || GameManager.getInstance().isScenePlay) {
                return 4;
            }else if (GameManager.getInstance().isCheckArray(GameManager.getInstance().FindJeongyeon, 8)
                || GameManager.getInstance().Salt == 0) {
                return 1;
            }else if (GameManager.getInstance().Salt == 1) {
                return 2;
            }
        }

        return index;
    }

    private void afterAction() {

        if (_key_num == 110) this.gameObject.SetActive(false);
        else if (_key_num == 702) this.gameObject.SetActive(false);
        else if (_key_num == 116 && GameManager.getInstance().GetMainChapter() == 12) {
            GameManager.getInstance().Wallpaper = 1;
            GameManager.getInstance().CheckMainChapter();
            SceneManager.LoadScene("2_Baby");
        }else if (_key_num == 83 && GameManager.getInstance().Wallpaper == 1) {
            this.gameObject.SetActive(false);
        } else if (_key_num == 1903) {
            if (!isInputPassword) { PasswordUIManager password = new PasswordUIManager(this.gameObject, 1231); }
        } else if (_key_num == 2002) {
            PasswordUIManager password = new PasswordUIManager(this.gameObject, 4362);
        } else if(_key_num == 401) {
            GameManager.getInstance().MeetCharacter[1]++;
            GameManager.getInstance().CheckMainChapter();
        }else if(_key_num == 3001) {
            GameManager.getInstance().FindCharacter[0] = 1;
            this.gameObject.SetActive(false);
        } else if (_key_num == 3002) {
            GameManager.getInstance().FindCharacter[2] = 1;
            this.gameObject.SetActive(false);
        } else if (_key_num == 3003) {
            GameManager.getInstance().FindCharacter[3] = 1;
            this.gameObject.SetActive(false);
        } else if (_key_num == 3004) {
            GameManager.getInstance().FindCharacter[1] = 1;
            this.gameObject.SetActive(false);
        } else if(_key_num == 7) {
            GameManager.getInstance().DeadCharacter[0] = 1;
        }else if(_key_num == 1806 && (Inventory.getInstance().isExitItem(20) ||
            Inventory.getInstance().isExitItem(21) || Inventory.getInstance().isExitItem(22))){
            spriteRenderer.sprite = InfoByChapter[2].sprite;
        }else if(_key_num == 1201) {
            GameManager.getInstance().DeadCharacter[3] = 1;
        }else if(_key_num == 1701) {
            GameManager.getInstance().HomeConstruct = 1;
        } else if (_key_num == 202 && Inventory.getInstance().isExitItem(10)) {
            this.gameObject.SetActive(false);
        }
    }

    private void afterCollision() {
        if(_key_num == 1501) {
            GameManager.getInstance().MeetCharacter[0] = 1;
            GameManager.getInstance().CheckMainChapter();
        }
    }

    private void afterUsingItem(int item_Key) {
        if (_key_num == 13002 && item_Key == 15) {
            GameManager.getInstance().BreakDisplay = 1;
            Inventory.getInstance().deleteItem(15);
        } else if(_key_num == 1005 && item_Key == 10) {
            spriteRenderer.sprite = InfoByChapter[2].sprite;
            GameManager.getInstance().OpenBabyBox = 1;
        }else if(_key_num == 1 && !_t_thing_f_portal && item_Key == 43) {
            GameManager.getInstance().GroundKey = 1;
        }else if(_key_num == 3201 && item_Key == 20) {
            Inventory.getInstance().deleteItem(20);
        }else if(!_t_thing_f_portal && _key_num == 7 && item_Key == 13) {
            GameManager.getInstance().OpenPantry = 1;
        }else if(!_t_thing_f_portal && _key_num == 18 && item_Key == 24) {
            GameManager.getInstance().OpenGarret = 1;
        }
    }

    public void inputPassword(bool isValid) {
        isInputPassword = true;
        isValidPassword = isValid;

        if(_key_num == 1903) {
            if (isValid) {
                ScriptManager.getInstance().showScript(true, new int[] { InfoByChapter[1].outputByCall.script_key });
                spriteRenderer.sprite = InfoByChapter[2].sprite;
            } else if (!isValid) {
                ScriptManager.getInstance().showScript(true, new int[] { InfoByChapter[3].outputByCall.script_key });
                isInputPassword = false;
            }
        }else if (_key_num == 2002) {
            if (isValid) {
                ScriptManager.getInstance().showScript(true, new int[] { InfoByChapter[1].outputByCall.script_key });
                GameManager.getInstance().CorrectPassword = 1;
                GameManager.getInstance().CheckMainChapter();
            } else if (!isValid) {
                ScriptManager.getInstance().showScript(true, new int[] { InfoByChapter[1].outputByCall.script_key_isExistitem });
                isInputPassword = false;
            }
        }
    }
}
