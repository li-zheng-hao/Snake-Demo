using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData {
    private static StaticData _staticData;

    public static StaticData Instance{
        get
        {
            if (_staticData == null)
                _staticData = new StaticData();
            return _staticData;
        }
    }
    public string usingSkinName = "1";
	
}
