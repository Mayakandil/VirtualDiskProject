# 🧠 Virtual Disk Operation in C#

## 📌 Overview
This project simulates a **virtual disk** using a regular file on your system.  
It allows you to perform basic disk operations like reading and writing data in **clusters (blocks)** of 1024 bytes, similar to how a real filesystem handles disk sectors.

---

## ⚙️ Features

- **Initialize(path)**  
  Opens an existing virtual disk file or creates a new one if it doesn’t exist.

- **ReadCluster(n)**  
  Reads a 1024-byte cluster from the given cluster number.

- **WriteCluster(n, data)**  
  Writes up to 1024 bytes of data into the specified cluster.

- **GetDiskSize()**  
  Returns the total size of the virtual disk (in bytes).

- **CloseDisk()**  
  Safely closes the disk stream and ensures all data is written to disk.

---

## 🧩 How It Works

The class uses a **`FileStream`** object to represent the virtual disk file.
Each cluster = **1024 bytes**, and the class reads/writes to the correct location using **`Seek()`**.

Example:
- Cluster 0 starts at byte 0  
- Cluster 1 starts at byte 1024  
- Cluster 2 starts at byte 2048  
... and so on.

When writing, the program calls:
```csharp
diskStream.Flush();
