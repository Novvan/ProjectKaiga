using UnityEngine;

public class BuildMgee : MonoBehaviour, IBuildStrategy
{
    [SerializeField] private GameObject _product;
    [SerializeField] private int _bossSpawnToAmmount;
    [SerializeField] private GameManager _gm;
    private bool _spawned = false;

    private void Update()
    {
        if (_gm != null) if (_gm.Score >= _bossSpawnToAmmount && !_spawned) create();
    }

    public void create()
    {
        _spawned = true;
        GameObject boss = Instantiate(_product, gameObject.transform.position, Quaternion.identity) as GameObject;

        boss.transform.parent = gameObject.transform;
    }
}
