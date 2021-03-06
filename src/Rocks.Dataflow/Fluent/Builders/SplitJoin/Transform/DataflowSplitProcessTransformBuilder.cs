﻿using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using JetBrains.Annotations;
using Rocks.Dataflow.SplitJoin;

namespace Rocks.Dataflow.Fluent.Builders.SplitJoin.Transform
{
	public partial class DataflowSplitProcessTransformBuilder<TStart, TParent, TInputItem, TOutputItem> :
		DataflowBuilder<
			DataflowSplitProcessTransformBuilder<TStart, TParent, TInputItem, TOutputItem>,
			TStart,
			SplitJoinItem<TParent, TInputItem>, SplitJoinItem<TParent, TOutputItem>>
	{
		private readonly Func<TParent, TInputItem, Task<TOutputItem>> processAsync;
		private readonly Func<TParent, TInputItem, TOutputItem> processSync;


		public DataflowSplitProcessTransformBuilder ([CanBeNull] IDataflowBuilder<TStart, SplitJoinItem<TParent, TInputItem>> previousBuilder,
		                                             [NotNull] Func<TParent, TInputItem, Task<TOutputItem>> processAsync)
			: base (previousBuilder)
		{
			if (processAsync == null)
				throw new ArgumentNullException (nameof(processAsync));

			this.processAsync = processAsync;
		}


		public DataflowSplitProcessTransformBuilder ([CanBeNull] IDataflowBuilder<TStart, SplitJoinItem<TParent, TInputItem>> previousBuilder,
		                                             [NotNull] Func<TParent, TInputItem, TOutputItem> processSync)
			: base (previousBuilder)
		{
			if (processSync == null)
				throw new ArgumentNullException (nameof(processSync));

			this.processSync = processSync;
		}


		/// <summary>
		///     Creates a dataflow block from current configuration.
		/// </summary>
		protected override IPropagatorBlock<SplitJoinItem<TParent, TInputItem>, SplitJoinItem<TParent, TOutputItem>> CreateBlock ()
		{
			var block = this.processAsync != null
				            ? DataflowSplitJoin.CreateTransformBlockAsync (this.processAsync, this.options, this.DefaultExceptionLogger)
				            : DataflowSplitJoin.CreateTransformBlock (this.processSync, this.options, this.DefaultExceptionLogger);

			return block;
		}


		/// <summary>
		///     Gets the builder instance that will be returned from the
		///     <see cref="DataflowExecutionBlockBuilder{TStart,TBuilder,TInput}" /> methods.
		/// </summary>
		protected override DataflowSplitProcessTransformBuilder<TStart, TParent, TInputItem, TOutputItem> Builder => this;
	}
}