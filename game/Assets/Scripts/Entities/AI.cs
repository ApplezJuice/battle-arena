using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace ArenaGame
{
    public class AI : MonoBehaviour
    {

        [SerializeField]
        public string Name;
        public long ID;
        public int HP;
        public int Strength;
        public int Defence;
        private int HPPotionStartCount = 1;
        private int HPPotionCount = 1;
        private float attackSpeed = 1.5f;

        private int energy = 100;
        private int energyStartAmount = 100;
        public bool specialUsed = false;
        private int energyRechargePerTick = 5;
        private float energyTimer;
        private float energyTickRate = 4f;

        [SerializeField]
        TestPlayerRepository playerDatabase;

        private GameObject target;
        Entity entityTarget;
        private long tarID;

        private Animator entityAnimator;

        // specials
        public bool isBlocking = false;
        public bool isSpecialAttack = false;

        private TextMeshProUGUI playerHP;

        // UI
        [SerializeField]
        private Transform DamagePopupPrefab;
        [SerializeField]
        private Transform SkillStatusPopupPrefab;

        [SerializeField]
        public GameObject winStateUI;
        [SerializeField]
        public GameObject loseStateUI;

        float attackTimer;

        [SerializeField]
        public Scoreboard scoreBoard;
        [SerializeField]
        public NextRoundMenu endRoundMenu;

        public Transform nextRoundMenu;

        private bool choseBuffs = false;

        private void Awake()
        {
            Strength = Random.Range(8, 12);
            HP = 100;
            Defence = Random.Range(7, 14);
            HPPotionCount = 1;
        }

        // Start is called before the first frame update
        void Start()
        {
            entityAnimator = GetComponent<Animator>();

            target = GameObject.FindGameObjectWithTag("Player2");
            entityTarget = target.GetComponent<Entity>();
            playerHP = GameObject.Find("EnemyHPTMP").GetComponent<TextMeshProUGUI>();

            playerHP.text = HP.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (!entityTarget.betweenRounds)
            {
                // reset chosen buffs so it will choose again
                if (choseBuffs)
                {
                    choseBuffs = false;
                }

                if (HP < 40 && HPPotionCount > 0)
                {
                    UsePotion();
                }

                // Attack entity
                if (attackTimer >= attackSpeed && entityTarget.HP > 0 && HP > 0)
                {
                    entityAnimator.SetTrigger("attacking");
                    DamageTarget();
                    attackTimer = 0f;

                }

                // Recharge Energy
                if (energy < energyStartAmount)
                {
                    if (energyTimer >= energyTickRate)
                    {
                        regenEnergyTick();
                        energyTimer = 0f;
                        //Debug.Log(energy);
                    }
                }

                // timers
                energyTimer += Time.deltaTime;
                attackTimer += Time.deltaTime;
            }
            else if (entityTarget.betweenRounds && !choseBuffs)
            {
                ChooseBuffAndDebuff();
                choseBuffs = true;
            }
        }

        public void ResetGame()
        {
            HP = 100;
            energy = 100;
            specialUsed = false;
            HPPotionCount = HPPotionStartCount;

            playerHP.text = HP.ToString();
            //playerEnergy.text = energy.ToString();
            //enemyHP.text = entityTarget.HP.ToString();

            isSpecialAttack = false;
            isBlocking = false;
        }

        public void TakeDamage(int dmg)
        {
            HP -= dmg;
            // Set player HP if above 0
            if (HP > 0)
            {
                playerHP.text = HP.ToString();
            }
            else
            {
                // Player beats AI
                HP = 0;
                playerHP.text = HP.ToString();
                scoreBoard.PlayerOneScore++;
                scoreBoard.updateScoreboard(scoreBoard.PlayerOneScore + " - " + scoreBoard.PlayerTwoScore);
                //endRoundMenu.GenerateListOfBuffs();

                if (scoreBoard.PlayerOneScore == 3)
                {
                    winStateUI.SetActive(true);
                }
                else
                {
                    // set not over but next round
                    Transform nextRoundMenuTransform = Instantiate(nextRoundMenu, nextRoundMenu.transform.position, Quaternion.identity);
                    nextRoundMenuTransform.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
                    entityTarget.betweenRoundsTimer = 15f;
                    entityTarget.betweenRoundsTimerTick = 0f;
                    entityTarget.betweenRounds = true;
                    //entityTarget.resetGame();
                }
                
            }
        }

        private void DamageTarget()
        {
            // base dmg
            int dmg = (Random.Range(0, 10) * Strength) / 3;

            // AI STUFFZ
            if (energy >= 55 && HP > 30)
            {
                // high health and enough energy to use special attack
                dmg = ((Random.Range(0, 10) * Strength) / 3) + 7 * 3;
                energy -= 45;

            }
            else if (energy >= 35)
            {
                isBlocking = true;
                specialUsed = true;
                energy -= 35;
            }
            
            // END AI STUFFZ

            bool hasBlocked = false;

            if (entityTarget.isBlocking)
            {
                dmg = ((Random.Range(0, 10) * Strength) / 3) / 3;
                entityTarget.isBlocking = false;
                entityTarget.specialUsed = false;
                hasBlocked = true;
            }

            if (specialUsed)
            {
                if (isSpecialAttack && !entityTarget.isBlocking)
                {
                    dmg = ((Random.Range(0, 10) * Strength) / 3) + 7 * 3;
                    isSpecialAttack = false;
                }

                specialUsed = false;
            }
            dmg -= entityTarget.Defence / 3;

            if (dmg < 0)
            {
                dmg = 0;
            }

            // send the damage to the player
            entityTarget.AIDamagedYou(dmg);

            float posY = entityTarget.transform.position.y;
            Transform damagePopupTransform = Instantiate(DamagePopupPrefab, new Vector3(entityTarget.transform.position.x, posY += 1.5f, 0), Quaternion.identity);
            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

            if (hasBlocked == true)
            {
                damagePopup.Setup(dmg, false, true);
            }
            else if (dmg >= 31)
            {
                damagePopup.Setup(dmg, true, false);
            }
            else
            {
                damagePopup.Setup(dmg, false, false);
            }
        }

        private void UsePotion()
        {
            HPPotionCount--;
            if (HP + 40 > 100)
            {
                HP = 100;
            }
            else
            {
                HP += 40;
            }
            playerHP.text = HP.ToString();

        }

        private void regenEnergyTick()
        {
            // Tick
            int regen = energy + energyRechargePerTick;
            if (regen >= energyStartAmount)
            {
                energy = energyStartAmount;
            }
            else
            {
                energy += energyRechargePerTick;
            }

        }

        private void ChooseBuffAndDebuff()
        {
            // Auto choose something
            int randomBuff = Random.Range(0, playerDatabase.buffIndex.Count);
            int randomDebuff = Random.Range(0, playerDatabase.debuffIndex.Count);

            switch (playerDatabase.buffIndex.ElementAt(randomBuff).spellID)
            {
                case 1:
                    int tempStr = Strength + (int)(Strength * .10f);
                    Strength = tempStr;
                    break;
                case 2:
                    energyStartAmount += 10;
                    break;
                case 3:
                    HPPotionStartCount++;
                    break;
            }

            entityTarget.DebuffApplied(playerDatabase.debuffIndex.ElementAt(randomDebuff));
            
        }

        public void DebuffApplied(Buff appliedDeBuff)
        {
            switch (appliedDeBuff.spellID)
            {
                case 4:
                    float attackSpeedTemp = attackSpeed + (attackSpeed * .10f);
                    attackSpeed = attackSpeedTemp;
                    break;
                case 5:
                    Defence -= 2;
                    break;
                case 6:
                    energyStartAmount -= 10;
                    break;
            }

            Debug.Log("Debuff applied to enemy: " + appliedDeBuff.spellName);
        }
    }
}