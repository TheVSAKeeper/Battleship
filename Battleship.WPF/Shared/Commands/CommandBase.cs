﻿using System.Windows.Input;

namespace Battleship.WPF.Shared.Commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);

    void ICommand.Execute(object? parameter)
    {
        if (CanExecute(parameter))
            Execute(parameter);
    }

    protected virtual bool CanExecute(object? parameter) => true;

    protected static void OnCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }

    protected abstract void Execute(object? parameter);
}