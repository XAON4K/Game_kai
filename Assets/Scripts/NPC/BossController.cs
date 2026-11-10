using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    [SerializeField] private bool canWalk = true;
    [SerializeField] private float overlapRadius = 40f;
    [SerializeField] private float attackDistance = 10f;

    [Space(6)]

    [SerializeField] private int starterHealth = 1000;
    [SerializeField] private int damage = 35;
    [SerializeField] private float attackCooldown = 1.4f;

    [Space(6)]

    [SerializeField] private ProgressBar hpBar;
    [SerializeField] private GameObject rocketPrefab;

    [Space(5)]

    [SerializeField] private Transform[] muzlle;

    int lastMuzzleIndex;

    int hp;

    float lastTimeCheckedPlayer;
    float checkPlayerCooldown = .4f;
    float checkPlayerCooldownRandomizer = 0f;
    float checkPlayerRandomK = 7;

    bool isChasingPlayer = false;
    bool isOnCooldown = false;

    NavMeshAgent _agent;
    Transform _player;

    private void Start()
    {
        hp = starterHealth;
        _agent = GetComponent<NavMeshAgent>();
        _player = PlayerController.GetPlayer();

        UpdateHealthBar();
    }

    private void Update()
    {
        if (hp <= 0)
            Kill();

        if (Time.time > lastTimeCheckedPlayer + checkPlayerRandomK)
            CheckForPlayer();

        float distanceToPlayer = Vector3.Distance(_player.position, transform.position);

        if (!isChasingPlayer) return;

        if (distanceToPlayer < attackDistance)
            Attack();
        else
            GoToPlayer();
    }

    public void GiveDamage(int damage)
    {
        hp -= damage;

        UpdateHealthBar();
    }

    private void Kill()
    {
        PlayerController.ActivateLetter(.5f);
        Destroy(gameObject);
    }

    private void CheckForPlayer()
    {
        lastTimeCheckedPlayer = Time.time;
        checkPlayerRandomK = Random.Range(checkPlayerCooldown - checkPlayerCooldownRandomizer, checkPlayerCooldown + checkPlayerCooldownRandomizer);

        bool isPlayerHere = OverlapForPlayer();

        if (isPlayerHere && !isChasingPlayer)
        {
            ChangeChaseStatus(isPlayerHere);
        }
        else if (!isPlayerHere && isChasingPlayer)
        {
            ChangeChaseStatus(isPlayerHere);
        }
    }

    private void ChangeChaseStatus(bool newStatus)
    {
        isChasingPlayer = newStatus;
        _agent.isStopped = !newStatus;
    }

    private bool OverlapForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Ray ray = new Ray(transform.position, (PlayerController.GetPlayer().position - transform.position).normalized);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == collider)
                        return true;
                }
            }
        }

        return false;
    }

    private void GoToPlayer()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
    }

    #region Attacks

    private void Attack()
    {
        _agent.isStopped = true;

        if (isOnCooldown) return;

        StartCoroutine(GoOnCooldown());

        Shoot();
    }

    private void NearAttack()
    {
        PlayerController.GiveDamage(damage);
    }

    private void Shoot()
    {
        Vector3 spawnPos = muzlle[lastMuzzleIndex].position;

        GameObject rocket = Instantiate(rocketPrefab, spawnPos, Quaternion.identity);
        rocket.transform.forward = muzlle[lastMuzzleIndex].forward;

        lastMuzzleIndex += 1;
        if (lastMuzzleIndex >= 3)
            lastMuzzleIndex = 0;
    }

    private IEnumerator GoOnCooldown()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        isOnCooldown = false;
    }

    #endregion

    private void UpdateHealthBar()
    {
        float value = Vector2.Lerp(Vector2.zero, new Vector2(1, 0), (float)hp / (float)starterHealth).x;

        hpBar.value = value;
    }

    private Vector3 GetRandomVector3(float min, float max)
    {
        Vector3 result = new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        return result;
    }
}
