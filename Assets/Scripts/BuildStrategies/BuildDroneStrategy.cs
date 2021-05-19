using UnityEngine;

public class BuildDroneStrategy : MonoBehaviour, IBuildStrategy
{
    [SerializeField] private GameObject _product;
    [SerializeField] private int _droneAmmount;

    private void Start()
    {
        create();
    }

    public void create()
    {
        for (int i = 0; i < _droneAmmount; i++)
        {
            GameObject drone = Instantiate(_product, gameObject.transform.position, Quaternion.identity) as GameObject;

            drone.transform.parent = gameObject.transform;
        }
    }
}
