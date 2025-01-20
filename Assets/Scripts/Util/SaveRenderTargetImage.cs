using System.IO;
using UnityEngine;

/// <summary>
/// 아이템 이미지 추출을 위한 스크립트
/// </summary>
public class SaveRenderTargetImage : MonoBehaviour
{

    public RenderTexture RenderTexture;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            SaveRenderTexture();
        }
    }

    void SaveRenderTexture()
    {
        RenderTexture.active = RenderTexture;
        var tex2D = new Texture2D(RenderTexture.width, RenderTexture.height);
        tex2D.ReadPixels(new Rect(0,0, RenderTexture.width,RenderTexture.height),0,0);
        tex2D.Apply();
        var data = tex2D.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, "Resources");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        File.WriteAllBytes(Path.Combine(path, "a" + ".png"), data);

    }


}
