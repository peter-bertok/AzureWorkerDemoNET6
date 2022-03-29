# Azure Worker Service Demo for .NET 6
## Overview
The classic "console" project template in Visual Studio should not be used in most scenarios because it is too simple 
and doesn't integrate well with platforms, packages, and monitoring systems. There are two common scenarios that have
better templates:

1. **Command-line tools** should be C# PowerShell modules (DLL). This provides a huge suite of features for free, such as
tab-complete, parameter validation, multiple input/output streams, structured (object) streams, external formatting, and more.
1. **Server-hosted scheduled tasks** should use the "Worker Service" template. This is essentially an ASP.NET Core project
without the ASP part. It inherits many of the features of the web project templates such as configuration builders,
extensible structured logging, secrets management, and dependency injection.

This repository contains a sample of the second project type. It is a .NET 6 worker service template with some additional
NuGet packages added to demonstrate a reasonably feature-complete starting point for new projects.

## Features Demonstrated
1. Application Insights SDK integrated for monitoring, with:
	- A custom telemetry initializer to allow the display name of the application to be overridden.
	- A fake "request" to enable correlation to work correctly.
1. Azure Key Vault integration for secrets storage in production.
1. Microsoft.Data.SqlClient and Dapper for lightweight but efficient data access.
1. A sample worker demonstrating dependency injection for:
	- Logging
	- Application lifetime management
	- Configuration

## Usage Instructions
**IMPORTANT:** Do not clone this project and use it as-is! It is intended as a demo, not as a template. At a minimum, the following need to be changed for each new project based on this one:

1. The solution and project names.
1. The default namespace.
1. The "UserSecretsId" setting in the `csproj` file.
1. The database name in the `ConnectionString` setting in the `appsettings.json` file.