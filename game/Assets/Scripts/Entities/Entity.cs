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

        // UI
        [SerializeField]
        private Transform DamagePopupPrefab;


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

                if (tarID == this.ID)
                {
                    enemyHP = GameObject.Find("PlayerHPTMP").GetComponent<TextMeshProUGUI>();
                    playerHP = GameObject.Find("EnemyHPTMP").GetComponent<TextMeshProUGUI>();

                    target = GameObject.FindGameObjectWithTag("Player2");
                    tarID = target.GetComponent<Entity>().ID;
                }

                entityTarget = target.GetComponent<Entity>();

                playerHP.text = HP.ToString();
                enemyHP.text = entityTarget.HP.ToString();


            }
        }

        public void Update()
        {
            // TEST TOOL

            // ~~~~~~~~ RESTART GAME
            if (Input.GetKeyDown(KeyCode.R))
            {
                HP = 100;
                entityTarget.HP = 100;
            }

            // TEST TOOL

            if (attackTimer >= 3f && entityTarget.HP > 0 && HP > 0)
            {
                entityAnimator.SetTrigger("attacking");
                damageTarget();
                attackTimer = 0f;
                
            }
            attackTimer += Time.fixedDeltaTime;

        }

        private void damageTarget()
        {
            int dmg = (Random.Range(0, 10) * Strength) / 3;
            if (entityTarget.HP - dmg <= 0)
            {
                entityTarget.HP -= entityTarget.HP;
            }
            else
            {
                entityTarget.HP -= dmg;
            }

            enemyHP.text = entityTarget.HP.ToString();
            Debug.Log(Name + " HP: " + HP + " // " + entityTarget.Name + " HP: " + entityTarget.HP);

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

        private void LoadPlayerData(long ID)
        {
            

        }
    }
}
