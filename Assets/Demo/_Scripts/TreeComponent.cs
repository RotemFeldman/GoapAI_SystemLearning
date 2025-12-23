using System;
using UnityEngine;

namespace Demo
{
    public class TreeComponent : MonoBehaviour
    {
        private int durability = 3;
        private Animator _animator;

        public bool TryChop()
        {
            durability--;
            _animator.SetTrigger("Chop");
            if (durability == 0)
            {
                Chop();
                return true;
            }
            
            return false;
        }

        private void Start()
        {
            TreeManager.Instance.RegisterTree(this);
            _animator = GetComponent<Animator>();
        }

        private void Chop()
        {
            TreeManager.Instance.UnregisterTree(this);
            Destroy(gameObject);
        }
    }
}