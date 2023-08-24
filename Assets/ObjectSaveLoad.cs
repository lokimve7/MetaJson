using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

//랜덤하게 만들어지는 오브젝트의 정보
[System.Serializable]
public class ObjectInfo
{
    public int type;
    public Transform tr;
}

[System.Serializable]
public class SaveInfo
{
    public int type;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
}




public class ObjectSaveLoad : MonoBehaviour
{
    void Start()
    {
       
    }

    //만들어진 오브젝트들 담을 변수
    public List<ObjectInfo> objectList = new List<ObjectInfo>();

    void Update()
    {
        //1번키 누르면 랜덤한 모양, 크기, 위치, 회전 이 된 오브젝트 만들자.
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //모양을 랜덤하게 뽑자 (0 ~ 3)
            int type = Random.Range(0, 4);

            //type 모양으로 GameObject 만들자.
            GameObject go = GameObject.CreatePrimitive((PrimitiveType)type);

            //크기, 위치, 회전 랜덤하게 하자.
            go.transform.localScale = Vector3.one * Random.Range(0.5f, 2.0f);
            go.transform.position = Random.insideUnitSphere * Random.Range(1.0f, 20.0f);
            go.transform.rotation = Random.rotation;

            //만들어진 오브젝트의 정보를 List 에 담자.
            ObjectInfo info = new ObjectInfo();
            info.type = type;
            info.tr = go.transform;

            objectList.Add(info);
        }

        //2번키 누르면 objectList 의 정보를 json 으로 저장
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //objectList 를 기반으로 저장할 정보를 빼오자

            List<SaveInfo> saveInfoList = new List<SaveInfo>();

            for(int i = 0; i < objectList.Count; i++)
            {
                SaveInfo saveInfo = new SaveInfo();
                saveInfo.type = objectList[i].type;
                saveInfo.pos = objectList[i].tr.position;
                saveInfo.rot = objectList[i].tr.rotation;
                saveInfo.scale = objectList[i].tr.localScale;

                saveInfoList.Add(saveInfo);
            }

            //saveInfoList 을 이용해서 JsonData 로 만들자.
            JsonList<SaveInfo> jsonList = new JsonList<SaveInfo>();
            jsonList.data = saveInfoList;
            string jsonData = JsonUtility.ToJson(jsonList, true);
            print(jsonData);

            //jsonData 를 파일로 저장
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Create);
            //json string 데이터를 byte 배열로 만든다.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);
            //byteData 를 file 에 쓰자
            file.Write(byteData, 0, byteData.Length);
            file.Close();
            /*
             
            {
                "data" : [
                            {
                                "type" : 2,
                                "pos" : {"x":10, "y":20, "z":30 },
                                "rot" : {"x":11, "y":22, "z":33 },
                                "scale" : {"x":3, "y":3, "z":3 },
                            },
                            {
                                "type" : 2,
                                "pos" : {"x":10, "y":20, "z":30 },
                                "rot" : {"x":11, "y":22, "z":33 },
                                "scale" : {"x":3, "y":3, "z":3 },
                            }
                    ]
            }
             
             */
        }


        //3번키 누르면 objectInfo.txt 에서 데이터를 읽어서 오브젝틀 만들자.
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            //objectInfo.txt 를 읽어오자
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Open);
            //file 의 크기만큼 byte 배열을 할당한다.
            byte[] byteData = new byte[file.Length];
            //byteData 에 file 의 내용을 읽어온다.
            file.Read(byteData, 0, byteData.Length);
            //파일을 닫아주자
            file.Close();

            //byteData 를 Json 형태의 문자열로 만들자.
            string jsonData = Encoding.UTF8.GetString(byteData);

            //jsonData 를 이용해서 JsonList 에 Parsing 하자
            JsonList<SaveInfo> jsonList = JsonUtility.FromJson<JsonList<SaveInfo>>(jsonData);

            //jsonList.data 의 갯수 만큼 오브젝트를 생성하자
            for (int i = 0; i < jsonList.data.Count; i++)
            {
                //type 모양으로 GameObject 만들자.
                GameObject go = GameObject.CreatePrimitive((PrimitiveType)jsonList.data[i].type);

                //크기, 위치, 회전 랜덤하게 하자.
                go.transform.localScale = jsonList.data[i].scale;
                go.transform.position = jsonList.data[i].pos;
                go.transform.rotation = jsonList.data[i].rot;
            }
        }
    }
}
