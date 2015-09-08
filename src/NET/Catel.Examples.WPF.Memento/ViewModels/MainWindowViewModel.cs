﻿namespace Catel.Examples.WPF.Memento.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Catel.IoC;
    using MVVM;
    using Catel.Memento;
    using Data;
    using Services;
    using Models;
    using Properties;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private readonly IMementoService _mementoService;

        #region Variables
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService, IMementoService mementoService)
        {
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _mementoService = mementoService;

            Add = new TaskCommand(OnAddExecuteAsync);
            Edit = new TaskCommand(OnEditExecuteAsync, OnEditCanExecute);
            Remove = new TaskCommand(OnRemoveExecuteAsync, OnRemoveCanExecute);

            Undo = new Command(() => _mementoService.Undo(), () => _mementoService.CanUndo);
            Redo = new Command(() => _mementoService.Redo(), () => _mementoService.CanRedo);

            PersonCollection = new ObservableCollection<Person> {new Person {Gender = Gender.Male, FirstName = "Geert", MiddleName = "van", LastName = "Horrik"}, new Person {Gender = Gender.Male, FirstName = "Fred", MiddleName = "", LastName = "Retteket"}};

            _mementoService.RegisterCollection(PersonCollection);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return Resources.MainWindowTitle; }
        }

        /// <summary>
        /// Gets the collection of Persons.
        /// </summary>
        public ObservableCollection<Person> PersonCollection
        {
            get { return GetValue<ObservableCollection<Person>>(PersonCollectionProperty); }
            set { SetValue(PersonCollectionProperty, value); }
        }

        /// <summary>
        /// Register the PersonCollection property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PersonCollectionProperty = RegisterProperty("PersonCollection", typeof(ObservableCollection<Person>));

        /// <summary>
        /// Gets or sets the selected person.
        /// </summary>
        public Person SelectedPerson
        {
            get { return GetValue<Person>(SelectedPersonProperty); }
            set { SetValue(SelectedPersonProperty, value); }
        }

        /// <summary>
        /// Register the SelectedPerson property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedPersonProperty = RegisterProperty("SelectedPerson", typeof(Person), null);
        #endregion

        #region Commands
        /// <summary>
        /// Gets the Add command.
        /// </summary>
        public TaskCommand Add { get; private set; }

        /// <summary>
        /// Method to invoke when the Add command is executed.
        /// </summary>
        private async Task OnAddExecuteAsync()
        {
            var typeFactory = TypeFactory.Default;
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<PersonViewModel>(new Person());
            if (await _uiVisualizerService.ShowDialogAsync(viewModel) ?? false)
            {
                PersonCollection.Add(viewModel.Person);
            }
        }

        /// <summary>
        /// Gets the Edit command.
        /// </summary>
        public TaskCommand Edit { get; private set; }

        /// <summary>
        /// Method to check whether the Edit command can be executed.
        /// </summary>
        /// <returns></returns>
        private bool OnEditCanExecute()
        {
            return (SelectedPerson != null);
        }

        /// <summary>
        /// Method to invoke when the Edit command is executed.
        /// </summary>
        private async Task OnEditExecuteAsync()
        {
            var typeFactory = TypeFactory.Default;
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<PersonViewModel>(SelectedPerson);
            await _uiVisualizerService.ShowDialogAsync(viewModel);
        }

        /// <summary>
        /// Gets the Remove command.
        /// </summary>
        public TaskCommand Remove { get; private set; }

        /// <summary>
        /// Method to check whether the Remove command can be executed.
        /// </summary>
        /// <returns></returns>
        private bool OnRemoveCanExecute()
        {
            return (SelectedPerson != null);
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private async Task OnRemoveExecuteAsync()
        {
            if (await _messageService.ShowAsync("Are you sure you want to remove this person?", "Are you sure?", MessageButton.YesNo) == MessageResult.Yes)
            {
                PersonCollection.Remove(SelectedPerson);
            }
        }

        /// <summary>
        /// Gets the undo.
        /// </summary>
        public Command Undo { get; private set; }

        /// <summary>
        /// Gets the redo.
        /// </summary>
        public Command Redo { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Called when the view model has just been closed.
        /// <para/>
        /// This method also raises the <see cref="E:Catel.MVVM.ViewModelBase.ClosedAsync"/> event.
        /// </summary>
        /// <param name="result">The result to pass to the view. This will, for example, be used as <c>DialogResult</c>.</param>
        protected override async Task OnClosedAsync(bool? result)
        {
            _mementoService.UnregisterCollection(PersonCollection);

            await base.OnClosedAsync(result);
        }
        #endregion
    }
}
