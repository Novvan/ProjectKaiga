using UnityEngine;
using EZCameraShake;

public class ExplosiveScript : MonoBehaviour
{
    #region Variables
    public ParticleSystem explosionPrefab;
    public LayerMask layerMask;
    private ParticleSystem _particles;
    public float explosionPower = 200f;
    public float explosionRadius = 10f;
    private bool _playerHit = false;
    #endregion

    #region Custom callbacks
    public void explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, explosionRadius, layerMask);

        foreach (var collider in hitColliders)
        {
            if (collider.transform.root.GetComponent<Rigidbody>() != null)
            {
                if (collider.tag == "Player" && !_playerHit)
                {
                    collider.transform.root.GetComponent<PlayerController>().takeDamage(explosionPower);
                    _playerHit = true;
                }
                else if (collider.tag == "Enemy")
                {
                    Debug.Log("enemy explo");
                    collider.transform.root.GetComponent<EnemyController>().takeDamage(explosionPower * 2);
                    collider.GetComponent<Rigidbody>().AddForce((collider.transform.position - this.transform.position) * explosionPower + collider.transform.up * explosionPower * 2f);
                }
            }
        }
        CameraShaker.Instance.ShakeOnce(2.5f, 2.5f, 0.1f, 1f);

        _particles = ParticleSystem.Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        _particles.Play();
        FindObjectOfType<AudioManager>().Play("Explosion");
        Destroy(this.gameObject, 0.05f);
    }
    #endregion
}
