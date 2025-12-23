using System;
using Demo._Scripts.Utilities;
using UnityEngine;
using TMPro;

namespace Demo._Scripts
{
    public class LogPile : Singleton<LogPile> , ILogPlacementArea
    {
        public int CurrentLogCount = 0;
        public SpriteRenderer Renderer;
        public TextMeshPro Text;

        private float growPerLog = 0.2f;
        
        public void AddLog()
        {
            if (CurrentLogCount == 0)
            {
                Renderer.color = Color.white;
            }
            CurrentLogCount++;
            Renderer.transform.localScale *= 1 + growPerLog;
            UpdateText();
        }
        
        public void RemoveLog()
        {
            CurrentLogCount--;
            if (CurrentLogCount == 0)
            {
                Renderer.color = Color.gray;
            }
            Renderer.transform.localScale *= 1 - growPerLog;
            UpdateText();
        }

        private void UpdateText()
        {
            Text.text = CurrentLogCount.ToString() + " Logs";
        }

        private void Start()
        {
            if (CurrentLogCount == 0) Renderer.color = Color.gray;
        }
    }
}