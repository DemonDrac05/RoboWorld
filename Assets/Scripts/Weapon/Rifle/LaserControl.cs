using UnityEngine;

public class LaserControl : MonoBehaviour
{
    private ParticleSystem laserBeam;

    private void Start()
    {
        laserBeam = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        var player = Player.player;
        if (player.stateMachine.playerState != player.swordAttackState)
        {
            if (Input.GetKey(KeyCode.Z) && !laserBeam.isPlaying)
            {
                laserBeam.Play();
            }
            if (Input.GetKeyUp(KeyCode.Z) && laserBeam.isPlaying)
            {
                laserBeam.Stop();
            }
        }
        else
        {
            laserBeam.Stop();
        }

        //laserBeam.transform.position = new Vector3(player.transform.position.x, 1.3f, player.transform.position.z);
    }
}
