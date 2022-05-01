using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private Collider eggCollider;
    private Collider _playerCollider;


    private void Start()
    {
        _playerCollider = FindObjectOfType<PlayerController>().GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _playerCollider)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other == eggCollider)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}