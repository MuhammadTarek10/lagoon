# 🌊 Lagoon

<!--toc:start-->

- [🌊 Lagoon](#🌊-lagoon)
  - [🎥 Overview](#🎥-overview)
  - [📸 Screenshots](#📸-screenshots)
    - [🛠 Admin Features](#🛠-admin-features)
    - [🌟 Customer Experience](#🌟-customer-experience)
    - [💳 Authentication and Payment](#💳-authentication-and-payment)
  - [✨ Features](#features)
  - [🛠️ Prerequisites](#🛠️-prerequisites)
  - [📦 Libraries Used](#📦-libraries-used)
  - [🚀 Getting Started](#🚀-getting-started)
    - [Installation](#installation)
  - [📖 Project Structure](#📖-project-structure)
  <!--toc:end-->

**Lagoon** is a modern villa booking platform built with **ASP.NET Core MVC**, following **Clean Architecture** principles. Lagoon enables users to:

- 🏡 Browse and book luxurious villas.
- 🗓️ Check real-time availability.
- 💳 Make secure payments powered by **Stripe**.

With its **scalable architecture** and **intuitive design**, Lagoon is perfect for seamless booking experiences!

## 🎥 Overview

Watch the video below for a brief introduction to Lagoon and its features:

[![Lagoon Overview](https://i.vimeocdn.com/video/1965800311-3c68153dee646d819d11ed208e6c924adb669f10762053e42b8eaf34869cf033-d_300x169?r=pad)](https://vimeo.com/1042747453)

This video provides a quick walkthrough of Lagoon’s functionality and how you can start using the platform to book villas and make payments.

## 📸 Screenshots

Explore Lagoon's features through the following screenshots:

### 🛠 Admin Features

- **Admin Home Page**  
  ![Admin Home Page](./screenshots/admin-home-page.png)

- **Dashboard**  
  ![Dashboard](./screenshots/dashboard.png)

- **Villa Number List**  
  ![Villa Number List](./screenshots/villa-number-list.png)

- **Amenity List**  
  ![Amenity List](./screenshots/amenity-list.png)

- **Booking List**  
  ![Booking List](./screenshots/booking-list.png)

### 🌟 Customer Experience

- **Customer Home Page**  
  ![Customer Home Page](./screenshots/customer-home-page.png)

- **Home Page**  
  ![Home Page](./screenshots/home-page.png)

- **Villa List**  
  ![Villa List](./screenshots/villa-list.png)

- **Villa Details**  
  ![Villa Details](./screenshots/villa-detials.png)

- **Finalize Booking**  
  ![Finalize Booking](./screenshots/finalize-booking.png)

- **Order Confirmed**  
  ![Order Confirmed](./screenshots/order-confirmed.png)

### 💳 Authentication and Payment

- **Login Page**  
  ![Login Page](./screenshots/login.png)

- **Stripe Payment**  
  ![Stripe Payment](./screenshots/stripe-payment.png)

## ✨ Features

- 🏗️ **Clean Architecture**: Built for scalability and maintainability.
- 💳 **Stripe Integration**: Secure and smooth payment processing.
- 📱 **Responsive UI**: Optimized for all devices.
- 📊 **Admin Dashboard**: Manage villas, bookings, and users effortlessly.
- 🧼 **Clean Code**: Easy to read, maintain, and extend.
- 🤝 **User-Friendly Interface**: Simplifies the booking process for everyone.

## 🛠️ Prerequisites

To run Lagoon, ensure you have:

- **.NET CLI** or any **.NET SDK** installed on your system.
- SQLite as the database engine.

## 📦 Libraries Used

Lagoon leverages the power of:

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Design`
- `Stripe.net`
- `AspNetCore.Identity`

## 🚀 Getting Started

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/MuhammadTarek10/lagoon
   cd Lagoon
   dotnet run --project Lagoon.Web
   ```

## 📖 Project Structure

Lagoon is organized into multiple layers, adhering to the principles of Clean Architecture. Below is an overview of the project structure:

```plaintext
Lagoon
│
├── Lagoon.Application        # Application layer for services and utilities
│   ├── Common                # Shared application-level utilities
│   ├── Services              # Application-specific services
│   ├── Utilities             # Helper functions and utilities
│   └── Lagoon.Application.csproj
│
├── Lagoon.Domain             # Domain layer for core business logic
│   ├── Entities              # Domain models (e.g., Villa, Booking)
│   └── Lagoon.Domain.csproj
│
├── Lagoon.Infrastructure     # Infrastructure layer for external dependencies
│   ├── Data                  # Database configurations and context
│   ├── Migrations            # EF Core migrations
│   ├── Repositories          # Implementation of data access repositories
│   └── Lagoon.Infrastructure.csproj
│
├── Lagoon.Web                # Web layer for the ASP.NET Core MVC front-end
│   ├── Controllers           # MVC controllers
│   ├── Models                # Data models for the web layer
│   ├── ViewModels            # View models for passing data to views
│   ├── Views                 # Razor views for the UI
│   ├── wwwroot               # Static files (CSS, JS, images, etc.)
│   ├── Properties            # Application properties
│   ├── app.db                # SQLite database file
│   ├── dev.db                # Development SQLite database file
│   ├── appsettings.json      # Application settings
│   ├── appsettings.Development.json  # Development-specific settings
│   ├── Program.cs            # Entry point of the application
│   └── Lagoon.Web.csproj
│
├── Lagoon.sln                # Solution file for the Lagoon project
```
