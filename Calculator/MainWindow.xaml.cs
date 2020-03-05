using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public string leftNumber = "", rightNumber = "", operation = "";
        public string text = "";
        double firstNumber, secondNumber, result;
        const string error = "Error: numbers can not be devided by zero";
        public MainWindow()
        {
            InitializeComponent();
        }

        public void FindShowNumbers(string numberFromBotton, ref string number)
        {
            if (numberFromBotton == "0" && (number == "" || number == "0"))
            {
                number = "0";
            }
            else
            {
                if (number == "0") { number = ""; }
                if (number == leftNumber && number == result.ToString()) { number = numberFromBotton; } //TODO
                else { number += numberFromBotton; }
            }
            BlockExpression.Text = (leftNumber + operation + rightNumber);
        }

        public void ClearVariables()
        {
            leftNumber = "";
            rightNumber = "";
            operation = "";
            text = "";
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
                    if (rightNumber == "0")
                    {
                        BlockExpression.Text = error;
                        ClearVariables();
                    }
                    else
                    {
                        result = firstNumber / secondNumber;
                        GetExpressionResult(get);
                    }
                    break;
                default:
                    BlockExpression.Text = "0";
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BlockExpression.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string get = (String)((Button)e.OriginalSource).Content; //поменять название переменной
            int ifNumber;

            bool correct = Int32.TryParse(get, out ifNumber);

            if (get == "C")
            {
                BlockExpression.Text = "0";
                ClearVariables();
            }

            else if (get == "=")
            {
                if (leftNumber == "")
                {
                    BlockExpression.Text = "0";
                }
                else if (leftNumber != "" && rightNumber == "")
                {
                    BlockExpression.Text = leftNumber;
                    operation = "";
                }
                else
                {
                    CountExpressionResult(get);
                }
            }
            else
            {
                if (correct == true)
                {
                    if (operation == "")
                    {
                        FindShowNumbers(get, ref leftNumber);
                    }

                    else
                    {
                        FindShowNumbers(get, ref rightNumber);
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
                        operation = get;
                        BlockExpression.Text = (leftNumber + operation);
                    }
                    else
                    {
                        CountExpressionResult(get);
                        if (get != "=" && BlockExpression.Text != error) { operation = get; }
                    }
                }
            }
        }
    }
}
