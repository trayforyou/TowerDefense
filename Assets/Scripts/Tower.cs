using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class Tower : MonoBehaviour
{
    private Shooter _shooter;

    private void Awake() => 
        _shooter = GetComponent<Shooter>();

    public void Stop() => 
        _shooter.Stop();
}