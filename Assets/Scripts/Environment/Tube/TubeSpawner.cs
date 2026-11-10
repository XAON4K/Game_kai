using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
    public enum ObstacleType
    {
        Door,
        Obstacle
    }


    [SerializeField] private TubeScriptableObject _settings;

    [Space(5)]

    [SerializeField] private GameObject wallsPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject ExitTrigger;
    [SerializeField] private GameObject[] obstaclesPrefabs;
    [SerializeField] private GameObject[] doorsPrefabs;

    private void Start()
    {
        if(_settings == null)
            _settings = SceneTransition.GetTubeSettings;
        
        GenerateTube();
    }

    private void GenerateTube()
    {
        float floorDistance = 200;

        Vector3 wallsSpawnPosition = new Vector3 (0, -(_settings.tubeHeight + floorDistance) / 2, 0);

        GameObject walls = Instantiate(wallsPrefab, wallsSpawnPosition, Quaternion.identity);
        walls.transform.localScale = new Vector3(1, _settings.tubeHeight + floorDistance, 1);

        Instantiate(floorPrefab, new Vector2(0, -(_settings.tubeHeight + floorDistance)), Quaternion.identity);
        Instantiate(ExitTrigger, new Vector2(0, -_settings.tubeHeight), Quaternion.identity);

        foreach (Obstacle obstacel in _settings.obstacles)
        {
            GameObject prefab = obstacel.Type == ObstacleType.Door ? doorsPrefabs[obstacel.Index] : obstaclesPrefabs[obstacel.Index];
            Instantiate(prefab, new Vector2(0, -obstacel.Height), Quaternion.identity);
        }
    }
}
