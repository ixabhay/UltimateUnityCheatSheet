using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SSUpload : MonoBehaviour
{


    public RawImage previewImage;

    private RenderTexture renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void UploadScreenCapture()
    {
        // var txtures = ScreenCapture.CaptureScreenshotAsTexture();
        renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
        AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGBA32, ReadbackCompleted);



        previewImage.texture = renderTexture;


        StartCoroutine(UploadMultipleFiles());

    }

    void ReadbackCompleted(AsyncGPUReadbackRequest request)
    {
        // Render texture no longer needed, it has been read back.
        DestroyImmediate(renderTexture);

        using (var imageBytes = request.GetData<byte>())
        {
            // do something with the pixel data.
        }
    }

    public static void FlipTextureVertically(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        var newPixels = new Color[originalPixels.Length];

        var width = original.width;
        var rows = original.height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
    }

    public static void FlipTextureHorizontally(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        var newPixels = new Color[originalPixels.Length];

        var width = original.width;
        var rows = original.height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[(width - x - 1) + y * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
    }

    public IEnumerator UploadMultipleFiles()
    {
        //  rawImage.texture = texture2Ds[0];
        //  texture2D = texture2Ds;
        WWWForm form = new WWWForm();
        //    print("files length is " + texture2Ds.Count);
        //    print("files length is " + spritesNames.Count);
        /*for (int i = 0; i < sprites.Length; i++)
        {*/
        // print("i = " + i);
        byte[] imageData = toTexture2D(renderTexture).EncodeToPNG();





     //   using (MemoryStream ms = new MemoryStream())
     //   {
     //       BinaryFormatter bf = new BinaryFormatter();
     //       bf.Serialize(ms, tex);
     //       imageData = ms.ToArray();
     //   }

        string enc = Convert.ToBase64String(imageData);





        print("file size is " + imageData.Length);
        //  string name = spritesNames[0];
        // print("name is " + name);



        //form.AddField("UserId", 42);


        // form.AddBinaryData("file", imageData, "test.png", "image/png");


        //form.AddField("file", enc);





        form.AddBinaryData("file", imageData, "test.png", "image/png");


        UnityWebRequest www = UnityWebRequest.Post("localhost:3001/upload", form);
        print("UploadMultipleFiles 2");
        yield return www.SendWebRequest();
        print("UploadMultipleFiles 3");
        if (www.isHttpError || www.isNetworkError)
            Debug.Log(www.error);
        else
            Debug.Log("Uploaded " + " files Successfully");
        //}
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);




        tex.Apply();


        FlipTextureVertically(tex);



        return tex;
    }



}
