using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaGame
{
    public class TestPlayerRepository : MonoBehaviour
    {
        public Dictionary<long, string> playerID = new Dictionary<long, string>();
        public List<Entity> entities = new List<Entity>();

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

            for (int i = 0; i < entities.Count; i++)
            {
                //Debug.Log(entities[i].HP);
                //Debug.Log(entities[i].Strength);
            }

        }

        public Entity GenerateStats(Entity player)
        {
            Entity newPlayer = player;
            newPlayer.Strength = 10 + (int)player.ID;
            newPlayer.HP = 100 + (int)player.ID;
            
            return newPlayer;
        }
    }
}
