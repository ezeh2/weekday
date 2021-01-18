# Overview
Weekday is a ASP.NET core Web Application, which is hosted here: https://weekday.azurewebsites.net
It has following features:
* Weekdays of a entered date is shown
* Generating a hash from a password+hostname. This allows to generate different hashed passwords for different websites from the same base password.

# Demonstration of Security Features
Following response headers are used:
* X-XSS-Protection
* X-Frame-Options
* X-Content-Type-Options
* Content-Security-Policy

# Properly Configuring caching for ASP.NET Core Application
To make sure sure that changed JS- and CSS-Files are loaded after an ASP.NET is updated on IIS,
following response-header is added:
* Cache-Control: no-cache

This is necessary because JS- and CSS-Files are served directly by IIS outside of the ASP.NET MVC.

https://developer.mozilla.org/de/docs/Web/HTTP/Headers/Cache-Control
https://docs.microsoft.com/en-us/iis/configuration/system.webserver/staticcontent/clientcache

