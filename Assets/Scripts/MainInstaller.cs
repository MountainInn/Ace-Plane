using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind(typeof(UserInput),
                  typeof(CoinSpawner),
                  typeof(Plane))
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<Missile.ILockOnTarget>()
            .FromComponentInHierarchy()
            .WhenInjectedInto<Missile>()
            .Lazy();
        Container
            .Bind<Explosive>()
            .FromComponentSibling()
            .AsSingle();

        Container
            .BindFactory<Missile, Missile.Factory>()
            .FromComponentInNewPrefabResource("Prefabs/PlayerPrefab")
            .AsTransient();
        Container
            .Bind<Missile.Factory>()
            .FromResolve()
            .AsSingle();
    }
}
