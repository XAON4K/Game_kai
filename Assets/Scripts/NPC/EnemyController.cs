using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool canWalk = true;
    [SerializeField] private float overlapRadius = 8f;
    [SerializeField] private float attackDistance = .7f;

    [Space(6)]
    
    [SerializeField] private int starterHealth = 100;
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackCooldown = 1;

    [Space(6)]

    [SerializeField] private bool isInfighter;
    [SerializeField] private WeaponsSettings _weaponSettings;

    [Space(7)]

    [SerializeField] private ProgressBar hpBar;

    [Space(5)]

    [SerializeField] private AudioSource audioSource;

    int hp;

    float lastTimeCheckedPlayer;
    float checkPlayerCooldown = 2;
    float checkPlayerCooldownRandomizer = .7f;
    float checkPlayerRandomK = 2;

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
        Destroy(gameObject);
    }

    private void CheckForPlayer()
    {
        lastTimeCheckedPlayer = Time.time;
        checkPlayerRandomK = Random.Range(checkPlayerCooldown - checkPlayerCooldownRandomizer, checkPlayerCooldown + checkPlayerCooldownRandomizer);

        bool isPlayerHere = OverlapForPlayer();
    
        if(isPlayerHere && !isChasingPlayer)
        {
            ChangeChaseStatus(isPlayerHere);
        }
        else if(!isPlayerHere && isChasingPlayer)
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

        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Ray ray = new Ray(transform.position, (PlayerController.GetPlayer().position - transform.position).normalized);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider == collider)
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

        if (isInfighter)
            NearAttack();
        else
            Shoot();
    }

    private void NearAttack()
    {
        PlayerController.GiveDamage(damage);
        audioSource.Play();
    }

    private void Shoot()
    {
        audioSource.Play();

        Vector3 shootPosition = _player.position + GetRandomVector3(-_weaponSettings.spreadKof, _weaponSettings.spreadKof);

        GameObject bullet = Instantiate(_weaponSettings.bulletPrefab, _weaponSettings.muzzle.position, Quaternion.identity);
        bullet.transform.forward = (shootPosition - bullet.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * _weaponSettings.bulletForce);

        Ray ray = new Ray(_weaponSettings.muzzle.position, (shootPosition - _weaponSettings.muzzle.position).normalized);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.CompareTag("Player"))
            {
                PlayerController.GiveDamage(damage);
                return;
            }
        }
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

[System.Serializable]
public class WeaponsSettings
{
    public float spreadKof = .7f;
    public float bulletForce = 550;

    [Space(5)]

    public Transform muzzle;
    public GameObject bulletPrefab;
}
