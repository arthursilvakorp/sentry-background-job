using Sentry;
using Sentry.Extensibility;

namespace SentryBackgroundJobShowcase;

public class MyEventProcessor : ISentryEventProcessor
{
    public SentryEvent? Process(SentryEvent @event)
    {
        if (@event.Exception == null) return null;
        @event.SetTag("Tag", "Tag Value Test");
        return @event;
    }
}