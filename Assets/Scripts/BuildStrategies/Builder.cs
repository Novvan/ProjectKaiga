using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    private IBuildStrategy _strategy;

    public Builder(IBuildStrategy strategy)
    {
        this._strategy = strategy;
    }

    public void setStrategy(IBuildStrategy strategy)
    {
        this._strategy = strategy;
    }

    public void build()
    {
        this._strategy.create();
    }
}
