﻿namespace Catel.Examples.WPF.Validation.ViewModels
{
    using System;
    using Data;
    using MVVM;
    using Models;

    /// <summary>
    /// ValidationInIValidator view model.
    /// </summary>
    public class ValidationInIValidatorViewModel : ViewModelBase
    {
        #region Variables
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationInIValidatorViewModel"/> class.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="deferValidationUntilFirstSave">if set to <c>true</c> [defer validation until first save].</param>
        public ValidationInIValidatorViewModel(ModelWithoutValidation person = null, bool deferValidationUntilFirstSave = true)
        {
            if (person == null)
            {
                person = new ModelWithoutValidation();
            }

            Person = person;
            DeferValidationUntilFirstSaveCall = deferValidationUntilFirstSave;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "Validation in IValidator"; } }

        /// <summary>
        /// Gets the person model.
        /// </summary>
        [Model]
        public ModelWithoutValidation Person
        {
            get { return GetValue<ModelWithoutValidation>(PersonProperty); }
            private set { SetValue(PersonProperty, value); }
        }

        /// <summary>
        /// Register the Person property so it is known in the class.
        /// </summary>
        public static readonly PropertyData PersonProperty = RegisterProperty("Person", typeof(ModelWithoutValidation));

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [ViewModelToModel("Person")]
        public string FirstName
        {
            get { return GetValue<string>(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        /// Register the FirstName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string));

        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        [ViewModelToModel("Person")]
        public string MiddleName
        {
            get { return GetValue<string>(MiddleNameProperty); }
            set { SetValue(MiddleNameProperty, value); }
        }

        /// <summary>
        /// Register the MiddleName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData MiddleNameProperty = RegisterProperty("MiddleName", typeof(string));

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [ViewModelToModel("Person")]
        public string LastName
        {
            get { return GetValue<string>(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        /// <summary>
        /// Register the LastName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LastNameProperty = RegisterProperty("LastName", typeof(string));
        #endregion

        #region Commands
        #endregion

        #region Methods
        #endregion
    }
}
