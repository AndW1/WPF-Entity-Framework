using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WPF_ENTITY.Models;

namespace WPF_ENTITY
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskNodeContext dbo;


        public MainWindow()
        {
            InitializeComponent();

            dbo = new TaskNodeContext();

            dbo.TaskNodes.Load();

            taskGrid.ItemsSource = dbo.TaskNodes.Local.ToBindingList();

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            dbo.SaveChanges();
        }

        //Обработка события нажатия CheckBox состояния в работе
        private void IsProgress_Click(object sender, RoutedEventArgs e)
        {
            if ((taskGrid.SelectedItem as TaskNode) != null)
            {
                var node = taskGrid.SelectedItem as TaskNode;

                if (node.Title == null)
                {
                    ShowInstuction();
                    if ((e.OriginalSource as CheckBox).IsChecked == true)
                    {
                        (e.OriginalSource as CheckBox).IsChecked = false;
                    }
                }

                else
                { //Если TaskNode !=null

                    (taskGrid.SelectedItem as TaskNode).IsStart = false;
                    (taskGrid.SelectedItem as TaskNode).IsFinish = false;
                    (taskGrid.SelectedItem as TaskNode).IsProgress = true;

                    dbo.SaveChanges();

                    dbo = new TaskNodeContext();

                    dbo.TaskNodes.Load();

                    taskGrid.ItemsSource = dbo.TaskNodes.Local.ToBindingList();
                }
            }
            else
            {
                ShowInstuction();
                (e.OriginalSource as CheckBox).IsChecked = false;
            }
        }


        //Обработка события нажатия CheckBox состояния не начато
        private void IsStart_Click(object sender, RoutedEventArgs e)
        {
          
            if ((taskGrid.SelectedItem as TaskNode) != null)
            {
                var node = taskGrid.SelectedItem as TaskNode;

                if (node.Title == null)
                {
                    ShowInstuction();
                    if ((e.OriginalSource as CheckBox).IsChecked == true)
                    {
                       ( e.OriginalSource as CheckBox).IsChecked = false;
                    }
                }

                else
                { //Если TaskNode !=null

                    (taskGrid.SelectedItem as TaskNode).IsStart = true;
                    (taskGrid.SelectedItem as TaskNode).IsFinish = false;
                    (taskGrid.SelectedItem as TaskNode).IsProgress = false;

                    dbo.SaveChanges();

                    dbo = new TaskNodeContext();

                    dbo.TaskNodes.Load();

                    taskGrid.ItemsSource = dbo.TaskNodes.Local.ToBindingList();
                }

            }
           

            else
            {
                ShowInstuction();
                (e.OriginalSource as CheckBox).IsChecked = false;
            }

        }

        //Обработка события нажатия CheckBox состояния выполнена
        private void IsFinish_Click(object sender, RoutedEventArgs e)
        {
            if ((taskGrid.SelectedItem as TaskNode) != null)
            {

                var node = taskGrid.SelectedItem as TaskNode;

                if (node.Title == null)
                {
                    ShowInstuction();
                    if ((e.OriginalSource as CheckBox).IsChecked == true)
                    {
                        (e.OriginalSource as CheckBox).IsChecked = false;
                    }
                }

                else
                { //Если TaskNode !=null

                    (taskGrid.SelectedItem as TaskNode).IsStart = false;
                    (taskGrid.SelectedItem as TaskNode).IsFinish = true;
                    (taskGrid.SelectedItem as TaskNode).IsProgress = false;

                    DateTime now = DateTime.Now;
                    (taskGrid.SelectedItem as TaskNode).DateFinish = now.Date;


                    dbo.SaveChanges();

                    dbo = new TaskNodeContext();

                    dbo.TaskNodes.Load();

                    taskGrid.ItemsSource = dbo.TaskNodes.Local.ToBindingList();
                }
            }
            else
            {
                ShowInstuction();
                (e.OriginalSource as CheckBox).IsChecked = false;
            }
        }

        private void ShowInstuction()
        {
            MessageBox.Show("Порядок добавления задачи\n" +
                "1. Заполните поле заголовка и даты\n" +
                "2. Нажмите клавишу 'Обновить'\n" +
                "3. Выберите состояние задачи");

        }

        //Фильтрация по заголовку
        private void TitleText_KeyUp(object sender, KeyEventArgs e)
        {
            string title = (e.OriginalSource as TextBox).Text;
            taskGrid.ItemsSource = dbo.TaskNodes.Local.Where(n => n.Title.StartsWith(title)).Select(n => n).ToList();
        }

        //Фильтр по дате начала
        private void DateStartFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime startDate =(DateTime)(e.OriginalSource as DatePicker).SelectedDate;
            taskGrid.ItemsSource = dbo.TaskNodes.Where(n => n.DateStart == startDate).Select(n => n).ToList();
        }
        //Фильтр по дате завершения
        private void DateFinishFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime finishDate = (DateTime)(e.OriginalSource as DatePicker).SelectedDate;
            taskGrid.ItemsSource = dbo.TaskNodes.Where(n => n.DateFinish == finishDate).Select(n => n).ToList();
        }

        //Фильтр состояния не начата
        private void ChboxStartFilter_Click(object sender, RoutedEventArgs e)
        {
            taskGrid.ItemsSource = dbo.TaskNodes.Where(n => n.IsStart == true).Select(n => n).ToList();

            ChboxProgressFilter.IsChecked = false;
            ChboxFinishFilter.IsChecked = false;
        }
        //Фильтр состояния в работе
        private void ChboxProgressFilter_Click(object sender, RoutedEventArgs e)
        {
            taskGrid.ItemsSource = dbo.TaskNodes.Where(n => n.IsProgress == true).Select(n => n).ToList();

            ChboxStartFilter.IsChecked = false;
            ChboxFinishFilter.IsChecked = false;
        }
        //Фильтр состояния завершена
        private void ChboxFinishFilter_Click(object sender, RoutedEventArgs e)
        {
            taskGrid.ItemsSource = dbo.TaskNodes.Where(n => n.IsFinish == true).Select(n => n).ToList();

            ChboxStartFilter.IsChecked = false;
            ChboxProgressFilter.IsChecked = false;

        }

        //Сбросить все фильтры
        private void Drop_Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            TitleText.Text = "";
            ChboxStartFilter.IsChecked = false;
            ChboxProgressFilter.IsChecked = false;
            ChboxFinishFilter.IsChecked = false;
         
            taskGrid.ItemsSource = dbo.TaskNodes.Local.ToBindingList();
        }

        private void ВeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < taskGrid.SelectedItems.Count; i++)
                {
                    TaskNode task = taskGrid.SelectedItems[i] as TaskNode;
                    if (task != null)
                    {
                        dbo.TaskNodes.Remove(task);
                    }
                }
            }
            dbo.SaveChanges();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            dbo.Dispose();
        }
    }
}
