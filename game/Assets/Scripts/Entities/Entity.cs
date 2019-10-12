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

        private int energy = 100;
        private bool specialUsed = false;
        private int energyRechargePerTick = 5;
        private float energyTimer;
        private float energyTickRate = 4f;

        [SerializeField]
        TestPlayerRepository playerDatabase;

        private GameObject target;
        Entity entityTarget;
        private long tarID;

        private Animator entityAnimator;

        // enemy
        private TextMeshProUGUI enemyHP;

        // player
        private TextMeshProUGUI playerHP;
        private TextMeshProUGUI playerEnergy;

        // UI
        [SerializeField]
        private Transform DamagePopupPrefab;
        [SerializeField]
        public GameObject winStateUI;
        [SerializeField]
        public GameObject loseStateUI;


        float attackTimer;

        public void Awake()
        {
            ID = playerDatabase.GeneratePlayerID(Name);

            LoadPlayerData(ID);
            playerDatabase.CreateEntity(this);
        }

        public void Start()
        {
            entityAnimator = GetComponent<Animator>();

            if (!target)
            {
                target = GameObject.FindGameObjectWithTag("Enemy");
                tarID = target.GetComponent<Entity>().ID;

                enemyHP = GameObject.Find("EnemyHPTMP").GetComponent<TextMeshProUGUI>();
                playerHP = GameObject.Find("PlayerHPTMP").GetComponent<TextMeshProUGUI>();
                playerEnergy = GameObject.Find("PlayerEnergyTMP").GetComponent<TextMeshProUGUI>();

                if (tarID == this.ID)
                {
                    enemyHP = GameObject.Find("PlayerHPTMP").GetComponent<TextMeshProUGUI>();
                    playerHP = GameObject.Find("EnemyHPTMP").GetComponent<TextMeshProUGUI>();

                    //playerEnergy = GameObject.Find("EnemyEnergyTMP").GetComponent<TextMeshProUGUI>();

                    target = GameObject.FindGameObjectWithTag("Player2");
                    tarID = target.GetComponent<Entity>().ID;
                }

                entityTarget = target.GetComponent<Entity>();

                playerHP.text = HP.ToString();
                playerEnergy.text = energy.ToString();
                enemyHP.text = entityTarget.HP.ToString();


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

            // TEST TOOL


            // Attack entity
            if (attackTimer >= 1.5f && entityTarget.HP > 0 && HP > 0)
            {
                entityAnimator.SetTrigger("attacking");
                damageTarget();
                attackTimer = 0f;
                
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
            entityTarget.HP = 100;
            energy = 100;
            specialUsed = false;

            playerHP.text = HP.ToString();
            playerEnergy.text = energy.ToString();
            enemyHP.text = entityTarget.HP.ToString();
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
            playerEnergy.text = energy.ToString();
        }

        private void damageTarget()
        {
            int dmg = (Random.Range(0, 10) * Strength) / 3;
            if (specialUsed)
            {
                dmg = ((Random.Range(0, 10) * Strength) / 3) + 7 * 3;
                specialUsed = false;
            }
            dmg -= entityTarget.Defence / 3;

            if (dmg < 0)
            {
                dmg = 0;
            }

            if (entityTarget.HP - dmg <= 0)
            {
                entityTarget.HP -= entityTarget.HP;
                if (target.name == "Enemy")
                {
                    winStateUI.SetActive(true);
                }
                else
                {
                    loseStateUI.SetActive(true);
                }
            }
            else
            {
                entityTarget.HP -= dmg;
            }

            enemyHP.text = entityTarget.HP.ToString();
            //Debug.Log(Name + " HP: " + HP + " // " + entityTarget.Name + " HP: " + entityTarget.HP);

            float posY = entityTarget.transform.position.y;
            Transform damagePopupTransform = Instantiate(DamagePopupPrefab, new Vector3(entityTarget.transform.position.x, posY += 1.5f, 0), Quaternion.identity);
            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

            if (dmg >= 31)
            {
                damagePopup.Setup(dmg, true);
            }
            else
            {
                damagePopup.Setup(dmg, false);
            }
            

        }

        public void SpecialAttack()
        {
            if (energy >= 45 && specialUsed == false)
            {
                // cost of energy to do the attack
                energy -= 45;
                specialUsed = true;
                playerEnergy.text = energy.ToString();

            }
            else if (energy < 45)
            {
                Debug.Log("Not enough energy!");
            }
            else
            {
                Debug.Log("Special is already activated!");
            }
        }

        private void LoadPlayerData(long ID)
        {
            

        }
    }
}
