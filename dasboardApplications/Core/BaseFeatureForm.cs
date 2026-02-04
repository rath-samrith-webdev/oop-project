using System.Windows.Forms;

namespace dasboardApplications.Core
{
    /// <summary>
    /// Base class for all forms that integrate into the dashboard.
    /// Provides consistent styling and integration hooks.
    /// </summary>
    public abstract class BaseFeatureForm : Form
    {
        protected BaseFeatureForm()
        {
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Called when the feature is loaded into the dashboard panel.
        /// </summary>
        public virtual void OnFeatureFocused()
        {
            this.Focus();
        }
    }
}
