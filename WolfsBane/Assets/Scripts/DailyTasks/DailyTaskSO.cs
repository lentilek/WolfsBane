using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTaskConfig", menuName = "Task Config")]
public class DailyTaskSO : ScriptableObject
{
    public string taskName;
    public string taskDescription;
    public int taskIndex;
}
