   M S B A M L   `   `   `  ����C  ?DoberVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:DoberVPNDoberVPN  5   	       DoberVPN.SettingsPage   5      TextlkJhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit/extended  5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   "localclr-namespace:DoberVPN  5   	     N� Title$  DoberVPN��5
   	   $	��230��6   $	��220��6'    N� 
Background �	#FF322F2F.��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   $��23��6,   �� 5      $	��92* �6    � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� -   5         ��Name$ timeIntervalComboBox��5      
�������$��
14,0,149,0q�6D   $��./#Jura Light)�6X   $��13(�6s   
������� �� SelectedIndex$ 0��6�    J� OpacityMask
 �����.@�5      �� ���	#FF322F2F.��62   No66   �� 5      ���	#FF322F2F.��62   1m66   �� 5      ���	#FF322F2F.��62   10m67   �� 5      ���	#FF322F2F.��62   30m67   �� 5      ���	#FF322F2F.��62   1h66   �� 5      ���	#FF322F2F.��62   2h66   5      �� 5    
   -   5         ��Name$ intervalLabel��5       $$��Интервал смены IP��6'   
�������$��Center=�6V   $
��Left=�6q   $��./#Jura Light)�6�   $��13(�6�   $��80,0,0,0q�6�   �� 5"   
   -   5        $ languagesComboBox��5"      
�������$��
14,0,149,0q�6A   $��1��6U   $��./#Jura Light)�6b   $��13(�6}   
�������$ 0��6�   
 �����.@�5#      �� ���	#FF322F2F.��62   Ru66   �� 5$      ���	#FF322F2F.��62   En66   5%      �� 5&   
   -   5        $ languageLabel��5&      $#��Язык интерфейса��6'   
�������$��1��6T   $��Center=�6a   $
��Left=�6|   $��./#Jura Light)�6�   $��13(�6�   $��80,0,0,0q�6�   �� 5(   
   -   5         ��Name$ autoLaunchCheckBox��5(      $��4��6/   $��
14,5,192,5q�6<   
�������
�������6w   �� 5)   
   -   5        	 ��Name$	 autoLaunchTextBlock��5)      $C��=Автозагрузка при запуске системы��61   $
��Wrap=�6Y   
�������$��Center=�6�   $
��Left=�6�   $��./#Jura Light)�6�   $��11(�6�   $��4��6�   $	��159��6�   $��43,0,0,0q�6�   5*      5+      