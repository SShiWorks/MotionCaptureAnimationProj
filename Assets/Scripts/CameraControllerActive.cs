using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraControllerActive : MonoBehaviour
{
    public GameObject mainCamera;

    public bool isNeedController;
    public float cameraMoveSpeed = 1f;
    public GameObject mainCameraDefault;
    public static CameraControllerActive API;
    public GameObject defaultCameraPosition;

    private void Start()
    {
        API = this;
        _mainCameraTransform = mainCamera.transform;
        _mainCameraTransform.parent = mainCamera.transform.parent;
    }

    private void Update()
    {
        CameraControl();
    }

    private void CameraControl()
    {
        if (isNeedController)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                cameraMoveSpeed = 2.5f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                cameraMoveSpeed = 1.0f;
            }

            Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetKey(KeyCode.W))
            {
                mainCamera.transform.Translate(Vector3.forward * (Time.deltaTime * cameraMoveSpeed));
            }

            if (Input.GetKey(KeyCode.S))
            {
                mainCamera.transform.Translate(Vector3.back * (Time.deltaTime * cameraMoveSpeed));
            }

            if (Input.GetKey(KeyCode.A))
            {
                mainCamera.transform.Translate(Vector3.left * (Time.deltaTime * cameraMoveSpeed));
            }

            if (Input.GetKey(KeyCode.D))
            {
                mainCamera.transform.Translate(Vector3.right * (Time.deltaTime * cameraMoveSpeed));
            }
            if(Input.GetKey(KeyCode.Q))
            {
                mainCamera.transform.Translate(Vector3.down * (Time.deltaTime * cameraMoveSpeed));
            }
            if(Input.GetKey(KeyCode.E))
            {
                mainCamera.transform.Translate(Vector3.up * (Time.deltaTime * cameraMoveSpeed));
            }

            var rotation = mainCamera.transform.rotation;
            rotation = Quaternion.Euler(
                rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraMoveSpeed,
                rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraMoveSpeed, 0);
            mainCamera.transform.rotation = rotation;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isNeedController = !isNeedController;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            _isAutoRoundWithCharacter = !_isAutoRoundWithCharacter;

            if (_isAutoRoundWithCharacter) return;
            MainCameraDefaultTransform();
        }

        if (_isAutoRoundWithCharacter)
        {
            AutoRoundWithCharacter();
        }

        if (_isAutoDirectWithCharacter)
        {
            AutoDirectWithCharacter();
        }
    }

    [HideInInspector] public bool _isAutoRoundWithCharacter;
    [HideInInspector] public bool _isAutoDirectWithCharacter;
    private Transform _mainCameraTransform;

    private void AutoRoundWithCharacter()
    {
        SceneModManage.API.CameraFacTestRoundWithMainCharacter(mainCamera, new Vector3(0, 0, 1),cameraAddRot);
    }

    public Vector3 cameraAddRot = new(0, 0, 0);

    private void AutoDirectWithCharacter()
    {
        SceneModManage.API.CameraFacDirectWithMainCharacter(mainCamera);
    }

    public void SetMainCameraPosition()
    {
        mainCamera.transform.position = defaultCameraPosition.transform.position;
        mainCamera.transform.rotation = defaultCameraPosition.transform.rotation;
        mainCamera.transform.localScale = defaultCameraPosition.transform.localScale;
    }

    private void MainCameraDefaultTransform()
    {
        mainCamera.transform.position = _mainCameraTransform.position;
        mainCamera.transform.rotation = _mainCameraTransform.rotation;
        mainCamera.transform.parent = mainCameraDefault.transform;
        mainCamera.transform.localScale = _mainCameraTransform.localScale;
    }
}