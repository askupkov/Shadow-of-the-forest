using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public static Action<float, float, float> cameraShake;
    public static Action<float> changeCameraSizeEvent;
    public static Action<Transform> changeFollowTargetEvent;
    [HideInInspector] public CinemachineFramingTransposer transposer;
    private CinemachineBasicMultiChannelPerlin channelPerlin;
    private CinemachineVirtualCamera virtualCamera;
    private float camSize;
    private Transform player;

    private void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cameraShake += shake;
        changeCameraSizeEvent += changeCameraSize;
        changeFollowTargetEvent += changeFollowTargent;
    }
    public float leftOfset, rightOfset;
    private void OnDisable()
    {
        cameraShake -= shake;
        changeCameraSizeEvent -= changeCameraSize;
        changeFollowTargetEvent -= changeFollowTargent;
    }

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        virtualCamera.m_Follow = player;
    }
    private void Update()
    {

    }

    void shake(float strength, float time, float fadeTime)
    {
        StartCoroutine(shakeCam(strength, time, fadeTime));
    }
    void changeCameraSize(float newSize)
    {
        StopCoroutine(changeSize(newSize));
        camSize = virtualCamera.m_Lens.OrthographicSize;
        StartCoroutine(changeSize(newSize));
    }
    void changeFollowTargent(Transform followObject)
    {
        if (followObject != null)
        {
            virtualCamera.m_Follow = followObject;
        }
    }


    public void FollowNull()
    {
        virtualCamera.m_Follow = null;
    }

    private IEnumerator shakeCam(float strength, float time, float fadeTime)
    {
        float OriginStrength = strength;
        channelPerlin.m_AmplitudeGain = strength;
        yield return new WaitForSeconds(time);
        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            strength -= Time.deltaTime * OriginStrength / fadeTime;
            channelPerlin.m_AmplitudeGain = strength;
        }
        channelPerlin.m_AmplitudeGain = 0;
    }
    private IEnumerator changeSize(float newSize)
    {
        if (virtualCamera.m_Lens.OrthographicSize == newSize) yield break;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(camSize, newSize, EaseInOut(i));
            yield return null;
        }

    }
    float EaseInOut(float x)
    {
        return x < 0.5 ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
    }
}

