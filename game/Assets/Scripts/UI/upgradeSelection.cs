using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaGame
{
    public class upgradeSelection : MonoBehaviour
    {
        public int buttonNumber;
        public bool isDebuff = false;
        private NextRoundMenu nextRoundMenu;
        // Start is called before the first frame update
        public void BuffOrDebuffButtonClicked()
        {
            // do something with this
            Debug.Log(buttonNumber);
            nextRoundMenu.ApplySelection(buttonNumber, isDebuff);
        }

        public void Start()
        {
            nextRoundMenu = GameObject.FindGameObjectWithTag("NextRoundMenu").GetComponent<NextRoundMenu>();
        }


    }
}
