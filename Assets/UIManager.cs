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

        //info �� ������ ��û�� ������
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
            print("�ڸ�Ʈ ����Ʈ : " + downloadHandler.text);

            string jsonData = "{\"data\" : " + downloadHandler.text + "}";

            //���� ���� jsonData �� ������ parsing 
            JsonList<CommentInfo> commentList = JsonUtility.FromJson<JsonList<CommentInfo>>(jsonData);

            comments = commentList.data;
        });

        //��û
        HttpManager.Get().SendRequest(info);
    }

    public void PostTest()
    {
        HttpInfo info = new HttpInfo();

        info.Set(RequestType.POST, "/sign_up", (DownloadHandler downloadHandler) => { 
            //Post ������ �������� �� �����κ��� ���� �ɴϴ�~
        });

        SignUpInfo signUpInfo = new SignUpInfo();
        signUpInfo.userName = "������";
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
        //�ٿ�ε�� Image �����͸� Sprite �� �����.
        // Texture2D --> Sprite
        Texture2D texture = ((DownloadHandlerTexture)downloadHandler).texture;
        downloadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
