using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class WebCamPhotoCamera : MonoBehaviour
{
    WebCamTexture webCamTexture;
    int i = 0, j = 0, p = 0;
    const int maxFish = 10; //the max fish creatable count
    public Texture2D[] tex = new Texture2D[maxFish];
    GameObject[] plane = new GameObject[maxFish];
    string photoPath;
    public GameObject plane_prefab;

    //Texture2D targetTexture;

    public Rect sourceRect;

    

    // Start is called before the first frame update

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int tmp = 0; tmp < devices.Length; tmp++)
        {
            Debug.Log(devices[tmp].name);
        }//デバイスの読み込み

        //webCamTexture = new WebCamTexture(devices[0].name);
        //webCamTexture = new WebCamTexture(devices[2].name, 860, 540, 30);
        webCamTexture = new WebCamTexture(devices[0].name, 860, 645, 30);
        GetComponent<Renderer> ().material.mainTexture = webCamTexture; //Add mesh Renderer to the GameObject to Which this script is attached to
        webCamTexture.Play();

        //GameObject plane_prefab = Resources.Load<GameObject>("PhotoOBJ");//prefab load
    }

    public IEnumerator TakePhoto(int i) //start this coroutine on some button click
    {
        //NOTE - you almost certainly hace to do this here:
        yield return new WaitForEndOfFrame();

        //it's a rare case where the Unity doco is pretty clear,
        //http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        //be sure to scroll down to the SECOND long example on that doco page

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels(0, 0, photo.width, photo.height));
        
        photo.Apply();

        int x = Mathf.FloorToInt(sourceRect.x);
        int y = Mathf.FloorToInt(sourceRect.y);
        int width = Mathf.FloorToInt(sourceRect.width);
        int height = Mathf.FloorToInt(sourceRect.height);
        Debug.Log(x);
        Debug.Log(y);
        Debug.Log(width);
        Debug.Log(height);
        Color[] pix = photo.GetPixels(x, y, width, height);
        Texture2D destTex = new Texture2D(width, height);
        destTex.SetPixels(pix);
        destTex.Apply();

        //Encode to a PNG
        byte[] bytes = destTex.EncodeToPNG();

        //Write out the PNG. of course you have to substitute your_path for something sensible
        //File.WriteAllBytes("Assets/Resources/photo" + i +".png", bytes);
        //File.ReadAllBytes("Assets/Resources/photo" + i + ".png");
        //AssetDatabase.ImportAsset("Assets/Resources/photo" + i + ".png");

        //File.WriteAllBytes("C:/Users/t/Documents/Phototest/photo" + i + ".png", bytes);
        File.WriteAllBytes( Application.dataPath +  "/Photos/photo" + i + ".png", bytes);
        //File.ReadAllBytes("C:/Users/t/Documents/Phototest/photo" + i + ".png");
        File.ReadAllBytes( Application.dataPath + "/Photos/photo" + i + ".png");


        //1/11


        //File.WriteAllBytes("C:/Users/TLab2/Documents/Phototest/photo" + i + ".png", bytes);
        // File.ReadAllBytes("C:/Users/TLab2/Documents/Phototest/photo" + i + ".png");
        //File.WriteAllBytes("Assets/Photo/photo" + i + ".png", bytes);
        //AssetDatabase.ImportAsset("Assets/Photo/photo" + i + ".png");

    }
    
    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (i >= maxFish)
            {
                i = 0;
            }

            StartCoroutine(TakePhoto(i));

            i++;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (j >= maxFish)
            {
                j = 0;
                
            }
            if (plane[j] != null)
            {
                Destroy(plane[j]);
            }

            //GameObject plane_prefab = Resources.Load<GameObject>("sakanaFish");//prefab load

            //photoPath = "C:/Users/TLab2/Documents/Phototest/photo" + j + ".png";


            //photoPath = "C:/Users/t/Documents/Phototest/photo" + j + ".png";
            photoPath = Application.dataPath + "/Photos/photo" + j + ".png";
            
            
            //Debug.Log(Application.dataPath);


            //Debug.Log(j);
            //Texture2D
            byte[] readBinary = ReadPngFile(photoPath);

            Texture2D texture = new Texture2D(480, 480);
            texture.LoadImage(readBinary);
            tex[j] = texture;

            //tex[j] = Resources.Load<Texture>("photo" + j);
            plane[j] = Instantiate(plane_prefab);

            Transform children = plane[j].transform.GetChild(1);//子オブジェクトを入れる変数


            Renderer rend = children.GetComponent<Renderer>();
            //Renderer rend = plane[j].GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Unlit/PhotoShader"));
            rend.material.SetTexture("_MainTex", tex[j]);

            j++;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (p >= maxFish)
            {
                p = 0;
            }

            Destroy(plane[p]);

            p++;
        }
    }

    byte[] ReadPngFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

    //以下洋ナシ
    /*
    static Texture2D ResizeTexture(Texture2D srcTexture, int newWidth, int newHeight)
    {
        var resizedTexture = new Texture2D(newWidth, newHeight);


        Graphics.ConvertTexture(srcTexture, resizedTexture);
        return resizedTexture;
    }

    private static Texture2D GetResized(Texture2D texture, int width, int height)
    {
        // リサイズ後のサイズを持つRenderTextureを作成して書き込む
        var rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(texture, rt);

        // リサイズ後のサイズを持つTexture2Dを作成してRenderTextureから書き込む
        var preRT = RenderTexture.active;
        RenderTexture.active = rt;
        var ret = new Texture2D(width, height);
        ret.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        ret.Apply();
        RenderTexture.active = preRT;

        RenderTexture.ReleaseTemporary(rt);
        return ret;
    }*/

}
