﻿using System;
using System.Windows.Forms;
using Rubberduck.UI.Refactorings;

namespace Rubberduck.Refactorings
{
    public class RefactoringPresenter<TModel, TView, TViewModel> : IDisposable
        where TModel : class
        where TView : Form, IRefactoringDialog2<TViewModel>, new()
        where TViewModel : class, IRefactoringViewModel<TModel>, new()
    {
        protected readonly TView view;
        protected DialogResult dialogResult;
        
        public static RefactoringPresenter<TModel, TView, TViewModel> Create(TModel model)
        {
            return new RefactoringPresenter<TModel, TView, TViewModel>(model, new TView(), new TViewModel());
        }
        
        public RefactoringPresenter(TModel model, TView view, TViewModel viewModel)
        {
            this.view = view;
            this.view.ViewModel = viewModel;
            this.view.ViewModel.Model = model;
        }

        public TModel Model => view.ViewModel.Model;
        public TView View => view;
        public TViewModel ViewModel => view.ViewModel;
        public virtual DialogResult DialogResult => dialogResult;

        public virtual TModel Show()
        {
            dialogResult = view.ShowDialog();
            if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
            {
                return view.ViewModel.Model;
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                view.Dispose();
            }
        }
    }
}

