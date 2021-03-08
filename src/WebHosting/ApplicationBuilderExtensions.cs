﻿using DotNetCore.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Prometheus;

namespace NetCoreCleanArchitecture.WebHosting
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNetCoreCleanArchitectureWebHosting(this IApplicationBuilder app)
        {
            // DotNetCore.AspNetCore
            app.UseCorsAllowAny();

            // ASP.NET Core HTTP request metrics
            app.UseHttpMetrics();

            return app;
        }

        public static IEndpointRouteBuilder MapNetCoreCleanArchitectureWebHosting(this IEndpointRouteBuilder endpoints)
        {
            // ASP.NET Core exporter middleware
            endpoints.MapMetrics();

            endpoints.MapHealthChecks("/health");

            return endpoints;
        }
    }
}
