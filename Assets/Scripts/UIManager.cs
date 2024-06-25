using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject[] DancePeople;
    public GameObject AllSong;
    public GameObject AllCam;
    public GameObject AllPeople;
    public GameObject AllScene;
    public GameObject Titel;
    public Color ButtonColer;
    public int NowSong = 1;
    public int NowCam = 1;
    public int NowPeople = 1;
    public int NowScene = 1;

    public Button[] songButton;
    public Button[] camButton;
    public Button[] peopleButton;

    public Button[] sceneButton;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //alt 隐藏鼠标
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = !Cursor.visible;
            if (CameraControllerActive.API.isNeedController)
            {
                CameraControllerActive.API.isNeedController = !CameraControllerActive.API.isNeedController;
            }

            
        }
    }

    public void StartPlay()
    {
        CloseAllUnfoldMenu();
        DanceController.API.DancePlay(NowSong, NowCam, NowPeople);
    }

    public void CloseUnfoldMenu(int MenuNum) //打开/关闭折叠UI
    {
        switch (MenuNum)
        {
            case 1:
                AllSong.SetActive(!AllSong.activeSelf);
                break;
            case 2:
                AllCam.SetActive(!AllCam.activeSelf);
                break;
            case 3:
                AllPeople.SetActive(!AllPeople.activeSelf);
                break;
            case 4:
                AllScene.SetActive(!AllScene.activeSelf);
                break;
        }
    }

    public void CloseAllUnfoldMenu() //关闭所有折叠UI
    {
        Titel.SetActive(false);
        AllSong.SetActive(false);
        AllCam.SetActive(false);
        AllPeople.SetActive(false);
        AllScene.SetActive(false);
    }

    public void SetSong(int songNum)
    {
        NowSong = songNum;
        SetButtonColor(songButton, NowSong - 1);
        switch (songNum)
        {
            case 1:
                RECamButtonColor();
                SetCam(songNum);
                SetPeople(1);
                //peopleButton[0].GetComponent<Image>().color = Color.red;
                SetScene(1);
                //sceneButton[0].GetComponent<Image>().color = Color.red;
                break;
            case 2:
                RECamButtonColor();
                SetCam(songNum);
                SetPeople(1);
                //peopleButton[0].GetComponent<Image>().color = Color.red;
                SetScene(1);
                //sceneButton[0].GetComponent<Image>().color = Color.red;
                break;
            case 3:
                RECamButtonColor();
                SetCam(songNum);
                SetPeople(1);
                //peopleButton[0].GetComponent<Image>().color = Color.red;
                SetScene(2);
                //sceneButton[1].GetComponent<Image>().color = Color.red;
                break;
            case 4:
                RECamButtonColor();
                SetCam(songNum);
                SetPeople(1);
                //peopleButton[0].GetComponent<Image>().color = Color.red;
                SetScene(1);
                //sceneButton[0].GetComponent<Image>().color = Color.red;
                break;
            case 5:
                RECamButtonColor();
                SetCam(songNum);
                SetPeople(1);
                //peopleButton[0].GetComponent<Image>().color = Color.red;
                SetScene(1);
                //sceneButton[0].GetComponent<Image>().color = Color.red;
                break;
            case 6:
                RECamButtonColor();
                SetCam(songNum);
                SetPeople(1);
                //peopleButton[0].GetComponent<Image>().color = Color.red;
                SetScene(1);
                //sceneButton[0].GetComponent<Image>().color = Color.red;
                break;
        }
    }

    public void SetCam(int camNum)
    {
        NowCam = camNum;
        SetButtonColor(camButton, NowCam - 1);
    }

    public void SetPeople(int peopleNum)
    {
        NowPeople = peopleNum;
        switch (peopleNum)
        {
            case 1:
                DancePeople[0].SetActive(true);
                DancePeople[1].SetActive(false);
                DancePeople[2].SetActive(false);
                DancePeople[3].SetActive(false);
                break;
            case 2:
                DancePeople[0].SetActive(false);
                DancePeople[1].SetActive(true);
                DancePeople[2].SetActive(false);
                DancePeople[3].SetActive(false);
                break;
            case 3:
                DancePeople[0].SetActive(false);
                DancePeople[1].SetActive(false);
                DancePeople[2].SetActive(true);
                DancePeople[3].SetActive(false);
                break;
            case 4:
                DancePeople[0].SetActive(false);
                DancePeople[1].SetActive(false);
                DancePeople[2].SetActive(false);
                DancePeople[3].SetActive(true);
                break;
        }

        SetButtonColor(peopleButton, NowPeople - 1);
        SceneModManage.API.CheckActiveCharacter();
    }

    public void SetScene(int sceneNum)
    {
        switch (sceneNum)
        {
            case 1:
                LittleThingsManage.API.SetColor(LittleThingsManage.API.colorsSets[0]);
                sceneButton[0].GetComponent<Image>().color = ButtonColer;
                sceneButton[1].GetComponent<Image>().color = Color.white;
                break;
            case 2:
                LittleThingsManage.API.SetColor(LittleThingsManage.API.colorsSets[1]);
                sceneButton[0].GetComponent<Image>().color = Color.white;
                sceneButton[1].GetComponent<Image>().color = ButtonColer;
                break;
        }
    }

    private void SetButtonColor(Button[] buttons, int num)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == num)
            {
                buttons[i].GetComponent<Image>().color = ButtonColer;
            }
            else
            {
                buttons[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    private void RECamButtonColor()
    {
        for (int i = 0; i < camButton.Length; i++)
        {
            camButton[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void SceneRE()
    {
        SceneManager.LoadScene(0);
    }
}