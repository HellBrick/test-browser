﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using HellBrick.TestBrowser.Common;
using Microsoft.VisualStudio.TestWindow.Data;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace HellBrick.TestBrowser.Models
{
	public class TestModel: PropertyChangedBase, INode
	{
		private ITest _test;

		public TestModel( ITest test )
		{
			_test = test;

			Location = _test.FullyQualifiedName.Substring( 0, _test.FullyQualifiedName.Length - _test.DisplayName.Length - 1 );
		}

		public string Location { get; private set; }
		public TestState State { get { return _test.State; } }
		public Guid ID { get { return _test.Id; } }
		public bool IsStale { get { return _test.Stale; } }
		public bool IsCurrentlyRunning { get { return _test.IsCurrentlyRunning; } }
		public TestResultData Result { get { return _test.Results.FirstOrDefault() as TestResultData; } }

		public event EventHandler<TestModel, EventArgs> SelectionChanged;
		private void RaiseSelectionChanged()
		{
			var handler = SelectionChanged;
			if ( handler != null )
				handler( this, EventArgs.Empty );
		}

		public void RaiseStateChanged()
		{
			base.NotifyOfPropertyChange( () => State );
			base.NotifyOfPropertyChange( () => IsStale );
			base.NotifyOfPropertyChange( () => IsCurrentlyRunning );
			base.NotifyOfPropertyChange( () => Result );
		}

		public override string ToString()
		{
			return String.Format( "[{0}] {1}/{2}", State, Location, Name );
		}

		#region INode Members

		public NodeType Type
		{
			get { return NodeType.Test; }
		}

		public string Name { get { return _test.DisplayName; } }
		public INode Parent { get; set; }

		private List<INode> _emptyList = new List<INode>();
		public ICollection<INode> Children
		{
			get { return _emptyList; }
		}

		public bool IsVisible
		{
			get { return true; }
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set { _isSelected = value; NotifyOfPropertyChange( () => IsSelected ); RaiseSelectionChanged(); }
		}

		#endregion
	}
}
