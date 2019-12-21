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
using System.IO;

namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int Max = 9;
        int number = 0;
        Test[] tests;
        public struct Test
        {
            public string question;
            public string CorrectAnswer;
            public string[] UnCorrect;
            public int correct;
            public int current;

            public bool generated;
        }
        
        public MainWindow()
        {
            InitializeComponent();
            OfLabel.Content = Max.ToString();
            Name = "EasyExam";
            tests = GetTest("../../Tests/Test_C++.txt");
            ShowQuestion(tests[number]);
            PushQuestions(tests);
        }

        public void Clear()
        {
            answer_A.Content = "A)";
            answer_B.Content = "B)";
            answer_C.Content = "C)";
            answer_D.Content = "D)";
            questionLabel.Content = " ";
        }

        

        public void PushQuestions(Test[] t)
        {
            string buffer;
            for(int i = 0; i< Max;i++)
            {
                buffer = (i+1).ToString();
                 buffer += ")          \t\t";
                panel.lst.Items.Add(buffer);
            }
        }

        public void ShowQuestion(Test t)
        {
            questionLabel.Content = number+1 + ")" + t.question;

            Random rand = new Random();

            int correct_number = 0;

            if (t.generated != true)
                correct_number = rand.Next(0, 4);
            else
                correct_number = t.correct;

            if (correct_number != 0)
                answer_A.Content += t.UnCorrect[0];
            else
                answer_A.Content += t.CorrectAnswer;

            if (correct_number != 1)
                answer_B.Content += t.UnCorrect[1];
            else
                answer_B.Content += t.CorrectAnswer;

            if (correct_number != 2)
                answer_C.Content += t.UnCorrect[2];
            else
                answer_C.Content += t.CorrectAnswer;

            if (correct_number == 3)
                answer_D.Content += t.CorrectAnswer;
            else
                answer_D.Content += t.UnCorrect[correct_number];

            tests[number].correct = correct_number;
            tests[number].generated = true;
        }


        public Test[] GetTest(string filename)
        {
            Test[] test = new Test[24];
            StreamReader reader = new StreamReader(filename);
            int k = 0;
            int uncorrect = 0;
            string str;
            try
            {
                while(true)
                {
                    str = reader.ReadLine();
                    for(int i = 0; i < str.Length;i++)
                    {
                        
                        if(str[i] != '#')
                        {
                            test[k].question += str[i];                         
                        }
                    }

                    str = reader.ReadLine();
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] != '!')
                        {
                            test[k].CorrectAnswer += str[i];
                        }
                    }
                    str = reader.ReadLine();
                    test[k].UnCorrect = new string[3];

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < str.Length; j++)
                        {
                            if (str[j] != '@')
                            {
                                test[k].UnCorrect[uncorrect] += str[j];
                            }
                        }
                        uncorrect++;
                        if(i != 2)
                        str = reader.ReadLine();
                    }
                    k++;
                    uncorrect = 0;
                    str = "";
                }
            }
            catch
            {

            }

            return test;
        }
        private void A_Click(object sender, RoutedEventArgs e)
        {
            string buffer = (number + 1).ToString();
             buffer += ")          \t\t";
            panel.lst.Items[number + 1] = buffer;
            tests[number].current = 0;
            panel.lst.Items[number + 1] += "\tA";
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            string buffer = (number + 1).ToString();
             buffer += ")          \t\t";
            panel.lst.Items[number + 1] = buffer;
            tests[number].current = 1;
            panel.lst.Items[number + 1] += "\tB";
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            string buffer = (number + 1).ToString();
             buffer += ")          \t\t";
            panel.lst.Items[number + 1] = buffer;
            tests[number].current = 2;
            panel.lst.Items[number + 1] += "\tC";
        }

        private void D_Click(object sender, RoutedEventArgs e)
        {
            string buffer = (number + 1).ToString();
             buffer += ")          \t\t";
            panel.lst.Items[number + 1] = buffer;
            tests[number].current = 3;
            panel.lst.Items[number + 1] += "\tD";
        }

        private void Mark_Click(object sender, RoutedEventArgs e)
        {
            string buffer = (number + 1).ToString();
             buffer += ")          \t\t";
            panel.lst.Items[number+1] = "*" + buffer;
        }

        public bool isChecked(string str, char param)
        {
            if (str[0] == param)
                return true;

            if (str[str.Length - 1] == param)
                return true;

            return false;
        }

        public void CheckRadioButtons()
        {
            mark.IsChecked = isChecked(panel.lst.Items[number + 1] as string, '*');
            answer_A.IsChecked = isChecked(panel.lst.Items[number + 1] as string, 'A');
            answer_B.IsChecked = isChecked(panel.lst.Items[number + 1] as string, 'B');
            answer_C.IsChecked = isChecked(panel.lst.Items[number + 1] as string, 'C');
            answer_D.IsChecked = isChecked(panel.lst.Items[number + 1] as string, 'D');
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (number != Max - 1)
            {
                Clear();
                ++number;
                CheckRadioButtons();
                CurrentLabel.Content = (number+1).ToString();
                ShowQuestion(tests[number]);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (number != 0)
            {
                Clear();
                --number;
                CheckRadioButtons();
                CurrentLabel.Content = (number+1).ToString();
                ShowQuestion(tests[number]);
            }
        }

        public double CheckAnswers()
        {
            double correct = 0;

            for(int i = 0; i < Max;i++)
            {
                if (tests[i].current == tests[i].correct)
                    correct++;
            }

            return (correct/Max)*100;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ви відповіли правильно на " + CheckAnswers().ToString() + "% відповідей!");
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (mainGrid.Background == Brushes.White)
            {
                mainGrid.Background = Brushes.Gray;
                dockPanel.Background = Brushes.DimGray;
                grid.Background = Brushes.DimGray;
            }
            else
            {
                mainGrid.Background = Brushes.White;
                dockPanel.Background = Brushes.Azure;
                grid.Background = Brushes.Azure;
            }
        }
    }
}