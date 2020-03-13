using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Calculator.Model;



namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private const string Error = "Error: numbers can not be divided by zero";
        private string result;
        private bool clickedM = false;
        
        private readonly Calc myCalc = new Calc();
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string clickedButton = (String)((Button)e.OriginalSource).Content;
            bool correct = Int32.TryParse(clickedButton, out _);

            if (clickedButton == "C")
            {
                BlockExpression.Text = "0";
                ClearVariables();
            }
            else if (clickedButton == "B")
            {
                if (myCalc.Operation == "" && myCalc.Operation != "=")
                {
                    if (clickedButton == "B")
                    {
                        if (myCalc.LeftNumber != result)
                        {
                            ParseOperationB(ref myCalc.LeftNumber);
                        }
                    }
                }
                else
                {
                    if (myCalc.RightNumber != "")
                    {
                        ParseOperationB(ref myCalc.RightNumber);
                    }
                }
                if (myCalc.Operation == "=")
                {
                    BlockExpression.Text = myCalc.LeftNumber;
                }
                else
                {
                    BlockExpression.Text = myCalc.LeftNumber + myCalc.Operation + myCalc.RightNumber;
                }
            }
            else if (clickedButton == "CE")
            {
                if (myCalc.Operation == "" || myCalc.Operation == "=")
                {
                    ClearVariables();
                }
                else
                {
                    myCalc.RightNumber = "0";
                }
                BlockExpression.Text = myCalc.LeftNumber + myCalc.Operation + myCalc.RightNumber;
            }
            else if (clickedButton == "M+" || clickedButton == "M-" || clickedButton == "MR" 
                     || clickedButton == "MC" || clickedButton == "MS")
            {
                if (myCalc.Operation == "" || myCalc.Operation == "=" || myCalc.RightNumber == "")
                {
                    ParseMemorySign(clickedButton, ref myCalc.LeftNumber);
                    if (clickedButton == "MR")
                    {
                        BlockExpression.Text = myCalc.LeftNumber;
                    }
                }
                else
                {
                    ParseMemorySign(clickedButton, ref myCalc.RightNumber);
                    if (clickedButton == "MR")
                    {
                        BlockExpression.Text = myCalc.LeftNumber + myCalc.Operation + myCalc.RightNumber;
                    }
                }
            }
            else if (clickedButton == "+/-")
            {
                if (myCalc.Operation == "" || myCalc.Operation == "=")
                {
                    AddPlusMinus(ref myCalc.LeftNumber);
                    BlockExpression.Text = myCalc.LeftNumber;
                }
                else
                {
                    AddPlusMinus(ref myCalc.RightNumber);
                    if (myCalc.Operation == myCalc.RightNumber.Substring(0, 1))
                    {
                        BlockExpression.Text =  myCalc.LeftNumber + myCalc.Operation + "("+ myCalc.RightNumber + ")";
                    }
                    BlockExpression.Text = myCalc.LeftNumber + myCalc.Operation + myCalc.RightNumber;
                }
            }
            else if (clickedButton == "=")
            {
                ParseEqualsSign(clickedButton);
            }
            else
            {
                if (correct || clickedButton == ",")
                {
                    if (myCalc.Operation == "=")
                    {
                        ClearVariables();
                    }
                    if (myCalc.Operation == "")
                    {
                        GetNumbers(clickedButton, ref myCalc.LeftNumber);
                    }
                    else
                    {
                        GetNumbers(clickedButton, ref myCalc.RightNumber);
                    }
                    BlockExpression.Text = myCalc.LeftNumber + myCalc.Operation + myCalc.RightNumber;
                }
                else
                {
                    if (myCalc.RightNumber == "")
                    {
                        myCalc.Operation = clickedButton;
                        BlockExpression.Text = myCalc.LeftNumber + myCalc.Operation;
                    }
                    else
                    {
                        ShowResult(clickedButton);
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string itemValue = menuItem.Header.ToString();

            if (itemValue == "Exit")
            {
                Close();
            }
            if (itemValue == "Info")
            {
                MessageBox.Show("Calculator. It can sum, subtract, multiply and divide numbers.\n" +
                    "Made by Donskaya Maria. 2020");
            }
        }

        private void GetNumbers(string signFromButton, ref string number)
        {
            if (signFromButton == ",")
            {
                AnalyzeDoubleNumbers(signFromButton, ref number);
            }
            else
            {
                FindNumbers(signFromButton, ref number);
            }
        }

        private void ParseEqualsSign(string signFromButton)
        {
            if (myCalc.RightNumber == "")
            {
                if (myCalc.LeftNumber[^1..] == ",")
                {
                    myCalc.LeftNumber = BlockExpression.Text[..^1];
                }
                BlockExpression.Text = myCalc.LeftNumber;
            }
            else
            {
                ShowResult(signFromButton);
            }
            myCalc.Operation = "=";
        }

        private void ParseOperationB(ref string number)
        {
            if (number.Length == 1)
            {
                number = "0";
            }
            else
            {
                number = number[..^1];
            }
        }

        private void ParseMemorySign(string signFromButton, ref string number)
        {
            switch (signFromButton)
            {
                case "MS":
                    clickedM = true;
                    myCalc.MemoryNumber = double.Parse(number);
                    BlockMemory.Text = myCalc.MemoryNumber.ToString();
                    break;
                case "M+":
                    clickedM = true;
                    myCalc.MemoryNumber += double.Parse(number);
                    BlockMemory.Text = myCalc.MemoryNumber.ToString();
                    break;
                case "M-":
                    clickedM = true;
                    myCalc.MemoryNumber -= double.Parse(number);
                    BlockMemory.Text = myCalc.MemoryNumber.ToString();
                    break;
                case "MR":
                    if (clickedM)
                    {
                        number = myCalc.MemoryNumber.ToString();
                        BlockMemory.Text = myCalc.MemoryNumber.ToString();
                    }
                    break;
                case "MC":
                    clickedM = false;
                    myCalc.MemoryNumber = 0;
                    BlockMemory.Text = "";
                    break;
            }
        }

        private void AddPlusMinus(ref string number)
        {
            if (number == "0" || number == "") return; 
            if (number.Contains("-"))
            {
                number = number[1..];
            }
            else
            {
                number = "-" + number;
            }
        }
        
        private void ShowResult(string signFromButton)
        {
            result = myCalc.CountExpressionResult();
            GetExpressionResult(signFromButton);
        }
        
        private void GetExpressionResult(string signFromButton)
        {
            ClearVariables();
            if (signFromButton == "=" || result == Error)
            {
                BlockExpression.Text = result;
                if (result == Error)
                {
                    result = "0";
                }
            }
            else
            {
                BlockExpression.Text = result + signFromButton;
                myCalc.Operation = signFromButton;
            }
            myCalc.LeftNumber = result;
        }
        
        private void ClearVariables()
        {
            myCalc.LeftNumber = "0";
            myCalc.RightNumber = "";
            myCalc.Operation = "";
        }
        
        private void FindNumbers(string numberFromButton, ref string number)
        {
            if (number.Length > 12) return;
            if (numberFromButton == "0" && (number == "" || number == "0"))
            {
                number = "0";
            }
            else
            {
                if (number == "0")
                {
                    number = ""; 
                }
                number += numberFromButton;
            }
        }

        private void AnalyzeDoubleNumbers(string signFromButton, ref string number)
        {
            if (number.Contains(",") || number.Length > 12) return;
            if (number == "")
            {
                number = "0,";
            }
            else
            {
                number += signFromButton;
            }
        }
    }
}