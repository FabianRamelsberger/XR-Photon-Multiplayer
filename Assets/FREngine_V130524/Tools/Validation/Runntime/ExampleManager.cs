/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using FRHelper;
using UnityEngine;

/// <summary>
///ExampleManager description
/// </summary>
public class ExampleManager : MonoBehaviour, IValidatable
{
    [Validation("GameObject prefab is required")]
    [SerializeField] [Tooltip("The required character to play the game with")]
    private GameObject _gameObject = null;

    [SerializeField] [Tooltip("Spawn point for the Character")]
    private Transform _spawnPoint = null;
    
    [Validation("Rigidbody is required")]
    [SerializeField] [Tooltip("Rigidbody for the Character")]
    private Rigidbody _rigidbody = null;
    public bool IsValid { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        if (_gameObject)
        {
            Instantiate(_gameObject, _spawnPoint.position, _spawnPoint.rotation);
        }
    }

    private void OnValidate()
    {
        IsValid = _gameObject != null;
        
        if (IsValid == false && Application.isPlaying)
        {
            Debug.LogError("No character prefab set", gameObject);
        }
    }
}
