using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static MovementController;

public class SceneModManage : MonoBehaviour
{
    public Vector3 cameraPosOutside;
    public Vector3 directCameraPos;
    public GameObject[] vol;
    [HideInInspector] public GameObject[] volR;
    [HideInInspector] public GameObject[] volL;
    [HideInInspector] public GameObject[] volRPoint;
    [HideInInspector] public GameObject[] volLPoint;
    [HideInInspector] public GameObject[] volRMat;
    [HideInInspector] public GameObject[] volLMat;

    public AudioSource audioSource;
    private float[] _audioDataL;

    private float[] _audioDataR;
    public GameObject movementCamera;
    public Camera[] facCamera;
    public GameObject[] renderTarget;
    public GameObject[] characters;

    private float indexC;

    private const string Emission = "_EMISSION";
    private const string EmissionMap = "_EmissionMap";
    private const string EmissionColor = "_EmissionColor";

    private const string URPToolLitToggle = "_UseEmission";
    private const string URPToolLitColor = "_EmissionColor";
    private const string URPToolLitMap = "_EmissionMap";
    private const string URPToolLitMulByBaseColor = "_EmissionMulByBaseColor";

    public static SceneModManage API;
    [HideInInspector] public GameObject _currentCharacter;

    // Start is called before the first frame update
    private void Start()
    {
        API = this;
        //print(vol[0]);
        // print(vol[0].transform.GetChild(0));
        // print(vol[0].transform.GetChild(0).GetChild(1).gameObject);


        VolSet();
        CameraTenderTargetSet();
        EnableCharacterHDR();
        CheckActiveCharacter();
    }

    /// <summary>两侧音量波形</summary>
    private void VolSet()
    {
        volL = new GameObject[16];
        volR = new GameObject[16];
        _audioDataL = new float[16];
        _audioDataR = new float[16];
        volLPoint = new GameObject[16];
        volRPoint = new GameObject[16];
        volLMat = new GameObject[16];
        volRMat = new GameObject[16];
        for (var i = 0; i <= 15; i++)

        {
            volL[i] = vol[0].transform.GetChild(i).GetChild(1).gameObject; //获取左组件的i号子物体的1号子物体
            volLPoint[i] = vol[0].transform.GetChild(i).GetChild(0).gameObject; //获取左组件的i号子物体的0号子物体
            volLMat[i] = vol[0].transform.GetChild(i).GetChild(1).GetChild(0).gameObject; //获取左组件的i号子物体的1号子物体的0号子物体
        }

        for (var i = 0; i <= 15; i++)
        {
            volR[i] = vol[1].transform.GetChild(i).GetChild(1).gameObject; //获取右组件的i号子物体的1号子物体
            volRPoint[i] = vol[1].transform.GetChild(i).GetChild(0).gameObject; //获取右组件的i号子物体的0号子物体
            volRMat[i] = vol[1].transform.GetChild(i).GetChild(1).GetChild(0).gameObject; //获取右组件的i号子物体的1号子物体的0号子物体
        }
    }

    // Update is called once per frame
    private void Update()
    {
        AudioViewTwoSide();
        CameraFacTestRoundWithMainCharacter(movementCamera);
        CameraFacDirectWithMainCharacter(facCamera[0].gameObject);
    }

    /// <summary>
    /// 两侧音量波形
    /// </summary>
    private void AudioViewTwoSide()
    {
        audioSource.GetOutputData(_audioDataL, 0);
        audioSource.GetOutputData(_audioDataR, 1);
        for (var i = 0; i <= 15; i++)
        {
            var scaleL = volL[i].transform.localScale;
            scaleL.x = Mathf.Abs(_audioDataL[i] * 5);
            volL[i].transform.localScale = scaleL;

            var scaleR = volR[i].transform.localScale;
            scaleR.x = Mathf.Abs(_audioDataR[i] * 5);
            volR[i].transform.localScale = scaleR;
        }
    }

    /// <summary>
    /// 检测当前角色，用于相机跟随，当且仅当一个角色为激活状态时
    /// </summary>
    public void CheckActiveCharacter()
    {
        foreach (var character in characters)
        {
            if (character.activeSelf)
            {
                _currentCharacter = character;
            }
        }
    }

    /// <summary>相机围绕跟随主角</summary>
    public void CameraFacTestRoundWithMainCharacter(GameObject mCamera)
    {
        var cameraPos = /* characters[0].transform.position + */cameraPosOutside;
        var rotation = _currentCharacter.transform.rotation.eulerAngles;
        var cameraRot = Quaternion.Euler(rotation.x, rotation.y + 180.0f, rotation.z);
        mCamera.transform.parent = _currentCharacter.transform;
        mCamera.transform.rotation = cameraRot;
        mCamera.transform.localPosition = cameraPos;
    }
    /// <summary>相机围绕跟随主角,额外向量参数</summary>
    public void CameraFacTestRoundWithMainCharacter(GameObject mCamera,Vector3 camAddPos,Vector3 camAddRot)
    {
        var cameraPos = /* characters[0].transform.position + */cameraPosOutside+camAddPos;
        var rotation = _currentCharacter.transform.rotation.eulerAngles;
        var cameraRot = Quaternion.Euler(rotation.x, rotation.y + 180.0f, rotation.z);
        mCamera.transform.parent = _currentCharacter.transform;
        mCamera.transform.rotation = cameraRot;
        mCamera.transform.localPosition = cameraPos;
    }


    /// <summary>相机直接跟随主角</summary><param name="mCamera">跟踪的相机</param>
    public void CameraFacDirectWithMainCharacter(GameObject mCamera, GameObject charGameObject)
    {
        var cameraPos = charGameObject.transform.position + cameraPosOutside - directCameraPos;
        mCamera.transform.position = cameraPos;
        mCamera.transform.eulerAngles = new Vector3(0, 180, 0);
    }
    /// <summary>相机直接跟随主角</summary><param name="mCamera">跟踪的相机</param>
    public void CameraFacDirectWithMainCharacter(GameObject mCamera)
    {
        var cameraPos = _currentCharacter.transform.position + cameraPosOutside - directCameraPos;
        mCamera.transform.position = cameraPos;
        mCamera.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    /// <summary>相机渲染目标设置</summary>
    private void CameraTenderTargetSet()
    {
        var renderTexture = new RenderTexture(3840, 2160, 24);
        var renderTextureCopy = new RenderTexture(1280, 1280, 24);
        facCamera[0].targetTexture = renderTexture;
        facCamera[1].targetTexture = renderTextureCopy;
        foreach (var target in renderTarget)
        {
            if (Math.Abs(target.transform.localScale.x / target.transform.localScale.y) - 1 == 0)
            {
                EnableAndDisplayOnPanel(target, renderTextureCopy, 0.8f);
            }
            else
            {
                EnableAndDisplayOnPanel(target, renderTexture, 0.65f);
            }
        }
    }

    /// <summary>
    /// 开启和设传入通用材质的hdr颜色发自发光
    /// </summary>
    /// <param name="gM"></param>
    /// <param name="rT"></param>
    /// <param name="color"></param>
    private static void EnableAndDisplayOnPanel(GameObject gM, Texture rT, float color)
    {
        TargetGet(gM).mainTexture = rT;
        TargetGet(gM).EnableKeyword(Emission);
        TargetGet(gM).SetTexture(EmissionMap, rT);
        TargetGet(gM).SetColor(EmissionColor, new Color(color, color, color));
    }

    /// <summary>开启角色的hdr</summary>
    private void EnableCharacterHDR()
    {
        foreach (var character in characters)
        {
            var skinnedMeshRendererCount = character.GetComponentsInChildren<SkinnedMeshRenderer>().Length;
            //print(skinnedMeshRendererCount);
            for (var i = 0; i <= skinnedMeshRendererCount; i++)
            {
                var gameObj = character.transform.GetChild(i).gameObject;
                if (!gameObj.name.StartsWith("00")) continue;
                var gameObjMat = gameObj.GetComponent<SkinnedMeshRenderer>().material;
                var gMT = gameObjMat.mainTexture;
                gameObjMat.SetFloat(URPToolLitToggle, 0.8f);
                gameObjMat.SetTexture(URPToolLitMap, gMT);
                //如果名字中含有“头发”则发光颜色为白色，否则为灰色
                if (gameObj.name.Contains("头发"))
                {
                    gameObjMat.SetColor(URPToolLitColor, new Color(0.55f, 0.55f, 0.55f));
                }
                else
                {
                    gameObjMat.SetColor(URPToolLitColor, new Color(0.5f, 0.5f, 0.5f));
                }
            }
        }
    }
}