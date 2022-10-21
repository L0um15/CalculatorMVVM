using KalkulatorMVVM.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KalkulatorMVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _screenVal;
        public string ScreenVal
        {
            get { return _screenVal; }
            set { _screenVal = value; OnPropertyChanged(); }
        }

        public ICommand AddNumberCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand GetResultCommand { get; set; }

        private List<string> _availableOperations = new List<string> { "+", "-", "/", "*" };
        
        private DataTable _dataTable = new DataTable();

        private bool _isLastSignAnOperation = false;

        public MainWindowViewModel() 
        {
            ScreenVal = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation, CanAddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
        }

        public void AddNumber(object obj)
        {
            string number = obj as string;
            if (ScreenVal == "0" && number != ",")
            {
                ScreenVal = string.Empty;
            }
            else if (number == "," && _availableOperations.Contains(ScreenVal.Substring(ScreenVal.Length - 1)))
            {
                number = "0,";
            }
            ScreenVal += number;
            _isLastSignAnOperation = false;
        }

        public void AddOperation(object obj)
        {
            string operation = obj as string;
            ScreenVal += operation;
            _isLastSignAnOperation = true;
        }

        public void ClearScreen(object obj)
        {
            ScreenVal = "0";
            _isLastSignAnOperation = false;
        }

        public void GetResult(object obj)
        {
            object result = _dataTable.Compute(ScreenVal.Replace(",", "."), "");
            ScreenVal = result.ToString();
        }

        public bool CanAddOperation(object obj)
        {
            return !_isLastSignAnOperation;
        }
        public bool CanGetResult(object obj)
        {
            return !_isLastSignAnOperation;
        }
    }
}
