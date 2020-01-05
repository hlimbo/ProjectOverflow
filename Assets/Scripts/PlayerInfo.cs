using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfoTemplate", menuName ="ScriptableObjects/PlayerInfoSO")]
public class PlayerInfo : ScriptableObject
{
    public string playerName;
    public Color color;
    public int marker;
    public float markerLifeSpan;
}
