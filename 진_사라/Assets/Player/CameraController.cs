using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 싱글 톤
    public static CameraController Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 카메라 위치 제한
    public Transform target;

    Vector3 velocity = Vector3.zero;

    public float smoothTime = .15f;

    public bool YMaxEnabled = false;
    public float YMaxValue = 0;
    public bool YMinEnabled = false;
    public float YMinValue = 0;

    public bool XMaxEnabled = false;
    public float XMaxValue = 0;
    public bool XMinEnabled = false;
    public float XMinValue = 0;

    // 카메라 배경 색 변경
    public Camera _camera;
    public SpriteRenderer bagColor;

    public float FadeTime = 2f; // Fade효과 재생시간
    public float start;
    public float end;
    public float time = 0f;
    public bool isPlaying = false;

    void Start()
    {
        _camera = GetComponent<Camera>();
        bagColor = GameObject.FindGameObjectWithTag("BackGround").GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector3 targetPos = target.position;


        if (YMinEnabled && YMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, YMaxValue);
        else if (YMinEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, target.position.y);
        else if (YMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, target.position.y, YMaxValue);


        if (XMinEnabled && XMaxEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, XMinValue, XMaxValue);
        else if (XMinEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, XMinValue, target.position.x);
        else if (XMaxEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, target.position.x, XMaxValue);

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    public void OutStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        start = 1f;
        end = 0f;
        StartCoroutine("fadeoutplay");    //코루틴 실행
    }

    IEnumerator fadeoutplay()
    {
        isPlaying = true;

        Color fadecolor = bagColor.color;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            bagColor.color = fadecolor;
            yield return null;
        }
        Destroy(bagColor.gameObject);
        isPlaying = false;

    }
}
//https://usroom.tistory.com/entry/%EC%B9%B4%EB%A9%94%EB%9D%BC-%EC%9D%B4%EB%8F%99-%EC%98%81%EC%97%AD-%EC%A0%9C%ED%95%9C%EC%9D%84-%EC%9C%84%ED%95%9C-%EB%B0%A9%EB%B2%954%EA%B0%9C-%ED%8F%AC%EC%A7%80%EC%85%98
