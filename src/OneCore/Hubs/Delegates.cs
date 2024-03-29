﻿namespace OneCore.Hubs;

public delegate Task<ResultType> InterceptHubMessage<TEvent>(TEvent message) where TEvent : IHubMessage;