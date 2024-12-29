using UnityEngine;

public class white_plat : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           FindFirstObjectByType<player_script>().OnWhitePlatformHit(this.gameObject.transform.eulerAngles.z);
           FindFirstObjectByType<player_script>().move_times = 0;
        }
    }
}


