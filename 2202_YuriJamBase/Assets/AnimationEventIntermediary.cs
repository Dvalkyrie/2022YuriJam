using UnityEngine;
using UnityEngine.Events;

public class AnimationEventIntermediary : MonoBehaviour
{
    public UnityEvent projectileHeacyKick_sprite;
    public UnityEvent projectileHeacyKick_launch;
    public UnityEvent projectileCrLightPunch_sprite;
    public UnityEvent projectileCrLightPunch_launch;
    public void ProjectileHeacyKick_sprite()
    {
        projectileHeacyKick_sprite.Invoke();
    }

    public void ProjectileHeacyKick_launch()
    {
        projectileHeacyKick_launch.Invoke();
    }
    public void ProjectileCrLightPunch_sprite()
    {
        projectileCrLightPunch_sprite.Invoke();
    }

    public void ProjectileCrLightPunch_launch()
    {
        projectileCrLightPunch_launch.Invoke();
    }
}
