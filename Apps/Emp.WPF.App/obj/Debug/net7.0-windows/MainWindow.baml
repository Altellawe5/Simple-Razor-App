   M S B A M L   `   `   `  ����G  CMenu.WPF.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Emp.WPF.AppEmp.WPF.App  5   	       Emp.WPF.App.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Emp.WPF.App     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $
��Auto �6   6.   .+�5   
   �� -   5        $��GetEmployeesBtn��5      $��1��6(   $��1��65   $	��Get��6E   $��Right=�6S   6o   �� 5   
   -   5        $��EmployeesListBox��5      $��0��6*   $��0��67   $��2��6G   $��Stretch=�6[   5      5      