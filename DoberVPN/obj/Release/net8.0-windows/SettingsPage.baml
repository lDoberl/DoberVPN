   M S B A M L   `   `   `  ���� C  ?DoberVPN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:DoberVPNDoberVPN      DoberVPN.SettingsPage   TextlkJhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit/extended  W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  "localclr-namespace:DoberVPN    N� Title$  DoberVPN��$	��230��$	��220�� N� 
Background �	#FF322F2F.���  � ColumnDefinitions �� $
��Auto �$��23���� $	��92* � � RowDefinitions �� �� �� �� �� .+��� -    ��Name$ timeIntervalComboBox��
�������$��
14,0,149,0q�$��./#Jura Light)�$��13(�
������� �� SelectedIndex$ 0�� J� OpacityMask
 �����.@��� ���	#FF322F2F.��No�� ���	#FF322F2F.��10m�� ���	#FF322F2F.��30m�� ���	#FF322F2F.��1h�� ���	#FF322F2F.��2h�� -    ��Name$ intervalLabel��$$��Интервал смены IP��
�������$��Center=�$
��Left=�$��./#Jura Light)�$��13(�$��80,0,0,0q��� -   $ languagesComboBox��
�������$��
14,0,149,0q�$��1��$��./#Jura Light)�$��13(�
�������$ 0��
 �����.@��� ���	#FF322F2F.��Ru�� ���	#FF322F2F.��En�� -   $ languageLabel��$#��Язык интерфейса��
�������$��1��$��Center=�$
��Left=�$��./#Jura Light)�$��13(�$��80,0,0,0q��� -    ��Name$ autoLaunchCheckBox��$��4��$��14,0,192,10q�
�������
��������� -   	 ��Name$	 autoLaunchTextBlock��$C��=Автозагрузка при запуске системы��$
��Wrap=�
�������$��Center=�$
��Left=�$��./#Jura Light)�$��11(�$��4��$	��159��$��43,0,0,0q�