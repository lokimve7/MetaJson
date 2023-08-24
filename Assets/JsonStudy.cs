using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct UserInfo
{
    //�̸�
    public string userName;
    //����
    public int age;
    //Ű
    public float height;
    //���� (true : ����, false : ����)
    public bool gender;
    //�����ϴ� ����
    public List<string> favoriteFood;
}

[System.Serializable]
public struct FriendInfo
{
    public List<UserInfo> data;
}


public class JsonStudy : MonoBehaviour
{
    //���� ����
    public UserInfo myInfo;

    //���� ������ ������ ��� �ִ� ����
    public List<UserInfo> friendList = new List<UserInfo>();

    //friendList �� key ���� ����� �ֱ� ���� ����ü
    FriendInfo info = new FriendInfo();

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            myInfo = new UserInfo();

            myInfo.userName = "������" + i;
            myInfo.age = 25;
            myInfo.height = 180.5f;
            myInfo.gender = false;
            myInfo.favoriteFood = new List<string>();
            myInfo.favoriteFood.Add("ġŲ");
            myInfo.favoriteFood.Add("���̹�ħ");
            myInfo.favoriteFood.Add("�ᳪ��");

            friendList.Add(myInfo);
        }

        info.data = friendList;

        string s = JsonUtility.ToJson(info, true);
        print(s);

        //{ 

        //"jsonData " : [
        //                {
        //                    "userName" : "������0",
        //                    "age" : 25,
        //                    "height" : 180.5,
        //                    "gender" : false,
        //                    "favoriteFood" : ["ġŲ", "���̹�ħ", "�ᳪ��" ]
        //                },
        //                {
        //                    "userName" : "������1",
        //                    "age" : 25,
        //                    "height" : 180.5,
        //                    "gender" : false,
        //                    "favoriteFood" : ["ġŲ", "���̹�ħ", "�ᳪ��" ]
        //                },
        //                {
        //                    "userName" : "������1",
        //                    "age" : 25,
        //                    "height" : 180.5,
        //                    "gender" : false,
        //                    "favoriteFood" : ["ġŲ", "���̹�ħ", "�ᳪ��" ]
        //                }
        //              ]
        //}
}

    void Update()
    {
        //1�� Ű������
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //myInfo �� Json ���·� ������.
            string jsonData = JsonUtility.ToJson(info, true);
            print(jsonData);
            //jsonData �� ���Ϸ� ����
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            //json string �����͸� byte �迭�� �����.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
            //byteData �� file �� ����
            file.Write(byteData, 0, byteData.Length);

            file.Close();
        }
        

        //2��Ű�� ������ 
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //myInfo.txt �� �о����
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);
            //file �� ũ�⸸ŭ byte �迭�� �Ҵ��Ѵ�.
            byte[] byteData = new byte[file.Length];
            //byteData �� file �� ������ �о�´�.
            file.Read(byteData, 0, byteData.Length);
            //������ �ݾ�����
            file.Close();

            //byteData �� ���ڿ��� �ٲ���
            string jsonData = Encoding.UTF8.GetString(byteData);

            //���ڿ��� �Ǿ��ִ� jsonData �� myInfo �� parsing �Ѵ�.
            myInfo = JsonUtility.FromJson<UserInfo>(jsonData);
        }
    }
}
