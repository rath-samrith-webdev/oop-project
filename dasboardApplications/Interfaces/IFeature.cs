using System;
using System.Windows.Forms;

namespace dasboardApplications.Interfaces
{
    /// <summary>
    /// Represents a feature that can be displayed and managed by the Dashboard.
    /// </summary>
    public interface IFeature
    {
        string FeatureName { get; }
        Form GetForm();
    }
}
