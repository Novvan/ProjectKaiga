using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
public class WeaponHandler : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Player _player;
    private InputHandler _input;
    private AudioManager _am;

    [Header("UI")]
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private GameObject _weaponInfo;
    [SerializeField] private GameObject _weaponNameUI;
    [SerializeField] private GameObject _weaponAmmoUI;

    [Header("Weapons")]
    [SerializeField] private Gun[] _playerLoadout;
    [SerializeField] private Transform _weaponSocket;
    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private LayerMask _canBeShot;
    private ParticleSystem _muzzleFlash;
    private Transform _anchor;
    private Transform _hipSlot;
    private Transform _adsSlot;
    private GameObject _currentWeapon;
    private int _currentWeaponIndex;
    private Vector3 _hitArea;
    private float _cooldown;
    private bool _reloading;

    public Gun[] PlayerLoadout => _playerLoadout;
    public int CurrentWeaponIndex => _currentWeaponIndex;
    #endregion

    #region MonoBehaviour callbacks
    private void Start()
    {
        _input = this.GetComponent<InputHandler>();
        _am = FindObjectOfType<AudioManager>();

        foreach (var gun in _playerLoadout)
        {
            gun.reserveAmmo = gun.maxAmmo;
            gun.clipAmmo = gun.magazineSize;
        }
    }

    private void Update()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.transform.localPosition = Vector3.Lerp(_currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            _weaponAmmoUI.GetComponent<TMPro.TextMeshProUGUI>().text = _playerLoadout[_currentWeaponIndex].clipAmmo + " / " + _playerLoadout[_currentWeaponIndex].reserveAmmo;
            _weaponNameUI.GetComponent<TMPro.TextMeshProUGUI>().text = _playerLoadout[_currentWeaponIndex].name;
        }
    }
    #endregion

    #region Custom callbacks
    public void checkForInputs()
    {
        if (!_reloading)
        {

            if (Input.GetKeyDown(_input.firstWeapon))
            {
                _equipGun(0);
            }
            else if (Input.GetKeyDown(_input.secondWeapon))
            {
                _equipGun(1);
            }
            else if (_currentWeapon != null)
            {
                _aim(Input.GetMouseButton(_input.aim));
                if (Input.GetMouseButton(_input.shoot) && _cooldown <= 0)
                {
                    _shoot();
                }
                if (Input.GetKeyDown(_input.reload))
                {
                    StartCoroutine(_reloadWeapon());
                }
            }
            _cooldown -= Time.deltaTime;
        }
    }

    IEnumerator _reloadWeapon()
    {
        if (_playerLoadout[_currentWeaponIndex].reserveAmmo > 0)
        {
            _reloading = true;

            int _remainingClip = _playerLoadout[_currentWeaponIndex].clipAmmo;

            for (int i = _remainingClip; i < _playerLoadout[_currentWeaponIndex].magazineSize; i++)
            {
                _playerLoadout[_currentWeaponIndex].reserveAmmo--;
                _playerLoadout[_currentWeaponIndex].clipAmmo++;
            }

            _currentWeapon.SetActive(false);
            _am.Play("Reload");
            yield return new WaitForSeconds(_playerLoadout[_currentWeaponIndex].reloadTime);
            _reloading = false;
            _currentWeapon.SetActive(true);
        }
        else
        {
            _am.Play("NoAmmo");
            yield break;
        }
    }

    private void _equipGun(int i)
    {
        if (_currentWeapon != null)
        {
            _weaponInfo.SetActive(false);
            Destroy(_currentWeapon);
        }
        else
        {
            _weaponInfo.SetActive(true);
            GameObject newWeapon = Instantiate(_playerLoadout[i].prefab, _weaponSocket.position, _weaponSocket.rotation, _weaponSocket) as GameObject;

            newWeapon.transform.localEulerAngles = Vector3.zero;
            newWeapon.transform.localPosition = Vector3.zero;

            _muzzleFlash = newWeapon.GetComponentInChildren<ParticleSystem>();
            _currentWeapon = newWeapon;
            _currentWeaponIndex = i;
        }
    }

    private void _calculateHitArea()
    {
        Transform _cam = transform.Find("Camera/CameraHolder/Camera");
        float _weaponHitArea = _playerLoadout[_currentWeaponIndex].hitArea;
        _hitArea = _cam.position + _cam.forward * 1000f;


        if (Input.GetMouseButton(_input.aim))
        {
            float _reducedWeaponHitArea = _weaponHitArea / 10;

            _hitArea += Random.Range(-_reducedWeaponHitArea, _reducedWeaponHitArea) * _cam.up;
            _hitArea += Random.Range(-_reducedWeaponHitArea, _reducedWeaponHitArea) * _cam.right;
            _hitArea -= _cam.position;
            _hitArea.Normalize();
        }
        else
        {
            _hitArea += Random.Range(-_weaponHitArea, _weaponHitArea) * _cam.up;
            _hitArea += Random.Range(-_weaponHitArea, _weaponHitArea) * _cam.right;
            _hitArea -= _cam.position;
            _hitArea.Normalize();
        }
    }

    private void _aim(bool _isAiming)
    {

        _anchor = _currentWeapon.transform.Find("Anchor");
        _hipSlot = _currentWeapon.transform.Find("States/Hip");
        _adsSlot = _currentWeapon.transform.Find("States/Ads");


        if (_isAiming)
        {
            _crossHair.SetActive(false);
            _anchor.position = Vector3.Lerp(_anchor.position, _adsSlot.position, _playerLoadout[_currentWeaponIndex].adsSpeed * Time.deltaTime);
            _anchor.rotation = Quaternion.Lerp(_anchor.rotation, _adsSlot.rotation, _playerLoadout[_currentWeaponIndex].adsSpeed * Time.deltaTime);
        }
        else
        {
            _crossHair.SetActive(true);
            _anchor.position = Vector3.Lerp(_anchor.position, _hipSlot.position, _playerLoadout[_currentWeaponIndex].adsSpeed * Time.deltaTime);
            _anchor.rotation = Quaternion.Lerp(_anchor.rotation, _hipSlot.rotation, _playerLoadout[_currentWeaponIndex].adsSpeed * Time.deltaTime);
        }
    }


    private void _shoot()
    {
        if (_playerLoadout[_currentWeaponIndex].clipAmmo <= 0)
        {
            _cooldown = _playerLoadout[_currentWeaponIndex].rateOfFire;
            _am.Play("NoAmmo");
        }
        else
        {
            RaycastHit _hit;
            Transform _cam = transform.Find("Camera/CameraHolder/Camera");
            _am.Play("Shot");


            _muzzleFlash.Play();

            _calculateHitArea();

            if (Physics.Raycast(_cam.position, _hitArea, out _hit, _playerLoadout[_currentWeaponIndex].range / 5, _canBeShot))
            {
                if (_hit.collider.tag == "Enemy")
                {
                    _am.Play("Hitmark");
                    EnemyController _ec = _hit.collider.GetComponent<EnemyController>();
                    if (_ec != null) _ec.takeDamage(_playerLoadout[_currentWeaponIndex].damage);
                }
                else if (_hit.collider.tag == "Explosive")
                {
                    _hit.collider.GetComponent<ExplosiveScript>().explode();
                }
                else if (_hit.collider.tag == "Headshot")
                {
                    _am.Play("Hitmark");
                    _am.Play("Crit");
                    EnemyController _ec = _hit.collider.GetComponentInParent<EnemyController>();
                    if (_ec != null) _ec.takeDamage(_playerLoadout[_currentWeaponIndex].damage);
                }
                else
                {
                    GameObject newHole = Instantiate(_bulletHolePrefab, _hit.point + _hit.normal * 0.001f, Quaternion.identity) as GameObject;
                    newHole.transform.LookAt(_hit.point + _hit.normal);
                    Destroy(newHole, 3f);
                }

                GameObject newBullet = Instantiate(_playerLoadout[_currentWeaponIndex].bulletPrefab, _muzzleFlash.transform) as GameObject;
                newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 25 * Time.deltaTime, ForceMode.Impulse);

                Destroy(newBullet, 1.5f);
            }

            _currentWeapon.transform.Rotate(_playerLoadout[_currentWeaponIndex].recoil, 0, 0);
            _currentWeapon.transform.position += _currentWeapon.transform.forward * _playerLoadout[_currentWeaponIndex].kickback;
            _cooldown = _playerLoadout[_currentWeaponIndex].rateOfFire;
            _playerLoadout[_currentWeaponIndex].clipAmmo--;
        }

    }
    #endregion
}
