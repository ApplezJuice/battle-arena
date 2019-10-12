using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaGame
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField]
        public GameObject playerStatsBox;
        Entity playerEntity;

        // Start is called before the first frame update
        void Start()
        {
            playerEntity = GameObject.FindGameObjectWithTag("Player2").GetComponent<Entity>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SpecialAttackClicked()
        {
            if (playerEntity)
            {
                playerEntity.SpecialAttack();
            }
            
            //Debug.Log("Special Attack Clicked");
        }
        public void BlockClicked()
        {
            Debug.Log("Block Clicked");
        }

        public void CharacterSheetClicked()
        {
            playerStatsBox.SetActive(!playerStatsBox.activeInHierarchy);
        }
    }
}