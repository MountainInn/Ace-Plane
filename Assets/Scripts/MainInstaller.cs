using UnityEngine;
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
            .Bind<ParticleSystem>()
            .FromMethod(()=> FindObjectOfType<ParticleSystem>())
            .WhenInjectedInto<Explosive>()
            .Lazy();

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
            .FromComponentInNewPrefabResource("Missile")
            .AsTransient()
            .Lazy();
        Container
            .Bind<Missile.MissilePool>()
            .FromNew()
            .AsSingle();
    }
}
