# Torff Web Server 

> A custom, from-scratch HTTP Web Server and Reverse Proxy built in pure C# to demystify how web servers and gateways work under the hood.

## About the Project

Torff was born from the desire to understand the fundamental mechanics of the web. Instead of relying on robust, production-ready servers like Kestrel, IIS, or Nginx, this project takes a step back to the basics. 

By utilizing raw TCP Sockets (`System.Net.Sockets`) in pure C#, Torff handles everything manually: opening network ports, listening for incoming connections, parsing raw HTTP text requests, routing, and building formatted HTTP responses.

Torff also acts as an **API Gateway**, capable of translating HTTP traffic into a clean, optimized internal protocol (TTP) to securely proxy requests to backend frameworks.

## Architecture


```text
/Torff
├── torff.json                 # Global configuration file (Ports, HTTPS, Timeouts)
├── Torff.sln                  # The .NET Solution file
└── src/
    ├── Torff.Server/          # PROJECT 1: The Gateway / Web Server
    │   ├── Adapters/          # Translates HTTP <-> TTP
    │   ├── Config/            # JSON configuration loaders
    │   ├── Core/              # The heart of the server (Async TCP Listeners)
    │   ├── Http/              # HTTP Protocol logic (Parsers, Chunking)
    │   ├── Routing/           # Static file serving and Proxy routing
    │   └── wwwroot/           # Public static files (HTML, CSS, Images)
    │
    └── Torff.Ttp/             # PROJECT 2: The Protocol Library
        ├── TtpRequest.cs      # The clean data contract for incoming requests
        └── TtpResponse.cs     # The clean data contract for backend responses
```

## What is TTP?

Torff Transfer Protocol (TTP) is a lightweight class library (.dll) acting as the contract between the Torff Gateway and custom web frameworks. It strips away the heavy text-parsing of standard HTTP, providing backend applications with clean, structured data for maximum efficiency in a microservices environment.

## Configuration (torff.json)

You can tweak the server's behavior on the fly by editing the root JSON file:

```json
{
  "Port": 8080,
  "WebRoot": "src/Torff.Server/wwwroot",
  "EnableKeepAlive": true,
  "TimeoutSeconds": 5,
  "EnableHttps": false,
  "CertificatePath": "torff-cert.pfx",
  "CertificatePassword": "senha123"
}
```

# Getting Started

The easiest way to run Torff is by using Docker.

- Start the server using Docker Compose:

```bash
docker-compose up --build
```

- Open your browser and navigate to:

(Depending on your torff.json configuration)

```Plaintext
HTTP: http://localhost:8080

HTTPS: https://localhost:5001 (Accept the self-signed dev certificate)
```