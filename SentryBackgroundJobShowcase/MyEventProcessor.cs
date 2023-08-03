using Sentry;
using Sentry.Extensibility;

namespace SentryBackgroundJobShowcase;

public class MyEventProcessor : ISentryEventProcessor
{
    public SentryEvent? Process(SentryEvent @event)
    {
        @event.SetTag("Tag", "Tag Value Test");
        return @event;
    }
}