using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighscoreData
{
    private static HighscoreData _current;

    public static HighscoreData Current => _current ??= new HighscoreData();

    public string saveName;

    public string id1;
    public int days1;
    public int turistseaten1;
    public float panicmeter1;

    public string id2;
    public int days2;
    public int turistseaten2;
    public float panicmeter2;

    public string id3;
    public int days3;
    public int turistseaten3;
    public float panicmeter3;

    public string id4;
    public int days4;
    public int turistseaten4;
    public float panicmeter4;

    public string id5;
    public int days5;
    public int turistseaten5;
    public float panicmeter5;
}
