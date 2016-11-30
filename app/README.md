# VueNetCoreBoilerplate

> A boilerplate of Vue application in ASP.Net Core using **N-Layer Architecture**.

## Publish to IIS

* Add JWT_SECRET to IIS environment variable. On IIS open `Configuration Editor` choose Section: `system.webServer/aspNetCore` choose From: `ApplicationHost.config <location path='SPA'/>`.
Pick `environmentVariables` and add value in the editor. Close editor, click apply and restart IIS.