using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaGame
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        public string Name;
        public long ID;
        public int HP;
        public int Strength;

        [SerializeField]
        TestPlayerRepository playerDatabase;

        private GameObject target;

        public void Awake()
        {
            ID = playerDatabase.GeneratePlayerID(Name);

            LoadPlayerData(ID);
            playerDatabase.CreateEntity(this);
        }

        public void Start()
        {
            if (!target)
            {
                target = GameObject.FindGameObjectWithTag("Enemy");
                long tarID = target.GetComponent<Entity>().ID;

                if (tarID == this.ID)
                {
                    target = GameObject.FindGameObjectWithTag("Player2");
                    tarID = target.GetComponent<Entity>().ID;
                }

                Debug.Log(target.GetComponent<Entity>().ID);

                Entity tar = target.GetComponent<Entity>();

                Debug.Log("Enemy Str: " + tar.Strength);

            }
        }

        private void LoadPlayerData(long ID)
        {
            

        }
    }
}
