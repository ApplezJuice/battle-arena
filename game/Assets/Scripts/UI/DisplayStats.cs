using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace ArenaGame
{
    public class DisplayStats : MonoBehaviour
    {
        [SerializeField]
        public TextMeshProUGUI entityName;
        [SerializeField]
        public TextMeshProUGUI HP;
        [SerializeField]
        public TextMeshProUGUI Str;
        [SerializeField]
        public TextMeshProUGUI Def;
        [SerializeField]
        public Entity entityRef;

        public bool statsNeedUpdating;

        // Start is called before the first frame update
        void Start()
        {
            InitializeStats();
            
        }

        private void InitializeStats()
        {
            entityName.text = entityRef.Name;
            HP.text = entityRef.HP.ToString();
            Str.text = entityRef.Strength.ToString();
            Def.text = entityRef.Defence.ToString();
        }

    }

}