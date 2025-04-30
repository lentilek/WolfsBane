using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Line
{
    public Sprite portrait;
    public string name;
    public string text;
}
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue Config")]
public class DialogueSO : ScriptableObject
{
    public List<Line> lines = new List<Line>();
}
