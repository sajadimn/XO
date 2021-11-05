using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type{
    None = 0,
    X = 1,
    O = 2,
}
[CreateAssetMenu(menuName = "Configs/GameConfigs", order = 31)]
public class GameConfigs : ScriptableObject
{
    public Sprite playerIcon;
    public Sprite botIcon;
    public Sprite iconX;
    public Sprite iconO;
    public int tableSize = 3;
    public Type starter = Type.O;
    public AudioClip menuAudio = null;
    public AudioClip gameAudio = null;
    public AudioClip botSelectAudio = null;
    public AudioClip playerSelectAudio = null;
    public AudioClip clickAudio = null;
}
