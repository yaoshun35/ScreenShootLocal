using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


// this behavior capture game window, and save it in local floder. Named by index number autoly..
public class photo_shooter : MonoBehaviour
{

    //a simple text to show debug info;
    public Text _text;


    //hide something,usually,UI elements,when capturing.
    public GameObject[] hideWhenShoot;

    //the path where we save  the photoes
    private string PhotoPath;

    // name by index number
    public string PhotoName;

    //shoot sound and count sound
    public AudioSource _as;

    public AudioClip[] auClip;

    // shoot effect
    //public Animator _an;


    void Start()
    {

        // full path of picture save Floder. if in editor ,return Asset path.if EXE ,return data floder path;
        PhotoPath = Application.dataPath + "/P";

        //if the floder does not exist,creat it !
        if (false == System.IO.Directory.Exists(PhotoPath))
        {

            System.IO.Directory.CreateDirectory(PhotoPath);

        }


        //create the first picture ,and name it 000;
        StartCoroutine(initiaPhotoDic());

    }


    //shoot button clicked ,start count time ,then run the shoot delay function
    public void shoot()
    {
        //hide elements which not needed when capture
        foreach (var i in hideWhenShoot)
        {
            i.transform.localScale = Vector3.zero;
        }

        //Count 3 animation show, the animator on it will player autoly
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);


        //play count sound
        _as.clip = auClip[2];
        _as.Play();


        StartCoroutine(shootDelay());

    }


    //we have hide things not needed ,start shoot
    IEnumerator shootDelay() {

        //wait 3s 
        yield return new WaitForSeconds(3.0f);

        //shoot sound,shoot effect
        _as.clip = auClip[0];
        _as.Play();
       // _an.Play("n");

        //turn off count3 canvas
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);


        //progress of picture:1.read to texture2d 2.encode to jpg byte
        Texture2D Te = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        //Texture2D为内存中一种图片纹理格式
        //位置1,2的两个数字是指定了纹理的宽高。
        Te.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0, true);
        //读取屏幕像素
        Te.Apply();
        //保存对Texture2D的更改
        byte[] bytes = Te.EncodeToJPG();
        //将Texture2D转换为PNG格式储存在 bytes这个变量中


        // to get the name of lastest name of picture
        PhotoName  = LastPhotoName();

        File.WriteAllBytes(PhotoPath + PhotoName, bytes);


       // File.WriteAllBytes(PhotoPath + PhotoName, bytes);


        Debug.Log(Application.dataPath);

        
        //shoot finished, show those elements
        foreach (var i in hideWhenShoot)
        {
            i.transform.localScale = Vector3.one;
        }
    }


    //basing existed files name , return the newest name 
    public string LastPhotoName() {

        string _name;

        //new directory class ,feed it photo path
        DirectoryInfo photoDic = new DirectoryInfo(PhotoPath);

        //get all files in this dic,and save it in a group
        
        FileInfo[] photoFiles = photoDic.GetFiles();

        //queued by index name , so we can get the last one's . split it by dot,and return it's number
        _name = (photoFiles[photoFiles.Length-1].Name.Split('.')[0]);

        //forece convert to int
        int index = Convert.ToInt32(_name);

        // add one ,because it will be the next name
        index++;

        //if 6,convert it as"/006.png"
        if (index < 10)
        {


            _name = "/00" + index.ToString() + ".png";


        }
        else if (index < 100)
        {
            _name = "/0" + index.ToString() + ".png";

        }
        //if 116,convert it as"/116.png"
        else
        {

            _name ="/"+index.ToString() + ".png";


        }

        //debug the name
        _text.text = _name;
        return _name;

        

    }


    // call it in start FUnction , create a photo named 000.
    IEnumerator initiaPhotoDic() {

        yield return null;

        _text.text ="do ienumerator";


        Texture2D Tea = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

        Tea.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0, true);

        Tea.Apply();

        byte[] bytes = Tea.EncodeToJPG();

        File.WriteAllBytes(PhotoPath + "/000.png", bytes);


    }


}




