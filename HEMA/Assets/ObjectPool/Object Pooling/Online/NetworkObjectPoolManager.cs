﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

[AddComponentMenu("Object Pooling/Network Object Pool Manager")]
[Serializable]
public class NetworkObjectPoolManager : NetworkBehaviour
{
    [SerializeField]
    private static NetworkObjectPoolManager m_Instance = null;
    public static NetworkObjectPoolManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    public List<GOListWrapper> ObjectsToPool = new List<GOListWrapper>();
    public List<string> ObjectPoolNames = new List<string>();
    public List<IntListWrapper> ObjectPoolStartAmounts = new List<IntListWrapper>();
    public List<int> SubListSizes = new List<int>();

    [SerializeField]
    private Dictionary<string, NetworkGameObjectPool> m_ObjectPoolDictionary = new Dictionary<string, NetworkGameObjectPool>();

    void Start()
    {
        if (isServer)
        {
            m_Instance = this;

			if (GameObject.FindObjectsOfType(this.GetType()).Length > 1)
            {
                Debug.LogError("Can't have more than one Object Pool Manager in a scene.");
            }

            for(int i = 0; i < ObjectPoolStartAmounts.Count; ++i)
            {
                m_ObjectPoolDictionary.Add(ObjectPoolNames[i], null);
				m_ObjectPoolDictionary[ObjectPoolNames[i]] = new NetworkGameObjectPool();

                for (int j = 0; j < ObjectsToPool[i].InnerList.Count; ++j)
				{
					m_ObjectPoolDictionary[ObjectPoolNames[i]].AddSource(ObjectsToPool[i].InnerList[j], ObjectPoolStartAmounts[i].InnerList[j]);
				}
            }
        }
    }

    [Server]
	public NetworkGameObjectPool GetPoolForObject(string i_ObjectPoolTag)
	{
		NetworkGameObjectPool returnedPool = null;

		try
		{
			returnedPool = m_ObjectPoolDictionary[i_ObjectPoolTag];
		}
		catch(KeyNotFoundException ex)
		{
			Debug.LogError(string.Format("The requested tag wasn't found in the dictionary.{0}Make sure that the tag is correct and that a pool by that name exists in your inspector.{0}{1}", Environment.NewLine, ex.Message));
		}

		return returnedPool;
	}
	
	[Server]
	public static GameObject PullObject(string i_ObjectPoolTag)
	{
		GameObject returnedObject = null;

		try
		{
			returnedObject = Instance.GetPoolForObject(i_ObjectPoolTag).PullObject();
		}
		catch(Exception ex)
		{
			if(ex is NullReferenceException)
			{
				Debug.LogError(string.Format("Couldn't find pool with tag: {0}{1}{2}", i_ObjectPoolTag, Environment.NewLine, ex.Message));
			}
			else if(ex is NoPoolSourcesException)
			{
				Debug.LogError(ex.Message);
			}
		}

		return returnedObject;
	}
}
