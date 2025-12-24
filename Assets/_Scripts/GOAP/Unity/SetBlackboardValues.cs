using System;
using System.Collections.Generic;
using BlackboardSystem;
using GOAP.Scriptable;
using UnityEngine;

namespace GOAP.Unity
{
    [RequireComponent(typeof(ScriptableGoapAgent))]
    public class SetBlackboardValues : MonoBehaviour
    {
        [SerializeField] private List<BlackboardEntryData> _toSet = new List<BlackboardEntryData>();

        private void Start()
        {
            var blackboard = GetComponent<ScriptableGoapAgent>().Blackboard;

            foreach (var data in _toSet)
            {
                data.SetValueOnBlackboard(blackboard);
            }
        }
    }
}