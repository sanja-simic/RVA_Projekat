using System;
using System.Windows;
using System.Windows.Input;
using TravelSystem.Client.ViewModels;
using TravelSystem.Client.Commands;

namespace TravelSystem.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            
            // Subscribe to global command manager events
            GlobalCommandManager.Instance.CanUndoRedoChanged += OnCanUndoRedoChanged;
            
            // Initial button state update
            UpdateUndoRedoButtons();
        }

        private void OnCanUndoRedoChanged(object sender, EventArgs e)
        {
            UpdateUndoRedoButtons();
        }

        private void UpdateUndoRedoButtons()
        {
            // These will be implemented when you add the buttons to XAML
            // btnUndo.IsEnabled = GlobalCommandManager.Instance.CanUndo;
            // btnRedo.IsEnabled = GlobalCommandManager.Instance.CanRedo;
            
            Console.WriteLine($"Global Command Manager - Undo enabled: {GlobalCommandManager.Instance.CanUndo}, Redo enabled: {GlobalCommandManager.Instance.CanRedo}");
        }

        // Commands for XAML binding
        public ICommand UndoCommand => new RelayCommand(
            execute: _ => GlobalCommandManager.Instance.Undo(),
            canExecute: _ => GlobalCommandManager.Instance.CanUndo
        );

        public ICommand RedoCommand => new RelayCommand(
            execute: _ => GlobalCommandManager.Instance.Redo(),
            canExecute: _ => GlobalCommandManager.Instance.CanRedo
        );

        protected override void OnClosed(EventArgs e)
        {
            // Unsubscribe from events
            GlobalCommandManager.Instance.CanUndoRedoChanged -= OnCanUndoRedoChanged;
            base.OnClosed(e);
        }
    }
}
