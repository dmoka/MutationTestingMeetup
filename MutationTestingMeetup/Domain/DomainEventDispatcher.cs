using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace MutationTestingMeetup.Domain
{
    public interface IDomainEvent
    {
    }

    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent);
    }


    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch(IDomainEvent devent)
        {
            Type handlerType = typeof(IHandler<>).MakeGenericType(devent.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType);
            await handler.Handle((dynamic)devent);
        }
    }

    public interface IHandler<T> where T : IDomainEvent
    {
        Task Handle(T domainEvent);
    }
}
