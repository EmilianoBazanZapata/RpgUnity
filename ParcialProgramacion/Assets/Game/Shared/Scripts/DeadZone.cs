using Game.Managers;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Jugador cayó en el pozo!");
            GameManager.Instance.LoseGame();
        }
    }
}