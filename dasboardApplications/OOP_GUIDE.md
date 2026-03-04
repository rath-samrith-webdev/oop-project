# Nexus Dashboard: OOP Concepts & Implementation Guide

Welcome! This guide explains the core **Object-Oriented Programming (OOP)** concepts used in this project. We've designed the Nexus Dashboard to be modular, scalable, and easy to understand by following these fundamental principles.

---

## 🏗️ 1. Abstraction (Interfaces)
**What it is:** Defining "what" a component does without worrying about "how" it does it.

**In this project:**
We use **Interfaces** like `IFeature` and `IRepository<T>`.
- `IFeature`: Every game or page (like Tic Tac Toe or Home) must have a `FeatureName` and a `GetForm()` method. The Dashboard doesn't care if it's a game or a settings page; it just knows it's an `IFeature`.
- `IRepository<T>`: This defines how we talk to data (Get, Add, Delete). The rest of the app doesn't need to know if the data is in a file, a database, or a simple list.

```csharp
// Example from IFeature.cs
public interface IFeature {
    string FeatureName { get; }
    Form GetForm();
}
```

---

## 🧬 2. Inheritance
**What it is:** Creating a new class based on an existing one to reuse code.

**In this project:**
All our dashboard pages (Home, Tic Tac Toe, Car Racing) inherit from `BaseFeatureForm`.
- `BaseFeatureForm` handles the boring stuff (removing borders, docking to fill the screen).
- By inheriting, `HomeWelcome` gets all those settings for free!

```csharp
// HomeWelcome "is a" BaseFeatureForm
public class HomeWelcome : BaseFeatureForm, IFeature { ... }
```

---

## 🔒 3. Encapsulation
**What it is:** Bundling data and methods together and hiding the internal "guts" from the outside world.

**In this project:**
We use `private` fields and methods.
- In `HomeWelcome`, the `_customerRepo` is `private`. Other classes can't reach in and mess with the repository directly; they have to use the methods we provide.
- This prevents bugs and keeps the code "clean."

```csharp
private readonly IRepository<Customer> _customerRepo; // Hidden from outside!
```

---

## 🎭 4. Polymorphism
**What it is:** The ability for different classes to be treated as instances of the same parent class or interface.

**In this project:**
The main `Dashboard` has a list of `IFeature`. When you click a button, it just calls `GetForm()` on the feature.
- If it's `TicTacToe`, it shows the grid.
- If it's `HomeWelcome`, it shows the bento grid.
- The Dashboard handles them exactly the same way!

---

## 🔌 5. Dependency Injection (DI)
**What it is:** Giving a class the tools it needs to work, rather than making the class create them itself.

**In this project:**
When we create `HomeWelcome`, we "inject" (pass in) the repositories it needs through the **Constructor**. This makes the code much easier to test and change later.

```csharp
public HomeWelcome(IRepository<Customer> customerRepo, ...) {
    _customerRepo = customerRepo; // We get the tool, we don't build it here!
}
```

---

## 📊 Summary of Flow
1. **Interfaces** define the rules.
2. **Base Classes** provide the shared foundation.
3. **Features** (Games/Pages) implement the specific logic.
4. **Dashboard** ties them all together using Polymorphism.

Happy Coding! 🚀
