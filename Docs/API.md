![.NET Boxed Banner](../Images/Banner.png)

![ASP.NET Core API Boxed Preview Image](../Images/API-Preview.png)

## Technology Map

The ASP.NET Core API project template contains the following features:

![ASP.NET Core API Boxed Technology Map](../Images/API-Technology-Map.png)

## Optional Feature Selection

The ASP.NET Core API project template uses `dotnet new` to enable you to turn features of the project template on or off. Find out more about `dotnet new` [here](http://rehansaeed.com/custom-project-templates-using-dotnet-new/).

#### API

- **Swagger** (Default=On) - Swagger is a format for describing the endpoints in your API and letting you try out your site using its user interface.
- **Versioning** (Default=On) - Enable API versioning to version API endpoints.
- **XmlFormatter** - Choose whether to use the XML input/output formatter and which serializer to use.
  - **DataContractSerializer** - The default XML serializer you should use. Requires the use of [DataContract] and [DataMember] attributes.
  - **XmlSerializer** - The alternative XML serializer which is slower but gives more control. Uses the [XmlRoot], [XmlElement] and [XmlAttribute] attributes.
  - **None** (Default) - No XML formatter.

#### Project

- **Title** - The name of the project which determines the assembly product name. If the Swagger feature is enabled, shows the title on the Swagger UI.
- **Description** - A description of the project which determines the assembly description. If the Swagger feature is enabled, shows the description on the Swagger UI.
- **Author** - The name of the author of the project which determines the assembly author and copyright information.
- **Contact** - The contact details to use if someone wants to contact you about a security vulnerability or code of conduct issues.
- **ReadMe** - Add a README.md markdown file describing the project.
- **EditorConfig** - Add a .editorconfig file to set a fixed code style.
- **TreatWarningsAsErrors** - Treat warnings as errors.

#### Ports

- **HttpPort** (Default=5000) - Port number to use for the Kestrel HTTP endpoint in `launchSettings.json`.
- **HttpsPort** (Default=5001) - Port number to use for the Kestrel HTTPS endpoint in `launchSettings.json`.
- **IISExpressHttpsPort** (Default=44300) - Port number to use for the IIS Express HTTPS endpoint in `launchSettings.json` (Must be between 44300 and 44399).

#### Source Control

- **SourceControl** - Select which source control provider you are using if any, to add provider specific features.
  - **GitHub** (Default) - Adds .github directory containing a code of conduct, contributing guide, pull request template and issue templates.
  - **None** - No source control provider is being used.
- **GitHubUsername** - Your GitHub username or organisation name that the project lives under.
- **GitHubProject** - The name of your GitHub project.

#### Continuous Integration (CI)

- **GitHubActions** (Default=On) - Adds GitHub Actions continuous integration, automatic release drafting and [CodeQL](https://docs.github.com/en/free-pro-team@latest/github/finding-security-vulnerabilities-and-errors-in-your-code/about-code-scanning) security scanning.

#### Performance

- **Response Caching** (Default=On) - Response caching is allows the use of the `[ResponseCache]` attribute on your action methods. Cache settings (cache profiles) are stored in the configuration file and referred to by name.
- **Response Compression** (Default=On) - Enables dynamic GZIP response compression of HTTP responses. Not enabled for HTTPS to avoid the BREACH security vulnerability.

#### Security

- **HttpsEverywhere** (Default=Off) - Use the HTTPS scheme and TLS security across the entire site, redirects HTTP to HTTPS and adds a Strict Transport Security (HSTS) HTTP header with preloading enabled.
- **HstsPreload** (Default=Off) - Enable Strict Transport Security (HSTS) HTTP header with preloading.
- **CORS** (Default=On) - Browser security prevents a web page from making AJAX requests to another domain. This restriction is called the same-origin policy, and prevents a malicious site from reading sensitive data from another site. CORS is a W3C standard that allows a server to relax the same-origin policy. Using CORS, a server can explicitly allow some cross-origin requests while rejecting others.
- **HostFiltering** (Default=On) - A white-list of host names allowed by the Kestrel web server e.g. example.com. You don't need this if you are using a properly configured reverse proxy.
- **SecurityTxt** - Adds a security.txt file to allow people to contact you if they find a security vulnerability.

#### Web Server

- **ForwardedHeaders** (Default=On) - If you use a load balancer, updates the request host and scheme using the X-Forwarded-Host and X-Forwarded-Proto HTTP headers.
- **ReverseProxyWebServer** - The internet facing reverse proxy web server you want to use in front of the primary web server to host the site.
  - **None** (Default) - Use Kestrel directly instead of a reverse proxy.
  - **IIS** - A flexible, secure and manageable Web server for hosting anything on the Web using Windows Server. Select this option if you are deploying your site to Azure web apps.
  - **NGINX** - A free, open-source, cross-platform high-performance HTTP server and reverse proxy.
  - **Both** - Support both reverse proxy web servers.

#### Analytics

- **HealthCheck** (Default=On) - A health-check endpoint that returns the status of this API and its dependencies, giving an indication of its health.
- **OpenTelemetry** (Default=Off) - Instrument, generate, collect, and export telemetry data (metrics, logs, and traces) using the Open Telemetry standard.
- **Analytics** - Monitor internal information about how your application is running, as well as external user information.
  - **ApplicationInsights** - Monitor internal information about how your application is running, as well as external user information using the Microsoft Azure cloud.
  - **None** (Default) - Not using any analytics.
- **ApplicationInsightsKey** - Your Application Insights instrumentation key e.g. 11111111-2222-3333-4444-555555555555.

#### Cloud

- **CloudProvider** - Select which cloud provider you are using if any, to add cloud specific features.
  - **Azure** - The Microsoft Azure cloud. Adds logging features that let you see logs in the Azure portal.
  - **None** (Default) - No cloud provider is being used.

#### Tests

- **IntegrationTest** - Adds an integration test project.

#### Docker

- **Docker** (Default=On) - Adds an optimised Dockerfile to add the ability build a Docker image.
- **DockerRegistry** - The Docker container registry to push Docker images to.
  - **GitHubContainerRegistry** (Default) - Push Docker images to the GitHub Container Registry.
  - **DockerHub** - Push Docker images to Docker Hub.

#### Other

- **robots.txt** (Default=On) - Adds a robots.txt file to tell search engines not to index this site.
- **humans.txt** (Default=On) - Adds a humans.txt file where you can tell the world who wrote the application. This file is a good place to thank your developers.

## Always On Features

#### API

- **Example Controller** - The example `CarController` contains the following actions:
  - GET - Returns single car.
  - GET - Implements paging to return a single page worth of cars, where the page size is configurable.
  - POST - Add a new car.
  - PUT - Update an existing car.
  - PATCH - Update one or more properties of an existing car.
  - DELETE a single car.
- **Automatically Return Not Acceptable** - Returns a 406 Not Acceptable if the MIME type in the Accept HTTP header is not valid.

#### Performance

- **Caching** - Both in-memory and distributed cache are configured. You do need to specify where the distributed cache stores its data.
- **Caching Static Files** - Static files are cached by default using the `Cache-Control` HTTP header.
- **304 Not Modified** - Last-Modified and If-Modified-Since HTTP headers are used to return 304 Not Modified if a resource has not changed.
- **AddMvcCore** - Uses only the features and packages from ASP.NET Core required for an API. Uses ControllerBase instead of Controller.
- **Named Routes** - Uses named attribute routes for best performance and maintainability. All route names are specified as constants.
- **Server Timing** - Adds the [`Server-Timing` HTTP header](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Server-Timing) to responses, so browser developer tools can show how long the server took to respond.

#### Security

- **Kestrel Limits** - Allow configuring Kestrel security limits such as maximum request size via configuration and set sensible stricter defaults.
- **User Secrets** - This feature is turned on when the site is in development mode to allow storing of secrets on your development machine.
- **Remove Server HTTP Header** - Removes the Server HTTP header for security and performance.
- **Translates Models to ViewModels** - Using your models which usually come from a database directly in your controllers can result in a mass assignment attack. View models with a translation layer are used to avoid this.

#### Logging

- **Serilog** - Has [Serilog](https://serilog.net/) built in for an excellent structured logging experience.

#### Debugging

- **Developer Exception Page** - Shows detailed exception information in the browser. Turned on in development mode only for security.

#### Patterns & Practices

- **Command Pattern** - Writing all your code in your controllers can mean you end up with huge classes. The command pattern splits up each controller action into its own class.
- **Configure CacheProfile in JSON** - All cache profiles can be configured from a configuration file.

## How can I get it?

1. Install the latest [.NET Core SDK](https://dot.net).
2. Run `dotnet new --install Boxed.Templates` to install the project template.
3. Run `dotnet new api --help` to see how to select the feature of the project.
5. Run `dotnet new api --name "MyProject"` along with any other custom options to create a project from the template.

## Release Notes and To-Do List
You can find release notes for each version [here](https://github.com/Dotnet-Boxed/Templates/releases) and a To-Do list of new features and enhancements coming soon in the [projects](https://github.com/Dotnet-Boxed/Templates/projects) tab.

## Contributing

Please view the [Contributing](/.github/CONTRIBUTING.md) guide for more information.
