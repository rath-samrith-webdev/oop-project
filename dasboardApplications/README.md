# Nexus Dashboard: A Modular OOP Explorer 🚀

Nexus Dashboard is a modern, high-performance Windows Forms application designed to showcase advanced **Object-Oriented Programming (OOP)** patterns. It features a responsive bento-style dashboard, real-time leaderboards, and integrated mini-games like Tic Tac Toe and Car Racing.

![Dashboard Preview](https://via.placeholder.com/800x450.png?text=Nexus+Dashboard+Preview)

## ✨ key Features
- **Responsive Bento Grid**: A dynamic home screen that adapts to your data.
- **Top 3 Leaderboard**: Real-time integration with database records, featuring metallic-colored rank markers.
- **Modular Game System**: Easily plug in new features using the `IFeature` interface.
- **Advanced UI Theme**: Custom drawing logic for modern, polished WinForms controls.

## 🛠️ Code Examples

### 🧩 1. The Power of Interfaces (`IFeature`)
This project uses a "Plugin" architecture. Any new page simply implements `IFeature` to be automatically detected by the system.

```csharp
// Example from dasboardApplications/Interfaces/IFeature.cs
public interface IFeature
{
    string FeatureName { get; }
    Form GetForm();
}
```

### 🧬 2. Clean Inheritance (`BaseFeatureForm`)
Every screen in the dashboard inherits from this base class to ensure consistent styling and docking behavior without code duplication.

```csharp
// Used by HomeWelcome, TicTacToe, and more!
public class BaseFeatureForm : Form
{
    protected BaseFeatureForm()
    {
        this.TopLevel = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Dock = DockStyle.Fill;
    }
}
```

### 🎯 3. Responsive UI Logic
Our dashboard uses `TableLayoutPanel` with percentage-based sizing to stay symmetric on any screen size.

```csharp
TableLayoutPanel bentoGrid = new TableLayoutPanel {
    Dock = DockStyle.Top,
    ColumnCount = 3,
    RowCount = 2,
    Height = 620
};
bentoGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
bentoGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
```

## 📖 Learning OOP
For a deeper dive into how we used **Encapsulation**, **Inheritance**, and **Polymorphism**, check out our [OOP Guide](./OOP_GUIDE.md).

## 🚀 Getting Started
1. Clone the repository.
2. Open the solution in **Visual Studio 2022**.
3. Restore NuGet packages.
4. Press `F5` to run!

---
*Built with ❤️ for a better OOP learning experience.*
