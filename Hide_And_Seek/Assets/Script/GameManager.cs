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

    public bool isScenePlay;    //현정추가! 컷씬플레이중인지 여부


    private GameManager()
    {
        MainChapter = -1;

        EndScene = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        FindCharacter = new int[4] { 0, 0, 0, 0 };
        DeadCharacter = new int[4] { 0, 0, 0, 0 };
        MeetCharacter = new int[4] { 0, 0, 0, 0 };
        FindJeongyeon = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        CheckOverlap = new int[3] { 0, 0, 0 };


        Soju = 0; Salt = 0; SaltyWater = 0; GroundKey = 0;
        BreakDisplay = 0; Wallpaper = 0; HomeConstruct = 0; CorrectPassword = 0;


    }

    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    public void Update()
    {
        StoryPlayScene();
    }

    public void GetItem(int ItemKey) // 스토리에 관련된 아이템 얻을때
    {
        switch (ItemKey)
        {
            case SOJU: Soju = 1; break;
            case SALT: Salt = 1; break;
            case SALTYWATER: SaltyWater = 1; break;
            case GROUNDKEY: GroundKey = 1; break;
        }
    }

    public void CheckObject(string objName)
    {
        switch(objName)
        {
            case "wallpaper": Wallpaper = 1; break;
            case "cabinet": if (Inventory.getInstance().inventory.Contains(10)) BreakDisplay = 1; break;
        }
    }

    public void CheckMainChapter() // 챕터넘어갈 이벤트 만족했는지 확인
    {
        StoryPlayScene(); // 스토리 씬인지 확인

        //현정 컷신으로인한 챕터 번호수정있음! 주석추가있음
        //현정 추가
        if (MainChapter == -1)
        {
            SetMainChapter(0);
            //테스트해야하는데 자꾸 튜토링러 진행돼서 잠깐 아래 세줄 주석처리함;;
            //SetMainChapter(0);
            //isScenePlay = true;
            //PlayScene.getInstance().playScene(PlayScene.numScene.tutorial);


        }

        //튜토리얼씬 종료시 -> 1로간다.
        else if (MainChapter == 0 && !isScenePlay && EndScene[0] == 1)
            SetMainChapter(1);

        //숨은 친구들 다 찾으면 -> 2로
        else if (MainChapter == 1 && CheckArray(FindCharacter, 4))
            SetMainChapter(2);

        // hide_1_end 씬종료되고, 소주 얻었으면 ->3
        else if (MainChapter == 2 && !isScenePlay && EndScene[1] == 1 && Soju == 1)
            SetMainChapter(3);

        // hide_2_ready 씬 종료되면(준비끝) ->4
        else if (MainChapter == 3 && !isScenePlay && EndScene[2] == 1)
            SetMainChapter(4);

        // 게임시작 효정이 만나면 -> 5
        else if (MainChapter == 4 && DeadCharacter[0] == 1)
            SetMainChapter(5);

        // 정연이 5번 -> -2(엔딩)
        else if (MainChapter == 4 && MeetCharacter[1] == 5)
            SetMainChapter(-2);

        // 하빈이 만나면 ->6
        else if (MainChapter == 5 && MeetCharacter[2] == 1)
            SetMainChapter(6);

        // 소금있으면 ->7
        else if (MainChapter == 6 && !isScenePlay && EndScene[4] == 1 && Salt == 1)
            SetMainChapter(7);

        // 정연이 찾으려고 1층다돌면 ->8
        else if (MainChapter == 7 && !isScenePlay && EndScene[5] == 1 && CheckArray(FindJeongyeon, 8))
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
        else if (MainChapter >= 8 && !isScenePlay && EndScene[9] == 1 && BreakDisplay == 1)
        {
            if (CheckOverlap[1] == 0)
            {
                CheckOverlap[1] = 1;
                SetMainChapter(MainChapter + 1);
            }
        }

        // 세탁기 소리 씬재생후 정연이 죽음 확인-> 8+1
        else if (MainChapter >= 9 && DeadCharacter[2] == 1 && !isScenePlay && EndScene[10] == 1)
        {
            if (CheckOverlap[2] == 0)
            {
                CheckOverlap[2] = 1;
                SetMainChapter(MainChapter + 1);
            }
        }

        // (세탁기)씬종료후 서운이 죽음 확인하면 -> 12
        else if (MainChapter == 11 && DeadCharacter[3] == 1 && !isScenePlay && EndScene[11] == 1)
            SetMainChapter(12);

        // 아저씨한테 전화하는씬 후 띠벽지 확인하면 ->13
        else if (MainChapter == 12 && !isScenePlay && EndScene[3] == 1 && Wallpaper == 1)
            SetMainChapter(13);

        // 집구조도 확인가능 -> 14
        else if (MainChapter == 13 && HomeConstruct == 1)
            SetMainChapter(14);

        // 지하실 열쇠 -> 15
        else if (MainChapter == 14 && GroundKey == 1)
            SetMainChapter(15);

        // 비밀번호 입력후 일치 -> 16(엔딩)
        else if (MainChapter == 15 && CorrectPassword == 1)
            SetMainChapter(16);

        //배터리 부족시 엔딩
        if (FlashLight.getFlashData() <= 0)
            SetMainChapter(-6);

    }


    public void StoryPlayScene()
    {
        int Chapter = MainChapter;

        switch (Chapter)
        {
            // 튜토리얼일때 튜토리얼 씬진행
            case 0: if (Check_First_Time(PlayScene.numScene.tutorial))
                    ScenePlayAndEnd(PlayScene.numScene.tutorial);
                break;

            // 친구들 다 찾은 챕터. hide_1_end 씬 진행
            case 2: if (Check_First_Time(PlayScene.numScene.hide_1_end))
                    ScenePlayAndEnd(PlayScene.numScene.hide_1_end);
                break;

            // 소주 찾은 챕터. hide_2_ready 진행
            case 3: if (Check_First_Time(PlayScene.numScene.hide_2_ready))
                    ScenePlayAndEnd(PlayScene.numScene.hide_2_ready);
                break;

            //하빈이 만난 챕터. 소금이 없음 씬 진행
            case 6: if (Check_First_Time(PlayScene.numScene.habin_nosalt))
                ScenePlayAndEnd(PlayScene.numScene.habin_nosalt);
                break;

            // 소금이 있음.
            case 7:
                //정연이 찾아와 씬
                if (Check_First_Time(PlayScene.numScene.habin_havesalt))
                    ScenePlayAndEnd(PlayScene.numScene.habin_havesalt); 

                //정연이 없네 씬
                else if (Check_First_Time(PlayScene.numScene.no_jy) && EndScene[5] == 1 )
                 ScenePlayAndEnd(PlayScene.numScene.no_jy); 
                    break;

            // 방 다 뒤져서 정연이없는상태임
            case 8:
                  //하빈이도 없네
                  if (Check_First_Time(PlayScene.numScene.no_hb) && CheckNowRoomName("2_Hall") )
                      ScenePlayAndEnd(PlayScene.numScene.no_hb); 

                  //하빈이 죽었자나?!씬 
                  else if (Check_First_Time(PlayScene.numScene.hb_die) && CheckNowRoomName("2_Swimming") && EndScene[7] == 1)
                    ScenePlayAndEnd(PlayScene.numScene.hb_die); 
                  break;

            case 9: case 10:
                //장식장 뿌시는씬
                if (Check_First_Time(PlayScene.numScene.break_cabinet) && BreakDisplay == 1)
                ScenePlayAndEnd(PlayScene.numScene.break_cabinet);

                //뿌신후 다음 장롱에 숨는씬
                else if (Check_First_Time(PlayScene.numScene.after_break) && EndScene[9] == 1 )
                ScenePlayAndEnd(PlayScene.numScene.after_break);
                break;

            // 장롱에 숨은 상태. 정연이 주거따(세탁기 씬)
            case 11: if (Check_First_Time(PlayScene.numScene.jy_die))
                ScenePlayAndEnd(PlayScene.numScene.jy_die);
                break;

            //  서운이 죽음확인 한 상태. 아저씨한테 전화씬
            case 12: if (Check_First_Time(PlayScene.numScene.ringPhone))
                     ScenePlayAndEnd(PlayScene.numScene.ringPhone);
                 break;

        }

    }

    /// <summary>
    /// // 현재 방이름이 룸네임과 일치하면true/아니면false
    /// </summary>
    /// <param name="RoomName">방이름</param>
    /// <returns></returns>
    public bool CheckNowRoomName(string RoomName) 
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
    public void ScenePlayAndEnd(PlayScene.numScene sceneNumber)
    {
        isScenePlay = true;
        ScenePlay((int)sceneNumber);
        EndScene[(int)sceneNumber] = 1;
    }


    /// <summary>
    /// 씬을 처음 실행하는 건지 확인.  처음이면 true / 아니면 false
    /// </summary>
    /// <param name="sceneNumber">씬 enum값</param>
    /// <returns></returns>
    public bool Check_First_Time(PlayScene.numScene sceneNumber)
    {
        if (!isScenePlay && EndScene[(int)sceneNumber] == 0)
            return true;
        else
            return false;

    }
        


    /// <summary>
    /// GameManager.getInstance().ScenePlay(int EndingNum)
    /// </summary>
    /// <param name="EndingNum"></param>
    public void ScenePlay(int EndingNum) // 엔딩번호 전달받는 경우 playScene 호출 -> 해당스토리 재생
    {
        switch (EndingNum)
        {
            case -1: PlayScene.getInstance().playScene(PlayScene.numScene.Invalid_Obj); break;
            case -2: PlayScene.getInstance().playScene(PlayScene.numScene.JeongYeon); break;
            case -3: PlayScene.getInstance().playScene(PlayScene.numScene.ending_exit); break;
            case -4: PlayScene.getInstance().playScene(PlayScene.numScene.suspectDoll); break;
            case -5: PlayScene.getInstance().playScene(PlayScene.numScene.suspectKim); break;
            case -6: PlayScene.getInstance().playScene(PlayScene.numScene.batteryLack); break;
            case 0: PlayScene.getInstance().playScene(PlayScene.numScene.tutorial); break;
            case 1: PlayScene.getInstance().playScene(PlayScene.numScene.hide_1_end); break;
            case 2: PlayScene.getInstance().playScene(PlayScene.numScene.hide_2_ready); break;
            case 3: PlayScene.getInstance().playScene(PlayScene.numScene.ringPhone); break;
            case 4: PlayScene.getInstance().playScene(PlayScene.numScene.habin_nosalt); break;
            case 5: PlayScene.getInstance().playScene(PlayScene.numScene.habin_havesalt); break;
            case 6: PlayScene.getInstance().playScene(PlayScene.numScene.no_jy); break;
            case 7: PlayScene.getInstance().playScene(PlayScene.numScene.no_hb); break;
            case 8: PlayScene.getInstance().playScene(PlayScene.numScene.hb_die); break;
            case 9: PlayScene.getInstance().playScene(PlayScene.numScene.break_cabinet); break;
            case 10: PlayScene.getInstance().playScene(PlayScene.numScene.after_break); break;
            case 11: PlayScene.getInstance().playScene(PlayScene.numScene.jy_die); break;

        }

    }

    public bool CheckArray(int[] TestArray, int ArrayNum)
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

    public void ResetGame() // 새로 시작시 초기화
    {
        MainChapter = -1;

        EndScene = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        FindCharacter = new int[4] { 0, 0, 0, 0 };
        DeadCharacter = new int[4] { 0, 0, 0, 0 };
        MeetCharacter = new int[4] { 0, 0, 0, 0 };
        FindJeongyeon = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        CheckOverlap = new int[3] { 0, 0, 0 };


        Soju = 0; Salt = 0; SaltyWater = 0; GroundKey = 0;
        BreakDisplay = 0; Wallpaper = 0; HomeConstruct = 0; CorrectPassword = 0;
    }

    public int GetMainChapter() // 현재 챕터 반환
    {
        CheckMainChapter(); //현정추가 한번체크후 반환필요해보여서
        return MainChapter;
    }

    public void SetMainChapter(int Chapter) // 챕터 수정하기
    {
        MainChapter = Chapter;
    }


}


