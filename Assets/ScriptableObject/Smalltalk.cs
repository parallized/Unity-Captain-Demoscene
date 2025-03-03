using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Smalltalk", order = 1)]
public class Smalltalk : ScriptableObject
{
    public string position;
    public int totalTime;
    public string[] lines;
}