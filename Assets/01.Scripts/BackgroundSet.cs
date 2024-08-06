using UnityEngine;

public class BackgroundSet : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.instance == null || GameManager.instance.player == null || !GameManager.instance.onGame) return;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        float diffx = Mathf.Abs(playerPos.x - myPos.x);
        float diffy = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.vel;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        if (diffx > diffy)
        {
            transform.Translate(Vector3.right * dirX * 100);
        }
        else
        {
            transform.Translate(Vector3.up * dirY * 100);
        }
    }
}
