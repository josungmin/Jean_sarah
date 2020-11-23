using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct DialogWave
{
    public enum TextEffect
    {
        아직,
        안정해서,
        모린다
    }

    [SerializeField]
    public TextEffect textEffect;

    [SerializeField]
    public string Text;

    [SerializeField]
    public int Count;

    [SerializeField]
    public Color textColor;   // 텍스트 색 지정

    [SerializeField]
    public bool isMovePlayer; // 플레이어의 움직임이 가능 한가

    [SerializeField]
    public float waitingTime; // 말풍선 시작 전 대기 시간
}

