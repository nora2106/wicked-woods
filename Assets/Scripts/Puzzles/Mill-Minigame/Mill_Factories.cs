// factories serve as builder classes to assemble game parts

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

// implement view factory
public class MillViewFactory : IMillViewFactory
{
    public IMillView View { get; private set; }

    // create the view
    public MillViewFactory()
    {
        View = new MillView();
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
    public MillControllerFactory(IMillModel model, IMillView view)
    {
        Controller = new MillController(model, view);
    }
}