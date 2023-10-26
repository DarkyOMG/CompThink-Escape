using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DecompRoundType { Image, Text}
[CreateAssetMenu(fileName = "DecompActionSO", menuName = "ScriptableObjects/DecompActionSO", order = 1)]
public class DecompActionSO : ActionSO
{
    public DecompRoundType type;
    public Sprite targetImage;
    public string targetText;
    public List<int> correctIndices= new List<int>();

    public List<string> textPartList = new List<string>();
    public List<Sprite> imagePartList = new List<Sprite>();

}
