   M S B A M L   `   `   `  ���� O  KРассыльщик, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WpfAppMailSenderWpfAppMailSender  F BTabSwitcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:TabSwitcher;assembly=TabSwitcherTabSwitcher !    WpfAppMailSender.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:WpfAppMailSender     X TXceed.Wpf.Toolkit, Version=3.8.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  @tabSwitcher.clr-namespace:TabSwitcher;assembly=TabSwitcher    � Title$  Рассыльщик��$	��500��$	��780�� � WindowStartupLocation$ CenterScreen=� � Icon$ /img/mail-icon.ico��
������� � 	Resources  �� MergedDictionaries��  %    .��]� .+�o� $	��Top=�.@�m� $��Файл��.@�m� -   $��
Выход��$  TabSwitcher.TabSwitcherControl  -   $��Bottom=�$��25���� -    ��Name$ MainTabControl��.@��� -    ��Name$ 	TabSender��$<��6Формирование группы рассылки��$��3q�.��]� .+�`� $	��Top=�.�c� -    c�Name$ TbSender��$
��Left=�$��30��$	��Top=�$
��Auto�� c� Band$ 1��	 c� 	BandIndex$	 0��.@��� $	��130��.��'%Выбрать отправителя�� -   
 ��Name$
 CbSenderSelect��#   WpfAppMailSender.SendersClass   SendersDictionary#��Z $��25��$��0q�$	��Top=�$	��270�� �� SelectedIndex$ 0�� � ToolTip$+ %Выбрать отправителя������ .R��� ���� *Key+�� -    ��Name$ BtnAddSender��$ Добавить��$��Stretch=�$��25��.���� $��img/add.png��$��Right=��� -   $ BtnEditSender��$  Редактировать��$��Center=�$��25��.���� $��img/edit.png���� -	   $ BtnDeleteSender��$ Удалить��$��Right=�$��25��.���� $��img/delete.png��$��Right=�c� -
   $ TbServer��$
��Left=�$��30��$	��Top=�$
��Auto��$ 2��$	 0��.@��� $	��130��.��Выбрать сервер�� -   $
 CbServerSelect��&    WpfAppMailSender.SmtpServerClass   SmtpServerDictionary#��Z $��25��$��0q�$	��Top=�$	��270��$ 0��$! Выбрать сервер������ .R��� ���� *Key+�� -   $ BtnAddServer��$ Добавить��$��Stretch=�$��25��.���� $��img/add.png��$��Right=��� -   $ BtnEditServer��$  Редактировать��$��Center=�$��25��.���� $��img/edit.png���� -   $ BtnDeleteServer��$ Удалить��$��Right=�$��25��.���� $��img/delete.png��$��Right=�c� -   $ TbPlaner��$
��Left=�$��30��$	��Top=�$
��Auto��$ 1��$	 1��.@��� $	��200��.��*(Перейти в планировщик�� -   $ BtnClock��$. (Перейти в планировщик��$��Stretch=�$��25��.���� $��img/clock.png��$��Right=�c� -   $ TbAddressee��$
��Left=�$��30��$	��Top=�$
��Auto��$ 2��$	 1��.@��� $	��150��.��Список адресов�� -   $ BtnAddAddress��$ Добавить��$��Stretch=�$��25��.���� $��img/add.png��$��Right=��� -   $ BtnEditAddress��$  Редактировать��$��Center=�$��25��.���� $��img/edit.png���� -   $ BtnDeleteAddress��$ Удалить��$��Right=�$��25��.���� $��img/delete.png��$��Right=�&   System.Windows.Controls.DataGrid  -     Name$ DgEmails��$  WpfAppMailSender.DataBaseClass   Emails#��Z    AutoGenerateColumns .     
IsReadOnly .    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$	 №��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� *Id+  $ 
Почта��$
 Auto  �� *EmailAddress+  $# Имя отправителя��$ *  �� *Name+�� -   $ 	TabPlaner��$��Планировщик��$��3q�.���  � ColumnDefinitions �� $
��Auto �.+��� $��0��.+�&
   System.Windows.Controls.Calendar
  -    
 Name$ CldSchedulDateTimes�� 
  SelectionMode$ MultipleRange=�"  Xceed.Wpf.Toolkit.TimePicker  -     Name$ TpTimePicker��$��0,0,0,3q��� -   $ BtnSendClock��$��30������  �� Orientation$ 
Horizontal=�.+��� $��img/clock.png��$��16��$��16���� $��3,0,0,0q�$3��-Отправить запланировано���� -   $ BtnSendAtOnce��$��30��$��0,3,0,3q����� $ 
Horizontal=�.+��� $��img/SendAtOnce.png��$��16��$��16���� $��3,0,0,0q�$#��Отправить сразу���� -   $ TabEditMail��$!��Редактор писем��$��3q�.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��.+��� .��Тема сообщения�� -    ��Name$ 	TbSubject���� $��1��.��Текст сообщения�� -    ��Name$ RtbBodyMail��$��2��$��Stretch=�