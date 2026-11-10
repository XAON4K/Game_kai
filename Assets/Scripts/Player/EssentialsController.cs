using UnityEngine;

public class EssentialsController : MonoBehaviour
{
    [SerializeField] private bool isOriginal = false;

    private void Awake()
    {
        if(!isOriginal)
            Destroy(gameObject);

        // DontDestroyOnLoad(this.gameObject);
    }
}
