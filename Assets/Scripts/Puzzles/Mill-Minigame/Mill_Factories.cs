// factories serve as builder classes to assemble game parts
using UnityEngine;
// interface for model factory
public interface IMillModelFactory
{
    // get created model
    IMillModel Model { get; }
}

// implement model factory
public class MillModelFactory : IMillModelFactory
{
    public IMillModel Model { get; private set; }

    // create model
    public MillModelFactory()
    {
        Model = new MillModel();
    }
}

// interface for view factory
public interface IMillViewFactory
{
    // get created view
    IMillView View { get; }
}

// interface for rules factory
public interface IMillRulesFactory
{
    // get created view
    IMillRules Rules { get; }
}

// implement rules factory
public class MillRulesFactory : IMillRulesFactory
{
    public IMillRules Rules { get; private set; }

    // create model
    public MillRulesFactory()
    {
        Rules = new MillRules();
    }
}

// implement view factory
public class MillViewFactory : IMillViewFactory
{
    public IMillView View { get; private set; }

    // create the view
    public MillViewFactory()
    {
        var instance = UnityEngine.Object.Instantiate(new GameObject());
        instance.AddComponent<MillView>();
        View = instance.GetComponent<MillView>();
        // TODO: instantiate gameobjects for every board point
        // call setGMPosition() for each board point
        // assign View to view gameobject
    }
}

// interface for controller factory
public interface IMillControllerFactory
{
    IMillController Controller { get; }
}

// implement controller factory
public class MillControllerFactory : IMillControllerFactory
{
    public IMillController Controller { get; private set; }

    // create the controller
    public MillControllerFactory(IMillModel model, IMillView view, IMillRules rules)
    {
        Controller = new MillController(model, view, rules);
    }
}