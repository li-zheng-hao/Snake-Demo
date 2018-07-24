using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class CompUtil
{
    /// <summary>
    /// 在指定节点的下方创建一个新的GameObject
    /// </summary>
    /// <param name="_transform">父节点</param>
    /// <param name="name">名称</param>
    /// <returns>新节点变换对象</returns>
    public static Transform Create(Transform _transform, string name)
    {
        GameObject goNew = new GameObject(name);
        Transform trNew = goNew.transform;
        if (_transform != null)
        {
            trNew.SetParent(_transform);
        }
        trNew.localPosition = Vector3.zero;
        trNew.localScale = Vector3.one;
        trNew.localRotation = Quaternion.identity;
        goNew.name = name;
        return trNew;
    }
}
