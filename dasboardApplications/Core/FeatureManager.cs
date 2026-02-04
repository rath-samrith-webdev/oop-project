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
        private readonly List<IFeature> _features = new List<IFeature>();

        public void RegisterFeature(IFeature feature)
        {
            _features.Add(feature);
        }

        public IEnumerable<IFeature> GetFeatures() => _features;
    }
}
