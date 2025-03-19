using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewTaskConfig", menuName = "Task Config")]
public class DailyTaskSO : ScriptableObject
{
    public string taskName;
    public string taskDescription;
    public int taskIndex;
    public Sprite taskIcon;
}
