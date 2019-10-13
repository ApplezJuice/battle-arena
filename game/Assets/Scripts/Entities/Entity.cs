using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ArenaGame
{
    public class Entity : MonoBehaviour
    {

        [SerializeField]
        public string Name;
        public long ID;
        public int HP;
        public int Strength;
        public int Defence;
        private int HPPotionCount = 1;

        private int energy = 100;
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

        // enemy
        private TextMeshProUGUI enemyHP;
        private AI aiEntity;

        // player
        private TextMeshProUGUI playerHP;
        private TextMeshProUGUI playerEnergy;

        // UI
        [SerializeField]
        private Transform DamagePopupPrefab;
        [SerializeField]
        private Transform SkillStatusPopupPrefab;

        [SerializeField]
        public GameObject winStateUI;
        [SerializeField]
        public GameObject loseStateUI;

        public Camera mainCam;

        private bool isAI = false;
        private bool vsAI = false;
        private bool godMode = false;



        float attackTimer;

        public void Awake()
        {
            ID = playerDatabase.GeneratePlayerID(Name);

            playerDatabase.CreateEntity(this);
        }

        public void Start()
        {
            mainCam = Camera.main;
            entityAnimator = GetComponent<Animator>();

            if (!target)
            {
                // TODO: BAD should make the enemy take in the dmg
                enemyHP = GameObject.Find("EnemyHPTMP").GetComponent<TextMeshProUGUI>();

                playerHP = GameObject.Find("PlayerHPTMP").GetComponent<TextMeshProUGUI>();
                playerEnergy = GameObject.Find("PlayerEnergyTMP").GetComponent<TextMeshProUGUI>();

                target = GameObject.FindGameObjectWithTag("Enemy");
                if (target.GetComponent<AI>())
                {
                    // if there is an AI component
                   // Debug.Log("Vs AI!");
                    vsAI = true;
                    aiEntity = GameObject.FindGameObjectWithTag("Enemy").GetComponent<AI>();
                    enemyHP.text = aiEntity.HP.ToString();
                }
                else
                {
                    entityTarget = target.GetComponent<Entity>();
                    enemyHP.text = entityTarget.HP.ToString();
                }
                playerHP.text = HP.ToString();
                playerEnergy.text = energy.ToString();
                //tarID = target.GetComponent<Entity>().ID;



                //if (tarID == this.ID)
                //{
                //    isAI = true;
                //    enemyHP = GameObject.Find("PlayerHPTMP").GetComponent<TextMeshProUGUI>();
                //    playerHP = GameObject.Find("EnemyHPTMP").GetComponent<TextMeshProUGUI>();

                //    //playerEnergy = GameObject.Find("EnemyEnergyTMP").GetComponent<TextMeshProUGUI>();

                //    target = GameObject.FindGameObjectWithTag("Player2");
                //    tarID = target.GetComponent<Entity>().ID;
                //}



            }
        }

        public void Update()
        {
            // TEST TOOL

            // ~~~~~~~~ RESTART GAME
            if (Input.GetKeyDown(KeyCode.R))
            {
                resetGame();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!isAI)
                {
                    godMode = !godMode;
                }
            }

            if (godMode)
            {
                if (HP < 100)
                {
                    HP = 100;
                    playerHP.text = HP.ToString();
                }
            }

            // TEST TOOL

            //AI
            //if (isAI && HP < 40 && HPPotionCount > 0)
            //{
            //    UsePotion();
            //}
            // END AI


            // Attack entity
            if (!vsAI)
            {
                if (attackTimer >= 1.5f && entityTarget.HP > 0 && HP > 0)
                {
                    entityAnimator.SetTrigger("attacking");
                    damageTarget();
                    attackTimer = 0f;

                }
            }
            else
            {
                // vs AI
                if (attackTimer >= 1.5f && aiEntity.HP > 0 && HP > 0)
                {
                    entityAnimator.SetTrigger("attacking");
                    damageTarget();
                    attackTimer = 0f;

                }
            }

            // Recharge Energy
            if (energy < 100)
            {
                if (energyTimer >= energyTickRate)
                {
                    regenEnergyTick();
                    energyTimer = 0f;
                }
            }

            // timers
            energyTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;

        }

        private void GameWin()
        {
            winStateUI.SetActive(true);
        }

        private void GameLose()
        {
            loseStateUI.SetActive(true);
        }

        private void resetGame()
        {
            winStateUI.SetActive(false);
            loseStateUI.SetActive(false);

            HP = 100;
            //entityTarget.HP = 100;
            energy = 100;
            specialUsed = false;

            playerHP.text = HP.ToString();
            playerEnergy.text = energy.ToString();
            //enemyHP.text = entityTarget.HP.ToString();

            isSpecialAttack = false;
            isBlocking = false;

            if (vsAI)
            {
                target.GetComponent<AI>().ResetGame();
            }
        }

        private void regenEnergyTick()
        {
            // Tick
            int regen = energy + energyRechargePerTick;
            if (regen >= 100)
            {
                energy = 100;
            }
            else
            {
                energy += energyRechargePerTick;
            }
            if (!isAI)
            {
                playerEnergy.text = energy.ToString();
            }
        }

        private void damageTarget()
        {
            // base dmg
            int dmg = (Random.Range(0, 10) * Strength) / 3;

            // AI STUFFZ
            //if (isAI)
            //{
            //    if (energy >= 55 && HP < 30)
            //    {
            //        // high health and enough energy to use special attack
            //        dmg = ((Random.Range(0, 10) * Strength) / 3) + 7 * 3;
            //        energy -= 45;
            //        Debug.Log("Enemy Energy: " + energy);

            //    }
            //    else if (energy >= 35)
            //    {
            //        isBlocking = true;
            //        specialUsed = true;
            //        energy -= 35;
            //        Debug.Log("Enemy Energy: " + energy);
            //    }
            //}
            // END AI STUFFZ
            
            bool hasBlocked = false;

            if (!vsAI)
            {

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

                // Damage entity
                entityTarget.AIDamagedYou(dmg);

                //if (entityTarget.HP - dmg <= 0)
                //{
                //    entityTarget.HP -= entityTarget.HP;
                //    if (target.name == "Enemy")
                //    {
                //        winStateUI.SetActive(true);
                //    }
                //    else
                //    {
                //        loseStateUI.SetActive(true);
                //    }
                //}
                //else
                //{
                //    entityTarget.HP -= dmg;
                //}

                //enemyHP.text = entityTarget.HP.ToString();
                //Debug.Log(Name + " HP: " + HP + " // " + entityTarget.Name + " HP: " + entityTarget.HP);

                
            }
            else
            {
                // Facing AI
                if (aiEntity.isBlocking)
                {
                    dmg = ((Random.Range(0, 10) * Strength) / 3) / 3;
                    aiEntity.isBlocking = false;
                    aiEntity.specialUsed = false;
                    hasBlocked = true;
                }

                if (specialUsed)
                {
                    if (isSpecialAttack && !aiEntity.isBlocking)
                    {
                        dmg = ((Random.Range(0, 10) * Strength) / 3) + 7 * 3;
                        isSpecialAttack = false;
                    }

                    specialUsed = false;
                }
                dmg -= aiEntity.Defence / 3;

                if (dmg < 0)
                {
                    dmg = 0;
                }

                // Damage entity
                aiEntity.TakeDamage(dmg);
            }

            // happens vs both AI and player

            float posY = aiEntity.transform.position.y;
            Transform damagePopupTransform = Instantiate(DamagePopupPrefab, new Vector3(aiEntity.transform.position.x, posY += 1.5f, 0), Quaternion.identity);
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

        public void SpecialAttack()
        {
            if (energy >= 45 && specialUsed == false)
            {
                // cost of energy to do the attack
                energy -= 45;
                specialUsed = true;
                isSpecialAttack = true;
                playerEnergy.text = energy.ToString();

            }
            else if (energy < 45)
            {
                InstantiateSkillPopup("Not enough energy!");
                //Debug.Log("Not enough energy!");
            }
            else
            {
                InstantiateSkillPopup("Special in use");
                //Debug.Log("Special in use!");
            }
        }

        public void BlockSpecial()
        {
            int blockCost = 35;
            if (energy >= blockCost && specialUsed == false)
            {
                // cost of energy to do the block
                energy -= blockCost;
                specialUsed = true;
                isBlocking = true;
                playerEnergy.text = energy.ToString();

            }
            else if (energy < blockCost)
            {
                InstantiateSkillPopup("Not enough energy!");
                //Debug.Log("Not enough energy!");
            }
            else
            {
                InstantiateSkillPopup("Special in use");
                //Debug.Log("Special is already activated!");
            }
        }

        private void InstantiateSkillPopup(string message)
        {
            var center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - (Screen.height * .15f), 0f + 1f)) ;
            Transform skillStatusPopupTransform = Instantiate(SkillStatusPopupPrefab, center, Quaternion.identity);
            SkillStatusPopup skillStatusPopup = skillStatusPopupTransform.GetComponent<SkillStatusPopup>();

            skillStatusPopup.Setup(message);
        }

        public void UsePotion()
        {
            if (HPPotionCount > 0 && HP > 0)
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
                if (!isAI)
                {
                    TextMeshProUGUI potionCountText = GameObject.Find("PotionCount").GetComponent<TextMeshProUGUI>();
                    potionCountText.text = HPPotionCount.ToString();
                }
                playerHP.text = HP.ToString();

            }
            else
            {
                // no more potions
                InstantiateSkillPopup("Not enough potions!");
            }
        }

        public void AIDamagedYou(int dmg)
        {
            HP -= dmg;
            // Set player HP if above 0
            if (HP > 0)
            {
                playerHP.text = HP.ToString();
            }
            else
            {
                // you lose
                HP = 0;
                playerHP.text = HP.ToString();
                loseStateUI.SetActive(true);
            }

        }
    }
}
