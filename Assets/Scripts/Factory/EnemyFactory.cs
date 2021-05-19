using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour, IFactory
{
    private IBuildStrategy _product;
    private Builder _builder;
    public IBuildStrategy Product => _product;

    private void Start()
    {
        _builder = GetComponent<Builder>();
        _product = GetComponent<IBuildStrategy>();
    }

    public void create()
    {
        _builder.setStrategy(_product);
        _builder.build();
    }
}
