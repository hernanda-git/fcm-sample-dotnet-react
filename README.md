# FCM Push Notification Sample Project

![.NET Version](https://img.shields.io/badge/.NET-10-blueviolet)
![React](https://img.shields.io/badge/React-18.2-61DAFB?logo=react)
![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?logo=typescript)

A **modern, lightweight, and easy-to-use** sample project for implementing **Firebase Cloud Messaging (FCM)** push notifications. Built with:

- **.NET 10 Minimal API** for a clean and performant backend.
- **React + TypeScript + Vite** for a fast, type-safe frontend.

Perfect for developers looking to quickly integrate push notifications into their apps or learn FCM with a **production-ready example**.

---

## Why Use This Project?

- 🎯 **Minimal & Modern**: Clean code with .NET Minimal APIs and React + Vite for fast development.
- 🔒 **Secure**: Follows Firebase best practices for secure FCM integration.
- ⚡ **Production-Ready**: Includes OpenAPI docs, TypeScript, and environment-based config.
- 📚 **Beginner-Friendly**: Clear setup guides and well-documented code.
- 🌐 **Cross-Platform**: Works seamlessly for web notifications in foreground and background.

---

## Table of Contents

- [Project Structure](#project-structure)
- [Features](#features)
- [Backend: fcm-push-api](#backend-fcm-push-api)
- [Frontend: fcm-react-sample](#frontend-fcm-react-sample)
- [How to Use](#how-to-use)
- [Security Notes](#security-notes)
- [Contributing](#contributing)
- [License](#license)

---

## Project Structure

```
📂 fcm-push-api/        # .NET Minimal API for sending FCM notifications
📂 fcm-react-sample/    # React + TypeScript frontend for receiving notifications
```

---

## Features

- **Sleek Backend**: .NET 10 Minimal API for simple, high-performance endpoints.
- **Firebase Integration**: Uses Firebase Admin SDK for secure push notifications.
- **Interactive Frontend**: React + TypeScript with [react-toastify](https://fkhadra.github.io/react-toastify/) for beautiful notifications.
- **Type Safety**: TypeScript on both frontend and backend.
- **API Docs**: Auto-generated OpenAPI/Swagger UI for easy testing.
- **Flexible Config**: Environment-based setup for secure secrets management.

---

## Backend: fcm-push-api

A lightweight .NET 10 API for sending FCM push notifications to single or multiple devices.

### Setup

1. Clone the repo and navigate to `fcm-push-api`:

   ```bash
   cd fcm-push-api
   ```

2. Add your Firebase `serviceAccountKey.json` (download from [Firebase Console](https://console.firebase.google.com/)) to the `fcm-push-api` folder.

3. Update `appsettings.json` with the service account path:

   ```json
   "Firebase": {
     "ServiceAccountPath": "serviceAccountKey.json"
   }
   ```

4. Run the API:

   ```bash
   dotnet restore
   dotnet run
   ```

   Access the API at `http://localhost:5005` (configurable in [Properties/launchSettings.json](Properties/launchSettings.json)).

### Endpoints

- **POST `/api/notification/send`**  
  Send a notification to a single device.

  ```json
  {
    "token": "FCM_DEVICE_TOKEN",
    "title": "Hello",
    "body": "World",
    "data": { "key": "value" } // optional
  }
  ```

- **POST `/api/notification/send-bulk`**  
  Send notifications to multiple devices.

  ```json
  {
    "tokens": ["TOKEN1", "TOKEN2"],
    "title": "Bulk Title",
    "body": "Bulk Body",
    "data": { "foo": "bar" } // optional
  }
  ```

- **Interactive Docs**: Visit `http://localhost:5005/scalar` for Swagger UI in development.

### Key Files

- **[appsettings.json](appsettings.json)**: Configures logging and Firebase settings.
- **[Program.cs](Program.cs)**: Defines Minimal API setup and endpoints.
- **[Services/FcmService.cs](Services/FcmService.cs)**: Handles FCM logic.
- **[Endpoints/NotificationEndpoints.cs](Endpoints/NotificationEndpoints.cs)**: API endpoint definitions.

---

## Frontend: fcm-react-sample

A modern React + TypeScript app for receiving and displaying FCM push notifications.

### Setup

1. Navigate to the frontend folder:

   ```bash
   cd fcm-react-sample
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Add your public VAPID key to `.env`:

   ```
   VITE_PUBLIC_VAPID_KEY=YOUR_PUBLIC_VAPID_KEY
   ```

   (Find this in your Firebase project settings.)

4. Start the dev server:

   ```bash
   npm run dev
   ```

   Access the app at `http://localhost:5173`.

### How it Works

- **FCM Setup**: Requests notification permission and registers a service worker.
- **Token Retrieval**: Logs the FCM token to the browser console.
- **Foreground Notifications**: Displays toast notifications using [react-toastify](https://fkhadra.github.io/react-toastify/).
- **Background Notifications**: Uses a service worker for system notifications when the app is closed.

**Key Files:**
- [`src/services/notificationService.ts`](src/services/notificationService.ts): Manages FCM registration and messages.
- [`src/components/notification.tsx`](src/components/notification.tsx): Renders toast notifications.
- [`public/firebase-messaging-sw.js`](public/firebase-messaging-sw.js): Handles background notifications.

---

## How to Use

1. Start the backend (`dotnet run` in `fcm-push-api`).
2. Start the frontend (`npm run dev` in `fcm-react-sample`).
3. Open the React app and allow notifications.
4. Copy the FCM token from the browser console.
5. Send a notification via the API (use Swagger UI or an HTTP client like Postman).
6. See notifications in the browser (toast for foreground, system for background).

---

## Security Notes

- 🚨 **Keep `serviceAccountKey.json` private** and never commit it to Git.
- ✅ **VAPID public key** is safe to expose; keep the private key secret.
- 🔐 **Production**: Use HTTPS and add authentication to your API endpoints.

---

## Contributing

We'd love your contributions! Here's how to get started:

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/awesome-feature`).
3. Commit your changes (`git commit -m 'Add awesome feature'`).
4. Push to the branch (`git push origin feature/awesome-feature`).
5. Open a Pull Request.

Check out our [CONTRIBUTING.md](CONTRIBUTING.md) for more details.

---

## License

Licensed under the [MIT License](LICENSE). Feel free to use, modify, and share!

---

## Resources

- [Firebase Admin .NET SDK](https://firebase.google.com/docs/admin/setup)
- [Firebase Cloud Messaging](https://firebase.google.com/docs/cloud-messaging)
- [React Toastify](https://fkhadra.github.io/react-toastify/)
- [Vite](https://vitejs.dev/)
- [.NET Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)

---

**Star this repo ⭐ to support the project and start sending push notifications today!**
