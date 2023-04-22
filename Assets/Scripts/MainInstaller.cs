using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind(typeof(UserInput),
                  typeof(Plane),
                  typeof(CoinSpawner),
                  typeof(MissileSpawner),
                  typeof(ExplosionSpawner),
                  typeof(Vault),
                  typeof(SkinContainer),
                  typeof(MenuRadioGroup),
                  typeof(Score)
            )
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<AutoDestruct>()
            .FromComponentSibling()
            .AsTransient();

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
            .AsTransient();

        Container
            .BindFactory<Missile, Missile.Factory>()
            .FromComponentInNewPrefabResource("Missile")
            .AsCached()
            .Lazy();
        Container
            .Bind<Missile.MissilePool>()
            .FromNew()
            .AsCached();

        Container
            .BindFactory<MissileTrail, MissileTrail.Factory>()
            .FromComponentInNewPrefabResource("MissileTrail")
            .AsCached()
            .NonLazy();
        Container
            .Bind<MissileTrail.Pool>()
            .FromNew()
            .AsCached();


        Container
            .BindFactory<Coin, Coin.Factory>()
            .FromComponentInNewPrefabResource("Coin")
            .AsCached()
            .NonLazy();

        Container
            .BindFactory<ParticleSystem, ExplosionSpawner.ExplosionFactory>()
            .FromComponentInNewPrefabResource("Explosion")
            .AsCached()
            .NonLazy();

        Container
            .Bind<Coin.ICoinVault>()
            .To<Vault>()
            .FromComponentInHierarchy()
            .AsCached()
            .Lazy();

        Container
            .Bind(typeof(Vendible),
                  typeof(SkinSelect))
            .FromComponentInHierarchy()
            .AsTransient();
    }
}
