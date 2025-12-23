using Demo._Scripts.Utilities;
using UnityEngine;

namespace Demo._Scripts
{
    public class HouseBuilder : Singleton<HouseBuilder>, ILogPlacementArea
    {
        public GameObject HousePhase0;
        public GameObject HousePhase1;
        public GameObject HousePhase2;

        public int BuildPoints = 0;
        public int LogsAmount = 0;
        public int LogsNeedePerUpgrade = 3;
        
        public void AddLog()
        {
            LogsAmount++;
        }

        public void RemoveLog()
        {
            // noop
        }

        public bool CanUpgrade()
        {
            return LogsAmount >= LogsNeedePerUpgrade;
        }
        
        public void Upgrade()
        {
            LogsAmount -= LogsNeedePerUpgrade;
            UpdateVisualUpgrade();
            BuildPoints++;
        }
        
        private void UpdateVisualUpgrade()
        {
            switch (BuildPoints)
            {
                case 0: HousePhase0.SetActive(true); HousePhase1.SetActive(false); HousePhase2.SetActive(false); break;
                case 1: HousePhase0.SetActive(false); HousePhase1.SetActive(true); HousePhase2.SetActive(false); break;
                case 2: HousePhase0.SetActive(false); HousePhase1.SetActive(false); HousePhase2.SetActive(true); break;
                default: break;
            }
        }
    }
}