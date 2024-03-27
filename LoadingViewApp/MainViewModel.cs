using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Uno.Toolkit;

namespace LoadingViewApp;

public  class MainViewModel : ObservableObject
{
	public LoadableCommand MyCommand { get; }

	public MainViewModel()
	{
		MyCommand = new LoadableCommand(new AsyncRelayCommand(Load));
	}

	private async Task Load()
	{
		await Task.Delay(3000);
	}
}

public class LoadableCommand : ICommand, ILoadable
{
	private readonly AsyncRelayCommand _asyncCommand;

	public event EventHandler? CanExecuteChanged;
	public event EventHandler? IsExecutingChanged;


	public LoadableCommand(AsyncRelayCommand command)
	{
		_asyncCommand = command;
		_asyncCommand.CanExecuteChanged += (s, e) => CanExecuteChanged?.Invoke(this, e);
		_asyncCommand.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(_asyncCommand.IsRunning))
			{
				IsExecutingChanged?.Invoke(this, e);
			}
		};
	}

	public bool IsExecuting => _asyncCommand.IsRunning;

	public bool CanExecute(object? parameter) => _asyncCommand.CanExecute(parameter);

	public void Execute(object? parameter) => _asyncCommand.Execute(parameter);
}
