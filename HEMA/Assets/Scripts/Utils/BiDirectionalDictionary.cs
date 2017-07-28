using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class BiDirectionalDictionary<T1, T2> where T1 : Object where T2 : Object 
{
    private Dictionary<T1, T2> m_Dict1 = new Dictionary<T1, T2>();
    private Dictionary<T2, T1> m_Dict2 = new Dictionary<T2, T1>();

    public T2 this[T1 key]
    {
        get
        {
            return m_Dict1.ContainsKey(key) ? m_Dict1[key] : null;
        }
    }

    public T1 this[T2 key]
    {
        get
        {
            return m_Dict2.ContainsKey(key) ? m_Dict2[key] : null;
        }
    }

    public void Add(T1 t1, T2 t2)
    {
        m_Dict1.Add(t1, t2);
        m_Dict2.Add(t2, t1);
    }

    public void Remove(T1 key)
    {
        m_Dict2.Remove(m_Dict1[key]);
        m_Dict1.Remove(key);
    }

    public void Remove(T2 key)
    {
        m_Dict1.Remove(m_Dict2[key]);
        m_Dict2.Remove(key);
    }
}