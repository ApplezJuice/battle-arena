using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaGame
{
    public class TestPlayerRepository : MonoBehaviour
    {
        public Dictionary<long, string> playerID = new Dictionary<long, string>();
        public List<Entity> entities = new List<Entity>();
        public List<Buff> buffIndex = new List<Buff>();
        public List<Buff> debuffIndex = new List<Buff>();

        public void Awake()
        {
            GenerateListOfBuffs();
        }

        private void GenerateListOfBuffs()
        {
            TenPercentMoreDamage tenPercentMoreDamage = new TenPercentMoreDamage(1, "10% More Damage");
            buffIndex.Add(tenPercentMoreDamage);
            debuffIndex.Add(tenPercentMoreDamage);
        }

        public long GeneratePlayerID(string name)
        {
            //long newPlayerID = (long)Random.Range(0, long.MaxValue);
            long newPlayerID = 1;
            if (playerID.ContainsKey(newPlayerID))
            {
                do
                {
                    //newPlayerID = (long)Random.Range(0, long.MaxValue);
                    newPlayerID++;
                } while (playerID.ContainsKey(newPlayerID));

            }

            // add the id / name to the dictionary
            playerID.Add(newPlayerID, name);
            //Debug.Log("Player ID: " + newPlayerID + " / Player Name: " + name);

            return newPlayerID;
        }

        public void CreateEntity(Entity player)
        {
            Entity newPlayer = GenerateStats(player);
            entities.Add(newPlayer);
        }

        public Entity GenerateStats(Entity player)
        {
            Entity newPlayer = player;
            newPlayer.Strength = Random.Range(8, 12);
            newPlayer.HP = 100 + (int)player.ID;
            newPlayer.Defence = Random.Range(7,14);
            
            return newPlayer;
        }


    }
}
