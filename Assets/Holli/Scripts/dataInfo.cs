using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class dataInfo
{
    public string name,url;
    public List<float> pos, rot, scl;

    public dataInfo(string name, string url, List<float> pos, List<float> rot, List<float> scl)
    {
        this.name = name;
        this.url = url;
        this.pos = pos;
        this.rot = rot;
        this.scl = scl;
    }

    public string toString()
    {
        return name + ": " + url + "/" + pos + "/" + rot + "/" + scl ;
    }
}
