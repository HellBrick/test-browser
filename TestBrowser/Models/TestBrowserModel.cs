﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellBrick.TestBrowser.Core;
using Microsoft.VisualStudio.TestWindow.Controller;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace HellBrick.TestBrowser.Models
{
	public class TestBrowserModel: IDisposable
	{
		private TestServiceContext _serviceContext;

		public TestBrowserModel( TestServiceContext serviceContext )
		{
			TestList = new ObservableCollection<TestModel>();

			_serviceContext = serviceContext;
			_serviceContext.RequestFactory.StateChanged += OnStateChanged;
		}

		#region Global event handlers

		private void OnStateChanged( object sender, OperationStateChangedEventArgs e )
		{
			try
			{
				_serviceContext.Logger.Log( MessageLevel.Informational, e.ToString() );

				switch ( e.State )
				{
					case TestOperationStates.DiscoveryFinished:
						OnDiscoveryFinished( e );
						break;
				}
			}
			catch ( Exception ex )
			{
				_serviceContext.Logger.LogException( ex );
			}
		}

		private void OnDiscoveryFinished( OperationStateChangedEventArgs e )
		{
			using ( var reader = _serviceContext.Storage.ActiveUnitTestReader )
			{
				using ( var query = reader.GetAllTests() )
				{
					UpdateTestList( query.ToArray() );
				}
			}
		}

		private void UpdateTestList( ITest[] tests )
		{
			_serviceContext.Dispatcher.Invoke( () => TestList.Clear() );
			foreach ( var test in tests )
				_serviceContext.Dispatcher.Invoke( () => TestList.Add( new TestModel( test ) ) );
		}

		#endregion

		#region Properties

		public ObservableCollection<TestModel> TestList { get; private set; }

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_serviceContext.RequestFactory.StateChanged -= OnStateChanged;
		}

		#endregion
	}
}
