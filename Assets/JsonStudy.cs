using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct UserInfo
{
    //이름
    public string userName;
    //나이
    public int age;
    //키
    public float height;
    //성별 (true : 여성, false : 남성)
    public bool gender;
    //좋아하는 음식
    public List<string> favoriteFood;
}

[System.Serializable]
public struct FriendInfo
{
    public List<UserInfo> data;
}


public class JsonStudy : MonoBehaviour
{
    //나의 정보
    public UserInfo myInfo;

    //유저 정보를 여러개 들고 있는 변수
    public List<UserInfo> friendList = new List<UserInfo>();

    //friendList 의 key 값을 만들어 주기 위한 구조체
    FriendInfo info = new FriendInfo();

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            myInfo = new UserInfo();

            myInfo.userName = "김현진" + i;
            myInfo.age = 25;
            myInfo.height = 180.5f;
            myInfo.gender = false;
            myInfo.favoriteFood = new List<string>();
            myInfo.favoriteFood.Add("치킨");
            myInfo.favoriteFood.Add("오이무침");
            myInfo.favoriteFood.Add("콩나물");

            friendList.Add(myInfo);
        }

        info.data = friendList;

        string s = JsonUtility.ToJson(info, true);
        print(s);

        //{ 

        //"jsonData " : [
        //                {
        //                    "userName" : "김현진0",
        //                    "age" : 25,
        //                    "height" : 180.5,
        //                    "gender" : false,
        //                    "favoriteFood" : ["치킨", "오이무침", "콩나물" ]
        //                },
        //                {
        //                    "userName" : "김현진1",
        //                    "age" : 25,
        //                    "height" : 180.5,
        //                    "gender" : false,
        //                    "favoriteFood" : ["치킨", "오이무침", "콩나물" ]
        //                },
        //                {
        //                    "userName" : "김현진1",
        //                    "age" : 25,
        //                    "height" : 180.5,
        //                    "gender" : false,
        //                    "favoriteFood" : ["치킨", "오이무침", "콩나물" ]
        //                }
        //              ]
        //}
}

    void Update()
    {
        //1번 키누르면
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //myInfo 를 Json 형태로 만들자.
            string jsonData = JsonUtility.ToJson(info, true);
            print(jsonData);
            //jsonData 를 파일로 저장
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            //json string 데이터를 byte 배열로 만든다.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
            //byteData 를 file 에 쓰자
            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
        

        //2번키를 누르면 
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //myInfo.txt 를 읽어오자
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);
            //file 의 크기만큼 byte 배열을 할당한다.
            byte[] byteData = new byte[file.Length];
            //byteData 에 file 의 내용을 읽어온다.
            file.Read(byteData, 0, byteData.Length);
            //파일을 닫아주자
            file.Close();

            //byteData 를 문자열로 바꾸자
            string jsonData = Encoding.UTF8.GetString(byteData);

            //문자열로 되어있는 jsonData 를 myInfo 에 parsing 한다.
            myInfo = JsonUtility.FromJson<UserInfo>(jsonData);
        }
    }
}
