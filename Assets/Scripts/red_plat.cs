using UnityEngine;

public class red_plat : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindFirstObjectByType<player_script>().OnWhitePlatformHit(this.gameObject.transform.eulerAngles.z, 0.5f);
            FindFirstObjectByType<player_script>().OnRedPlatformHit();
            FindFirstObjectByType<player_script>().move_times = 0;
        }
    }
}
