using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance = null;

    public const int SOJU = 12;
    public const int SALT = 18;
    public const int SALTYWATER = 22;
    public const int GROUNDKEY = 43;



    public static int MainChapter; // 전체 메인 에피소드 관리

    public int[] EndScene;
    public int[] FindCharacter; // 친구찾기 배열
    public int[] DeadCharacter; // 죽은 친구 배열
    public int[] MeetCharacter; // 만난 친구 배열
    public int[] FindJeongyeon; // 정연 찾기 위해 1층 모든방 배열
    public int[] CheckOverlap; // 중복방지배열 

    /*획득 아이템*/
    public int Soju;
    public int Salt;
    public int SaltyWater;
    public int GroundKey; // 지하실 열쇠

    /*상태변수*/
    public int BreakDisplay; // 장식장 깨뜨림
    public int Wallpaper; // 띠벽지 발견
    public int HomeConstruct; // 집구조도
    public int CorrectPassword; // 비밀번호 일치 여부
    public int BabyBox;
    

    public bool isHide;
    public bool isScenePlay;    //현정추가! 컷씬플레이중인지 여부


    private GameManager()
    {
        MainChapter = 8;

        EndScene = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        FindCharacter = new int[4] { 0, 0, 0, 0 };
        DeadCharacter = new int[4] { 0, 0, 0, 0 };
        MeetCharacter = new int[2] { 0, 0 };
        FindJeongyeon = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        CheckOverlap = new int[3] { 0, 0, 0 };

        Soju = 0; Salt = 0; SaltyWater = 0; GroundKey = 0;
        BreakDisplay = 0; Wallpaper = 0; HomeConstruct = 0; CorrectPassword = 0;
        BabyBox = 0;

    }



    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }


    public void GetItem(int ItemKey) // 스토리에 관련된 아이템 얻을때
    {
        switch (ItemKey)
        {
            case SOJU: Soju = 1; break;
            case SALT: Salt = 1; break;
            case SALTYWATER: SaltyWater = 1; break;
            case GROUNDKEY: GroundKey = 1; break;

            //머리카락(효정/하빈/정연/서운)
            case 6: DeadCharacter[0] = 1; break;
            case 7: DeadCharacter[1] = 1; break;
            case 8: DeadCharacter[2] = 1; break;
            case 9: DeadCharacter[3] = 1; break;
        }
    }


    public void CheckMainChapter() // 챕터넘어갈 이벤트 만족했는지 확인
    {

        Debug.Log("겜매" + Player.hiding);
        //게임시작시 -> 0
        if (MainChapter == -1)
            SetMainChapter(0);

        //튜토리얼씬 종료시 -> 1로간다.
        else if (MainChapter == 0 && isSceneEnd(PlayScene.numScene.tutorial))
            SetMainChapter(1);

        //숨은 친구들 다 찾으면 -> 2로
        else if (MainChapter == 1 && isCheckArray(FindCharacter, 4))
            SetMainChapter(2);

        // hide_1_end 씬종료되고, 소주 얻었으면 ->3
        else if (MainChapter == 2 && isSceneEnd(PlayScene.numScene.hide_1_end) && Soju == 1)
            SetMainChapter(3);

        // hide_2_ready 씬 종료되면(준비끝) ->4
        else if (MainChapter == 3 && isSceneEnd(PlayScene.numScene.hide_2_ready))
        {
            //Debug.Log("아저씨 활성화");//호빈추가
            Enemy.start_enemy_working();//호빈추가
            SetMainChapter(4);
        }

        // 게임시작 효정이 만나면 -> 5
        else if (MainChapter == 4 && DeadCharacter[0] == 1)
            SetMainChapter(5);

        // 정연이 5번 -> -2(엔딩)
        else if (MainChapter == 4 && MeetCharacter[1] == 5)
            SetMainChapter(-2);

        // 하빈이 만나면 ->6
        else if (MainChapter == 5 && isCheckRoom("2_Hall"))
            SetMainChapter(6);

        // 소금있으면 ->7
        else if (MainChapter == 6 && isCheckRoom("2_Hall") && Salt == 1)
            SetMainChapter(7);

        // 정연이 찾으려고 1층다돌면 ->8
        else if (MainChapter == 7 && isSceneEnd(PlayScene.numScene.habin_havesalt) && isCheckArray(FindJeongyeon, 8))
            SetMainChapter(8);

        // 소금물 보유시 챕터 증가 -> 8+1
        else if (MainChapter >= 8 && SaltyWater == 1)
        {
            if (CheckOverlap[0] == 0)
            {
                CheckOverlap[0] = 1;
                SetMainChapter(MainChapter + 1);
            }
        }

        // 장식장 뿌시는 씬 재생후, 장식장 깨져있으면 챕터증가 -> 8+1
        else if (MainChapter >= 8 && BreakDisplay == 1)
        {
            if (CheckOverlap[1] == 0)
            {
                CheckOverlap[1] = 1;
                SetMainChapter(MainChapter + 1);
            }
        }

        // 세탁기 소리 씬재생후 정연이 죽음 확인-> 8+1
        else if (MainChapter >= 9 && isSceneEnd(PlayScene.numScene.after_break))
        {
            if (CheckOverlap[2] == 0)
            {
                CheckOverlap[2] = 1;
                SetMainChapter(MainChapter + 1);
            }
        }

        // (서운죽음)씬종료후 서운이 죽음 확인하면 -> 12
        else if (MainChapter == 11 && isSceneEnd(PlayScene.numScene.ringPhone) && DeadCharacter[3] == 1)
            SetMainChapter(12);

        // 띠벽지 확인하면 ->13
        else if (MainChapter == 12 && Wallpaper == 1)
            SetMainChapter(13);

        // 집구조도 확인가능 -> 14
        else if (MainChapter == 13 && HomeConstruct == 1)
            SetMainChapter(14);

        // 지하실 열쇠 -> 15
        else if (MainChapter == 14 && GroundKey == 1)
            SetMainChapter(15);

        // 비밀번호 입력후 일치 -> 16(엔딩)
        else if (MainChapter == 15 && CorrectPassword == 1)
            SetMainChapter(-3);

        //배터리 부족시 엔딩
        if (FlashLight.getFlashData() <= 0)
            SetMainChapter(-6);

        chapterPlayScene(); // 스토리 씬인지 확인


    }


    public void chapterPlayScene()
    {
        switch (MainChapter)
        {
            //잘못된 오브젝트 사용 엔딩
            case -1:
                scenePlay_End(PlayScene.numScene.Invalid_Obj);
                break;

            //정연엔딩
            case -2:
                scenePlay_End(PlayScene.numScene.JeongYeon);
                break;

            // 진엔딩(꿈)
            case -3:
                scenePlay_End(PlayScene.numScene.ending_exit);
                break;

            //인형인줄 엔딩
            case -4:
                scenePlay_End(PlayScene.numScene.suspectDoll);
                break;

            //아저씨 범인일때 엔딩
            case -5:
                scenePlay_End(PlayScene.numScene.suspectKim);
                break;


            // 튜토리얼일때 튜토리얼 씬진행
            case 0:
                if (isFirstTime(PlayScene.numScene.tutorial))
                    scenePlay_End(PlayScene.numScene.tutorial);
                break;

            // 친구들 다 찾은 챕터. hide_1_end 씬 진행
            case 2:
                if (isFirstTime(PlayScene.numScene.hide_1_end))
                    scenePlay_End(PlayScene.numScene.hide_1_end);
                break;

            // 소주 찾은 챕터. hide_2_ready 진행
            case 3:
                if (isFirstTime(PlayScene.numScene.hide_2_ready))
                    scenePlay_End(PlayScene.numScene.hide_2_ready);
                break;

            //하빈이 만난 챕터. 소금이 없음 씬 진행
            case 6:
                if (isFirstTime(PlayScene.numScene.habin_nosalt) && Salt == 0)
                    scenePlay_End(PlayScene.numScene.habin_nosalt);
                break;

            // 소금이 있음.
            case 7:
                //정연이 찾아와 씬
                if (isFirstTime(PlayScene.numScene.habin_havesalt))
                    scenePlay_End(PlayScene.numScene.habin_havesalt);

                //정연이 없네 씬
                else if
                    (isFirstTime(PlayScene.numScene.no_jy) && EndScene[5] == 1)
                    scenePlay_End(PlayScene.numScene.no_jy);
                break;

            // 방 다 뒤져서 정연이없는상태임
            case 8:
                //하빈이도 없네
                if (isFirstTime(PlayScene.numScene.no_hb) && isCheckRoom("2_Hall"))
                    scenePlay_End(PlayScene.numScene.no_hb);

                //하빈이 죽었자나?!씬 
                else if (isFirstTime(PlayScene.numScene.hb_die) && isCheckRoom("2_Swimming") && EndScene[7] == 1)
                    scenePlay_End(PlayScene.numScene.hb_die);

                //장식장 뿌시는씬
                if (isFirstTime(PlayScene.numScene.break_cabinet) && BreakDisplay == 1)
                    scenePlay_End(PlayScene.numScene.break_cabinet);
                break;

            //장식장 뿌셨거나 소금물 있을때
            case 9:
            case 10:
                //장식장 뿌시는씬
                if (isFirstTime(PlayScene.numScene.break_cabinet) && BreakDisplay == 1)
                    scenePlay_End(PlayScene.numScene.break_cabinet);

                //뿌신후 장롱안에서 세탁기 웅웅씬
                else if (isFirstTime(PlayScene.numScene.after_break) && EndScene[9] == 1 && isHide)
                {
                    scenePlay_End(PlayScene.numScene.after_break);
                    Player.hiding = false;
                }

                // 정연이 죽음씬
                else if (isFirstTime(PlayScene.numScene.jy_die) && isCheckRoom("1_Laundry"))
                    scenePlay_End(PlayScene.numScene.jy_die);
                break;

            // 서운이 죽게됨 
            case 11:
                if (isFirstTime(PlayScene.numScene.ringPhone) && isCheckRoom("2_Bed"))
                    scenePlay_End(PlayScene.numScene.ringPhone);
                break;


        }

    }

    /// <summary>
    /// // 현재 방이름이 룸네임과 일치하면true/아니면false
    /// </summary>
    /// <param name="RoomName">방이름</param>
    /// <returns></returns>
    public bool isCheckRoom(string RoomName)
    {
        if (SceneManager.GetActiveScene().name == RoomName)
            return true;
        else
            return false;
    }



    /// <summary>
    /// 씬 재생하기(EndScene = 1, isScenePlay = true)
    /// </summary>
    /// <param name="sceneNumber">씬 번호</param>
    public void scenePlay_End(PlayScene.numScene sceneNumber)
    {
        isScenePlay = true;
        PlayScene.getInstance().playScene(sceneNumber);
        EndScene[(int)sceneNumber] = 1;
    }


    /// <summary>
    /// 씬을 처음 실행하는 건지 확인.  처음이면 true / 아니면 false
    /// </summary>
    /// <param name="sceneNumber">씬 enum값</param>
    /// <returns></returns>
    public bool isFirstTime(PlayScene.numScene sceneNumber)
    {
        if (!isScenePlay && EndScene[(int)sceneNumber] == 0)
            return true;
        else
            return false;

    }


    /// <summary>
    /// 씬 종료했는지 확인하는 함수. 종료했으면 true 아니면 false 
    /// </summary>
    /// <param name="sceneNumber"></param>
    /// <returns></returns>
    public bool isSceneEnd(PlayScene.numScene sceneNumber)
    {
        if (!isScenePlay && EndScene[(int)sceneNumber] == 1)
            return true;
        else
            return false;
    }

    public void changeArrayState()
    {
        int Chapter = MainChapter;


        if (Chapter == 7) // 정연이 찾는 챕터일때
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "1_Bath": FindJeongyeon[0] = 1; break;
                case "1_Dining": FindJeongyeon[1] = 1; break;
                case "1_Front": FindJeongyeon[2] = 1; break;
                case "1_Hall": FindJeongyeon[3] = 1; break;
                case "1_Kitchen": FindJeongyeon[4] = 1; break;
                case "1_Laundry": FindJeongyeon[5] = 1; break;
                case "1_Living": FindJeongyeon[6] = 1; break;
                case "1_Reception": FindJeongyeon[7] = 1; break;
            }
        }


    }




    public bool isCheckArray(int[] TestArray, int ArrayNum)
    {
        int Num = 0;

        for (int i = 0; i < ArrayNum; i++)
        {
            if (TestArray[i] == 1)
                Num += 1;

        }

        if (ArrayNum == Num)
            return true;

        else
            return false;
    }

    public void resetGame() // 새로 시작시 초기화
    {
        MainChapter = 8;

        EndScene = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        FindCharacter = new int[4] { 0, 0, 0, 0 };
        DeadCharacter = new int[4] { 0, 0, 0, 0 };
        MeetCharacter = new int[2] { 0, 0 };
        FindJeongyeon = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        CheckOverlap = new int[3] { 0, 0, 0 };

        Soju = 0; Salt = 0; SaltyWater = 0; GroundKey = 0;
        BreakDisplay = 0; Wallpaper = 0; HomeConstruct = 0; CorrectPassword = 0;

        BabyBox = 0;
    }

    public int GetMainChapter() // 현재 챕터 반환
    {

        CheckMainChapter(); //현정추가 한번체크후 반환필요해보여서
        changeArrayState(); // 스토리진행변수체크
        return MainChapter;
    }

    public void SetMainChapter(int Chapter) // 챕터 수정하기
    {
        MainChapter = Chapter;
    }


}
