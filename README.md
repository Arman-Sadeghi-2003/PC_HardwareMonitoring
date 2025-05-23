# 🔧 PC Hardware Monitor

A cross-platform PC hardware monitoring application built using **C#**, **Avalonia UI**, and **MVVM** pattern. This tool helps visualize real-time system metrics such as CPU, GPU, RAM, and Motherboard details — complete with live charts, animations, and in-app notifications.

---

## 📸 Icon

<img src="https://github.com/user-attachments/assets/7d5ec7b2-3d16-49c4-bd24-5ef4503f6ba9" alt="HW_monitoring" width="300" height="300"/>

## 📸 Preview

> *(For future)*

---

## 🧠 Features

| Tab          | Key Features                                                                 |
|--------------|-------------------------------------------------------------------------------|
| **Home**     | - CPU & GPU Temp and Usage<br/>- RAM Usage<br/>- Live Charts & Stats         |
| **CPU**      | - Core Usage<br/>- Clock Speeds<br/>- Power & Temp Monitoring                |
| **GPU**      | - Temperature<br/>- Memory Usage<br/>- Load & Fan Speeds                     |
| **RAM**      | - Used vs Available Memory<br/>- RAM Type & Speed<br/>- Live Usage Chart     |
| **Motherboard** | - Board Temps<br/>- Fan Speeds<br/>- BIOS & Vendor Info                   |
| **Settings** | - Theme Switcher<br/>- Refresh Interval<br/>- Threshold Alerts<br/>- Run As Startup<br/>- Notification<br/>- Language|

---

## 🛠 Tech Stack

- **UI Framework**: [Avalonia UI](https://avaloniaui.net/)
- **Language**: C#
- **Patterns**: MVVM, Singleton
- **Charts**: [LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2) (Avalonia-compatible)
- **Notifications**: `Avalonia.Labs.Notifications`
- **Hardware Access**: [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)

---

## 🔌 Architecture

- `Services/` → Singleton services to fetch hardware data (CPU, GPU, RAM...)
- `ViewModels/` → MVVM-compliant ViewModels for UI logic
- `Views/` → Avalonia Views with DataContext binding
- `Models/` → Hardware data and chart models
- `Helpers/` → Utility functions, chart converters, extensions
