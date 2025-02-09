using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Scene.ins.FadeScene(("Game"));
        }
    }
}
