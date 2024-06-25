using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class MovementController
{
    [Serializable]
    public class GameObjectController
    {
        public GameObject objectName;
        public DirectMoveState moveState;
        public RotateEnumValue rotateEnumValue;
        public float speed;
        public float maxDistance;
        [HideInInspector] public Transform _selfTransform;

        public void Start()
        {
            if (objectName != null)
                _selfTransform = objectName.transform;
        }
    }

    [Serializable]
    public class ColorsSet
    {
        public string themeName;
        [ColorUsage(true, true)] public Color color_PointLight;
        [ColorUsage(true, true)] public Color color_Vol;
        [ColorUsage(true, true)] public Color color_VolPoint;
        [ColorUsage(true, true)] public Color[] color_Ultra;
        [ColorUsage(true, true)] public Color color_Floor;
        [ColorUsage(true, true)] public Color color_LampStrip;
        [ColorUsage(true, true)] public List<Color> color_Revolving;
        [ColorUsage(true, true)] public List<Color> color_FloorHart;
    }

    public static void Move(Transform transform, DirectMoveState moveState, float speed)
    {
        switch (moveState)
        {
            case DirectMoveState.Forward:
                transform.Translate(Vector3.forward * (Time.deltaTime * speed));
                break;
            case DirectMoveState.Back:
                transform.Translate(Vector3.back * (Time.deltaTime * speed));
                break;
            case DirectMoveState.Left:
                transform.Translate(Vector3.left * (Time.deltaTime * speed));
                break;
            case DirectMoveState.Right:
                transform.Translate(Vector3.right * (Time.deltaTime * speed));
                break;
            case DirectMoveState.Up:
                transform.Translate(Vector3.up * (Time.deltaTime * speed));
                break;
            case DirectMoveState.Down:
                transform.Translate(Vector3.down * (Time.deltaTime * speed));
                break;
        }
    }

    /// <summary>
    /// 检测最大移动距离
    /// </summary>
    /// <param name="transform">实时位置</param>
    /// <param name="starPos">初始位置</param>
    /// <param name="inSetDistance">设定的最大距离</param>
    /// <param name="xyz">移动的面</param>
    /// <returns></returns>
    public static bool MovementMaxInSixAxisAble(Transform transform, Transform starPos, float inSetDistance, string xyz)
    {
        var p = transform.position;
        var pS = starPos.position;
        var x = p.x - pS.x;
        var y = p.y - pS.y;
        var z = p.z - pS.z;
        float moveDistance = new();
        switch (xyz)
        {
            case "xy":
            case "yx":
                moveDistance = x * x + y * y;
                break;
            case "zx":
            case "xz":
                moveDistance = z * z + x * x;
                break;
            case "zy":
            case "yz":
                moveDistance = y * y + z * z;
                break;
            case "xyz":
            case "xzy":
            case "yzx":
            case "yxz":
            case "zxy":
            case "zyx":
                moveDistance = x * x + y * y + z * z;
                break;
        }

        return moveDistance >= inSetDistance * inSetDistance;
    }

    /// <summary>
    /// 旋转
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="randomEnumValue"></param>
    /// <param name="speed"></param>
    public static void Rotate(Transform transform, RotateEnumValue randomEnumValue, float speed)
    {
        switch (randomEnumValue)
        {
            case RotateEnumValue.X:
                transform.Rotate(Vector3.right * (Time.deltaTime * speed));
                break;
            case RotateEnumValue.Y:
                transform.Rotate(Vector3.up * (Time.deltaTime * speed));
                break;
            case RotateEnumValue.Z:
                transform.Rotate(Vector3.forward * (Time.deltaTime * speed));
                break;
        }
    }


    public enum DirectMoveState
    {
        Forward,
        Back,
        Left,
        Right,
        Up,
        Down
    }

    public enum RotateEnumValue
    {
        X,
        Y,
        Z
    }

    public static void StartSetValueForList(List<GameObjectController> list)
    {
        foreach (var hart in list)
        {
            hart.Start();
        }
    }

    /// <summary>获取材质</summary><param name="gM">组件</param><returns>材质</returns>
    public static Material TargetGet(GameObject gM) => gM.GetComponent<MeshRenderer>().material;
}