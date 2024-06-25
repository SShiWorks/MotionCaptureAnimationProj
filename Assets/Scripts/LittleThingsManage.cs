using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using static MovementController;
using Random = UnityEngine.Random;


//[ExecuteInEditMode]
public class LittleThingsManage : MonoBehaviour
{
    public static LittleThingsManage API;
    public List<GameObjectController> harts;
    public List<GameObjectController> hartsOutSide;
    public List<GameObjectController> hartsLittle;
    public List<GameObjectController> hartInLine;
    public List<GameObjectController> lightPanel;
    public GameObject[] hartLines;
    private List<GameObjectController> _harts;
    [Tooltip("地板上的大心心和对应描边的颜色组")] public Color[] colors1;

    private void Start()
    {
        API = this;
        StartSetValue();
        HartLineSetStartValue();
        SetHartLineColor();
        HartMoveSetStart();
        HartMoveSetLittleStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SetColor(setColor: colorsSets[0]);
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SetColor(colorsSets[1]);
        }
    }


    #region 颜色预设

    public Material m_Floor;
    public Material m_Vol;
    public Material m_VolPoint;
    public Material m_LampStrip;
    public Light m_PointLight;
    public Material[] m_UltraHart;
    public List<ColorsSet> colorsSets;

    public void SetColor(ColorsSet setColor)
    {
        m_Floor.SetColor("_BaseColor", Color.white);
        m_Floor.SetColor("_EmissionColor", setColor.color_Floor);
        colors.Clear();
        colors = setColor.color_Revolving.ToList();
        for (var i = 0; i < m_UltraHart.Length; i++)
        {
            m_UltraHart[i].SetColor("_BaseColor", Color.white);
            m_UltraHart[i].SetColor("_EmissionColor", setColor.color_Ultra[i]);
        }

        colors1 = setColor.color_FloorHart.ToArray();
        m_LampStrip.SetColor("_BaseColor",Color.white);
        m_LampStrip.SetColor("_EmissionColor", setColor.color_LampStrip);
        m_Vol.SetColor("_EmissionColor", setColor.color_Vol);
        m_VolPoint.SetColor("_EmissionColor", setColor.color_VolPoint);
        colors.Reverse();
        m_PointLight.color = setColor.color_PointLight;
    }

    #endregion

    #region 大星星

    private void HartMoveSetStart()
    {
        for (var i = 0; i < harts.Count; i++)
        {
            StartCoroutine(HartsMovement(harts[i], hartsOutSide[i]));
        }
    }

    private IEnumerator HartsMovement(GameObjectController hart, GameObjectController hartOutside)
    {
        while (true)
        {
            if (MovementMaxInSixAxisAble(hart.objectName.transform, hart._selfTransform, hart.maxDistance, "zx"))
            {
                hart.objectName.transform.position = hart._selfTransform.position;
                hartOutside.objectName.transform.position = hartOutside._selfTransform.position;
            }

            if (MovementMaxInSixAxisAble(hartOutside.objectName.transform, hartOutside._selfTransform,
                    hartOutside.maxDistance, "zx"))
            {
                hart.objectName.transform.position = hart._selfTransform.position;
                hartOutside.objectName.transform.position = hartOutside._selfTransform.position;
            }

            //随机一个额移动状态
            var moveStates = (DirectMoveState[])Enum.GetValues(typeof(DirectMoveState));
            var randomIndex = Random.Range(2, moveStates.Length);
            var randomIndex1 = Random.Range(0, colors1.Length);
            hart.moveState = moveStates[randomIndex];
            hartOutside.moveState = moveStates[randomIndex];
            Move(hart.objectName.transform, hart.moveState, hart.speed);
            Move(hartOutside.objectName.transform, hartOutside.moveState, hartOutside.speed * 5f);
            var hartMesh = hart.objectName.GetComponent<MeshRenderer>();
            var hartMeshOutside = hartOutside.objectName.GetComponent<MeshRenderer>();
            hartMesh.sharedMaterial.SetColor("_BaseColor", colors1[randomIndex1]);
            hartMeshOutside.sharedMaterial.SetColor("_EmissionColor", colors1[randomIndex1]);
            yield return new WaitForSeconds(hart.maxDistance / hart.speed);
        }

        yield break;
    }

    #endregion

    #region 小心心

    private void HartMoveSetLittleStart()
    {
        foreach (var hartLittle in hartsLittle)
        {
            var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.EnableKeyword("_EMISSION");
            StartCoroutine(HartsLittleMovement(hartLittle, material));
        }
    }

    private IEnumerator HartsLittleMovement(GameObjectController hart, Material mat)
    {
        while (true)
        {
            if (MovementMaxInSixAxisAble(hart.objectName.transform, hart._selfTransform, hart.maxDistance, "xyz"))
            {
                hart.objectName.transform.position = hart._selfTransform.position;
                hart.objectName.transform.eulerAngles = hart._selfTransform.eulerAngles;
                hart.objectName.transform.localScale = hart._selfTransform.localScale;
            }


            //随机一个额移动状态
            var moveStates = (DirectMoveState[])Enum.GetValues(typeof(DirectMoveState));
            var randomIndex = Random.Range(0, moveStates.Length);
            var randomIndex1 = Random.Range(0, colors1.Length);
            hart.moveState = moveStates[randomIndex];
            hart.objectName.transform.eulerAngles =
                new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            hart.objectName.transform.localScale = new Vector3(Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f),
                Random.Range(0.8f, 1.3f));
            Move(hart.objectName.transform, hart.moveState, hart.speed);
            var hartMesh = hart.objectName.GetComponent<MeshRenderer>();
            mat.SetColor("_BaseColor", colors1[randomIndex1]);
            mat.SetColor("_EmissionColor", colors1[randomIndex1]);
            hartMesh.sharedMaterial = mat;
            yield return new WaitForSeconds(hart.maxDistance / hart.speed);
        }

        yield break;
    }

    #endregion

    #region RetSetAnd初始化

    [SerializeField] private GameObject randomHartFather;

    /// <summary>
    /// 随机小心心
    /// </summary>
    [ContextMenu("SetRandomLittleHarts")]
    private void SetLittleHarts()
    {
        hartsLittle.Clear();
        for (var i = 0; i < randomHartFather.transform.childCount; i++)
        {
            //按顺序重命名子物体
            randomHartFather.transform.GetChild(i).name = "LittleHart" + i;
            var hart = new GameObjectController
            {
                objectName = randomHartFather.transform.GetChild(i).gameObject,
                speed = 0.2f,
                maxDistance = 0.2f,
            };
            hart.Start();
            hartsLittle.Add(hart);
        }
    }

    //初始化
    private void StartSetValue()
    {
        StartSetValueForList(harts);
        StartSetValueForList(hartsOutSide);
        StartSetValueForList(hartsLittle);
        StartSetValueForList(hartInLine);
        StartSetValueForList(lightPanel);
    }


    /// <summary>
    /// 初始化几列小爱心和颜色设定
    /// </summary>
    [ContextMenu("SetHartLineStartValue")]
    private void HartLineSetStartValue()
    {
        _harts = new List<GameObjectController>();
        foreach (var hartline in hartLines)
        {
            var hartCount = hartline.transform.childCount;
            for (var i = 0; i < hartCount; i++)
            {
                var hart = new GameObjectController
                {
                    objectName = hartline.transform.GetChild(i).gameObject,
                    speed = 1.0f,
                };
                hart.Start();
                _harts.Add(hart);
            }
        }
    }

    [Tooltip("设置的颜色")] [HideInInspector] public List<Color> colors;

    #endregion

    #region 几列小星星

    private void SetHartLineColor()
    {
        var mat0 = new List<Material>();
        var mat1 = new List<Material>();
        SetColor(colorsSets[0]);
        for (var i = 0; i < 10; i++)
        {
            var mat00 = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            var mat11 = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat0.Add(mat00);
            mat1.Add(mat11);
        }

        StartCoroutine(HartLineRevolvingLanternCoroutine(mat0, mat1));
        return;
    }

    /// <summary>
    /// 跑马灯
    /// </summary>
    /// <param name="mat0"></param>
    /// <param name="mat1"></param>
    /// <returns></returns>
    private IEnumerator HartLineRevolvingLanternCoroutine(IReadOnlyList<Material> mat0, IReadOnlyList<Material> mat1)
    {
        var indexHart = 0;
        while (true)
        {
            for (var indexRe = 0; indexRe < 10; indexRe++)
            {
                for (var i = 0; i < 6; i++)
                {
                    var hartIndex = 10 * i + ((indexHart + indexRe) % 10);
                    var hart = _harts[hartIndex].objectName.GetComponent<MeshRenderer>();
                    var materials = hart.sharedMaterials;

                    materials[0] = mat0[indexRe];
                    materials[1] = mat1[indexRe];
                    SetHartLineMate(materials[0], indexRe);
                    SetHartLineMate(materials[1], indexRe);
                    hart.sharedMaterials = materials;
                }
            }

            yield return new WaitForSeconds(0.6f);
            indexHart = (indexHart + 1) % 10; //0..10
        }

        yield break;

        void SetHartLineMate(Material mat, int index)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", colors[index]);
            mat.SetColor("_BaseColor", colors[index]);
        }
    }

    #endregion
}