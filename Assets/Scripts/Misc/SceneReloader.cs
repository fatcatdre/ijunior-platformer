using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        _player.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
