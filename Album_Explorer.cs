using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;


//1.if too many photos in floder ,we need to clear it 
//2.OPen and show pictures on the ALbum

public class Album_Explorer : MonoBehaviour {

    //ugui image element
    public Image _AlbumPage;

    //where the photo saved
    private string PhotoPath;
    // turn on/off the album window;
    private bool AlbumState=false;

    //hide something when album opened
    public GameObject[] hideBUttons;

    //which photo read
    public string PhotoName;

    //total number of photoes 
    public int maxIndex;
   
    //temp index of Page Changer
    public int tempIndex;

    public Text tempIndexText;

    //audio source play page sound
    public AudioSource _as;

    public AudioClip[] auClip;




//set the floder path
    private void Start()
    {

        PhotoPath = Application.dataPath + "/P";


    }


//show album,hide buttons by  set scale as zero;
    public void showPicturePannel() {


        if (AlbumState ==false) {

            _as.clip = auClip[1];
            _as.Play();

            AlbumState = true;

            _AlbumPage.gameObject.SetActive(true);

            foreach (var i in hideBUttons)
            {
                i.transform.localScale = Vector3.zero;
            }

            //call this function ,to show picture ,everytime album opended.

            refreshImage();


        }
        else if (AlbumState == true)
        {

            AlbumState = false;

            _AlbumPage.gameObject.SetActive(false);

            foreach (var i in hideBUttons)
            {
                i.transform.localScale = Vector3.one;
            }

        }



    }

    public void refreshImage() {

        PhotoName = LastPhotoName();
        tempIndexText.text = "最新照片，点击按钮翻页";

        StartCoroutine(GetImage());
        
        
     }


    //we need a function ,to get  the lasted one picture in folder

    public string LastPhotoName()
    {

        string _name;


        DirectoryInfo photoDic = new DirectoryInfo(PhotoPath);


        FileInfo[] photoFiles = photoDic.GetFiles();

        _name = (photoFiles[photoFiles.Length - 1].Name.Split('.')[0]);


        maxIndex = Convert.ToInt32(_name);

        tempIndex = maxIndex;
        //index++;

        if (maxIndex < 10)
        {


            _name = "/00" + maxIndex.ToString() + ".png";


        }
        else if (maxIndex < 100)
        {
            _name = "/0" + maxIndex.ToString() + ".png";

        }
        else
        {

            _name = "/" + maxIndex.ToString() + ".png";


        }

        
        return _name;



    }



    // load image from ssd .and show on UI

    IEnumerator GetImage()

    {

        WWW www = new WWW(PhotoPath+PhotoName);

        yield return www;

        Debug.Log(PhotoPath + PhotoName);

        //if path right and www download without error
        if (www != null && string.IsNullOrEmpty(www.error))

        {
            //same as capture, we need texture2d to save pixel info;

            Texture2D texture = new Texture2D(www.texture.width, www.texture.height);

            texture.SetPixels(www.texture.GetPixels());

            texture.Apply(true);

            texture.filterMode = FilterMode.Trilinear;

            // create a sprite, it can be used as texture of album page
            Sprite tempSprite = Sprite.Create(texture, new Rect(0, 0, 1920, 1080), new Vector2(0f, 0f));

            //set the sprite
            _AlbumPage.sprite = tempSprite;

            Debug.Log("www success");
        }
        else {

            Debug.Log("www failed");
            Debug.Log(www.error);


        }

    }


    // this function for local  pageChanger    function button

    //paraments is 1 or-1 feed in .
    public void ChangeAlbumButton(int value) {

        _as.clip = auClip[1];
        _as.Play();

        tempIndex += value;


        //tempIndex is for Album Changer, to tell the Refresh Function ,which picture we want to read.
        string tempPhotoName;

        if (tempIndex>maxIndex)
        {

            tempIndex = 1;
        }
        else if (tempIndex<=0)
        {
            tempIndex = maxIndex;
        }



        if (tempIndex < 10)
        {


            tempPhotoName = "/00" + tempIndex.ToString() + ".png";


        }
        else if (tempIndex < 100)
        {
            tempPhotoName = "/0" + tempIndex.ToString() + ".png";

        }
        else
        {

            tempPhotoName = "/" + tempIndex.ToString() + ".png";


        }

        //set PhotoName.

        PhotoName = tempPhotoName;

          tempIndexText.text= string.Format("第{0}张，总计{1}张",tempIndex,maxIndex);

     
        //Change the Photo
        StartCoroutine(GetImage());


    }


}
