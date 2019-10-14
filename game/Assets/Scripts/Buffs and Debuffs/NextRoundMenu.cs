﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ArenaGame
{
    public class NextRoundMenu : MonoBehaviour
    {
        private Buff[] buffsToChoose = new Buff[3];
        private Buff[] debuffsToChoose = new Buff[3];

        [SerializeField]
        public TestPlayerRepository repo;
        [SerializeField]
        public TextMeshProUGUI timerText;

        private const float NEXT_ROUND_TIMER = 30f;
        private float timerTick;

        private float oneSecondTimer;
        private int timerStart = (int)NEXT_ROUND_TIMER;

        [SerializeField]
        public Transform[] buffButton = new Transform[3];
        public Transform[] debuffButton = new Transform[3];

        [SerializeField]
        public Transform buffButtonPrefab;

        public void GenerateListOfBuffs()
        {
            for (int i = 0; i < 3; i++)
            {
                // loop 3 times for the buff selections
                buffsToChoose[i] = repo.buffIndex[Random.Range(0, repo.buffIndex.Count-1)];
                buffButton[i] = Instantiate(buffButtonPrefab, buffButtonPrefab.transform.position, Quaternion.identity);
                buffButton[i].transform.SetParent(GameObject.FindGameObjectWithTag("BuffSelection").transform, false);
            }
            for (int i = 0; i < 3; i++)
            {
                // loop 3 times for the buff selections
                debuffsToChoose[i] = repo.debuffIndex[Random.Range(0, repo.buffIndex.Count - 1)];
                debuffButton[i] = Instantiate(buffButtonPrefab, buffButtonPrefab.transform.position, Quaternion.identity);
                debuffButton[i].transform.SetParent(GameObject.FindGameObjectWithTag("DebuffSelection").transform, false);
            }
        }
        // Start is called before the first frame update
        void Awake()
        {
            repo = GameObject.Find("RepoTest").GetComponent<TestPlayerRepository>();
            GenerateListOfBuffs();
            timerText.text = timerStart.ToString();
        }

        private void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (oneSecondTimer >= 1f)
            {
                timerStart--;
                timerText.text = timerStart.ToString();
                oneSecondTimer = 0;
            }
            if (NEXT_ROUND_TIMER < timerTick)
            {
                // timer out
                Destroy(gameObject);

            }
            timerTick += Time.deltaTime;
            oneSecondTimer += Time.deltaTime;
        }
    }
}
