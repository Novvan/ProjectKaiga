using UnityEngine;

public class BuildTposer : MonoBehaviour, IBuildStrategy
{
    [SerializeField] private GameObject _product;
    [SerializeField] private int _TPoserAmmount;

    private void Start()
    {
        create();
    }

    public void create()
    {
        for (int i = 0; i < _TPoserAmmount; i++)
        {
            GameObject _tPoser = Instantiate(_product, gameObject.transform.position, Quaternion.identity) as GameObject;

            _tPoser.transform.parent = gameObject.transform;
        }
    }
}
