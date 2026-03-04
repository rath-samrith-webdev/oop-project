using System.Collections.Generic;
using dasboardApplications.Interfaces;

namespace dasboardApplications.Core
{
    /// <summary>
    /// Manages the registration and retrieval of dashboard features.
    /// Supports the Open/Closed principle by allowing new features to be added without modifying the dashboard core.
    /// </summary>
    public class FeatureManager
    {
        private readonly List<Func<IFeature>> _featureFactories = new List<Func<IFeature>>();
        private readonly Dictionary<Type, Func<IFeature>> _typeToFactory = new Dictionary<Type, Func<IFeature>>();

        public void RegisterFeature(Func<IFeature> factory)
        {
            var initial = factory();
            _featureFactories.Add(factory);
            _typeToFactory[initial.GetType()] = factory;
        }

        public IEnumerable<IFeature> GetFeatures()
        {
            foreach (var factory in _featureFactories)
            {
                yield return factory();
            }
        }

        public IFeature CreateInstance(Type type)
        {
            if (_typeToFactory.TryGetValue(type, out var factory))
            {
                return factory();
            }
            return (IFeature)Activator.CreateInstance(type);
        }
    }
}
