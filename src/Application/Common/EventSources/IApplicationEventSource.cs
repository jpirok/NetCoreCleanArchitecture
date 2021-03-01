﻿using NetCoreCleanArchitecture.Domain.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreCleanArchitecture.Application.Common.EventSources
{
    public interface IApplicationEventSource
    {
        long ApplicationPublished { get; }

        Task Publish<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default) where TDomainEvent : DomainEvent;
    }
}