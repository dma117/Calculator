using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public string leftNumber = "";
        public string rightNumber = "";
        public string operation = "";
        public string dot = "";
        double firstNumber;
        double secondNumber;
        double result;
        const string error = "Error: numbers can not be divided by zero";

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ProcessDivision(string get)
        {
            if (secondNumber == 0)
            {
                BlockExpression.Text = error;
                ClearVariables();
            }
            else
            {
                result = firstNumber / secondNumber;
                GetExpressionResult(get);
            }
        }

        public void FindShowNumbers(string numberFromButton, ref string number)
        {
            if (numberFromButton == "0" && (number == "" || number == "0"))
            {
                number = "0";
            }
            else
            {
                if (number == "0") { number = ""; }
                if (number == leftNumber && number == result.ToString()) { number = numberFromButton; }
                else { number += numberFromButton; }
            }
            BlockExpression.Text = (leftNumber + operation + rightNumber);
        }

        public void AnalyzeDoubleNumbers(string numberFromButton, ref string number)
        {
            if (!number.Contains(","))
            {
                if (number == "")
                {
                    number = "0,";
                }
                else if (number == result.ToString())
                {
                    number += numberFromButton;
                }
                else
                {
                    number += numberFromButton;
                }
            }

            BlockExpression.Text = (leftNumber + operation + rightNumber);

        }

        public void ClearVariables()
        {
            leftNumber = "";
            rightNumber = "";
            operation = "";
        }

        public void GetExpressionResult(string get)
        {
            ClearVariables();
            if (get == "=")
            {
                BlockExpression.Text = result.ToString();
            }
            else
            {

                BlockExpression.Text = (result.ToString() + get);
                operation = get;
            }
            leftNumber = result.ToString();
        }

        public void CountExpressionResult(string get)
        {
            firstNumber = double.Parse(leftNumber);
            secondNumber = double.Parse(rightNumber);

            switch (operation)
            {
                case "-":
                    result = firstNumber - secondNumber;
                    GetExpressionResult(get);
                    break;
                case "+":
                    result = firstNumber + secondNumber;
                    GetExpressionResult(get);
                    break;
                case "x":
                    result = firstNumber * secondNumber;
                    GetExpressionResult(get);
                    break;
                case "/":
                    ProcessDivision(get);
                    break;
                default:
                    BlockExpression.Text = "0";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string clickedButton = (String)((Button)e.OriginalSource).Content;
            int ifNumber;

            bool correct = Int32.TryParse(clickedButton, out ifNumber);

            if (clickedButton == "C")
            {
                BlockExpression.Text = "0";
                ClearVariables();
            }

            else if (clickedButton == "=")
            {
                if (leftNumber == "")
                {
                    BlockExpression.Text = "0";
                }
                else if (leftNumber != "" && rightNumber == "")
                {
                    if (leftNumber.Substring(leftNumber.Length - 1) == ",")
                    {
                        leftNumber = BlockExpression.Text.Substring(0,
                            BlockExpression.Text.Length - 1);
                    }
                    BlockExpression.Text = leftNumber;
                    operation = "=";
                }
                else
                {
                    CountExpressionResult(clickedButton);
                    operation = "=";
                }
            }
            else
            {
                if (correct == true)
                {
                    if (operation == "" || operation == "=")
                    {
                        if (operation == "=")
                        {
                            operation = "";
                            leftNumber = "";
                        }
                        FindShowNumbers(clickedButton, ref leftNumber);
                    }

                    else
                    {
                        FindShowNumbers(clickedButton, ref rightNumber);
                    }
                }
                else
                {
                    if (clickedButton == ",")
                    {
                        if (operation == "" || operation == "=")
                        {
                            if (operation == "=")
                            {
                                operation = "";
                                leftNumber = "0";
                            }
                            AnalyzeDoubleNumbers(clickedButton, ref leftNumber);
                        }
                        else
                        {
                            AnalyzeDoubleNumbers(clickedButton, ref rightNumber);
                        }
                    }
                    else
                    {
                        if (rightNumber == "")
                        {
                            if (leftNumber == "")
                            {
                                leftNumber = "0";
                            }
                            operation = clickedButton;
                            BlockExpression.Text = (leftNumber + operation);
                        }
                        else
                        {
                            CountExpressionResult(clickedButton);
                            if (clickedButton != "=" && BlockExpression.Text != error)
                            {
                                operation = clickedButton;
                            }
                        }
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
    }
}