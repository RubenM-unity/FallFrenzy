using UnityEngine;

public class violet_plat : MonoBehaviour
{
    public int bounce_times = 1; // Amount of times player can move after hitting a violet platform
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindFirstObjectByType<player_script>().OnWhitePlatformHit(this.gameObject.transform.eulerAngles.z);
            FindFirstObjectByType<player_script>().OnVioletPlatformHit(bounce_times);
        }
    }
}
