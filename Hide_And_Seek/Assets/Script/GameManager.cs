using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance = null;

    /*아이템 상수선언*/
    public const int SOJU = 12;
    public const int SALT = 18;
    public const int SALTYWATER = 22;
    public const int GROUNDKEY = 43;

    public static int MainChapter; // 전체 메인 에피소드 관리

    public int[] EndScene; // 씬 종료 체크 배열
    public int[] FindCharacter; // 친구찾기 배열
    public int[] DeadCharacter; // 죽은 친구 배열
    public int[] MeetCharacter; // 만난 친구 배열
    public int[] FindJeongyeon; // 정연 찾기 위해 1층 모든방 배열

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

        EndScene = new int[2]{ 0,0 };
        FindCharacter = new int[4] { 0, 0, 0, 0 };
        DeadCharacter = new int[4] { 0, 0, 0, 0 };
        MeetCharacter = new int[4] { 0, 0, 0, 0 };
        FindJeongyeon = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };


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
    
    public void CheckMainChapter() // 챕터넘어갈 이벤트 만족했는지 확인
    {
        //현정 컷신으로인한 챕터 번호수정있음! 주석추가있음
        //현정 추가
        if(MainChapter == -1) {
            //테스트해야하는데 자꾸 튜토링러 진행돼서 잠깐 아래 세줄 주석처리함;;
            //SetMainChapter(0);
            //isScenePlay = true;
            //PlayScene.getInstance().playScene(PlayScene.numScene.tutorial);
        }
        /*M=0 튜토리얼*/
        if (MainChapter == 0 && !isScenePlay) {
            SetMainChapter(1);
        }
        /*M=1 1차숨바꼭질(친구찾기)*/
        else if (MainChapter == 1 && CheckArray(FindCharacter, 4)) { // 숨바꼭질로 친구들 다 찾았을때
            //현정추가 컷씬시작
            SetMainChapter(2);
            isScenePlay = true;
            PlayScene.getInstance().playScene(PlayScene.numScene.hide_1_end);
        }
        //현정추가 컷씬종료(컷씬 시작 전,종료시 챕터+1 해주길바람)
        else if (MainChapter == 2 && !isScenePlay) {
            SetMainChapter(3);
        }
        /*M=2 소금물없어서 소주찾으러*/
        else if (MainChapter == 3 && Soju == 1) { // 소주 획득시
            SetMainChapter(4);
            isScenePlay = true;
            PlayScene.getInstance().playScene(PlayScene.numScene.hide_2_ready);
        }
        /*M=3 준비물 모두 모아서 장롱씬*/
        else if (MainChapter == 4 && !isScenePlay) // 2차 장롱씬 종료시
            SetMainChapter(5);

        /*M=4 시작 정연이 5번누르면 엔딩 또는 효정이 확인 */
        else if (MainChapter == 5 && MeetCharacter[1] == 5) { // 정연엔딩
            SetMainChapter(-2);
            isScenePlay = true;
            //PlayScene
        } else if (MainChapter == 5 && DeadCharacter[0] == 1)
            SetMainChapter(6);

        /*M=5 소금을 얻어야 겠다*///하빈만난부분
        else if (MainChapter == 6 && MeetCharacter[2] == 1)
            SetMainChapter(7);

        else if (MainChapter == 7 && Salt == 1) {
            SetMainChapter(8);
        } else if (MainChapter == 8 && CheckArray(FindJeongyeon, 8))
            SetMainChapter(9);

        //하빈gg
        else if (MainChapter >= 9 && SaltyWater == 1) {
            SetMainChapter(MainChapter + 1);
            //현정 : 두번실행방지필요!!!!!!!!
        } else if (MainChapter >= 9 && BreakDisplay == 1) {
            SetMainChapter(MainChapter + 1);
            //여기두!
        } else if (MainChapter >= 9 && DeadCharacter[2] == 1) {
            SetMainChapter(MainChapter + 1);
            //여기두!
        }
        /*M= 11 서운 죽음*/
        else if (MainChapter == 12 && DeadCharacter[3] == 1) {
            SetMainChapter(13);
            isScenePlay = true;
            PlayScene.getInstance().playScene(PlayScene.numScene.ringPhone);
        } else if (MainChapter == 13 && !isScenePlay) {
            SetMainChapter(14);
        }
        //띠벽지애기그림
        else if (MainChapter == 14 && Wallpaper == 1)
            SetMainChapter(15); //이제 인화실발견가능

        //집구조도확인
        else if (MainChapter == 15 && HomeConstruct == 1)
            SetMainChapter(16); //이제 지하실 가능

        //지하실+열쇠사용
        else if (MainChapter == 16 && GroundKey == 1)
            SetMainChapter(17); //이제 지하실의 바깥통로 사용가능

        //통로 문 염
        else if (MainChapter == 17 && CorrectPassword == 1) {
            SetMainChapter(18);
            isScenePlay = true;
            PlayScene.getInstance().playScene(PlayScene.numScene.ending_exit);
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



