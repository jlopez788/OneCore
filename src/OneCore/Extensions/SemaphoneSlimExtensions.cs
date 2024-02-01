﻿namespace OneCore.Extensions;

public static class SemaphoneSlimExtensions
{
    public static void WaitNext(this SemaphoreSlim semaphore, Action next, CancellationToken cancellationToken) => semaphore.WaitNext(() => {
        next();
        return 1;
    }, cancellationToken);

    public static T WaitNext<T>(this SemaphoreSlim semaphore, Func<T> next, CancellationToken cancellationToken)
    {
        try
        {
            semaphore.Wait(cancellationToken);
            return next();
        }
        finally { semaphore.Release(); }
    }

    public static Task WaitNextAsync(this SemaphoreSlim semaphore, Action next, CancellationToken cancellationToken) => semaphore.WaitNextAsync(() => {
        next();
        return Task.FromResult(1);
    }, cancellationToken);

    public static Task<T> WaitNextAsync<T>(this SemaphoreSlim semaphore, Func<T> next, CancellationToken cancellationToken) => semaphore.WaitNextAsync(() => Task.FromResult(next()), cancellationToken);

    public static Task WaitNextAsync(this SemaphoreSlim semaphore, Func<Task> next, CancellationToken cancellationToken) => semaphore.WaitNextAsync(async () => {
        await next();
        return 1;
    }, cancellationToken);

    public static async Task<T> WaitNextAsync<T>(this SemaphoreSlim semaphore, Func<Task<T>> next, CancellationToken cancellationToken)
    {
        T value = default!;
        try
        {
            await semaphore.WaitAsync(cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
                value = await next();
        }
        finally { semaphore.Release(); }
        return value;
    }
}