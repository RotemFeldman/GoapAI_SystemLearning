using System;
using UnityEngine;

namespace Demo._Scripts.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if(Instance == null) Instance = GetComponent<T>();
            else
            {
                Destroy(gameObject);
            }
        }
    }
}