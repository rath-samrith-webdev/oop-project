# Feature Integration Guide

This guide explains how to create and register new features in the Nexus Dashboard.

## Architecture Overview

The system uses an interface-based approach and a dynamic factory pattern to manage feature lifecycles.

- **`IFeature`**: Defines the contract for all features.
- **`BaseFeatureForm`**: A base class providing standard dashboard styling and integration hooks.
- **`FeatureManager`**: Handles feature registration using factory functions (`Func<IFeature>`).
- **`Dashboard`**: Manages the UI transitions and ensures old features are properly disposed of.

## Step-by-Step Integration

### 1. Create the Feature Class

Your feature should inherit from `BaseFeatureForm` and implement `IFeature`.

```csharp
using dasboardApplications.Core;
using dasboardApplications.Interfaces;
using System.Windows.Forms;

namespace dasboardApplications.Features.MyNewFeature
{
    public class MyFeature : BaseFeatureForm, IFeature
    {
        // IFeature properties
        public string FeatureName => "My New Feature";
        public Form GetForm() => this;

        public MyFeature()
        {
            InitializeComponent(); // If using Designer
            SetupUI();
        }

        private void SetupUI()
        {
            // Your UI setup logic here
        }

        public override void OnFeatureFocused()
        {
            base.OnFeatureFocused();
            // Additional logic when user switches to this feature
        }
    }
}
```

### 2. Register the Feature in Dashboard

Open `Dashboard.cs` and find the `RegisterFeatures()` method. Add your feature using a lambda factory:

```csharp
private void RegisterFeatures()
{
    // ... existing registrations
    _featureManager.RegisterFeature(() => new MyFeature());
}
```

## Lifecycle & Memory Management

The dashboard uses **Dynamic Instantiation**. This means:
1.  A new instance of your feature is created every time the user clicks its sidebar button.
2.  When switching away, the old instance's `Close()` and `Dispose()` methods are called automatically.

> [!IMPORTANT]
> If your feature uses background threads, Timers, or persistent resources, ensure they are properly cleaned up in the `Dispose(bool disposing)` method or the `FormClosing` event (which is triggered by the Dashboard).

## Styling Guidelines

To maintain visual consistency, use the `UITheme` class for colors and fonts:

- **Background**: `UITheme.SecondaryBackground`
- **Primary Text**: `UITheme.TextPrimary`
- **Accent Color**: `UITheme.AccentColor`
- **Fonts**: `UITheme.HeaderFont`, `UITheme.BodyFont`
