using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

        private const float NEXT_ROUND_TIMER = 15f;
        private float timerTick;

        private float oneSecondTimer;
        private int timerStart = (int)NEXT_ROUND_TIMER;

        [SerializeField]
        public Transform[] buffButton = new Transform[3];
        public Transform[] debuffButton = new Transform[3];

        [SerializeField]
        public Transform buffButtonPrefab;

        private bool buffApplied = false;
        private bool debuffApplied = false;

        [SerializeField]
        public Entity playerEntity;

        private AI aiEntity;

        public void GenerateListOfBuffs()
        {
            for (int i = 0; i < 3; i++)
            {
                // loop 3 times for the buff selections
                //buffsToChoose[i] = repo.buffIndex.ElementAt(Random.Range(0, repo.buffIndex.Count - 1));
                buffsToChoose[i] = repo.buffIndex[i];
                buffButton[i] = Instantiate(buffButtonPrefab, buffButtonPrefab.transform.position, Quaternion.identity);
                buffButton[i].transform.SetParent(GameObject.FindGameObjectWithTag("BuffSelection").transform, false);

                buffButton[i].transform.GetComponent<upgradeSelection>().buttonNumber = i;
                buffButton[i].GetComponentInChildren<TextMeshProUGUI>().text = buffsToChoose[i].spellName;
            }
            for (int i = 0; i < 3; i++)
            {
                // loop 3 times for the buff selections
                //debuffsToChoose[i] = repo.debuffIndex.ElementAt(Random.Range(0, repo.debuffIndex.Count - 1));
                debuffsToChoose[i] = repo.debuffIndex[i];
                debuffButton[i] = Instantiate(buffButtonPrefab, buffButtonPrefab.transform.position, Quaternion.identity);
                debuffButton[i].transform.SetParent(GameObject.FindGameObjectWithTag("DebuffSelection").transform, false);

                debuffButton[i].transform.GetComponent<upgradeSelection>().isDebuff = true;
                debuffButton[i].transform.GetComponent<upgradeSelection>().buttonNumber = i;
                debuffButton[i].GetComponentInChildren<TextMeshProUGUI>().text = debuffsToChoose[i].spellName;
            }
        }
        // Start is called before the first frame update
        void Awake()
        {
            aiEntity = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AI>();
            repo = GameObject.Find("RepoTest").GetComponent<TestPlayerRepository>();
            GenerateListOfBuffs();
            timerText.text = timerStart.ToString();

            playerEntity = GameObject.FindGameObjectWithTag("Player2").GetComponent<Entity>();
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

        public void ApplySelection(int buffNumber, bool isDebuff)
        {
            if (isDebuff && !debuffApplied)
            {
                //Debug.Log("Debuff added: " + debuffsToChoose[buffNumber]);

                // will have to redo when multiplayer happens
                // setting active buffs and debuffs
                aiEntity.DebuffApplied(debuffsToChoose[buffNumber]);

                debuffApplied = true;
                Destroy(GameObject.FindGameObjectWithTag("DebuffSelection"));
            }
            else if (!isDebuff && !buffApplied)
            {
                // buff
                Debug.Log("Buff added: " + buffsToChoose[buffNumber]);

                // will have to redo when multiplayer happens
                // setting active buffs and debuffs
                playerEntity.BuffApplied(buffsToChoose[buffNumber]);

                buffApplied = true;
                Destroy(GameObject.FindGameObjectWithTag("BuffSelection"));
            }
        }
    }
}
