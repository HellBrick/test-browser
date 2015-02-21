﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace HellBrick.TestBrowser.Models
{
	public class TestModel: PropertyChangedBase
	{
		private ITest _test;

		public TestModel( ITest test )
		{
			_test = test;

			Namespace = _test.FullyQualifiedName.Substring( 0, _test.FullyQualifiedName.Length - _test.DisplayName.Length - 1 );
		}

		public string Namespace { get; private set; }
		public string Name { get { return _test.DisplayName; } }
		public TestState State { get { return _test.State; } }
		public Guid ID { get { return _test.Id; } }
		public bool IsStale { get { return _test.Stale; } }
		public bool IsCurrentlyRunning { get { return _test.IsCurrentlyRunning; } }

		public void RaiseStateChanged()
		{
			base.NotifyOfPropertyChange( () => State );
			base.NotifyOfPropertyChange( () => IsStale );
			base.NotifyOfPropertyChange( () => IsCurrentlyRunning );
		}

		public override string ToString()
		{
			return String.Format( "[{0}] {1}/{2}", State, Namespace, Name );
		}
	}
}
