using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        private void Awake()
        {
            Strength = Random.Range(8, 12);
            HP = 100;
            Defence = Random.Range(7, 14);
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
            if (HP < 40 && HPPotionCount > 0)
            {
                UsePotion();
            }

            // Attack entity
            if (attackTimer >= 1.5f && entityTarget.HP > 0 && HP > 0)
            {
                entityAnimator.SetTrigger("attacking");
                DamageTarget();
                attackTimer = 0f;

            }

            // Recharge Energy
            if (energy < 100)
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

        public void ResetGame()
        {
            HP = 100;
            energy = 100;
            specialUsed = false;

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
                winStateUI.SetActive(true);
            }
        }

        private void DamageTarget()
        {
            // base dmg
            int dmg = (Random.Range(0, 10) * Strength) / 3;

            // AI STUFFZ
            if (energy >= 55 && HP < 30)
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
            if (regen >= 100)
            {
                energy = 100;
            }
            else
            {
                energy += energyRechargePerTick;
            }

        }
    }
}