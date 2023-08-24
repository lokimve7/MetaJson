using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image downloadImage;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGet()
    {
        //todos
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.GET, "/todos", (DownloadHandler downloadHandler) => {
            print("OnReceiveGet : " + downloadHandler.text);
        });

        //info.Set(RequestType.GET, "/todos", OnReceiveGet);

        //info 의 정보로 요청을 보내자
        HttpManager.Get().SendRequest(info);
    }

    void OnReceiveGet(DownloadHandler downloadHandler)
    {
        print("OnReceiveGet : " + downloadHandler.text);
    }

    public List<CommentInfo> comments;
    public void OnClickComment()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.GET, "/comments", (DownloadHandler downloadHandler) => {
            print("코멘트 리스트 : " + downloadHandler.text);

            string jsonData = "{\"data\" : " + downloadHandler.text + "}";

            //응답 받은 jsonData 를 변수에 parsing 
            JsonList<CommentInfo> commentList = JsonUtility.FromJson<JsonList<CommentInfo>>(jsonData);

            comments = commentList.data;
        });

        //요청
        HttpManager.Get().SendRequest(info);
    }

    public void PostTest()
    {
        HttpInfo info = new HttpInfo();

        info.Set(RequestType.POST, "/sign_up", (DownloadHandler downloadHandler) => { 
            //Post 데이터 전송했을 때 서버로부터 응답 옵니다~
        });

        SignUpInfo signUpInfo = new SignUpInfo();
        signUpInfo.userName = "김현진";
        signUpInfo.age = 25;
        signUpInfo.birthday = "830410";

        info.body = JsonUtility.ToJson(signUpInfo);

        HttpManager.Get().SendRequest(info);
    }

    public void OnClickDownloadImage()
    {
        HttpInfo info = new HttpInfo();
        info.Set(
            RequestType.TEXTURE, 
            "https://via.placeholder.com/150/92c952", 
            OnCompleteDownloadTexture,
            false);

        HttpManager.Get().SendRequest(info);
    }

    void OnCompleteDownloadTexture(DownloadHandler downloadHandler)
    {
        //다운로드된 Image 데이터를 Sprite 로 만든다.
        // Texture2D --> Sprite
        Texture2D texture = ((DownloadHandlerTexture)downloadHandler).texture;
        downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
