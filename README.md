# Habit Tracker — C# Academy Project

A simple, console‑based **Habit Tracking application** built in C# as part of the C# Academy curriculum. This project helps users build consistency by creating habits, tracking daily progress, and storing data for long‑term review.

---

## 📌 Features

- **Create new habits**  
  Define habits you want to track, such as reading, exercise, hydration, or study time.

- **Log daily progress**  
  Mark habits as completed for the day and build streaks over time.

- **Persistent data storage**  
  Habit data is stored in the `Habit_Tracker_Data` project, ensuring progress is saved between sessions.

- **Unit testing support**  
  The `Habit_Tracker_Test` project includes tests to validate core functionality.

- **Clean project structure**  
  Organized into separate projects for logic, data, and testing within the solution file `Habit_Tracker.sln`.

---

## 📂 Project Structure

Habit_Tracker/
│
├── Habit_Tracker/           # Main console application
├── Habit_Tracker_Data/      # Data access, storage, and models
├── Habit_Tracker_Test/      # Unit tests
├── .vscode/                 # Editor configuration
├── .github/                 # GitHub workflows (if any)
├── Habit_Tracker.sln        # Solution file
└── .gitignore

---

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 or later  
- A C#‑compatible IDE (Visual Studio, VS Code, Rider)

### Run the Application

```bash
git clone https://github.com/JakePorter05/Habit_Tracker
cd Habit_Tracker
dotnet run --project Habit_Tracker

dotnet test

