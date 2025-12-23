using System;
using System.Collections.Generic;
using System.Linq;
using Demo._Scripts.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demo
{
    public class TreeManager : Singleton<TreeManager>
    {
        private List<TreeInfo> trees = new List<TreeInfo>();
        
        public void RegisterTree(TreeComponent tree)
        {
            trees.Add(new TreeInfo(tree));
        }
        
        public void UnregisterTree(TreeComponent tree)
        {
            trees.RemoveAll(t => t.Tree == tree);
        }

        public bool AvailableTrees()
        {
            foreach (var t in trees)
            {
                if (!t.IsAssigned) return true;
            }
            return false;
        }
        
        public bool TryGetTree(out TreeComponent tree)
        {
            var targetEntry = trees.FirstOrDefault(t => !t.IsAssigned);

            if (targetEntry == null)
            {
                tree = null;
                return false;
            }
            
            targetEntry.Assign();
            tree = targetEntry.Tree;
            return true;
        }

        private class TreeInfo 
        {
            public TreeComponent Tree;
            public bool IsAssigned;
            
            public TreeInfo(TreeComponent tree)
            {
                Tree = tree;
                IsAssigned = false;
            }
            
            public void Assign() => IsAssigned = true;
        }
    }
}