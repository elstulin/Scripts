using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMonster : NetworkBehaviour
{
    private StatsSystem stats;
    private LayerMask layerMask;
    private new Transform transform;
    public Vector3 startPosition;
    public Vector3[] rndPosition;
    private CharacterController characterController;
    private Animator animator;
    public float attackTimer;
    public float attackSpeed = 1;
    public float agroRange = 15f;
    public StatsSystem enemyTarget;
    public float agroTimer;
    private bool combat;
    private bool angry;
    public bool run;
    private bool land;
    private float fallTime;
    private float followTime;
    private float damageTakenTimer;
    private float calculatePathTimer;
    NavMeshAgent agent;
    private Vector3 lastEnemyPosition;
    bool directPath;

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        // rootPosition.y = agent.nextPosition.y;
        transform.position = new Vector3(rootPosition.x, transform.position.y, rootPosition.z);
        agent.nextPosition = transform.position;
    }
    private void OnEnable()
    {
        InstPlayer.OnPlayerInstanceEvent += OnInstancePlayer;
    }
    private void OnDisable()
    {
        InstPlayer.OnPlayerInstanceEvent -= OnInstancePlayer;
    }
    void OnInstancePlayer()
    {

//#if !UNITY_EDITOR
        Destroy(this);
        Destroy(agent);
//#endif
    }
    void Start()
    {
        layerMask = LayerMask.GetMask("Default", "Terrain", "Player", "wef");
        stats = GetComponent<StatsSystem>();
        characterController = GetComponent<CharacterController>();
        animator = stats.animator;
        transform = GetComponent<Transform>();
        if (rndPosition.Length > 0)
            startPosition = rndPosition[Random.Range(0, rndPosition.Length)];
        else
            startPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;
    }
    private void FixedUpdate()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;

        if (characterController.enabled)
        {
            land = characterController.isGrounded;
            if (!land)
            {
                fallTime += fixedDeltaTime;
                characterController.Move(new Vector3(0, -9.8f * fallTime * fixedDeltaTime, 0));
            }
            else
            {
                fallTime = 0;
                characterController.Move(new Vector3(0, -10, 0));
            }
        }
    }
    void RemoveEnemyTarget()
    {
        // enemyTarget.TargetRpcGetAttacked(enemyTarget.networkIdentity.connectionToClient,-1);
        calculatePathTimer = 0;
        agroTimer = 0;
        followTime = 0;
        int damagedTakenFromIndex = stats.damagedFrom.IndexOf(enemyTarget);
        if (damagedTakenFromIndex > -1)
        {
            stats.damagedFrom.RemoveAt(damagedTakenFromIndex);
            stats.damageFrom.RemoveAt(damagedTakenFromIndex);
        }
        if (combat)
        {
            combat = false;
            animator.SetBool("Combat", false);

        }
        if (angry)
        {
            angry = false;
            animator.SetBool("Angry", false);
        }
        agent.ResetPath();
        directPath = false;
        enemyTarget = null;
    }
    [ServerCallback]
    void Update()
    {
        // if(agentPositionRefreshTimer)
        // agent.enabled = false;
        // agent.enabled = true;

        if (stats.minHealth <= 0)
        {
            //  if(enemyTarget)
            // enemyTarget.TargetRpcGetAttacked(enemyTarget.networkIdentity.connectionToClient, -1);
            agent.enabled = false;
            return;
        }
        float deltaTime = Time.deltaTime;
        int cornersLength = agent.path.corners.Length;
        Vector3 pos;
        if (damageTakenTimer <= 0)
        {
            StatsSystem enemyFromDamageTaken = GetEnemyFromDamageTaken();
            damageTakenTimer = 1;
            if (enemyFromDamageTaken)
            {
                if (enemyTarget != enemyFromDamageTaken)
                {
                    enemyTarget = enemyFromDamageTaken;
                    //enemyTarget.TargetRpcGetAttacked(enemyTarget.networkIdentity.connectionToClient, 1);
                    CallAlly(agroRange);
                    agroTimer = 0;
                    agent.ResetPath();
                    directPath = false;
                    calculatePathTimer = 0;
                }
            }
        }
        else
            damageTakenTimer -= deltaTime;
        if (!enemyTarget)
        {
            enemyTarget = FindEnemyInRadius(agroRange);
            if (enemyTarget)
            {
                //enemyTarget.TargetRpcGetAttacked(enemyTarget.networkIdentity.connectionToClient, 1);
                CallAlly(agroRange);
                agent.ResetPath();
                directPath = false;
                calculatePathTimer = 0;
                agroTimer = 4;
            }
        }
        if (enemyTarget)
        {

            attackTimer -= deltaTime;
            followTime += deltaTime;
            if (followTime >= 7f)
            {
                RemoveEnemyTarget();
                return;
            }

            if (enemyTarget.minHealth <= 0)
            {
                RemoveEnemyTarget();
                return;
            }
            Vector3 enemyTargetPosition = enemyTarget.transform.position;
            pos = enemyTargetPosition;
            float dis = Vector3.Distance(enemyTargetPosition, transform.position);

            if (dis <= 3)
            {
                agroTimer = 0;
            }
            else
            {
                if (agroTimer > 0)
                    agroTimer -= deltaTime;
            }

            if (agroTimer <= 0)
            {
                if (!combat)
                {
                    combat = true;
                    animator.SetBool("Combat", true);

                }
                if (angry)
                {
                    angry = false;
                    animator.SetBool("Angry", false);
                }
            }
            else
            {
                if (combat)
                {
                    combat = false;
                    animator.SetBool("Combat", false);

                }
                if (!angry)
                {
                    angry = true;
                    animator.SetBool("Angry", true);
                }
            }
            if (combat)
            {
                if (dis <= agroRange)
                    followTime = 0;
                if (dis <= stats.rangeAttack + 1.2f)
                {
                    if (dis <= stats.rangeAttack)
                    {
                        if (cornersLength > 0)
                            agent.ResetPath();
                        directPath = false;
                    }
                    if (attackTimer <= 0)
                    {
                        if (dis < 1.2f)
                        {
                            stats.SetBoolAsTrigger("Back");
                        }
                        else
                        {

                            stats.SetBoolAsTrigger("AttackTrigger");
                            attackTimer = 1f / attackSpeed;
                        }
                    }
                    transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemyTargetPosition - transform.position), 0.5f).eulerAngles.y, 0);
                }
                else
                {
                    if (calculatePathTimer <= 0 || Vector3.Distance(lastEnemyPosition, enemyTargetPosition) > 0.1f)
                    {
                        //agent.SetDestination(enemyTargetPosition);
                        NavMeshPath path = new NavMeshPath();
                        agent.CalculatePath(enemyTargetPosition, path);
                        if ((int)path.status < 2)
                        {
                            agent.Warp(transform.position);
                            agent.SetPath(path);
                            directPath = false;
                        }
                        else
                        {
                            directPath = true;
                        }
                        float time = dis * 0.1f;
                        if (time > 5)
                            time = 5;
                        calculatePathTimer = time;
                        lastEnemyPosition = enemyTargetPosition;
                    }
                    else
                    {
                        if (cornersLength == 0)
                            directPath = true;
                        calculatePathTimer -= deltaTime;
                    }

                }



            }
            else
            {
                if (dis >= agroRange)
                {

                    RemoveEnemyTarget();
                    return;
                }
                transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemyTargetPosition - transform.position), 0.2f).eulerAngles.y, 0);
            }

        }
        else
        {

            float dis = Vector3.Distance(startPosition, transform.position);
            pos = startPosition;
            if (dis > 1f)
            {
                if (calculatePathTimer <= 0)
                {
                    agent.SetDestination(startPosition);
                    directPath = false;
                    calculatePathTimer = 5;
                }
                else
                {
                    calculatePathTimer -= deltaTime;
                }
            }
            else
            {
                if (cornersLength > 0)
                    agent.ResetPath();
                directPath = false;
            }
        }
        if (directPath)
        {
            if (!run)
            {
                run = true;
                animator.SetBool("Run", true);
            }
            transform.rotation = Quaternion.Euler(0, Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos - transform.position), 0.5f).eulerAngles.y, 0);
        }
        else
        {
            if (agent.remainingDistance > 1f)
            {
                if (!run)
                {
                    run = true;
                    animator.SetBool("Run", true);
                }

            }
            else
            if (run)
            {
                run = false;
                animator.SetBool("Run", false);
            }
        }
        if (agent.path.corners.Length > 2)
            for (int i = 0; i < cornersLength - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
            }
    }
    StatsSystem FindEnemyInRadius(float radius)
    {
        int indexWinner = -1;
        float mindis = 100000;
        List<StatsSystem> worldCharacters = GOManager.worldChars;
        for (int i = 0; i < worldCharacters.Count; i++)
        {
            StatsSystem worldChar = worldCharacters[i];
            if (worldChar.minHealth > 0)
            {

                bool perm = InteractionOfTypesPattern(worldChar);
                if (perm)
                {
                    float dis = Vector3.Distance(worldChar.transform.position, transform.position);
                    if (Vector3.Dot((worldChar.transform.position - transform.position).normalized, transform.forward) > 0)
                    {
                        if (dis <= radius && dis < mindis)
                        {
                            mindis = dis;
                            indexWinner = i;
                        }

                    }
                    else
                    if (dis <= radius / 2 && dis < mindis)
                    {
                        mindis = dis;
                        indexWinner = i;
                    }
                }
            }
        }
        if (indexWinner > -1)
        {
            RaycastHit raycastHit;
            if (Physics.Linecast(transform.position + new Vector3(0, 1.75f, 0) + transform.forward, worldCharacters[indexWinner].transform.position + new Vector3(0, 1f, 0), out raycastHit, layerMask))
            {
                if (raycastHit.collider.name == worldCharacters[indexWinner].name)
                {
                    return worldCharacters[indexWinner];
                }
            }


        }
        return null;
    }
    public void CallAlly(float radius)
    {
        List<StatsSystem> worldCharacters = GOManager.worldChars;
        for (int i = 0; i < worldCharacters.Count; i++)
        {
            StatsSystem worldChar = worldCharacters[i];
            if (stats.type == worldChar.type)
            {
                float dis = Vector3.Distance(worldChar.transform.position, transform.position);
                if (dis <= radius)
                {
                    AIMonster ally = worldChar.GetComponent<AIMonster>();
                    if (!ally.enemyTarget)
                        ally.enemyTarget = enemyTarget;
                }
            }
        }
    }
    public StatsSystem GetEnemyFromDamageTaken()
    {

        int indexWinner = -1;
        int maxDmg = 0;
        List<StatsSystem> worldCharacters = stats.damagedFrom;
        for (int i = 0; i < worldCharacters.Count; i++)
        {
            StatsSystem worldChar = worldCharacters[i];
            if (!worldChar)
            {
                stats.damagedFrom.RemoveAt(i);
                stats.damageFrom.RemoveAt(i);
                continue;
            }
            if (worldChar.minHealth > 0)
            {
                int dmg = stats.damageFrom[i];
                float dis = Vector3.Distance(worldChar._transform.position, transform.position);
                if (dmg / dis > maxDmg)
                {
                    maxDmg = dmg;
                    indexWinner = i;
                }
            }
        }
        if (indexWinner > -1)
        {
            return worldCharacters[indexWinner];
        }
        return null;
    }
    public void DealDamage()
    {
        if (!enemyTarget) return;
        int iHits = 0;
        List<StatsSystem> worldCharacters = GOManager.worldChars;
        for (int i = 0; i < worldCharacters.Count; i++)
        {
            if (iHits >= 10) return;
            StatsSystem targetStats = worldCharacters[i].GetComponent<StatsSystem>();
            if (targetStats.minHealth <= 0) continue;
            Transform trTarget = targetStats.transform;
            if (trTarget == transform) continue;
            float dis = Vector3.Distance(trTarget.position, transform.position);
            if (dis > stats.rangeAttack) continue;
            float dot = Vector3.Dot(trTarget.position - transform.position, transform.forward);
            if (dot < 0.5f) continue;
            bool perm = true;
            if (targetStats.type == stats.type)
                perm = false;
            if (enemyTarget == targetStats)
                perm = true;
            if (perm)
            {

                int dmg1 = (stats.weaponDamage + stats.strength);
                dmg1 *= (int)(10f / (10f + iHits));
                iHits++;
                bool crit = false;
                int dmg2 = 0;
                int dmg3 = stats.weaponFireDamage;
                int dmg4 = stats.weaponMagicDamage;
                float crtforce = 1;
                if (stats.humanoid)
                {
                    if (Random.Range(0, 101) < stats.skill1h)
                    {
                        crit = true;
                        crtforce = 2;
                    }
                }
                else
                {
                    crit = true;
                }
                if (dmg1 * 10 >= targetStats.minHealth && targetStats.rigidbodyPart)
                {
                    targetStats.rigidbodyPart.AddForce(transform.forward * dmg1 / targetStats.maxHealth * 80000 * crtforce, ForceMode.Force);
                }
                targetStats.TakeDamage(dmg1, dmg2, dmg3, dmg4, crit, gameObject);
            }
        }
    }
    bool InteractionOfTypesPattern(StatsSystem ss)
    {
        bool perm = false;

        //for Neutral
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Neutral && ss.type == StatsSystem.Type.Golem)
            perm = true;
        //for Paladin
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Paladin && ss.type == StatsSystem.Type.Golem)
            perm = true;

        //for Mage
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Mage && ss.type == StatsSystem.Type.Golem)
            perm = true;

        //for Mercenary
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Orc)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Mercenary && ss.type == StatsSystem.Type.Golem)
            perm = true;

        // for Orc
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Orc && ss.type == StatsSystem.Type.Mage)
            perm = true;

        // for Creature
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Creature && ss.type == StatsSystem.Type.Mage)
            perm = true;

        // for Bandit
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Mage)
            perm = true;
        if (stats.type == StatsSystem.Type.Bandit && ss.type == StatsSystem.Type.Orc)
            perm = true;

        // for Golem
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Undead)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Mage)
            perm = true;
        if (stats.type == StatsSystem.Type.Golem && ss.type == StatsSystem.Type.Orc)
            perm = true;

        // for Undead
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Creature)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Paladin)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Mercenary)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Golem)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Bandit)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Neutral)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Mage)
            perm = true;
        if (stats.type == StatsSystem.Type.Undead && ss.type == StatsSystem.Type.Orc)
            perm = true;


        return perm;
    }
}
