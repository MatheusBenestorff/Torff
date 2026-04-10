# Torff Web Server 

> A custom, from-scratch HTTP Web Server built in pure C# to demystify how web servers work under the hood.

## About the Project

Torff was born from the desire to understand the fundamental mechanics of the web. Instead of relying on robust, production-ready servers like Kestrel, IIS, or Nginx, this project takes a step back to the basics. 

By utilizing raw TCP Sockets (`System.Net.Sockets`) in pure C#, Torff handles everything manually: opening network ports, listening for incoming connections, parsing raw HTTP text requests, routing, and building formatted HTTP responses.

## Objectives
- Understand TCP/IP network fundamentals.
- Parse and interpret the HTTP/1.1 Protocol manually.
- Manage memory and byte streams.
- Handle concurrent requests (Multithreading/Asynchronous programming).

## Architecture

The project is structured into modular components to keep the codebase clean and scalable:

```text
src/
├── Core/             # The heart of the server (TCP Listeners and network loops)
├── Http/             # HTTP Protocol logic (Parsers, Request/Response models)
├── Routing/          # Business logic and file serving routes
└── wwwroot/          # Public static files (HTML, CSS, Images)
```

# Getting Started

The easiest way to run Torff is by using Docker.

- Start the server using Docker Compose:

```bash
docker-compose up --build
```

- Open your browser and navigate to:

```Plaintext
http://localhost:8080
```