﻿using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rocks.Commands;
using Void = Rocks.Commands.Void;


namespace Rocks.Dataflow.Commands.Tests.FluentTests.Infrastructure
{
	public class IncrementAsyncCommand : IAsyncCommand<Void>
	{
		public int Number { get; set; }
		public IProducerConsumerCollection<int> Result { get; set; }
	}


	[UsedImplicitly]
	internal class IncrementAsyncCommandHandler : IAsyncCommandHandler<IncrementAsyncCommand, Void>
	{
		public async Task<Void> ExecuteAsync (IncrementAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			await Task.Yield ();

			command.Result.TryAdd (command.Number + 1);

			return Void.Result;
		}
	}
}