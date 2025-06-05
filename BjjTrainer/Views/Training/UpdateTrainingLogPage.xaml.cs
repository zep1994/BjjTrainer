using System;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace BjjTrainer.Views.Training
{
    [QueryProperty(nameof(LogId), "logId")]
    public partial class UpdateTrainingLogPage : ContentPage
    {
        private SfDataForm dataForm;

        public int LogId
        {
            get => BindingContext is UpdateTrainingLogViewModel vm ? vm.LogId : 0;
            set
            {
                if (value > 0 && BindingContext is not UpdateTrainingLogViewModel)
                {
                    BindingContext = new UpdateTrainingLogViewModel(value);
                    InitializeViewModel();
                }
            }
        }

        public UpdateTrainingLogPage()
        {
            InitializeComponent();
            dataForm = new SfDataForm();
        }

        private async void InitializeViewModel()
        {
            if (BindingContext is UpdateTrainingLogViewModel viewModel)
            {
                await viewModel.LoadTrainingLogDetailsAsync();
            }
        }

        private async void OnEditMovesClicked(object sender, EventArgs e)
        {
            if (BindingContext is UpdateTrainingLogViewModel viewModel)
            {
                await viewModel.EditMovesAsync();
            }
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is UpdateTrainingLogViewModel viewModel)
            {
                dataForm.Commit();

                if (dataForm.Validate())
                {
                    bool success = await viewModel.UpdateLogAsync();

                    if (success)
                    {
                        await DisplayAlert("Success", "Training log updated successfully.", "OK");
                        await Shell.Current.GoToAsync($"///ShowTrainingLogPage?logId={LogId}");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to update log. Please try again.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Validation Error", "Please check the form for errors.", "OK");
                }
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"///ShowTrainingLogPage?logId={LogId}");
        }

        private void OnGenerateDataFormItem(object sender, GenerateDataFormItemEventArgs e)
        {
            if (e.DataFormItem is DataFormTextEditorItem textEditorItem &&
                (e.DataFormItem.FieldName == "TrainingTime" ||
                e.DataFormItem.FieldName == "RoundsRolled" ||
                e.DataFormItem.FieldName == "Submissions" ||
                e.DataFormItem.FieldName == "Taps"))
            {
                textEditorItem.Keyboard = Keyboard.Numeric;
                textEditorItem.Background = Colors.DarkSlateGray;
            }
        }
    }
}
