using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public enum PlayerPrefsIndex { hp = 0, x = 1, y = 2, z = 3, room = 4, spot = 5 };
    public static string[] PlayerPrefsKey = {"Player_hp", "Player_x", "Player_y", "Player_z",
        "Player_Pos_room", "Player_Pos_spot"};

    public static GameObject Player_obj;
    public const float Speed_walk = 0.8f, Speed_run = 1.5f, Hp_max = 300f;
    public const int Ani_Idle = 0, Ani_Walk = 1, Ani_Run = 2;

    public float Hp, Speed;
    public bool Tire;
    public static bool hiding;
    public GameObject Hiding_UI_Prefab;
    private static GameObject Hiding_UI_Obj;
    public ISpot SpotInfo;
    public static int Player_Last_Portal_num;

    public Animator Animator;
    public Move move;

    public GameObject Flash_Prefab;
    public FlashLight Flash = null;

    public GameObject Light_Prefab;
    public Light_Directional Light = null;

    public GameObject JoyStick_Prefab;

    
    static RaycastHit2D[] hits_up = new RaycastHit2D[] { },
                            hits_down = new RaycastHit2D[] { };

    private void Awake() {
        Hp = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], Hp_max);

        Vector3 pos = new Vector3();
        pos.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
        pos.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
        pos.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);
        this.transform.position = pos;
        
        int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
        int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
        SpotInfo = new ISpot((Room)room, spot);
        
    }
    
    public static void Init(float pHp, Vector3 pPosition, ISpot pSpot) {

        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], pPosition.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], pPosition.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], pPosition.z);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], pHp);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], (int)pSpot._room);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], pSpot._spot);
        PlayerPrefs.Save();
        GameObject pl = GameObject.Find("Player");
        if (pl != null) {
            pl.transform.position = pPosition;
            pl.GetComponent<Player>().SpotInfo = pSpot;
            pl.GetComponent<Player>().Hp = pHp;
        }
    }
    
    public static void getPlayerData(ref float pHp, ref Vector3 pPosition, ref ISpot pSpot) {
        GameObject pl = GameObject.Find("Player");
        if(pl != null) {
            Player player = pl.GetComponent<Player>();
            pHp = player.Hp;
            pPosition = Player.get_player_pos();
            pSpot = player.SpotInfo;
        } else {
            pHp = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], Hp_max);
            pPosition.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
            pPosition.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
            pPosition.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);
            int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
            int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
            pSpot = new ISpot((Room)room, spot);
        }
    }

    private void Start() {
        SpotInfo._room = Scene_Manager.getInstance().get_room_info(SceneManager.GetActiveScene().name);
        int chapter = GameManager.getInstance().GetMainChapter();

        if(GameManager.getInstance().isScenePlay) {
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }
        GameObject joyStick = Instantiate(JoyStick_Prefab);

        Player_obj = this.gameObject;
        Tire = false;
        hiding = false;
        Animator = GetComponent<Animator>();
        move = joyStick.GetComponent<Move>();
        Speed = Speed_walk;

        if (chapter < 4) {
            GameObject f = Instantiate(Light_Prefab);
            f.name = "Light";
            Light = f.GetComponent<Light_Directional>();
        } else {
            GameObject f = Instantiate(Flash_Prefab);
            f.name = "Flash";
            Flash = f.GetComponent<FlashLight>();
            Flash.LinkUser(this.gameObject);
        }

    }

    private void OnDestroy() {
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], this.transform.position.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], this.transform.position.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], this.transform.position.z);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.hp], this.Hp);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], (int)this.SpotInfo._room);
        PlayerPrefs.SetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], this.SpotInfo._spot);
        PlayerPrefs.Save();
    }
    
    private void Update() {
        if (ScriptManager.getInstance().isPlaying || GameManager.getInstance().isScenePlay) {
            Animator.SetInteger("State", Ani_Idle);
            Animator.speed = Speed_walk;
            return;
        }

<<<<<<< HEAD
=======
        if (hiding)
        {
            if (Input.GetButtonDown("Action"))
            {

                Enemy._enemy.transform.position = Enemy.ENEMY_INIT_LOC;
                
                hiding = false;
                Destroy(Hiding_UI_Obj);
            }
            return;
        }
        set_spot_info();
        
>>>>>>> master
        Animator.SetInteger("State", Ani_Idle);
        Speed = Speed_walk;
        Hp = (Hp >= Hp_max) ? Hp_max : Hp + (30f * Time.deltaTime);
        Animator.SetBool("Back", false);


        if (!Tire) {
            movement();
            Animator.speed = Speed;
        }

        if (Hp <= 0) {
            Tire = true;
            Animator.speed = Speed_run;
            Animator.SetInteger("State", Ani_Idle);
            Invoke("heal", 2.0f);
        }
        
        Raycasting();
    }


    void Raycasting() {
        if (GameObject.Find("Player") == null) return;
        hits_up = Physics2D.RaycastAll(this.transform.position, Vector3.up);
        hits_down = Physics2D.RaycastAll(this.transform.position, Vector3.down);
    }

    void set_spot_info()
    {
        if (GameObject.Find("Player") == null) return;
        GameObject[] spots = GameObject.FindGameObjectsWithTag("Spot");
        float nearest_distance = 10000f;
        GameObject nearest_spot = null;
        for (int i = 0; i < spots.Length; i++)
        {
            if (Vector3.Distance(this.transform.position, spots[i].transform.position) <= nearest_distance)
            {
                nearest_distance = Vector3.Distance(this.transform.position, spots[i].transform.position);
                nearest_spot = spots[i];
            }
        }
        SpotInfo = new ISpot(Scene_Manager.getInstance().get_room_info(SceneManager.GetActiveScene().name), int.Parse(nearest_spot.name));
    }
    
    public static Object_State check_up_down(string s) {
        if (GameObject.Find("Player") == null) return Object_State.too_far;
        int i;

        for (i = 0; i < hits_up.Length; i++) {
            if (hits_up[i].collider.name == s) {
                return Object_State.Object_back;
            }
        }
        for (i = 0; i < hits_down.Length; i++) {
            if (hits_down[i].collider.name == s) {
                return Object_State.Object_front;
            }
        }
        return Object_State.too_far;
    }

    private void movement() {
        if (move.Horizontal == 0 && move.Vertical == 0) {
            return;
        }

        Animator.SetInteger("State", Ani_Walk);

        if (move.Run) {
            Hp -= 1.5f;
            Animator.SetInteger("State", Ani_Run);
            Speed = Speed_run;
            Animator.speed = Speed_run;
        }
        if (move.Vertical > 0 && (move.Horizontal < 0.4 && move.Horizontal > -0.4)) {
            Animator.SetBool("Back", true);
        }
        
        transform.Translate(Vector3.right * Speed * move.Horizontal * Time.deltaTime);
        transform.Translate(Vector3.up * Speed * move.Vertical * Time.deltaTime);

        
        if (move.Horizontal > 0) {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        } else if (move.Horizontal < 0) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }

    public void action() {
        if (Tire) return;

        if (hiding) {
            Enemy._enemy.transform.position = Enemy.ENEMY_INIT_LOC;

            hiding = false;
            Destroy(Hiding_UI_Obj);
        } else {
            GameObject nearObject = findNearObject();
            if (nearObject != null) {
                nearObject.SendMessage("action");
            }
        }
    }

    public void action_item() {
        if (Tire || hiding) return;
        int itemKey = Inventory.getInstance().curEquipItem;
        if (itemKey == -1) return;
        GameObject nearObject = findNearObject();
        if (nearObject != null) {
            nearObject.SendMessage("useitem", itemKey);
        }
    }

    private GameObject findNearObject() {
        Vector2 examdistance = new Vector2(-0.04468793f * transform.localScale.x, 0.006384373f);
        Vector2 examPosition = transform.position;
        examPosition += examdistance;
        Collider2D[] objects = Physics2D.OverlapBoxAll(examPosition, new Vector2(0.1f, 0.1f),
            0, 1 << LayerMask.NameToLayer("Object"));
        
        if (objects.Length == 0) {
            return null;
        }
        
        float minDistance = 10;
        int nearObjectIndex = 0;
        for (int i = 0; i < objects.Length; i++) {
            Vector2 heading = objects[i].transform.position - this.transform.position;
            if (minDistance > heading.sqrMagnitude) {
                minDistance = heading.sqrMagnitude;
                nearObjectIndex = i;
            }
        }
        return objects[nearObjectIndex].gameObject;
    }

    private void heal() {
        Animator.speed = Speed_walk;
        Tire = false;
    }
    
    public static void set_player_pos(Vector3 v) {
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], v.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], v.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], v.z);
        PlayerPrefs.Save();
        GameObject pl = GameObject.Find("Player");
        if (pl != null) {
            pl.transform.position = v;
        }
    }
    public static Vector3 get_player_pos() {
        GameObject go = GameObject.Find("Player");
        if (go != null) {
            return go.transform.position;
        } else {
            Vector3 pos = new Vector3();
            pos.x = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.x], 0);
            pos.y = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.y], 0);
            pos.z = PlayerPrefs.GetFloat(PlayerPrefsKey[(int)PlayerPrefsIndex.z], 0);
            return pos;
        }
    }
    public static ISpot get_player_spot()
    {
        GameObject go = GameObject.Find("Player");
        if(go != null) {
            return Player.Player_obj.GetComponent<Player>().SpotInfo;
        } else {
            int room = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.room], 0);
            int spot = PlayerPrefs.GetInt(PlayerPrefsKey[(int)PlayerPrefsIndex.spot], 0);
            ISpot ispot = new ISpot((Room)room, spot);
            return ispot;
        }
        
    }

    public void player_hide(){
        hiding = true;
        GameManager.getInstance().CheckMainChapter();
        Hiding_UI_Obj = Instantiate(Hiding_UI_Prefab);
        Enemy.player_start_hiding(SpotInfo);
    }

    public void player_hide_exit() {
        if (hiding) {
            Enemy._enemy.transform.position = Enemy.ENEMY_INIT_LOC;

            hiding = false;
            Destroy(Hiding_UI_Obj);
        }
    }
}
